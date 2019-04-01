using System.Windows.Forms;

namespace MscrmTools.PortalCodeEditor.Forms
{
    public partial class ExportOptions : Form
    {
        public string ExportFolder
        {
            get => txtExportFolder.Text;
            set {
                txtExportFolder.Text = value;
                fldrBrowser.SelectedPath = value;
                btnValidate.Enabled = !(string.IsNullOrEmpty(ExportFolder));
            }
        }
        public bool ClearFolderBeforeExport { get => chkClearItems.Checked; set => chkClearItems.Checked = value; }
        public string SearchText { get => txtSearch.Text; set => txtSearch.Text = value; }
        public bool SearchContents { get => chkSearchInContent.Checked; set => chkSearchInContent.Checked = value; }

        public ExportOptions()
        {
            InitializeComponent();
        }

        private void lnkChooseExportFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fldrBrowser.ShowDialog();
            ExportFolder = fldrBrowser.SelectedPath;
        }

        private void btnValidate_Click(object sender, System.EventArgs e)
        {
            if (chkClearItems.Checked) {

                if (DialogResult.Yes != MessageBox.Show(this,
                    "You have selected 'Clear Export Folder before Export'. " +
                    "This will delete all contents within the selected folder.  Are you sure you would like to delete all files and folders?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) {

                    return;
                }
            }
            DialogResult = DialogResult.OK;
            Close();

        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
