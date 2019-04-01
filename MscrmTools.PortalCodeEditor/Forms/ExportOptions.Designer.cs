namespace MscrmTools.PortalCodeEditor.Forms
{
    partial class ExportOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelExportOptions = new System.Windows.Forms.Panel();
            this.labelExportOptions = new System.Windows.Forms.Label();
            this.panelExportOptionsButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.pnlSearchOptions = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.chkSearchInContent = new System.Windows.Forms.CheckBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.panelExportFolder = new System.Windows.Forms.Panel();
            this.lnkChooseExportFolder = new System.Windows.Forms.LinkLabel();
            this.fldrBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.chkClearItems = new System.Windows.Forms.CheckBox();
            this.txtExportFolder = new System.Windows.Forms.TextBox();
            this.labelExportOptsDetails = new System.Windows.Forms.Label();
            this.panelExportOptions.SuspendLayout();
            this.panelExportOptionsButtons.SuspendLayout();
            this.pnlSearchOptions.SuspendLayout();
            this.panelExportFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExportOptions
            // 
            this.panelExportOptions.BackColor = System.Drawing.Color.White;
            this.panelExportOptions.Controls.Add(this.labelExportOptsDetails);
            this.panelExportOptions.Controls.Add(this.labelExportOptions);
            this.panelExportOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExportOptions.Location = new System.Drawing.Point(0, 0);
            this.panelExportOptions.Margin = new System.Windows.Forms.Padding(6);
            this.panelExportOptions.Name = "panelExportOptions";
            this.panelExportOptions.Size = new System.Drawing.Size(800, 122);
            this.panelExportOptions.TabIndex = 11;
            // 
            // labelExportOptions
            // 
            this.labelExportOptions.AutoSize = true;
            this.labelExportOptions.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExportOptions.Location = new System.Drawing.Point(22, 6);
            this.labelExportOptions.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelExportOptions.Name = "labelExportOptions";
            this.labelExportOptions.Size = new System.Drawing.Size(376, 45);
            this.labelExportOptions.TabIndex = 0;
            this.labelExportOptions.Text = "Export Portal Code Items";
            // 
            // panelExportOptionsButtons
            // 
            this.panelExportOptionsButtons.Controls.Add(this.btnCancel);
            this.panelExportOptionsButtons.Controls.Add(this.btnValidate);
            this.panelExportOptionsButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelExportOptionsButtons.Location = new System.Drawing.Point(0, 360);
            this.panelExportOptionsButtons.Margin = new System.Windows.Forms.Padding(4);
            this.panelExportOptionsButtons.Name = "panelExportOptionsButtons";
            this.panelExportOptionsButtons.Size = new System.Drawing.Size(800, 74);
            this.panelExportOptionsButtons.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(650, 11);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 42);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidate.Enabled = false;
            this.btnValidate.Location = new System.Drawing.Point(498, 11);
            this.btnValidate.Margin = new System.Windows.Forms.Padding(6);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(138, 42);
            this.btnValidate.TabIndex = 1;
            this.btnValidate.Text = "OK";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // pnlSearchOptions
            // 
            this.pnlSearchOptions.Controls.Add(this.txtSearch);
            this.pnlSearchOptions.Controls.Add(this.lblSearch);
            this.pnlSearchOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchOptions.Location = new System.Drawing.Point(0, 216);
            this.pnlSearchOptions.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.pnlSearchOptions.Name = "pnlSearchOptions";
            this.pnlSearchOptions.Size = new System.Drawing.Size(800, 38);
            this.pnlSearchOptions.TabIndex = 17;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Location = new System.Drawing.Point(144, 5);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(643, 29);
            this.txtSearch.TabIndex = 95;
            // 
            // chkSearchInContent
            // 
            this.chkSearchInContent.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.chkSearchInContent.FlatAppearance.CheckedBackColor = System.Drawing.Color.PowderBlue;
            this.chkSearchInContent.Location = new System.Drawing.Point(14, 260);
            this.chkSearchInContent.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chkSearchInContent.Name = "chkSearchInContent";
            this.chkSearchInContent.Size = new System.Drawing.Size(269, 50);
            this.chkSearchInContent.TabIndex = 94;
            this.chkSearchInContent.Text = "Search In Content";
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSearch.Location = new System.Drawing.Point(0, 0);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(4, 0, 7, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.lblSearch.Size = new System.Drawing.Size(133, 38);
            this.lblSearch.TabIndex = 93;
            this.lblSearch.Text = "Search Text";
            // 
            // panelExportFolder
            // 
            this.panelExportFolder.Controls.Add(this.txtExportFolder);
            this.panelExportFolder.Controls.Add(this.lnkChooseExportFolder);
            this.panelExportFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExportFolder.Location = new System.Drawing.Point(0, 122);
            this.panelExportFolder.Name = "panelExportFolder";
            this.panelExportFolder.Size = new System.Drawing.Size(800, 94);
            this.panelExportFolder.TabIndex = 18;
            // 
            // lnkChooseExportFolder
            // 
            this.lnkChooseExportFolder.AutoSize = true;
            this.lnkChooseExportFolder.Location = new System.Drawing.Point(14, 20);
            this.lnkChooseExportFolder.Name = "lnkChooseExportFolder";
            this.lnkChooseExportFolder.Size = new System.Drawing.Size(202, 25);
            this.lnkChooseExportFolder.TabIndex = 1;
            this.lnkChooseExportFolder.TabStop = true;
            this.lnkChooseExportFolder.Text = "Choose Export Folder";
            this.lnkChooseExportFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChooseExportFolder_LinkClicked);
            // 
            // chkClearItems
            // 
            this.chkClearItems.Location = new System.Drawing.Point(14, 303);
            this.chkClearItems.Name = "chkClearItems";
            this.chkClearItems.Size = new System.Drawing.Size(355, 50);
            this.chkClearItems.TabIndex = 19;
            this.chkClearItems.Text = "Clear Export Folder before Export?";
            this.chkClearItems.UseVisualStyleBackColor = true;
            // 
            // txtExportFolder
            // 
            this.txtExportFolder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtExportFolder.Location = new System.Drawing.Point(223, 20);
            this.txtExportFolder.Multiline = true;
            this.txtExportFolder.Name = "txtExportFolder";
            this.txtExportFolder.ReadOnly = true;
            this.txtExportFolder.Size = new System.Drawing.Size(550, 65);
            this.txtExportFolder.TabIndex = 2;
            // 
            // labelExportOptsDetails
            // 
            this.labelExportOptsDetails.Location = new System.Drawing.Point(30, 59);
            this.labelExportOptsDetails.Name = "labelExportOptsDetails";
            this.labelExportOptsDetails.Size = new System.Drawing.Size(723, 53);
            this.labelExportOptsDetails.TabIndex = 1;
            this.labelExportOptsDetails.Text = "The export folder structure will match the structure in the treeview.  Only items" +
    " with Content will be exported.";
            // 
            // ExportOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 434);
            this.Controls.Add(this.chkSearchInContent);
            this.Controls.Add(this.chkClearItems);
            this.Controls.Add(this.pnlSearchOptions);
            this.Controls.Add(this.panelExportOptionsButtons);
            this.Controls.Add(this.panelExportFolder);
            this.Controls.Add(this.panelExportOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ExportOptions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panelExportOptions.ResumeLayout(false);
            this.panelExportOptions.PerformLayout();
            this.panelExportOptionsButtons.ResumeLayout(false);
            this.pnlSearchOptions.ResumeLayout(false);
            this.pnlSearchOptions.PerformLayout();
            this.panelExportFolder.ResumeLayout(false);
            this.panelExportFolder.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelExportOptions;
        private System.Windows.Forms.Label labelExportOptions;
        private System.Windows.Forms.Panel panelExportOptionsButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Panel pnlSearchOptions;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkSearchInContent;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Panel panelExportFolder;
        private System.Windows.Forms.LinkLabel lnkChooseExportFolder;
        private System.Windows.Forms.FolderBrowserDialog fldrBrowser;
        private System.Windows.Forms.CheckBox chkClearItems;
        private System.Windows.Forms.TextBox txtExportFolder;
        private System.Windows.Forms.Label labelExportOptsDetails;
    }
}