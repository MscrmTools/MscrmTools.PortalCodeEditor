namespace MscrmTools.PortalCodeEditor.Forms
{
    partial class SettingsDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkForceUpdate = new System.Windows.Forms.CheckBox();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.tpMinifying = new System.Windows.Forms.TabPage();
            this.chkStyleRemoveComments = new System.Windows.Forms.CheckBox();
            this.chkObfuscateJavaScript = new System.Windows.Forms.CheckBox();
            this.tpColors = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlLiquidObject = new System.Windows.Forms.Panel();
            this.lblLiquidObjectSample = new System.Windows.Forms.Label();
            this.pnlLiquidTag = new System.Windows.Forms.Panel();
            this.lblLiquidTagSample = new System.Windows.Forms.Label();
            this.btnLiquidObject = new System.Windows.Forms.Button();
            this.btnLiquidTag = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.chkUseEnhancedDataModel = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpMinifying.SuspendLayout();
            this.tpColors.SuspendLayout();
            this.pnlLiquidObject.SuspendLayout();
            this.pnlLiquidTag.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 92);
            this.panel1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Settings";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(668, 9);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidate.Location = new System.Drawing.Point(545, 9);
            this.btnValidate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(112, 35);
            this.btnValidate.TabIndex = 1;
            this.btnValidate.Text = "OK";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnValidate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 347);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 62);
            this.panel2.TabIndex = 15;
            // 
            // chkForceUpdate
            // 
            this.chkForceUpdate.AutoSize = true;
            this.chkForceUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkForceUpdate.Location = new System.Drawing.Point(3, 3);
            this.chkForceUpdate.Name = "chkForceUpdate";
            this.chkForceUpdate.Size = new System.Drawing.Size(770, 24);
            this.chkForceUpdate.TabIndex = 16;
            this.chkForceUpdate.Text = "Force records update even if it has changed since its last load (ie. RowVersion m" +
    "ismatch)";
            this.chkForceUpdate.UseVisualStyleBackColor = true;
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpGeneral);
            this.tcMain.Controls.Add(this.tpMinifying);
            this.tcMain.Controls.Add(this.tpColors);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 92);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(784, 255);
            this.tcMain.TabIndex = 17;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.chkUseEnhancedDataModel);
            this.tpGeneral.Controls.Add(this.chkForceUpdate);
            this.tpGeneral.Location = new System.Drawing.Point(4, 29);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpGeneral.Size = new System.Drawing.Size(776, 222);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // tpMinifying
            // 
            this.tpMinifying.Controls.Add(this.chkStyleRemoveComments);
            this.tpMinifying.Controls.Add(this.chkObfuscateJavaScript);
            this.tpMinifying.Location = new System.Drawing.Point(4, 29);
            this.tpMinifying.Name = "tpMinifying";
            this.tpMinifying.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpMinifying.Size = new System.Drawing.Size(750, 222);
            this.tpMinifying.TabIndex = 1;
            this.tpMinifying.Text = "Minifying";
            this.tpMinifying.UseVisualStyleBackColor = true;
            // 
            // chkStyleRemoveComments
            // 
            this.chkStyleRemoveComments.AutoSize = true;
            this.chkStyleRemoveComments.Location = new System.Drawing.Point(8, 37);
            this.chkStyleRemoveComments.Name = "chkStyleRemoveComments";
            this.chkStyleRemoveComments.Size = new System.Drawing.Size(217, 24);
            this.chkStyleRemoveComments.TabIndex = 1;
            this.chkStyleRemoveComments.Text = "CSS : Remove comments";
            this.chkStyleRemoveComments.UseVisualStyleBackColor = true;
            // 
            // chkObfuscateJavaScript
            // 
            this.chkObfuscateJavaScript.AutoSize = true;
            this.chkObfuscateJavaScript.Location = new System.Drawing.Point(9, 8);
            this.chkObfuscateJavaScript.Name = "chkObfuscateJavaScript";
            this.chkObfuscateJavaScript.Size = new System.Drawing.Size(234, 24);
            this.chkObfuscateJavaScript.TabIndex = 0;
            this.chkObfuscateJavaScript.Text = "JavaScript : Obfuscate code";
            this.chkObfuscateJavaScript.UseVisualStyleBackColor = true;
            // 
            // tpColors
            // 
            this.tpColors.Controls.Add(this.label3);
            this.tpColors.Controls.Add(this.label2);
            this.tpColors.Controls.Add(this.pnlLiquidObject);
            this.tpColors.Controls.Add(this.pnlLiquidTag);
            this.tpColors.Controls.Add(this.btnLiquidObject);
            this.tpColors.Controls.Add(this.btnLiquidTag);
            this.tpColors.Location = new System.Drawing.Point(4, 29);
            this.tpColors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpColors.Name = "tpColors";
            this.tpColors.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpColors.Size = new System.Drawing.Size(750, 222);
            this.tpColors.TabIndex = 2;
            this.tpColors.Text = "Colors";
            this.tpColors.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Liquid Objects highlighting color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Liquid Tag highlighting color";
            // 
            // pnlLiquidObject
            // 
            this.pnlLiquidObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLiquidObject.Controls.Add(this.lblLiquidObjectSample);
            this.pnlLiquidObject.Location = new System.Drawing.Point(346, 57);
            this.pnlLiquidObject.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlLiquidObject.Name = "pnlLiquidObject";
            this.pnlLiquidObject.Size = new System.Drawing.Size(340, 34);
            this.pnlLiquidObject.TabIndex = 5;
            // 
            // lblLiquidObjectSample
            // 
            this.lblLiquidObjectSample.AutoSize = true;
            this.lblLiquidObjectSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLiquidObjectSample.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLiquidObjectSample.Location = new System.Drawing.Point(0, 0);
            this.lblLiquidObjectSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLiquidObjectSample.Name = "lblLiquidObjectSample";
            this.lblLiquidObjectSample.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblLiquidObjectSample.Size = new System.Drawing.Size(120, 26);
            this.lblLiquidObjectSample.TabIndex = 0;
            this.lblLiquidObjectSample.Text = "{{ type }}";
            // 
            // pnlLiquidTag
            // 
            this.pnlLiquidTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLiquidTag.Controls.Add(this.lblLiquidTagSample);
            this.pnlLiquidTag.Location = new System.Drawing.Point(346, 12);
            this.pnlLiquidTag.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlLiquidTag.Name = "pnlLiquidTag";
            this.pnlLiquidTag.Size = new System.Drawing.Size(340, 34);
            this.pnlLiquidTag.TabIndex = 4;
            // 
            // lblLiquidTagSample
            // 
            this.lblLiquidTagSample.AutoSize = true;
            this.lblLiquidTagSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLiquidTagSample.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLiquidTagSample.Location = new System.Drawing.Point(0, 0);
            this.lblLiquidTagSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLiquidTagSample.Name = "lblLiquidTagSample";
            this.lblLiquidTagSample.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblLiquidTagSample.Size = new System.Drawing.Size(307, 26);
            this.lblLiquidTagSample.TabIndex = 0;
            this.lblLiquidTagSample.Text = "{% if type = \"LiquidTag\" %}";
            // 
            // btnLiquidObject
            // 
            this.btnLiquidObject.Location = new System.Drawing.Point(696, 60);
            this.btnLiquidObject.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLiquidObject.Name = "btnLiquidObject";
            this.btnLiquidObject.Size = new System.Drawing.Size(38, 35);
            this.btnLiquidObject.TabIndex = 3;
            this.btnLiquidObject.Text = "...";
            this.btnLiquidObject.UseVisualStyleBackColor = true;
            this.btnLiquidObject.Click += new System.EventHandler(this.btnLiquidObject_Click);
            // 
            // btnLiquidTag
            // 
            this.btnLiquidTag.Location = new System.Drawing.Point(696, 12);
            this.btnLiquidTag.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLiquidTag.Name = "btnLiquidTag";
            this.btnLiquidTag.Size = new System.Drawing.Size(38, 35);
            this.btnLiquidTag.TabIndex = 2;
            this.btnLiquidTag.Text = "...";
            this.btnLiquidTag.UseVisualStyleBackColor = true;
            this.btnLiquidTag.Click += new System.EventHandler(this.btnLiquidTag_Click);
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            // 
            // chkUseEnhancedDataModel
            // 
            this.chkUseEnhancedDataModel.AutoSize = true;
            this.chkUseEnhancedDataModel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkUseEnhancedDataModel.Location = new System.Drawing.Point(3, 27);
            this.chkUseEnhancedDataModel.Name = "chkUseEnhancedDataModel";
            this.chkUseEnhancedDataModel.Size = new System.Drawing.Size(770, 24);
            this.chkUseEnhancedDataModel.TabIndex = 17;
            this.chkUseEnhancedDataModel.Text = "Use Enhanced Data Model";
            this.chkUseEnhancedDataModel.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 409);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.OptionsDialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpMinifying.ResumeLayout(false);
            this.tpMinifying.PerformLayout();
            this.tpColors.ResumeLayout(false);
            this.tpColors.PerformLayout();
            this.pnlLiquidObject.ResumeLayout(false);
            this.pnlLiquidObject.PerformLayout();
            this.pnlLiquidTag.ResumeLayout(false);
            this.pnlLiquidTag.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkForceUpdate;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpMinifying;
        private System.Windows.Forms.CheckBox chkObfuscateJavaScript;
        private System.Windows.Forms.CheckBox chkStyleRemoveComments;
        private System.Windows.Forms.TabPage tpColors;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button btnLiquidObject;
        private System.Windows.Forms.Button btnLiquidTag;
        private System.Windows.Forms.Panel pnlLiquidObject;
        private System.Windows.Forms.Label lblLiquidObjectSample;
        private System.Windows.Forms.Panel pnlLiquidTag;
        private System.Windows.Forms.Label lblLiquidTagSample;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkUseEnhancedDataModel;
    }
}