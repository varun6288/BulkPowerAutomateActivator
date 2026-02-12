namespace BulkPowerAutomateActivator
{
    partial class BulkPowerAutomateActivatorControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.btnLoadFlows = new System.Windows.Forms.Button();
            this.cmbSolutions = new System.Windows.Forms.ComboBox();
            this.lblSolution = new System.Windows.Forms.Label();
            this.btnLoadSolutions = new System.Windows.Forms.Button();
            this.dgvFlows = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFlowName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOwner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModifiedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelActions = new System.Windows.Forms.Panel();
            this.btnActivateSelected = new System.Windows.Forms.Button();
            this.btnDeactivateSelected = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.panelChangeOwner = new System.Windows.Forms.Panel();
            this.lblNewOwner = new System.Windows.Forms.Label();
            this.txtSearchUser = new System.Windows.Forms.TextBox();
            this.btnSearchUsers = new System.Windows.Forms.Button();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.btnChangeOwner = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.rtbLog = new System.Windows.Forms.RichTextBox();

            this.tableLayoutMain.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlows)).BeginInit();
            this.panelActions.SuspendLayout();
            this.panelChangeOwner.SuspendLayout();
            this.SuspendLayout();

            // tableLayoutMain
            this.tableLayoutMain.ColumnCount = 1;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Controls.Add(this.panelToolbar, 0, 0);
            this.tableLayoutMain.Controls.Add(this.dgvFlows, 0, 1);
            this.tableLayoutMain.Controls.Add(this.panelActions, 0, 2);
            this.tableLayoutMain.Controls.Add(this.panelChangeOwner, 0, 3);
            this.tableLayoutMain.Controls.Add(this.progressBar, 0, 4);
            this.tableLayoutMain.Controls.Add(this.rtbLog, 0, 5);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 6;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutMain.Size = new System.Drawing.Size(800, 600);
            this.tableLayoutMain.TabIndex = 0;

            // panelToolbar
            this.panelToolbar.Controls.Add(this.btnLoadFlows);
            this.panelToolbar.Controls.Add(this.cmbSolutions);
            this.panelToolbar.Controls.Add(this.lblSolution);
            this.panelToolbar.Controls.Add(this.btnLoadSolutions);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelToolbar.Location = new System.Drawing.Point(3, 3);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(794, 34);
            this.panelToolbar.TabIndex = 0;

            // btnLoadSolutions
            this.btnLoadSolutions.Location = new System.Drawing.Point(3, 5);
            this.btnLoadSolutions.Name = "btnLoadSolutions";
            this.btnLoadSolutions.Size = new System.Drawing.Size(110, 25);
            this.btnLoadSolutions.TabIndex = 0;
            this.btnLoadSolutions.Text = "Load Solutions";
            this.btnLoadSolutions.UseVisualStyleBackColor = true;
            this.btnLoadSolutions.Click += new System.EventHandler(this.btnLoadSolutions_Click);

            // lblSolution
            this.lblSolution.AutoSize = true;
            this.lblSolution.Location = new System.Drawing.Point(120, 10);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(51, 13);
            this.lblSolution.TabIndex = 1;
            this.lblSolution.Text = "Solution:";

            // cmbSolutions
            this.cmbSolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSolutions.FormattingEnabled = true;
            this.cmbSolutions.Location = new System.Drawing.Point(175, 7);
            this.cmbSolutions.Name = "cmbSolutions";
            this.cmbSolutions.Size = new System.Drawing.Size(350, 21);
            this.cmbSolutions.TabIndex = 2;

            // btnLoadFlows
            this.btnLoadFlows.Location = new System.Drawing.Point(535, 5);
            this.btnLoadFlows.Name = "btnLoadFlows";
            this.btnLoadFlows.Size = new System.Drawing.Size(110, 25);
            this.btnLoadFlows.TabIndex = 3;
            this.btnLoadFlows.Text = "Load Flows";
            this.btnLoadFlows.UseVisualStyleBackColor = true;
            this.btnLoadFlows.Click += new System.EventHandler(this.btnLoadFlows_Click);

            // dgvFlows
            this.dgvFlows.AllowUserToAddRows = false;
            this.dgvFlows.AllowUserToDeleteRows = false;
            this.dgvFlows.AllowUserToResizeRows = false;
            this.dgvFlows.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFlows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlows.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colSelect,
                this.colFlowName,
                this.colState,
                this.colOwner,
                this.colModifiedOn
            });
            this.dgvFlows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFlows.Location = new System.Drawing.Point(3, 43);
            this.dgvFlows.Name = "dgvFlows";
            this.dgvFlows.RowHeadersVisible = false;
            this.dgvFlows.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFlows.Size = new System.Drawing.Size(794, 288);
            this.dgvFlows.TabIndex = 1;

            // colSelect
            this.colSelect.FillWeight = 30F;
            this.colSelect.HeaderText = "Select";
            this.colSelect.Name = "colSelect";

            // colFlowName
            this.colFlowName.FillWeight = 150F;
            this.colFlowName.HeaderText = "Flow Name";
            this.colFlowName.Name = "colFlowName";
            this.colFlowName.ReadOnly = true;

            // colState
            this.colState.FillWeight = 80F;
            this.colState.HeaderText = "State";
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;

            // colOwner
            this.colOwner.FillWeight = 120F;
            this.colOwner.HeaderText = "Owner";
            this.colOwner.Name = "colOwner";
            this.colOwner.ReadOnly = true;

            // colModifiedOn
            this.colModifiedOn.FillWeight = 100F;
            this.colModifiedOn.HeaderText = "Modified On";
            this.colModifiedOn.Name = "colModifiedOn";
            this.colModifiedOn.ReadOnly = true;

            // panelActions
            this.panelActions.Controls.Add(this.btnDeactivateSelected);
            this.panelActions.Controls.Add(this.btnActivateSelected);
            this.panelActions.Controls.Add(this.btnDeselectAll);
            this.panelActions.Controls.Add(this.btnSelectAll);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActions.Location = new System.Drawing.Point(3, 337);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(794, 34);
            this.panelActions.TabIndex = 2;

            // btnSelectAll
            this.btnSelectAll.Location = new System.Drawing.Point(3, 5);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(90, 25);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);

            // btnDeselectAll
            this.btnDeselectAll.Location = new System.Drawing.Point(100, 5);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(90, 25);
            this.btnDeselectAll.TabIndex = 1;
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);

            // btnActivateSelected
            this.btnActivateSelected.Location = new System.Drawing.Point(200, 5);
            this.btnActivateSelected.Name = "btnActivateSelected";
            this.btnActivateSelected.Size = new System.Drawing.Size(130, 25);
            this.btnActivateSelected.TabIndex = 2;
            this.btnActivateSelected.Text = "Activate Selected";
            this.btnActivateSelected.UseVisualStyleBackColor = true;
            this.btnActivateSelected.Click += new System.EventHandler(this.btnActivateSelected_Click);

            // btnDeactivateSelected
            this.btnDeactivateSelected.Location = new System.Drawing.Point(340, 5);
            this.btnDeactivateSelected.Name = "btnDeactivateSelected";
            this.btnDeactivateSelected.Size = new System.Drawing.Size(140, 25);
            this.btnDeactivateSelected.TabIndex = 3;
            this.btnDeactivateSelected.Text = "Deactivate Selected";
            this.btnDeactivateSelected.UseVisualStyleBackColor = true;
            this.btnDeactivateSelected.Click += new System.EventHandler(this.btnDeactivateSelected_Click);

            // panelChangeOwner
            this.panelChangeOwner.Controls.Add(this.btnChangeOwner);
            this.panelChangeOwner.Controls.Add(this.cmbUsers);
            this.panelChangeOwner.Controls.Add(this.btnSearchUsers);
            this.panelChangeOwner.Controls.Add(this.txtSearchUser);
            this.panelChangeOwner.Controls.Add(this.lblNewOwner);
            this.panelChangeOwner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChangeOwner.Name = "panelChangeOwner";
            this.panelChangeOwner.TabIndex = 5;

            // lblNewOwner
            this.lblNewOwner.AutoSize = true;
            this.lblNewOwner.Location = new System.Drawing.Point(3, 10);
            this.lblNewOwner.Name = "lblNewOwner";
            this.lblNewOwner.Size = new System.Drawing.Size(66, 13);
            this.lblNewOwner.TabIndex = 0;
            this.lblNewOwner.Text = "New Owner:";

            // txtSearchUser
            this.txtSearchUser.Location = new System.Drawing.Point(75, 7);
            this.txtSearchUser.Name = "txtSearchUser";
            this.txtSearchUser.Size = new System.Drawing.Size(180, 20);
            this.txtSearchUser.TabIndex = 1;

            // btnSearchUsers
            this.btnSearchUsers.Location = new System.Drawing.Point(262, 5);
            this.btnSearchUsers.Name = "btnSearchUsers";
            this.btnSearchUsers.Size = new System.Drawing.Size(100, 25);
            this.btnSearchUsers.TabIndex = 2;
            this.btnSearchUsers.Text = "Search Users";
            this.btnSearchUsers.UseVisualStyleBackColor = true;
            this.btnSearchUsers.Click += new System.EventHandler(this.btnSearchUsers_Click);

            // cmbUsers
            this.cmbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(370, 7);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(250, 21);
            this.cmbUsers.TabIndex = 3;

            // btnChangeOwner
            this.btnChangeOwner.Location = new System.Drawing.Point(628, 5);
            this.btnChangeOwner.Name = "btnChangeOwner";
            this.btnChangeOwner.Size = new System.Drawing.Size(110, 25);
            this.btnChangeOwner.TabIndex = 4;
            this.btnChangeOwner.Text = "Change Owner";
            this.btnChangeOwner.UseVisualStyleBackColor = true;
            this.btnChangeOwner.Click += new System.EventHandler(this.btnChangeOwner_Click);

            // progressBar
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(3, 377);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(794, 24);
            this.progressBar.TabIndex = 3;

            // rtbLog
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLog.ForeColor = System.Drawing.Color.LimeGreen;
            this.rtbLog.Location = new System.Drawing.Point(3, 407);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(794, 190);
            this.rtbLog.TabIndex = 4;
            this.rtbLog.Text = "";

            // BulkPowerAutomateActivatorControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutMain);
            this.Name = "BulkPowerAutomateActivatorControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.panelToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlows)).EndInit();
            this.panelActions.ResumeLayout(false);
            this.panelChangeOwner.ResumeLayout(false);
            this.panelChangeOwner.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnLoadSolutions;
        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.ComboBox cmbSolutions;
        private System.Windows.Forms.Button btnLoadFlows;
        private System.Windows.Forms.DataGridView dgvFlows;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlowName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOwner;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModifiedOn;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Button btnActivateSelected;
        private System.Windows.Forms.Button btnDeactivateSelected;
        private System.Windows.Forms.Panel panelChangeOwner;
        private System.Windows.Forms.Label lblNewOwner;
        private System.Windows.Forms.TextBox txtSearchUser;
        private System.Windows.Forms.Button btnSearchUsers;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Button btnChangeOwner;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}
