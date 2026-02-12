using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using XrmToolBox.Extensibility;

namespace BulkPowerAutomateActivator
{
    public partial class BulkPowerAutomateActivatorControl : PluginControlBase
    {
        public BulkPowerAutomateActivatorControl()
        {
            InitializeComponent();
            ConnectionUpdated += OnConnectionUpdated;
        }

        private void OnConnectionUpdated(object sender, EventArgs e)
        {
            cmbSolutions.Items.Clear();
            cmbSolutions.Tag = null;
            dgvFlows.Rows.Clear();
            cmbUsers.Items.Clear();
            cmbUsers.Tag = null;
            txtSearchUser.Text = "";
            rtbLog.Clear();
            progressBar.Value = 0;
            AppendLog("Connection updated. Use 'Load Solutions' to begin.");
        }

        private void btnLoadSolutions_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadSolutions);
        }

        private void btnLoadFlows_Click(object sender, EventArgs e)
        {
            if (cmbSolutions.SelectedItem == null)
            {
                MessageBox.Show("Please select a solution first.", "No Solution Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ExecuteMethod(LoadFlows);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvFlows.Rows)
            {
                row.Cells["colSelect"].Value = true;
            }
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvFlows.Rows)
            {
                row.Cells["colSelect"].Value = false;
            }
        }

        private void btnActivateSelected_Click(object sender, EventArgs e)
        {
            ExecuteMethod(ActivateSelectedFlows);
        }

        private void btnDeactivateSelected_Click(object sender, EventArgs e)
        {
            ExecuteMethod(DeactivateSelectedFlows);
        }

        private void LoadSolutions()
        {
            AppendLog("Loading solutions...");

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading solutions...",
                Work = (worker, args) =>
                {
                    var query = new QueryExpression("solution")
                    {
                        ColumnSet = new ColumnSet("solutionid", "friendlyname", "uniquename", "version"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("isvisible", ConditionOperator.Equal, true)
                            }
                        },
                        Orders = { new OrderExpression("friendlyname", OrderType.Ascending) }
                    };

                    args.Result = Service.RetrieveMultiple(query);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        AppendLog("Error loading solutions: " + args.Error.Message, Color.Red);
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var results = (EntityCollection)args.Result;
                    cmbSolutions.Items.Clear();

                    var solutionMap = new Dictionary<string, Guid>();

                    foreach (var entity in results.Entities)
                    {
                        var name = entity.GetAttributeValue<string>("friendlyname");
                        var uniqueName = entity.GetAttributeValue<string>("uniquename");
                        var version = entity.GetAttributeValue<string>("version");
                        var display = $"{name} ({uniqueName}) v{version}";
                        cmbSolutions.Items.Add(display);
                        solutionMap[display] = entity.Id;
                    }

                    cmbSolutions.Tag = solutionMap;
                    AppendLog($"Loaded {results.Entities.Count} solutions.");

                    if (cmbSolutions.Items.Count > 0)
                    {
                        cmbSolutions.SelectedIndex = 0;
                    }
                }
            });
        }

        private void LoadFlows()
        {
            var solutionMap = cmbSolutions.Tag as Dictionary<string, Guid>;
            if (solutionMap == null || cmbSolutions.SelectedItem == null)
                return;

            var selectedKey = cmbSolutions.SelectedItem.ToString();
            if (!solutionMap.ContainsKey(selectedKey))
                return;

            var solutionId = solutionMap[selectedKey];
            AppendLog($"Loading flows for solution: {selectedKey}");

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading flows...",
                Work = (worker, args) =>
                {
                    // Step 1: Get solution components of type 29 (Workflow)
                    var componentQuery = new QueryExpression("solutioncomponent")
                    {
                        ColumnSet = new ColumnSet("objectid"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("solutionid", ConditionOperator.Equal, solutionId),
                                new ConditionExpression("componenttype", ConditionOperator.Equal, 29)
                            }
                        }
                    };

                    var components = Service.RetrieveMultiple(componentQuery);
                    var workflowIds = components.Entities
                        .Select(e => e.GetAttributeValue<Guid>("objectid"))
                        .Where(id => id != Guid.Empty)
                        .ToArray();

                    if (workflowIds.Length == 0)
                    {
                        args.Result = new EntityCollection();
                        return;
                    }

                    // Step 2: Query workflow entity for cloud flows (category=5)
                    var flowQuery = new QueryExpression("workflow")
                    {
                        ColumnSet = new ColumnSet("workflowid", "name", "statecode", "statuscode", "modifiedon", "category", "ownerid"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("workflowid", ConditionOperator.In, workflowIds.Cast<object>().ToArray()),
                                new ConditionExpression("category", ConditionOperator.Equal, 5)
                            }
                        },
                        Orders = { new OrderExpression("name", OrderType.Ascending) }
                    };

                    args.Result = Service.RetrieveMultiple(flowQuery);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        AppendLog("Error loading flows: " + args.Error.Message, Color.Red);
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var results = (EntityCollection)args.Result;
                    dgvFlows.Rows.Clear();

                    foreach (var entity in results.Entities)
                    {
                        var name = entity.GetAttributeValue<string>("name") ?? "(unnamed)";
                        var state = entity.GetAttributeValue<OptionSetValue>("statecode");
                        var stateLabel = state != null && state.Value == 1 ? "Active" : "Draft";
                        var modifiedOn = entity.GetAttributeValue<DateTime?>("modifiedon");
                        var modifiedStr = modifiedOn.HasValue ? modifiedOn.Value.ToLocalTime().ToString("g") : "";
                        var ownerRef = entity.GetAttributeValue<EntityReference>("ownerid");
                        var ownerName = ownerRef != null ? ownerRef.Name ?? "" : "";

                        var rowIndex = dgvFlows.Rows.Add(false, name, stateLabel, ownerName, modifiedStr);
                        dgvFlows.Rows[rowIndex].Tag = entity.Id;
                    }

                    AppendLog($"Found {results.Entities.Count} cloud flow(s) in the selected solution.");
                }
            });
        }

        private void ActivateSelectedFlows()
        {
            var selectedFlows = new List<KeyValuePair<Guid, string>>();

            foreach (DataGridViewRow row in dgvFlows.Rows)
            {
                var isChecked = row.Cells["colSelect"].Value as bool?;
                if (isChecked == true && row.Tag is Guid flowId)
                {
                    var flowName = row.Cells["colFlowName"].Value?.ToString() ?? "(unnamed)";
                    selectedFlows.Add(new KeyValuePair<Guid, string>(flowId, flowName));
                }
            }

            if (selectedFlows.Count == 0)
            {
                MessageBox.Show("Please select at least one flow to activate.", "No Flows Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppendLog($"Activating {selectedFlows.Count} flow(s)...");
            progressBar.Minimum = 0;
            progressBar.Maximum = selectedFlows.Count;
            progressBar.Value = 0;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Activating flows...",
                Work = (worker, args) =>
                {
                    int succeeded = 0;
                    int failed = 0;

                    for (int i = 0; i < selectedFlows.Count; i++)
                    {
                        var flow = selectedFlows[i];
                        try
                        {
                            var request = new SetStateRequest
                            {
                                EntityMoniker = new EntityReference("workflow", flow.Key),
                                State = new OptionSetValue(1),  // Activated
                                Status = new OptionSetValue(2)  // Activated
                            };

                            Service.Execute(request);
                            succeeded++;
                            worker.ReportProgress((i + 1) * 100 / selectedFlows.Count,
                                $"OK|Activated: {flow.Value}");
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            worker.ReportProgress((i + 1) * 100 / selectedFlows.Count,
                                $"ERR|FAILED to activate '{flow.Value}': {ex.Message}");
                        }
                    }

                    args.Result = new int[] { succeeded, failed };
                },
                ProgressChanged = (args) =>
                {
                    var raw = args.UserState as string;
                    if (raw != null)
                    {
                        ParseAndLog(raw);
                    }

                    var progress = (int)((double)(args.ProgressPercentage) / 100 * selectedFlows.Count);
                    if (progress <= progressBar.Maximum)
                    {
                        progressBar.Value = progress;
                    }
                },
                PostWorkCallBack = (args) =>
                {
                    progressBar.Value = progressBar.Maximum;

                    if (args.Error != null)
                    {
                        AppendLog("Unexpected error: " + args.Error.Message, Color.Red);
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var counts = (int[])args.Result;
                    var summaryColor = counts[1] > 0 ? Color.Orange : Color.LimeGreen;
                    AppendLog($"Activation complete. Succeeded: {counts[0]}, Failed: {counts[1]}", summaryColor);

                    LoadFlows();
                }
            });
        }

        private void DeactivateSelectedFlows()
        {
            var selectedFlows = new List<KeyValuePair<Guid, string>>();

            foreach (DataGridViewRow row in dgvFlows.Rows)
            {
                var isChecked = row.Cells["colSelect"].Value as bool?;
                if (isChecked == true && row.Tag is Guid flowId)
                {
                    var flowName = row.Cells["colFlowName"].Value?.ToString() ?? "(unnamed)";
                    selectedFlows.Add(new KeyValuePair<Guid, string>(flowId, flowName));
                }
            }

            if (selectedFlows.Count == 0)
            {
                MessageBox.Show("Please select at least one flow to deactivate.", "No Flows Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppendLog($"Deactivating {selectedFlows.Count} flow(s)...");
            progressBar.Minimum = 0;
            progressBar.Maximum = selectedFlows.Count;
            progressBar.Value = 0;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Deactivating flows...",
                Work = (worker, args) =>
                {
                    int succeeded = 0;
                    int failed = 0;

                    for (int i = 0; i < selectedFlows.Count; i++)
                    {
                        var flow = selectedFlows[i];
                        try
                        {
                            var request = new SetStateRequest
                            {
                                EntityMoniker = new EntityReference("workflow", flow.Key),
                                State = new OptionSetValue(0),  // Draft
                                Status = new OptionSetValue(1)  // Draft
                            };

                            Service.Execute(request);
                            succeeded++;
                            worker.ReportProgress((i + 1) * 100 / selectedFlows.Count,
                                $"OK|Deactivated: {flow.Value}");
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            worker.ReportProgress((i + 1) * 100 / selectedFlows.Count,
                                $"ERR|FAILED to deactivate '{flow.Value}': {ex.Message}");
                        }
                    }

                    args.Result = new int[] { succeeded, failed };
                },
                ProgressChanged = (args) =>
                {
                    var raw = args.UserState as string;
                    if (raw != null)
                    {
                        ParseAndLog(raw);
                    }

                    var progress = (int)((double)(args.ProgressPercentage) / 100 * selectedFlows.Count);
                    if (progress <= progressBar.Maximum)
                    {
                        progressBar.Value = progress;
                    }
                },
                PostWorkCallBack = (args) =>
                {
                    progressBar.Value = progressBar.Maximum;

                    if (args.Error != null)
                    {
                        AppendLog("Unexpected error: " + args.Error.Message, Color.Red);
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var counts = (int[])args.Result;
                    var summaryColor = counts[1] > 0 ? Color.Orange : Color.LimeGreen;
                    AppendLog($"Deactivation complete. Succeeded: {counts[0]}, Failed: {counts[1]}", summaryColor);

                    LoadFlows();
                }
            });
        }

        private void btnSearchUsers_Click(object sender, EventArgs e)
        {
            ExecuteMethod(SearchUsers);
        }

        private void btnChangeOwner_Click(object sender, EventArgs e)
        {
            ExecuteMethod(ChangeOwnerOfSelectedFlows);
        }

        private void SearchUsers()
        {
            var searchText = txtSearchUser.Text.Trim();
            if (searchText.Length < 2)
            {
                MessageBox.Show("Please enter at least 2 characters to search.", "Search Text Too Short",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppendLog($"Searching users matching '{searchText}'...");

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Searching users...",
                Work = (worker, args) =>
                {
                    var query = new QueryExpression("systemuser")
                    {
                        ColumnSet = new ColumnSet("systemuserid", "fullname", "domainname", "applicationid"),
                        Criteria = new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                new ConditionExpression("isdisabled", ConditionOperator.Equal, false)
                            },
                            Filters =
                            {
                                new FilterExpression(LogicalOperator.Or)
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression("fullname", ConditionOperator.Like, $"%{searchText}%"),
                                        new ConditionExpression("domainname", ConditionOperator.Like, $"%{searchText}%")
                                    }
                                }
                            }
                        },
                        Orders = { new OrderExpression("fullname", OrderType.Ascending) }
                    };

                    args.Result = Service.RetrieveMultiple(query);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        AppendLog("Error searching users: " + args.Error.Message, Color.Red);
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var results = (EntityCollection)args.Result;
                    cmbUsers.Items.Clear();
                    var userMap = new Dictionary<string, Guid>();

                    foreach (var entity in results.Entities)
                    {
                        var fullname = entity.GetAttributeValue<string>("fullname") ?? "";
                        var domainname = entity.GetAttributeValue<string>("domainname") ?? "";
                        var applicationId = entity.GetAttributeValue<Guid?>("applicationid");

                        string display;
                        if (applicationId.HasValue && applicationId.Value != Guid.Empty)
                        {
                            display = $"{fullname} [App User]";
                        }
                        else
                        {
                            display = $"{fullname} ({domainname})";
                        }

                        cmbUsers.Items.Add(display);
                        userMap[display] = entity.Id;
                    }

                    cmbUsers.Tag = userMap;
                    AppendLog($"Found {results.Entities.Count} user(s).");

                    if (cmbUsers.Items.Count > 0)
                    {
                        cmbUsers.SelectedIndex = 0;
                    }
                }
            });
        }

        private void ChangeOwnerOfSelectedFlows()
        {
            var userMap = cmbUsers.Tag as Dictionary<string, Guid>;
            if (userMap == null || cmbUsers.SelectedItem == null)
            {
                MessageBox.Show("Please search and select a user first.", "No User Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedUserKey = cmbUsers.SelectedItem.ToString();
            if (!userMap.ContainsKey(selectedUserKey))
                return;

            var selectedUserId = userMap[selectedUserKey];

            var selectedFlows = new List<KeyValuePair<Guid, string>>();

            foreach (DataGridViewRow row in dgvFlows.Rows)
            {
                var isChecked = row.Cells["colSelect"].Value as bool?;
                if (isChecked == true && row.Tag is Guid flowId)
                {
                    var flowName = row.Cells["colFlowName"].Value?.ToString() ?? "(unnamed)";
                    selectedFlows.Add(new KeyValuePair<Guid, string>(flowId, flowName));
                }
            }

            if (selectedFlows.Count == 0)
            {
                MessageBox.Show("Please select at least one flow to change owner.", "No Flows Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppendLog($"Changing owner of {selectedFlows.Count} flow(s) to '{selectedUserKey}'...");
            progressBar.Minimum = 0;
            progressBar.Maximum = selectedFlows.Count;
            progressBar.Value = 0;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Changing owner of flows...",
                Work = (worker, args) =>
                {
                    int succeeded = 0;
                    int failed = 0;

                    for (int i = 0; i < selectedFlows.Count; i++)
                    {
                        var flow = selectedFlows[i];
                        try
                        {
                            var request = new AssignRequest
                            {
                                Assignee = new EntityReference("systemuser", selectedUserId),
                                Target = new EntityReference("workflow", flow.Key)
                            };

                            Service.Execute(request);
                            succeeded++;
                            worker.ReportProgress((i + 1) * 100 / selectedFlows.Count,
                                $"OK|Changed owner: {flow.Value}");
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            worker.ReportProgress((i + 1) * 100 / selectedFlows.Count,
                                $"ERR|FAILED to change owner of '{flow.Value}': {ex.Message}");
                        }
                    }

                    args.Result = new int[] { succeeded, failed };
                },
                ProgressChanged = (args) =>
                {
                    var raw = args.UserState as string;
                    if (raw != null)
                    {
                        ParseAndLog(raw);
                    }

                    var progress = (int)((double)(args.ProgressPercentage) / 100 * selectedFlows.Count);
                    if (progress <= progressBar.Maximum)
                    {
                        progressBar.Value = progress;
                    }
                },
                PostWorkCallBack = (args) =>
                {
                    progressBar.Value = progressBar.Maximum;

                    if (args.Error != null)
                    {
                        AppendLog("Unexpected error: " + args.Error.Message, Color.Red);
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var counts = (int[])args.Result;
                    var summaryColor = counts[1] > 0 ? Color.Orange : Color.LimeGreen;
                    AppendLog($"Change owner complete. Succeeded: {counts[0]}, Failed: {counts[1]}", summaryColor);

                    LoadFlows();
                }
            });
        }

        private void ParseAndLog(string raw)
        {
            if (raw.StartsWith("ERR|"))
            {
                AppendLog(raw.Substring(4), Color.Red);
            }
            else if (raw.StartsWith("OK|"))
            {
                AppendLog(raw.Substring(3));
            }
            else
            {
                AppendLog(raw);
            }
        }

        private void AppendLog(string message, Color? color = null)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => AppendLog(message, color)));
                return;
            }

            var text = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor = color ?? Color.LimeGreen;
            rtbLog.AppendText(text);
            rtbLog.SelectionColor = Color.LimeGreen;
            rtbLog.ScrollToCaret();
        }
    }
}
