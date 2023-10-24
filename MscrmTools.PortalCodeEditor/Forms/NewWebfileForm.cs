using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace MscrmTools.PortalCodeEditor.Forms
{
    public partial class NewWebfileForm : Form
    {
        private readonly bool isEnhancedModel;
        private readonly List<Entity> publishStates;
        private readonly IOrganizationService service;
        private readonly EntityReference websiteReference;

        public NewWebfileForm(IOrganizationService service, EntityReference websiteReference, List<Entity> publishStates, bool isEnhancedModel)
        {
            InitializeComponent();

            this.service = service;
            this.websiteReference = websiteReference;
            this.isEnhancedModel = isEnhancedModel;
            this.publishStates = publishStates;

            if (publishStates?.Count > 0)
            {
                cbbPublishStates.Items.AddRange
                (publishStates.Where(l =>
                        l.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_websiteid").Id == websiteReference.Id)
                    .Select(l => l.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_name")).Cast<object>().ToArray());
                cbbPublishStates.SelectedIndex = 0;
            }
            else
            {
                cbbPublishStates.Enabled = false;
            }
        }

        public Entity Annotation { get; private set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show(this, "Please define a name for the web file", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!Path.HasExtension(txtName.Text))
            {
                MessageBox.Show(this, "Please define an extension in the web file name", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var ext = Path.GetExtension(txtName.Text).ToLower();
            if (ext != ".js" && ext != ".css")
            {
                MessageBox.Show(this, "Only Javascript and Css file are supported", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnCancel.Enabled = false;
            btnValidate.Enabled = false;

            Entity webFile = null;
            try
            {
                webFile = new Entity($"{(isEnhancedModel ? "mspp" : "adx")}_webfile")
                {
                    Attributes =
                    {
                        {$"{(isEnhancedModel ? "mspp": "adx")}_name", txtName.Text},
                        {$"{(isEnhancedModel ? "mspp": "adx")}_websiteid", websiteReference.Id == Guid.Empty ? null : websiteReference},
                        {$"{(isEnhancedModel ? "mspp": "adx")}_partialurl", txtName.Text}
                    }
                };

                if (cbbPublishStates.SelectedItem != null)
                {
                    webFile[$"{(isEnhancedModel ? "mspp" : "adx")}_publishingstateid"] = publishStates.First(l =>
                             l.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_websiteid").Id == websiteReference.Id &&
                             l.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_name") == cbbPublishStates.SelectedItem.ToString())
                        .ToEntityReference();
                }

                webFile.Id = service.Create(webFile);

                Annotation = new Entity("annotation")
                {
                    Attributes =
                    {
                        {"filename", txtName.Text},
                        {"documentbody","LyogY29udGVudCBoZXJlKi8=" },
                        {"mimetype", ext == ".js" ? "text/javascript" : "text/css"},
                        {"objecttypecode", webFile.LogicalName},
                        {"objectid", webFile.ToEntityReference()},
                    }
                };

                Annotation.Id = service.Create(Annotation);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (FaultException<OrganizationServiceFault> error)
            {
                if (webFile != null)
                    service.Delete(webFile.LogicalName, webFile.Id);

                var errorMessage = "An error ocurred while creating the web file: ";

                if (error.HResult == -2146233087 && ext == ".js")
                {
                    errorMessage +=
                        "It seems Javascript files are not allowed in your Dynamics 365/CDS instance. Please remove this restriction before creating a new Javascript web file";
                }
                else
                {
                    errorMessage += error.Message;
                }

                MessageBox.Show(this, errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnCancel.Enabled = true;
                btnValidate.Enabled = true;
            }
        }
    }
}