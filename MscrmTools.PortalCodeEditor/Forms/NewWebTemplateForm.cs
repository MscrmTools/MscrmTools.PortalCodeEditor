using Microsoft.Xrm.Sdk;
using System;
using System.Windows.Forms;

namespace MscrmTools.PortalCodeEditor.Forms
{
    public partial class NewWebTemplateForm : Form
    {
        private readonly bool isEnhancedModel;
        private readonly IOrganizationService service;
        private readonly EntityReference websiteReference;

        public NewWebTemplateForm(IOrganizationService service, EntityReference websiteReference, bool isEnhancedModel)
        {
            InitializeComponent();

            this.service = service;
            this.websiteReference = websiteReference;
            this.isEnhancedModel = isEnhancedModel;
        }

        public Entity Template { get; private set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show(this, "Please define a name for the web template", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            btnCancel.Enabled = false;
            btnValidate.Enabled = false;

            try
            {
                Template = new Entity($"{(isEnhancedModel ? "mspp" : "adx")}_webtemplate")
                {
                    Attributes =
                    {
                        {$"{(isEnhancedModel ? "mspp": "adx")}_name", txtName.Text},
                        {$"{(isEnhancedModel ? "mspp": "adx")}_mimetype", txtMimeType.Text}
                    }
                };

                if (websiteReference != null)
                {
                    Template[$"{(isEnhancedModel ? "mspp" : "adx")}_websiteid"] = websiteReference;
                }
                Template.Id = service.Create(Template);

                Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(this, "An error ocurred while creating the web template: " + error.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnCancel.Enabled = true;
                btnValidate.Enabled = true;
            }
        }
    }
}