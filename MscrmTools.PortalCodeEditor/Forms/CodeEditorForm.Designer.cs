namespace MscrmTools.PortalCodeEditor.Forms
{
    partial class CodeEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeEditorForm));
            this.tsCodeContent = new System.Windows.Forms.ToolStrip();
            this.tsddbFile = new System.Windows.Forms.ToolStripDropDownButton();
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
            this.tsbGetLatestVersion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tslResourceName = new System.Windows.Forms.ToolStripLabel();
            this.tsbComment = new System.Windows.Forms.ToolStripButton();
            this.tsbnUncomment = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslName = new System.Windows.Forms.ToolStripLabel();
            this.scintilla = new ScintillaNET.Scintilla();
            this.tsCodeContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsCodeContent
            // 
            this.tsCodeContent.AutoSize = false;
            this.tsCodeContent.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsCodeContent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbFile,
            this.tsSeparatorEdit,
            this.tsddbEdit,
            this.toolStripSeparatorMinifyJS,
            this.tsbMinifyJS,
            this.tsbBeautify,
            this.tsbGetLatestVersion,
            this.toolStripSeparator10,
            this.tslResourceName,
            this.tsbComment,
            this.tsbnUncomment,
            this.toolStripSeparator1,
            this.tslName});
            this.tsCodeContent.Location = new System.Drawing.Point(0, 0);
            this.tsCodeContent.Name = "tsCodeContent";
            this.tsCodeContent.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.tsCodeContent.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsCodeContent.Size = new System.Drawing.Size(1156, 46);
            this.tsCodeContent.TabIndex = 5;
            this.tsCodeContent.Text = "toolStripScriptContent";
            this.tsCodeContent.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsCodeContent_ItemClicked);
            // 
            // tsddbFile
            // 
            this.tsddbFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSave,
            this.tsmiUpdate});
            this.tsddbFile.Image = ((System.Drawing.Image)(resources.GetObject("tsddbFile.Image")));
            this.tsddbFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbFile.Name = "tsddbFile";
            this.tsddbFile.Size = new System.Drawing.Size(65, 43);
            this.tsddbFile.Text = "File";
            this.tsddbFile.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsddbFile_DropDownItemClicked);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Enabled = false;
            this.tsmiSave.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSave.Image")));
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.Size = new System.Drawing.Size(249, 34);
            this.tsmiSave.Text = "Save (Ctrl+S)";
            this.tsmiSave.ToolTipText = "Save this content in memory. This does not update the portal record of the connec" +
    "ted organization";
            // 
            // tsmiUpdate
            // 
            this.tsmiUpdate.Enabled = false;
            this.tsmiUpdate.Image = ((System.Drawing.Image)(resources.GetObject("tsmiUpdate.Image")));
            this.tsmiUpdate.Name = "tsmiUpdate";
            this.tsmiUpdate.Size = new System.Drawing.Size(249, 34);
            this.tsmiUpdate.Text = "Update (Ctrl+U)";
            this.tsmiUpdate.ToolTipText = "Update the portal record of the connected organization";
            // 
            // tsSeparatorEdit
            // 
            this.tsSeparatorEdit.Name = "tsSeparatorEdit";
            this.tsSeparatorEdit.Size = new System.Drawing.Size(6, 46);
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
            this.tsddbEdit.Size = new System.Drawing.Size(69, 43);
            this.tsddbEdit.Text = "Edit";
            this.tsddbEdit.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsddbEdit_DropDownItemClicked);
            // 
            // tsmiFind
            // 
            this.tsmiFind.Image = ((System.Drawing.Image)(resources.GetObject("tsmiFind.Image")));
            this.tsmiFind.Name = "tsmiFind";
            this.tsmiFind.Size = new System.Drawing.Size(279, 34);
            this.tsmiFind.Text = "Find (Ctrl+F)";
            // 
            // tsmiReplace
            // 
            this.tsmiReplace.Image = ((System.Drawing.Image)(resources.GetObject("tsmiReplace.Image")));
            this.tsmiReplace.Name = "tsmiReplace";
            this.tsmiReplace.Size = new System.Drawing.Size(279, 34);
            this.tsmiReplace.Text = "Replace (Ctrl+H)";
            // 
            // tsmiGoToLine
            // 
            this.tsmiGoToLine.Name = "tsmiGoToLine";
            this.tsmiGoToLine.Size = new System.Drawing.Size(279, 34);
            this.tsmiGoToLine.Text = "Go To Line (Ctrl+G)";
            // 
            // toolStripSeparatorMinifyJS
            // 
            this.toolStripSeparatorMinifyJS.Name = "toolStripSeparatorMinifyJS";
            this.toolStripSeparatorMinifyJS.Size = new System.Drawing.Size(6, 46);
            this.toolStripSeparatorMinifyJS.Visible = false;
            // 
            // tsbMinifyJS
            // 
            this.tsbMinifyJS.Image = ((System.Drawing.Image)(resources.GetObject("tsbMinifyJS.Image")));
            this.tsbMinifyJS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMinifyJS.Name = "tsbMinifyJS";
            this.tsbMinifyJS.Size = new System.Drawing.Size(136, 43);
            this.tsbMinifyJS.Text = "Compress";
            this.tsbMinifyJS.ToolTipText = "This feature compress/minify a script web resource. It does not obfuscate the cod" +
    "e, just remove useless formatting.\r\nBe careful when using this feature! There is" +
    " no way to beautify minified JavaScript";
            this.tsbMinifyJS.Visible = false;
            // 
            // tsbBeautify
            // 
            this.tsbBeautify.Image = ((System.Drawing.Image)(resources.GetObject("tsbBeautify.Image")));
            this.tsbBeautify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBeautify.Name = "tsbBeautify";
            this.tsbBeautify.Size = new System.Drawing.Size(120, 43);
            this.tsbBeautify.Text = "Beautify";
            this.tsbBeautify.ToolTipText = "This feature make uglified JavaScript readable \r\n\r\nThanks to ghost6991 for his wo" +
    "rk on the beautifier in C# : https://github.com/ghost6991/Jsbeautifier";
            this.tsbBeautify.Visible = false;
            // 
            // tsbGetLatestVersion
            // 
            this.tsbGetLatestVersion.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetLatestVersion.Image")));
            this.tsbGetLatestVersion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGetLatestVersion.Name = "tsbGetLatestVersion";
            this.tsbGetLatestVersion.Size = new System.Drawing.Size(138, 43);
            this.tsbGetLatestVersion.Text = "Get Latest";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 46);
            // 
            // tslResourceName
            // 
            this.tslResourceName.Name = "tslResourceName";
            this.tslResourceName.Size = new System.Drawing.Size(0, 43);
            this.tslResourceName.Visible = false;
            // 
            // tsbComment
            // 
            this.tsbComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbComment.Image = ((System.Drawing.Image)(resources.GetObject("tsbComment.Image")));
            this.tsbComment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbComment.Name = "tsbComment";
            this.tsbComment.Size = new System.Drawing.Size(32, 43);
            this.tsbComment.Text = "Comment";
            // 
            // tsbnUncomment
            // 
            this.tsbnUncomment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbnUncomment.Image = ((System.Drawing.Image)(resources.GetObject("tsbnUncomment.Image")));
            this.tsbnUncomment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbnUncomment.Name = "tsbnUncomment";
            this.tsbnUncomment.Size = new System.Drawing.Size(32, 43);
            this.tsbnUncomment.Text = "Uncomment";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 46);
            // 
            // tslName
            // 
            this.tslName.Name = "tslName";
            this.tslName.Size = new System.Drawing.Size(90, 43);
            this.tslName.Text = "tslName";
            // 
            // scintilla
            // 
            this.scintilla.AutomaticFold = ((ScintillaNET.AutomaticFold)(((ScintillaNET.AutomaticFold.Show | ScintillaNET.AutomaticFold.Click) 
            | ScintillaNET.AutomaticFold.Change)));
            this.scintilla.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.Location = new System.Drawing.Point(0, 46);
            this.scintilla.Margin = new System.Windows.Forms.Padding(6);
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(1156, 1040);
            this.scintilla.TabIndex = 6;
            this.scintilla.TextChanged += new System.EventHandler(this.scintilla_TextChanged);
            // 
            // CodeEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 1086);
            this.Controls.Add(this.scintilla);
            this.Controls.Add(this.tsCodeContent);
            this.Name = "CodeEditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeEditorForm_FormClosing);
            this.Load += new System.EventHandler(this.CodeEditor_Load);
            this.tsCodeContent.ResumeLayout(false);
            this.tsCodeContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsCodeContent;
        private System.Windows.Forms.ToolStripDropDownButton tsddbFile;
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
        private System.Windows.Forms.ToolStripButton tsbGetLatestVersion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripLabel tslResourceName;
        private System.Windows.Forms.ToolStripButton tsbComment;
        private System.Windows.Forms.ToolStripButton tsbnUncomment;
        private ScintillaNET.Scintilla scintilla;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel tslName;
    }
}