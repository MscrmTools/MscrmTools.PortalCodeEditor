﻿namespace MscrmTools.PortalCodeEditor.Forms
{
    partial class CodeTreeViewForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeTreeViewForm));
            this.tvCodeItems = new System.Windows.Forms.TreeView();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.llCollapseAll = new System.Windows.Forms.LinkLabel();
            this.lblSeparator = new System.Windows.Forms.Label();
            this.llExpandAll = new System.Windows.Forms.LinkLabel();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.chkSearchInContent = new System.Windows.Forms.CheckBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlPendingChanges = new System.Windows.Forms.Panel();
            this.lblPendingDetails = new System.Windows.Forms.Label();
            this.llApplyChanges = new System.Windows.Forms.LinkLabel();
            this.cmsTreeview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFromPortal = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateNewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkOnlyItemsWithCode = new System.Windows.Forms.CheckBox();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlPendingChanges.SuspendLayout();
            this.cmsTreeview.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvCodeItems
            // 
            this.tvCodeItems.CheckBoxes = true;
            this.tvCodeItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCodeItems.HideSelection = false;
            this.tvCodeItems.Location = new System.Drawing.Point(0, 77);
            this.tvCodeItems.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tvCodeItems.Name = "tvCodeItems";
            this.tvCodeItems.Size = new System.Drawing.Size(437, 655);
            this.tvCodeItems.TabIndex = 9;
            this.tvCodeItems.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvCodeItems_AfterCheck);
            this.tvCodeItems.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCodeItems_AfterSelect);
            this.tvCodeItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvCodeItems_MouseDown);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.chkOnlyItemsWithCode);
            this.pnlHeader.Controls.Add(this.llCollapseAll);
            this.pnlHeader.Controls.Add(this.lblSeparator);
            this.pnlHeader.Controls.Add(this.llExpandAll);
            this.pnlHeader.Controls.Add(this.chkSelectAll);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 51);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(437, 26);
            this.pnlHeader.TabIndex = 6;
            // 
            // llCollapseAll
            // 
            this.llCollapseAll.AutoSize = true;
            this.llCollapseAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.llCollapseAll.Location = new System.Drawing.Point(252, 0);
            this.llCollapseAll.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llCollapseAll.Name = "llCollapseAll";
            this.llCollapseAll.Size = new System.Drawing.Size(89, 20);
            this.llCollapseAll.TabIndex = 91;
            this.llCollapseAll.TabStop = true;
            this.llCollapseAll.Text = "Collapse all";
            this.llCollapseAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCollapseAll_LinkClicked);
            // 
            // lblSeparator
            // 
            this.lblSeparator.AutoSize = true;
            this.lblSeparator.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSeparator.Location = new System.Drawing.Point(341, 0);
            this.lblSeparator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeparator.Name = "lblSeparator";
            this.lblSeparator.Size = new System.Drawing.Size(14, 20);
            this.lblSeparator.TabIndex = 90;
            this.lblSeparator.Text = "|";
            // 
            // llExpandAll
            // 
            this.llExpandAll.AutoSize = true;
            this.llExpandAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.llExpandAll.Location = new System.Drawing.Point(355, 0);
            this.llExpandAll.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llExpandAll.Name = "llExpandAll";
            this.llExpandAll.Size = new System.Drawing.Size(82, 20);
            this.llExpandAll.TabIndex = 89;
            this.llExpandAll.TabStop = true;
            this.llExpandAll.Text = "Expand all";
            this.llExpandAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llExpandAll_LinkClicked);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkSelectAll.Location = new System.Drawing.Point(0, 0);
            this.chkSelectAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(166, 26);
            this.chkSelectAll.TabIndex = 88;
            this.chkSelectAll.Text = "Select/Unselect all";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtSearch);
            this.pnlFooter.Controls.Add(this.chkSearchInContent);
            this.pnlFooter.Controls.Add(this.lblSearch);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 732);
            this.pnlFooter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(437, 34);
            this.pnlFooter.TabIndex = 7;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(60, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(344, 26);
            this.txtSearch.TabIndex = 95;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // chkSearchInContent
            // 
            this.chkSearchInContent.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSearchInContent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSearchInContent.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkSearchInContent.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.chkSearchInContent.FlatAppearance.CheckedBackColor = System.Drawing.Color.PowderBlue;
            this.chkSearchInContent.Image = ((System.Drawing.Image)(resources.GetObject("chkSearchInContent.Image")));
            this.chkSearchInContent.Location = new System.Drawing.Point(404, 0);
            this.chkSearchInContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSearchInContent.Name = "chkSearchInContent";
            this.chkSearchInContent.Size = new System.Drawing.Size(33, 34);
            this.chkSearchInContent.TabIndex = 94;
            this.chkSearchInContent.UseVisualStyleBackColor = true;
            this.chkSearchInContent.CheckedChanged += new System.EventHandler(this.chkSearchInContent_CheckedChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSearch.Location = new System.Drawing.Point(0, 0);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblSearch.Size = new System.Drawing.Size(60, 26);
            this.lblSearch.TabIndex = 93;
            this.lblSearch.Text = "Search";
            // 
            // pnlPendingChanges
            // 
            this.pnlPendingChanges.BackColor = System.Drawing.SystemColors.Info;
            this.pnlPendingChanges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPendingChanges.Controls.Add(this.lblPendingDetails);
            this.pnlPendingChanges.Controls.Add(this.llApplyChanges);
            this.pnlPendingChanges.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPendingChanges.Location = new System.Drawing.Point(0, 0);
            this.pnlPendingChanges.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlPendingChanges.Name = "pnlPendingChanges";
            this.pnlPendingChanges.Size = new System.Drawing.Size(437, 51);
            this.pnlPendingChanges.TabIndex = 8;
            this.pnlPendingChanges.Visible = false;
            // 
            // lblPendingDetails
            // 
            this.lblPendingDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPendingDetails.Location = new System.Drawing.Point(0, 0);
            this.lblPendingDetails.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPendingDetails.Name = "lblPendingDetails";
            this.lblPendingDetails.Size = new System.Drawing.Size(435, 20);
            this.lblPendingDetails.TabIndex = 1;
            this.lblPendingDetails.Tag = "{0} item{1} need to be pushed to the portal{2}";
            this.lblPendingDetails.Text = "{0} item(s) need to be pushed to the portal(s)";
            // 
            // llApplyChanges
            // 
            this.llApplyChanges.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.llApplyChanges.Location = new System.Drawing.Point(0, 29);
            this.llApplyChanges.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llApplyChanges.Name = "llApplyChanges";
            this.llApplyChanges.Size = new System.Drawing.Size(435, 20);
            this.llApplyChanges.TabIndex = 0;
            this.llApplyChanges.TabStop = true;
            this.llApplyChanges.Text = "Apply changes to portal";
            this.llApplyChanges.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.llApplyChanges.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llApplyChanges_LinkClicked);
            // 
            // cmsTreeview
            // 
            this.cmsTreeview.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsTreeview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiUpdate,
            this.tsmiRefreshFromPortal,
            this.tsmiCreateNewItem});
            this.cmsTreeview.Name = "cmsTreeview";
            this.cmsTreeview.Size = new System.Drawing.Size(239, 94);
            this.cmsTreeview.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTreeview_Opening);
            this.cmsTreeview.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsTreeview_ItemClicked);
            // 
            // tsmiUpdate
            // 
            this.tsmiUpdate.Name = "tsmiUpdate";
            this.tsmiUpdate.Size = new System.Drawing.Size(238, 30);
            this.tsmiUpdate.Text = "Update";
            // 
            // tsmiRefreshFromPortal
            // 
            this.tsmiRefreshFromPortal.Name = "tsmiRefreshFromPortal";
            this.tsmiRefreshFromPortal.Size = new System.Drawing.Size(238, 30);
            this.tsmiRefreshFromPortal.Text = "Refresh from portal";
            // 
            // tsmiCreateNewItem
            // 
            this.tsmiCreateNewItem.Name = "tsmiCreateNewItem";
            this.tsmiCreateNewItem.Size = new System.Drawing.Size(238, 30);
            this.tsmiCreateNewItem.Tag = "Create new {0}";
            this.tsmiCreateNewItem.Text = "Create new {0}";
            // 
            // chkOnlyItemsWithCode
            // 
            this.chkOnlyItemsWithCode.AutoSize = true;
            this.chkOnlyItemsWithCode.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkOnlyItemsWithCode.Location = new System.Drawing.Point(166, 0);
            this.chkOnlyItemsWithCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkOnlyItemsWithCode.Name = "chkOnlyItemsWithCode";
            this.chkOnlyItemsWithCode.Size = new System.Drawing.Size(179, 26);
            this.chkOnlyItemsWithCode.TabIndex = 92;
            this.chkOnlyItemsWithCode.Text = "Only items with code";
            this.chkOnlyItemsWithCode.UseVisualStyleBackColor = true;
            this.chkOnlyItemsWithCode.CheckedChanged += new System.EventHandler(this.chkOnlyItemsWithCode_CheckedChanged);
            // 
            // CodeTreeViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 766);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.tvCodeItems);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlPendingChanges);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "CodeTreeViewForm";
            this.ShowIcon = false;
            this.TabText = "Code Items";
            this.Text = "CodeTreeViewForm";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlPendingChanges.ResumeLayout(false);
            this.cmsTreeview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvCodeItems;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.LinkLabel llCollapseAll;
        private System.Windows.Forms.Label lblSeparator;
        private System.Windows.Forms.LinkLabel llExpandAll;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkSearchInContent;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Panel pnlPendingChanges;
        private System.Windows.Forms.Label lblPendingDetails;
        private System.Windows.Forms.LinkLabel llApplyChanges;
        private System.Windows.Forms.ContextMenuStrip cmsTreeview;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdate;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFromPortal;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateNewItem;
        private System.Windows.Forms.CheckBox chkOnlyItemsWithCode;
    }
}