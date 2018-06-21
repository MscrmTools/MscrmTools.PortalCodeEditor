using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MscrmTools.PortalCodeEditor.AppCode;
using MscrmTools.PortalCodeEditor.AppCode.EventArgs;
using MscrmTools.PortalCodeEditor.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MscrmTools.PortalCodeEditor
{
    public partial class MyPluginControl : PluginControlBase, IGitHubPlugin, IHelpPlugin, IShortcutReceiver
    {
        #region Variables

        private CodeTreeViewForm ctvf;
        private bool isLegacyPortal;
        private Settings mySettings;
        private List<EditablePortalItem> portalItems;

        #endregion Variables

        #region Constructor

        public MyPluginControl()
        {
            InitializeComponent();

            dpMain.Theme = new VS2015LightTheme();

            portalItems = new List<EditablePortalItem>();
        }

        #endregion Constructor

        #region Properties

        public string HelpUrl => "https://github.com/MscrmTools/MscrmTools.PortalCodeEditor/wiki";
        public string RepositoryName => "MscrmTools.PortalCodeEditor";
        public string UserName => "MscrmTools";

        #endregion Properties

        #region This events

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            //Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();
            }

            ctvf = new CodeTreeViewForm { Service = Service };
            ctvf.PortalItemSelected += Ctvf_PortalItemSelected;
            ctvf.ActionRequested += Ctvf_ActionRequested;
            ctvf.Show(dpMain, DockState.DockLeft);
        }

        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        #endregion This events

        #region Treeview events

        private void Content_UpdateRequested(object sender, UpdateRequestedEventArgs e)
        {
            Update(new List<EditablePortalItem> { e.Item.Parent });
        }

        private void Ctvf_ActionRequested(object sender, EventArgs e)
        {
            if (e is UpdatePendingChangesEventArgs updateItemsArgs)
            {
                Update(updateItemsArgs.Items.ToList());
                return;
            }

            if (e is RefreshContentEventArgs refreshArgs)
            {
                if (refreshArgs.Item is EditablePortalItem epi)
                {
                    foreach (var item in epi.Items)
                    {
                        item.Refresh(Service);

                        foreach (var page in dpMain.Contents.OfType<CodeEditorForm>().Where(c => c.Item == item))
                        {
                            page.RefreshContent(item.Content);
                        }
                    }
                }
                else
                {
                    if (refreshArgs.Item is CodeItem ci)
                    {
                        ci.Refresh(Service);

                        foreach (var page in dpMain.Contents.OfType<CodeEditorForm>().Where(c => c.Item == ci))
                        {
                            page.RefreshContent(ci.Content);
                        }
                    }
                }
            }
        }

        private void Ctvf_PortalItemSelected(object sender, PortalItemSelectedEventArgs e)
        {
            if (e.Item != null)
            {
                var content = dpMain.Contents.OfType<CodeEditorForm>().FirstOrDefault(c => c.Item == e.Item);
                if (content == null)
                {
                    content = new CodeEditorForm(e.Item, mySettings, Service);
                    content.UpdateRequested += Content_UpdateRequested;
                    content.TabPageContextMenuStrip = cmsTab;
                    content.Show(dpMain, DockState.Document);
                }
                else
                {
                    content.Show(dpMain, content.DockState);
                }
            }
        }

        #endregion Treeview events

        #region Main menu events

        private void LoadItems()
        {
            ctvf.Enabled = false;
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading portal items...",
                Work = (bw, e) =>
                {
                    portalItems = new List<EditablePortalItem>();
                    bw.ReportProgress(0, "Loading Web pages...");
                    portalItems.AddRange(WebPage.GetItems(Service));
                    bw.ReportProgress(0, "Loading Entity forms...");
                    portalItems.AddRange(EntityForm.GetItems(Service, ref isLegacyPortal));
                    bw.ReportProgress(0, "Loading Entity lists...");
                    portalItems.AddRange(EntityList.GetItems(Service, ref isLegacyPortal));
                    bw.ReportProgress(0, "Loading Web templates...");
                    portalItems.AddRange(WebTemplate.GetItems(Service, ref isLegacyPortal));
                    bw.ReportProgress(0, "Loading Web files...");
                    portalItems.AddRange(WebFile.GetItems(Service));
                    bw.ReportProgress(0, "Loading Web form steps...");
                    portalItems.AddRange(WebFormStep.GetItems(Service));
                    bw.ReportProgress(0, "Loading Content Snippets...");
                    portalItems.AddRange(ContentSnippet.GetItems(Service, ref isLegacyPortal));

                    if (!isLegacyPortal)
                    {
                        bw.ReportProgress(0, "Loading Portal languages...");
                        ctvf.Languages = Service.RetrieveMultiple(new QueryExpression("adx_websitelanguage")
                        {
                            ColumnSet = new ColumnSet(true)
                        }).Entities.ToList();
                    }
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        if (((FaultException<OrganizationServiceFault>)e.Error).Detail.ErrorCode == -2147217149)
                        {
                            MessageBox.Show(e.Error.ToString());

                            var message =
                                "Unable to load code items: Please ensure you are targeting an organization linked to a Microsoft Portal (not a legacy Adxstudio one)";
                            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogError(message);
                        }
                        else
                        {
                            MessageBox.Show(this, $"An error occured while loading code items: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogError($"An error occured while loading code items: {e.Error.ToString()}");
                        }
                    }

                    ctvf.DisplayCodeItems(portalItems, isLegacyPortal);
                    ctvf.Enabled = true;
                }
            });
        }

        private void tsbCredits_Click(object sender, EventArgs e)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Credits to the following authors for their icons:");
            message.AppendLine();
            message.AppendLine("\"Load items\" icon made by Gregor Cresnar from www.flaticon.com");
            message.AppendLine("\"Update\" icon made by Freepik from www.flaticon.com");
            message.AppendLine("\"Settings\" icon made by Freepik from www.flaticon.com");
            message.AppendLine();
            message.AppendLine("Credits for the code editor:");
            message.AppendLine();
            message.AppendLine("ScintillaNET from https://github.com/jacobslusser/ScintillaNET");

            MessageBox.Show(this, message.ToString(), "Icons credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsbLoadItems_Click(object sender, EventArgs e)
        {
            if (dpMain.Contents.OfType<CodeEditorForm>().Any())
            {
                var message =
                    $"Reloading items will close the {dpMain.Contents.OfType<CodeEditorForm>().Count()} item{(dpMain.Contents.OfType<CodeEditorForm>().Count() > 1 ? "s" : "")} opened.\n\nAre you sure you want to reload items?";
                if (MessageBox.Show(this, message, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.No)
                    return;
            }

            var list = dpMain.Contents.OfType<CodeEditorForm>().ToList();
            foreach (var item in list)
            {
                item.Close();
            }

            ExecuteMethod(LoadItems);
        }

        private void tsbSettings_Click(object sender, EventArgs e)
        {
            var sDialog = new SettingsDialog(mySettings);
            sDialog.ShowDialog(this);

            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        private void tsbUpdateCheckedItems_Click(object sender, EventArgs e)
        {
            Update(ctvf.CheckedItems);
        }

        #endregion Main menu events

        #region Tab management

        private void closeAllButThisTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dpMain.ActiveContent is CodeEditorForm cef)
            {
                var list = dpMain.Contents.OfType<CodeEditorForm>().ToList();
                list.Remove(cef);
                CloseForms(list);
            }
        }

        private void closeAllTabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseForms(dpMain.Contents.OfType<CodeEditorForm>().ToList());
        }

        private void CloseForms(List<CodeEditorForm> forms)
        {
            foreach (var form in forms)
            {
                form.Close();
            }
        }

        private void closeThisTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dpMain.ActiveContent is CodeEditorForm cef)
                CloseForms(new List<CodeEditorForm> { cef });
        }

        #endregion Tab management

        #region Other methods

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail,
            string actionName, object parameter)
        {
            if (ctvf != null)
                ctvf.Service = newService;

            base.UpdateConnection(newService, detail, actionName, parameter);
        }

        private void Update(List<EditablePortalItem> items)
        {
            WorkAsync(new WorkAsyncInfo
            {
                AsyncArgument = items,
                Message = "Updating selected item(s)...",
                Work = (bw, e) =>
                {
                    bool hasError = false;
                    var itemsToProcess = (List<EditablePortalItem>)e.Argument;

                    foreach (var item in itemsToProcess)
                    {
                        bw.ReportProgress(0, $"Updating {item.GetType().Name} \"{item.Name}\"");
                        try
                        {
                            item.Update(Service, mySettings.ForceUpdate);
                        }
                        catch (FaultException<OrganizationServiceFault> error)
                        {
                            if (itemsToProcess.Count == 1)
                            {
                                e.Result = $"'{item.Name}' ({item.GetType().Name} for website '{item.WebsiteReference.Name}') was not updated because it has changed since it has been loaded in Portal Code Editor. \n\nPlease note that this change might not concern code attribute your are editing. \n\nYou can force the update in the settings";
                                continue;
                            }

                            if (error.Detail.ErrorCode == -2147088254)
                            {
                                LogError("'{0}' ({1} for website '{2}') was not updated because it has changed since it has been loaded in Portal Code Editor. Please note that this change might not concern code attribute your are editing. You can force the update in the settings",
                                    item.Name,
                                    item.GetType().Name,
                                    item.WebsiteReference.Name);

                                hasError = true;
                                continue;
                            }

                            throw;
                        }
                    }

                    if (hasError)
                    {
                        throw new Exception("At least one record has not been updated. Please review the logs");
                    }
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (e.Result != null)
                    {
                        MessageBox.Show(this, e.Result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
        }

        #endregion Other methods

        #region IShortcutReceiver

        private bool isCtrlK;

        public void ReceiveKeyDownShortcut(KeyEventArgs e)
        {
            if (dpMain.ActiveContent is CodeEditorForm cef)
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    cef.Save();
                }
                else if (e.Control && e.KeyCode == Keys.U)
                {
                    if (isCtrlK)
                    {
                        cef.UncommentSelectedLines();
                    }
                    else
                    {
                        cef.UpdateItem();
                    }

                    isCtrlK = false;
                }
                else if (e.Control && e.KeyCode == Keys.G)
                {
                    cef.GoToLine();
                }
                else if (e.Control && e.KeyCode == Keys.F)
                {
                    cef.Find(false, this);
                }
                else if (e.Control && e.KeyCode == Keys.H)
                {
                    cef.Find(true, this);
                }
                else if (e.Control && e.KeyCode == Keys.K)
                {
                    isCtrlK = true;
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    if (isCtrlK)
                    {
                        cef.CommentSelectedLines();
                    }

                    isCtrlK = false;
                }
                else
                {
                    isCtrlK = false;
                }
            }
        }

        public void ReceiveKeyPressShortcut(KeyPressEventArgs e)
        {
        }

        public void ReceiveKeyUpShortcut(KeyEventArgs e)
        {
        }

        public void ReceivePreviewKeyDownShortcut(PreviewKeyDownEventArgs e)
        {
        }

        #endregion IShortcutReceiver
    }
}