using System;
using System.Collections.Generic;
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
                        AppendLog("Error loading solutions: " + args.Error.Message);
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
                        ColumnSet = new ColumnSet("workflowid", "name", "statecode", "statuscode", "modifiedon", "category"),
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
                        AppendLog("Error loading flows: " + args.Error.Message);
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

                        var rowIndex = dgvFlows.Rows.Add(false, name, stateLabel, modifiedStr);
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
                                $"Activated: {flow.Value}");
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            worker.ReportProgress((i + 1) * 100 / selectedFlows.Count,
                                $"FAILED to activate '{flow.Value}': {ex.Message}");
                        }
                    }

                    args.Result = new int[] { succeeded, failed };
                },
                ProgressChanged = (args) =>
                {
                    var message = args.UserState as string;
                    if (message != null)
                    {
                        AppendLog(message);
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
                        AppendLog("Unexpected error: " + args.Error.Message);
                        MessageBox.Show(args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var counts = (int[])args.Result;
                    AppendLog($"Activation complete. Succeeded: {counts[0]}, Failed: {counts[1]}");

                    // Refresh the flows list to show updated states
                    LoadFlows();
                }
            });
        }

        private void AppendLog(string message)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => AppendLog(message)));
                return;
            }

            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            rtbLog.ScrollToCaret();
        }
    }
}
