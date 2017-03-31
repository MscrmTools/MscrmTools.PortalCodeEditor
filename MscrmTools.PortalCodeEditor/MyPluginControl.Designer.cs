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
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLoadItems = new System.Windows.Forms.ToolStripButton();
            this.tsbCredits = new System.Windows.Forms.ToolStripButton();
            this.tsbUpdateCheckedItems = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSettings = new System.Windows.Forms.ToolStripButton();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.ctv = new MscrmTools.PortalCodeEditor.Controls.CodeTreeView();
            this.tcCodeContents = new MscrmTools.PortalCodeEditor.Controls.CustomTabControl();
            this.tsCodeContent = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparatorEdit = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbEdit = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiFind = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGoToLine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorMinifyJS = new System.Windows.Forms.ToolStripSeparator();
            this.tsbMinifyJS = new System.Windows.Forms.ToolStripButton();
            this.tsbBeautify = new System.Windows.Forms.ToolStripButton();
            this.tsbDoFolding = new System.Windows.Forms.ToolStripButton();
            this.tsbGetLatestVersion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tslResourceName = new System.Windows.Forms.ToolStripLabel();
            this.tsbComment = new System.Windows.Forms.ToolStripButton();
            this.tsbnUncomment = new System.Windows.Forms.ToolStripButton();
            this.pnlContentFooter = new System.Windows.Forms.Panel();
            this.lblItemSelected = new System.Windows.Forms.Label();
            this.cmsTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeThisTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllTabsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colseAllButThisTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.tsCodeContent.SuspendLayout();
            this.pnlContentFooter.SuspendLayout();
            this.cmsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMainMenu
            // 
            this.tsMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.toolStripSeparator1,
            this.tsbLoadItems,
            this.tsbCredits,
            this.tsbUpdateCheckedItems,
            this.toolStripSeparator2,
            this.tsbSettings});
            this.tsMainMenu.Location = new System.Drawing.Point(0, 0);
            this.tsMainMenu.Name = "tsMainMenu";
            this.tsMainMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.tsMainMenu.Size = new System.Drawing.Size(1126, 32);
            this.tsMainMenu.TabIndex = 1;
            this.tsMainMenu.Text = "tsMain";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(28, 29);
            this.tsbClose.Text = "Close";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbLoadItems
            // 
            this.tsbLoadItems.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadItems.Image")));
            this.tsbLoadItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadItems.Name = "tsbLoadItems";
            this.tsbLoadItems.Size = new System.Drawing.Size(127, 29);
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
            this.tsbCredits.Size = new System.Drawing.Size(71, 29);
            this.tsbCredits.Text = "Credits";
            this.tsbCredits.Click += new System.EventHandler(this.tsbCredits_Click);
            // 
            // tsbUpdateCheckedItems
            // 
            this.tsbUpdateCheckedItems.Image = ((System.Drawing.Image)(resources.GetObject("tsbUpdateCheckedItems.Image")));
            this.tsbUpdateCheckedItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpdateCheckedItems.Name = "tsbUpdateCheckedItems";
            this.tsbUpdateCheckedItems.Size = new System.Drawing.Size(215, 29);
            this.tsbUpdateCheckedItems.Text = "Update checked items";
            this.tsbUpdateCheckedItems.Click += new System.EventHandler(this.tsbUpdateCheckedItems_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbSettings
            // 
            this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(104, 29);
            this.tsbSettings.Text = "Settings";
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 32);
            this.scMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.ctv);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.tcCodeContents);
            this.scMain.Panel2.Controls.Add(this.tsCodeContent);
            this.scMain.Panel2.Controls.Add(this.pnlContentFooter);
            this.scMain.Size = new System.Drawing.Size(1126, 845);
            this.scMain.SplitterDistance = 378;
            this.scMain.SplitterWidth = 6;
            this.scMain.TabIndex = 2;
            // 
            // ctv
            // 
            this.ctv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctv.Location = new System.Drawing.Point(0, 0);
            this.ctv.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.ctv.Name = "ctv";
            this.ctv.Size = new System.Drawing.Size(378, 845);
            this.ctv.TabIndex = 0;
            this.ctv.PortalItemSelected += new System.EventHandler<MscrmTools.PortalCodeEditor.AppCode.EventArgs.PortalItemSelectedEventArgs>(this.ctv_PortalItemSelected);
            this.ctv.ActionRequested += new System.EventHandler(this.ctv_ActionRequested);
            // 
            // tcCodeContents
            // 
            this.tcCodeContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCodeContents.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tcCodeContents.Location = new System.Drawing.Point(0, 38);
            this.tcCodeContents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tcCodeContents.Name = "tcCodeContents";
            this.tcCodeContents.SelectedIndex = 0;
            this.tcCodeContents.Size = new System.Drawing.Size(742, 773);
            this.tcCodeContents.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcCodeContents.TabIndex = 5;
            this.tcCodeContents.SelectedIndexChanged += new System.EventHandler(this.tcCodeContents_SelectedIndexChanged);
            this.tcCodeContents.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tcCodeContents_MouseClick);
            // 
            // tsCodeContent
            // 
            this.tsCodeContent.AutoSize = false;
            this.tsCodeContent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.tsSeparatorEdit,
            this.tsddbEdit,
            this.toolStripSeparatorMinifyJS,
            this.tsbMinifyJS,
            this.tsbBeautify,
            this.tsbDoFolding,
            this.tsbGetLatestVersion,
            this.toolStripSeparator10,
            this.tslResourceName,
            this.tsbComment,
            this.tsbnUncomment});
            this.tsCodeContent.Location = new System.Drawing.Point(0, 0);
            this.tsCodeContent.Name = "tsCodeContent";
            this.tsCodeContent.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.tsCodeContent.Size = new System.Drawing.Size(742, 38);
            this.tsCodeContent.TabIndex = 4;
            this.tsCodeContent.Text = "toolStripScriptContent";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSave,
            this.tsmiUpdate});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(56, 35);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // tsmiSave
            // 
            this.tsmiSave.Enabled = false;
            this.tsmiSave.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSave.Image")));
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSave.Size = new System.Drawing.Size(218, 30);
            this.tsmiSave.Text = "Save";
            this.tsmiSave.ToolTipText = "Save this content in memory. This does not update the portal record of the connec" +
    "ted organization";
            this.tsmiSave.Click += new System.EventHandler(this.fileMenuSave_Click);
            // 
            // tsmiUpdate
            // 
            this.tsmiUpdate.Enabled = false;
            this.tsmiUpdate.Image = ((System.Drawing.Image)(resources.GetObject("tsmiUpdate.Image")));
            this.tsmiUpdate.Name = "tsmiUpdate";
            this.tsmiUpdate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.tsmiUpdate.Size = new System.Drawing.Size(218, 30);
            this.tsmiUpdate.Text = "Update";
            this.tsmiUpdate.ToolTipText = "Update the portal record of the connected organization";
            this.tsmiUpdate.Click += new System.EventHandler(this.fileMenuUpdate_Click);
            // 
            // tsSeparatorEdit
            // 
            this.tsSeparatorEdit.Name = "tsSeparatorEdit";
            this.tsSeparatorEdit.Size = new System.Drawing.Size(6, 38);
            // 
            // tsddbEdit
            // 
            this.tsddbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFind,
            this.tsmiReplace,
            this.tsmiGoToLine});
            this.tsddbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsddbEdit.Image")));
            this.tsddbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbEdit.Name = "tsddbEdit";
            this.tsddbEdit.Size = new System.Drawing.Size(60, 35);
            this.tsddbEdit.Text = "Edit";
            // 
            // tsmiFind
            // 
            this.tsmiFind.Image = ((System.Drawing.Image)(resources.GetObject("tsmiFind.Image")));
            this.tsmiFind.Name = "tsmiFind";
            this.tsmiFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.tsmiFind.Size = new System.Drawing.Size(242, 30);
            this.tsmiFind.Text = "Find";
            this.tsmiFind.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // tsmiReplace
            // 
            this.tsmiReplace.Image = ((System.Drawing.Image)(resources.GetObject("tsmiReplace.Image")));
            this.tsmiReplace.Name = "tsmiReplace";
            this.tsmiReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.tsmiReplace.Size = new System.Drawing.Size(242, 30);
            this.tsmiReplace.Text = "Replace";
            this.tsmiReplace.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // tsmiGoToLine
            // 
            this.tsmiGoToLine.Name = "tsmiGoToLine";
            this.tsmiGoToLine.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.tsmiGoToLine.Size = new System.Drawing.Size(242, 30);
            this.tsmiGoToLine.Text = "Go To Line";
            this.tsmiGoToLine.Click += new System.EventHandler(this.goToLineToolStripMenuItem_Click);
            // 
            // toolStripSeparatorMinifyJS
            // 
            this.toolStripSeparatorMinifyJS.Name = "toolStripSeparatorMinifyJS";
            this.toolStripSeparatorMinifyJS.Size = new System.Drawing.Size(6, 38);
            this.toolStripSeparatorMinifyJS.Visible = false;
            // 
            // tsbMinifyJS
            // 
            this.tsbMinifyJS.Image = ((System.Drawing.Image)(resources.GetObject("tsbMinifyJS.Image")));
            this.tsbMinifyJS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMinifyJS.Name = "tsbMinifyJS";
            this.tsbMinifyJS.Size = new System.Drawing.Size(120, 35);
            this.tsbMinifyJS.Text = "Compress";
            this.tsbMinifyJS.ToolTipText = "This feature compress/minify a script web resource. It does not obfuscate the cod" +
    "e, just remove useless formatting.\r\nBe careful when using this feature! There is" +
    " no way to beautify minified JavaScript";
            this.tsbMinifyJS.Visible = false;
            this.tsbMinifyJS.Click += new System.EventHandler(this.tsbMinifyJS_Click);
            // 
            // tsbBeautify
            // 
            this.tsbBeautify.Image = ((System.Drawing.Image)(resources.GetObject("tsbBeautify.Image")));
            this.tsbBeautify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBeautify.Name = "tsbBeautify";
            this.tsbBeautify.Size = new System.Drawing.Size(103, 35);
            this.tsbBeautify.Text = "Beautify";
            this.tsbBeautify.ToolTipText = "This feature make uglified JavaScript readable \r\n\r\nThanks to ghost6991 for his wo" +
    "rk on the beautifier in C# : https://github.com/ghost6991/Jsbeautifier";
            this.tsbBeautify.Visible = false;
            this.tsbBeautify.Click += new System.EventHandler(this.tsbBeautify_Click);
            // 
            // tsbDoFolding
            // 
            this.tsbDoFolding.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDoFolding.Image = ((System.Drawing.Image)(resources.GetObject("tsbDoFolding.Image")));
            this.tsbDoFolding.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDoFolding.Name = "tsbDoFolding";
            this.tsbDoFolding.Size = new System.Drawing.Size(133, 35);
            this.tsbDoFolding.Text = "Enable Folding";
            this.tsbDoFolding.Click += new System.EventHandler(this.tsbDoFolding_Click);
            // 
            // tsbGetLatestVersion
            // 
            this.tsbGetLatestVersion.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetLatestVersion.Image")));
            this.tsbGetLatestVersion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGetLatestVersion.Name = "tsbGetLatestVersion";
            this.tsbGetLatestVersion.Size = new System.Drawing.Size(118, 35);
            this.tsbGetLatestVersion.Text = "Get Latest";
            this.tsbGetLatestVersion.Click += new System.EventHandler(this.tsbGetLatestVersion_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 38);
            // 
            // tslResourceName
            // 
            this.tslResourceName.Name = "tslResourceName";
            this.tslResourceName.Size = new System.Drawing.Size(0, 35);
            this.tslResourceName.Visible = false;
            // 
            // tsbComment
            // 
            this.tsbComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbComment.Image = ((System.Drawing.Image)(resources.GetObject("tsbComment.Image")));
            this.tsbComment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbComment.Name = "tsbComment";
            this.tsbComment.Size = new System.Drawing.Size(28, 35);
            this.tsbComment.Text = "Comment";
            this.tsbComment.Click += new System.EventHandler(this.tsbComment_Click);
            // 
            // tsbnUncomment
            // 
            this.tsbnUncomment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbnUncomment.Image = ((System.Drawing.Image)(resources.GetObject("tsbnUncomment.Image")));
            this.tsbnUncomment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbnUncomment.Name = "tsbnUncomment";
            this.tsbnUncomment.Size = new System.Drawing.Size(28, 35);
            this.tsbnUncomment.Text = "Uncomment";
            this.tsbnUncomment.Click += new System.EventHandler(this.tsbnUncomment_Click);
            // 
            // pnlContentFooter
            // 
            this.pnlContentFooter.Controls.Add(this.lblItemSelected);
            this.pnlContentFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlContentFooter.Location = new System.Drawing.Point(0, 811);
            this.pnlContentFooter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlContentFooter.Name = "pnlContentFooter";
            this.pnlContentFooter.Size = new System.Drawing.Size(742, 34);
            this.pnlContentFooter.TabIndex = 1;
            // 
            // lblItemSelected
            // 
            this.lblItemSelected.AutoSize = true;
            this.lblItemSelected.Location = new System.Drawing.Point(6, 6);
            this.lblItemSelected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblItemSelected.Name = "lblItemSelected";
            this.lblItemSelected.Size = new System.Drawing.Size(0, 20);
            this.lblItemSelected.TabIndex = 0;
            // 
            // cmsTab
            // 
            this.cmsTab.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeThisTabToolStripMenuItem,
            this.closeAllTabsToolStripMenuItem,
            this.colseAllButThisTabToolStripMenuItem});
            this.cmsTab.Name = "cmsTab";
            this.cmsTab.Size = new System.Drawing.Size(259, 94);
            // 
            // closeThisTabToolStripMenuItem
            // 
            this.closeThisTabToolStripMenuItem.Name = "closeThisTabToolStripMenuItem";
            this.closeThisTabToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.closeThisTabToolStripMenuItem.Text = "Close this tab";
            this.closeThisTabToolStripMenuItem.Click += new System.EventHandler(this.closeThisTabToolStripMenuItem_Click);
            // 
            // closeAllTabsToolStripMenuItem
            // 
            this.closeAllTabsToolStripMenuItem.Name = "closeAllTabsToolStripMenuItem";
            this.closeAllTabsToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.closeAllTabsToolStripMenuItem.Text = "Close all tabs";
            this.closeAllTabsToolStripMenuItem.Click += new System.EventHandler(this.closeAllTabsToolStripMenuItem_Click);
            // 
            // colseAllButThisTabToolStripMenuItem
            // 
            this.colseAllButThisTabToolStripMenuItem.Name = "colseAllButThisTabToolStripMenuItem";
            this.colseAllButThisTabToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.colseAllButThisTabToolStripMenuItem.Text = "Colse all but this tab";
            this.colseAllButThisTabToolStripMenuItem.Click += new System.EventHandler(this.colseAllButThisTabToolStripMenuItem_Click);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.tsMainMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1126, 877);
            this.OnCloseTool += new System.EventHandler(this.MyPluginControl_OnCloseTool);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.tsMainMenu.ResumeLayout(false);
            this.tsMainMenu.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.tsCodeContent.ResumeLayout(false);
            this.tsCodeContent.PerformLayout();
            this.pnlContentFooter.ResumeLayout(false);
            this.pnlContentFooter.PerformLayout();
            this.cmsTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.CodeTreeView ctv;
        private System.Windows.Forms.ToolStrip tsMainMenu;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbLoadItems;
        private System.Windows.Forms.Panel pnlContentFooter;
        private System.Windows.Forms.Label lblItemSelected;
        private System.Windows.Forms.ToolStripButton tsbCredits;
        private System.Windows.Forms.ToolStripButton tsbUpdateCheckedItems;
        private Controls.CustomTabControl tcCodeContents;
        private System.Windows.Forms.ToolStrip tsCodeContent;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiUpdate;
        private System.Windows.Forms.ToolStripSeparator tsSeparatorEdit;
        private System.Windows.Forms.ToolStripDropDownButton tsddbEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiFind;
        private System.Windows.Forms.ToolStripMenuItem tsmiReplace;
        private System.Windows.Forms.ToolStripMenuItem tsmiGoToLine;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorMinifyJS;
        private System.Windows.Forms.ToolStripButton tsbMinifyJS;
        private System.Windows.Forms.ToolStripButton tsbBeautify;
        private System.Windows.Forms.ToolStripButton tsbDoFolding;
        private System.Windows.Forms.ToolStripButton tsbGetLatestVersion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripLabel tslResourceName;
        private System.Windows.Forms.ToolStripButton tsbComment;
        private System.Windows.Forms.ToolStripButton tsbnUncomment;
        private System.Windows.Forms.ContextMenuStrip cmsTab;
        private System.Windows.Forms.ToolStripMenuItem closeThisTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllTabsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colseAllButThisTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbSettings;
    }
}
