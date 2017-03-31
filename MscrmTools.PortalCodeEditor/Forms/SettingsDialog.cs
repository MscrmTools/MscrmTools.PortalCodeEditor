using System;
using System.Windows.Forms;

namespace MscrmTools.PortalCodeEditor.Forms
{
    public partial class SettingsDialog : Form
    {
        private readonly Settings mySettings;

        public SettingsDialog(Settings mySettings)
        {
            InitializeComponent();

            this.mySettings = mySettings;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            mySettings.ForceUpdate = chkForceUpdate.Checked;
            mySettings.ObfuscateJavascript = chkObfuscateJavaScript.Checked;
            mySettings.RemoveCssComments = chkStyleRemoveComments.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void OptionsDialog_Load(object sender, EventArgs e)
        {
            chkForceUpdate.Checked = mySettings.ForceUpdate;
            chkObfuscateJavaScript.Checked = mySettings.ObfuscateJavascript;
            chkStyleRemoveComments.Checked = mySettings.RemoveCssComments;
        }
    }
}