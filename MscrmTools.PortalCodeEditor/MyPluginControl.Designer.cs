using MscrmTools.PortalCodeEditor.AppCode.EventArgs;

namespace MscrmTools.PortalCodeEditor
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPluginControl));
            this.tsMainMenu = new System.Windows.Forms.ToolStrip();
            this.tsbLoadItems = new System.Windows.Forms.ToolStripButton();
            this.tsbCredits = new System.Windows.Forms.ToolStripButton();
            this.tsbUpdateCheckedItems = new System.Windows.Forms.ToolStripButton();
            this.tsbExportPortalItems = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSettings = new System.Windows.Forms.ToolStripButton();
            this.cmsTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeThisTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllTabsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colseAllButThisTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dpMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.tsMainMenu.SuspendLayout();
            this.cmsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMainMenu
            // 
            this.tsMainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoadItems,
            this.tsbCredits,
            this.tsbUpdateCheckedItems,
            this.tsbExportPortalItems,
            this.toolStripSeparator2,
            this.tsbSettings});
            this.tsMainMenu.Location = new System.Drawing.Point(0, 0);
            this.tsMainMenu.Name = "tsMainMenu";
            this.tsMainMenu.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.tsMainMenu.Size = new System.Drawing.Size(1375, 37);
            this.tsMainMenu.TabIndex = 1;
            this.tsMainMenu.Text = "tsMain";
            // 
            // tsbLoadItems
            // 
            this.tsbLoadItems.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadItems.Image")));
            this.tsbLoadItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadItems.Name = "tsbLoadItems";
            this.tsbLoadItems.Size = new System.Drawing.Size(146, 34);
            this.tsbLoadItems.Text = "Load items";
            this.tsbLoadItems.Click += new System.EventHandler(this.tsbLoadItems_Click);
            // 
            // tsbCredits
            // 
            this.tsbCredits.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbCredits.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCredits.Image = ((System.Drawing.Image)(resources.GetObject("tsbCredits.Image")));
            this.tsbCredits.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCredits.Name = "tsbCredits";
            this.tsbCredits.Size = new System.Drawing.Size(81, 34);
            this.tsbCredits.Text = "Credits";
            this.tsbCredits.Click += new System.EventHandler(this.tsbCredits_Click);
            // 
            // tsbUpdateCheckedItems
            // 
            this.tsbUpdateCheckedItems.Image = ((System.Drawing.Image)(resources.GetObject("tsbUpdateCheckedItems.Image")));
            this.tsbUpdateCheckedItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpdateCheckedItems.Name = "tsbUpdateCheckedItems";
            this.tsbUpdateCheckedItems.Size = new System.Drawing.Size(250, 34);
            this.tsbUpdateCheckedItems.Text = "Update checked items";
            this.tsbUpdateCheckedItems.Click += new System.EventHandler(this.tsbUpdateCheckedItems_Click);
            // 
            // tsbExportPortalItems
            // 
            this.tsbExportPortalItems.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportPortalItems.Image")));
            this.tsbExportPortalItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportPortalItems.Name = "tsbExportPortalItems";
            this.tsbExportPortalItems.Size = new System.Drawing.Size(220, 34);
            this.tsbExportPortalItems.Text = "Export Portal Items";
            this.tsbExportPortalItems.Click += new System.EventHandler(this.tsbExportPortalItems_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbSettings
            // 
            this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(119, 34);
            this.tsbSettings.Text = "Settings";
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // cmsTab
            // 
            this.cmsTab.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeThisTabToolStripMenuItem,
            this.closeAllTabsToolStripMenuItem,
            this.colseAllButThisTabToolStripMenuItem});
            this.cmsTab.Name = "cmsTab";
            this.cmsTab.Size = new System.Drawing.Size(276, 106);
            // 
            // closeThisTabToolStripMenuItem
            // 
            this.closeThisTabToolStripMenuItem.Name = "closeThisTabToolStripMenuItem";
            this.closeThisTabToolStripMenuItem.Size = new System.Drawing.Size(275, 34);
            this.closeThisTabToolStripMenuItem.Text = "Close this tab";
            this.closeThisTabToolStripMenuItem.Click += new System.EventHandler(this.closeThisTabToolStripMenuItem_Click);
            // 
            // closeAllTabsToolStripMenuItem
            // 
            this.closeAllTabsToolStripMenuItem.Name = "closeAllTabsToolStripMenuItem";
            this.closeAllTabsToolStripMenuItem.Size = new System.Drawing.Size(275, 34);
            this.closeAllTabsToolStripMenuItem.Text = "Close all tabs";
            this.closeAllTabsToolStripMenuItem.Click += new System.EventHandler(this.closeAllTabsToolStripMenuItem_Click);
            // 
            // colseAllButThisTabToolStripMenuItem
            // 
            this.colseAllButThisTabToolStripMenuItem.Name = "colseAllButThisTabToolStripMenuItem";
            this.colseAllButThisTabToolStripMenuItem.Size = new System.Drawing.Size(275, 34);
            this.colseAllButThisTabToolStripMenuItem.Text = "Colse all but this tab";
            this.colseAllButThisTabToolStripMenuItem.Click += new System.EventHandler(this.closeAllButThisTabToolStripMenuItem_Click);
            // 
            // dpMain
            // 
            this.dpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpMain.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dpMain.Location = new System.Drawing.Point(0, 37);
            this.dpMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dpMain.Name = "dpMain";
            this.dpMain.Size = new System.Drawing.Size(1375, 1015);
            this.dpMain.TabIndex = 5;
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dpMain);
            this.Controls.Add(this.tsMainMenu);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1375, 1052);
            this.OnCloseTool += new System.EventHandler(this.MyPluginControl_OnCloseTool);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.tsMainMenu.ResumeLayout(false);
            this.tsMainMenu.PerformLayout();
            this.cmsTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip tsMainMenu;
        private System.Windows.Forms.ToolStripButton tsbLoadItems;
        private System.Windows.Forms.ToolStripButton tsbCredits;
        private System.Windows.Forms.ToolStripButton tsbUpdateCheckedItems;
        private System.Windows.Forms.ContextMenuStrip cmsTab;
        private System.Windows.Forms.ToolStripMenuItem closeThisTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllTabsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colseAllButThisTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbSettings;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpMain;
        private System.Windows.Forms.ToolStripButton tsbExportPortalItems;
    }
}
