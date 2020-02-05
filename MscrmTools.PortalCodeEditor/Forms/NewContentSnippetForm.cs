using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MscrmTools.PortalCodeEditor.Forms
{
    public partial class NewContentSnippetForm : Form
    {
        private readonly List<Entity> languages;
        private readonly IOrganizationService service;
        private readonly int snipperType;
        private readonly EntityReference websiteReference;

        public NewContentSnippetForm(int snipperType, IOrganizationService service, EntityReference websiteReference,
            List<Entity> languages)
        {
            InitializeComponent();

            this.service = service;
            this.websiteReference = websiteReference;
            this.languages = languages;
            this.snipperType = snipperType;

            if (languages?.Count > 0)
            {
                cbbLanguages.Items.AddRange
                (languages.Where(l =>
                        l.GetAttributeValue<EntityReference>("adx_websiteid").Id == websiteReference.Id)
                    .Select(l => l.GetAttributeValue<string>("adx_name")).Cast<object>().ToArray());
            }
            else
            {
                cbbLanguages.Enabled = false;
                txtDisplayName.Enabled = false;
            }
        }

        public Entity Snippet { get; private set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show(this, "Please define a name for the content snippet", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            if (cbbLanguages.Items.Count > 0 && cbbLanguages.SelectedItem == null)
            {
                MessageBox.Show(this, "Please select a language for the content snippet", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            btnCancel.Enabled = false;
            btnValidate.Enabled = false;

            try
            {
                Snippet = new Entity("adx_contentsnippet")
                {
                    Attributes =
                    {
                        {"adx_name", txtName.Text},
                        {"adx_websiteid", websiteReference.Id == Guid.Empty ? null : websiteReference},
                        {"adx_type", new OptionSetValue(snipperType)}
                    }
                };

                if (cbbLanguages.SelectedItem != null)
                {
                    Snippet["adx_contentsnippetlanguageid"] = languages.First(l =>
                            l.GetAttributeValue<string>("adx_name") == cbbLanguages.SelectedItem.ToString())
                        .ToEntityReference();
                    Snippet["adx_display_name"] = txtDisplayName.Text;
                }

                Snippet.Id = service.Create(Snippet);

                Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(this, "An error ocurred while creating the content snippet: " + error.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnCancel.Enabled = true;
                btnValidate.Enabled = true;
            }
        }
    }
}