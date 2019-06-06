using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MscrmTools.PortalCodeEditor.AppCode;
using MscrmTools.PortalCodeEditor.AppCode.EventArgs;
using MscrmTools.PortalCodeEditor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
                    LoadPortalItems(bw, isLegacyPortal);
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    // handle any errors returned
                    this.HandlLoadPortalItemErrors(e);

                    ctvf.DisplayCodeItems(portalItems, isLegacyPortal);
                    ctvf.Enabled = true;
                }
            });
        }

        #region Load Portal Items helper methods

        /// <summary>
        /// Helper method to handle the errors returned when loading portal items
        /// </summary>
        /// <param name="e"></param>
        private void HandlLoadPortalItemErrors(RunWorkerCompletedEventArgs e)
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
        }

        /// <summary>
        /// Helper method for loading the portal items
        /// </summary>
        /// <param name="bw"></param>
        private void LoadPortalItems(BackgroundWorker bw, bool isLegacyPortal)
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
        }

        #endregion Load Portal Items helper methods

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

        #region Export to disk

        /// <summary>
        /// Helper method to ensure that the path is combined correctly, removing trailing spaces
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newFolder"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        private string AppendToPath(string path, string newFolder, bool create = false)
        {
            path = Path.Combine(path.Trim(), newFolder.Trim());

            if (!Directory.Exists(path) && create)
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// Export the Portal contents to individual files on disk
        /// </summary>
        /// <param name="portalItems">New list of EditablePortalItem objects</param>
        /// <param name="exportFolder">Target folder for the export</param>
        /// <param name="isLegacyPortal">Flag indicating whether this is an older version of the Portal</param>
        /// <param name="searchText">Text to filter the list</param>
        /// <param name="searchInContent">Flag indicating whether to search within the Content as well as names</param>
        /// <param name="clearFolder">Before export, delete all contents</param>
        private void Export(List<EditablePortalItem> portalItems, string exportFolder, bool isLegacyPortal,
            string searchText = null,
            bool searchInContent = true,
            bool clearFolder = false)
        {
            // preset the filtered list to the current list of items
            var filteredItems = portalItems;

            // if search text has been provided, search the list
            if (searchText != null)
            {
                // search case insensitive
                searchText = searchText.ToLower();

                filteredItems = portalItems.Where(i => searchText.Length == 0
                                                     || (i.Name?.ToLower().Contains(searchText) ?? false)
                                                     || searchInContent &&
                                                     i.Items.Any(i2 => i2.Content?.ToLower().Contains(searchText) ?? false))
                    .ToList();
            }

            // build out the folder strucure. this will mimick the tree view structure pretty closely.
            // delete the folders?
            if (clearFolder)
            {
                DirectoryInfo di = new DirectoryInfo(exportFolder);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

            // keep track of the names by type by website that have been exported so we do not overrite content
            // might be easier to look for file...
            var websiteNameList = new Dictionary<Guid, Dictionary<string, Dictionary<Guid, string>>>();

            // begin the work!
            foreach (var item in filteredItems)
            {
                var currentPath = "";
                var websiteReference = item.WebsiteReference;

                if (websiteReference == null) { continue; }

                // build current folder path starting with the website, appended to the selected folder
                var websiteName = websiteReference.Id == Guid.Empty ? "(Not website related)" : websiteReference.Name;
                websiteName = EditablePortalItem.EscapeForFileName(websiteName);
                currentPath = Path.Combine(exportFolder, websiteName);

                // add a new structure to track names for this website
                if (!websiteNameList.ContainsKey(websiteReference.Id))
                {
                    websiteNameList.Add(websiteReference.Id, GetWebSiteNameList());
                }
                var nameList = websiteNameList[websiteReference.Id];

                if (!Directory.Exists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);
                    Directory.CreateDirectory(Path.Combine(currentPath, WebPage.NODENAME));

                    if (!isLegacyPortal)
                    {
                        Directory.CreateDirectory(Path.Combine(currentPath, ContentSnippet.NODENAME));
                        Directory.CreateDirectory(Path.Combine(currentPath, EntityForm.NODENAME));
                        Directory.CreateDirectory(Path.Combine(currentPath, EntityList.NODENAME));
                        Directory.CreateDirectory(Path.Combine(currentPath, WebFormStep.NODENAME));
                        Directory.CreateDirectory(Path.Combine(currentPath, WebTemplate.NODENAME));
                    }

                    Directory.CreateDirectory(Path.Combine(websiteName, WebFile.NODENAME));
                }

                if (item is WebPage page)
                {
                    // new web page, append to the current path
                    currentPath = AppendToPath(currentPath, WebPage.NODENAME);

                    if (page.IsRoot || page.ParentPageId == Guid.Empty)
                    {
                        var name = GetItemUniqueName(nameList[WebPage.NODENAME], page, page.PartialUrl);

                        // new folder for web page
                        currentPath = AppendToPath(currentPath, name);

                        if (isLegacyPortal)
                        {
                            // path to JS and CSS file only
                            page.WriteContent(currentPath);
                        }
                    }
                    else
                    {
                        // find the parent page Id to get the name of the folder
                        // TODO - do we want to account for the root page?
                        var parent = portalItems
                            .FirstOrDefault(w => (w as WebPage)?.Id == page.ParentPageId) as WebPage;

                        if (parent == null)
                        {
                            continue;
                        }
                        var name = GetItemUniqueName(nameList[WebPage.NODENAME], parent, page.PartialUrl);

                        // new path for website and the language
                        currentPath = AppendToPath(currentPath, name);
                        currentPath = AppendToPath(currentPath, page.Language);

                        // path to JS and CSS file only
                        page.WriteContent(currentPath);
                    }
                }
                else if (item is EntityForm form)
                {
                    var name = GetItemUniqueName(nameList[EntityForm.NODENAME], form);

                    // new entity form, append to the current path
                    currentPath = AppendToPath(currentPath, EntityForm.NODENAME);
                    currentPath = AppendToPath(currentPath, name);

                    form.WriteContent(currentPath);
                }
                else if (item is EntityList list)
                {
                    var name = GetItemUniqueName(nameList[EntityList.NODENAME], list);

                    // new entity list, append to the current path
                    currentPath = AppendToPath(currentPath, EntityList.NODENAME);
                    currentPath = AppendToPath(currentPath, name);

                    list.WriteContent(currentPath);
                }
                else if (item is WebTemplate template)
                {
                    var name = GetItemUniqueName(nameList[WebTemplate.NODENAME], template);

                    // new Web Template, append to the current path
                    currentPath = AppendToPath(currentPath, WebTemplate.NODENAME);
                    currentPath = AppendToPath(currentPath, name);

                    template.WriteContent(currentPath);
                }
                else if (item is WebFile file)
                {
                    // var name = GetItemUniqueName(nameList[WebFile.NODENAME], file);
                    // NOTE assume the filename is unique
                    // new Web File, append to the current path
                    currentPath = AppendToPath(currentPath, WebFile.NODENAME);
                    currentPath = Path.Combine(currentPath, file.Name);

                    file.WriteContent(currentPath);
                }
                else if (item is WebFormStep wfStep)
                {
                    // NOTE assume the WF Steps are unique within the WF
                    // new Web Form Step... first, append to web forms
                    currentPath = AppendToPath(currentPath, WebFormStep.NODENAME);

                    // then find the web form name for the root folder
                    if (wfStep.WebFormReference != null)
                    {
                        // add the web form name to the path
                        currentPath = AppendToPath(currentPath, WebFormStep.EscapeForFileName(wfStep.WebFormReference.Name));
                    }
                    // now down to the web form step name for a folder and then the js
                    var name = wfStep.EscapeName();
                    currentPath = AppendToPath(currentPath, name);

                    wfStep.WriteContent(currentPath);
                }
                else if (item is ContentSnippet snippet)
                {
                    var name = GetItemUniqueName(nameList[ContentSnippet.NODENAME], snippet);

                    // new content snippet.  put all in the same folder, appending extension
                    currentPath = AppendToPath(currentPath, ContentSnippet.NODENAME);
                    currentPath = AppendToPath(currentPath, name);

                    snippet.WriteContent(currentPath);
                }
                else
                {
                    throw new Exception($"Unsupported portal item type: {item.GetType().Name}");
                }
            }
        }

        /// <summary>
        /// Ensure that the name of the item is unique.  For example, two Web Pages named PageOne.
        /// </summary>
        /// <param name="nameList"></param>
        /// <param name="newItem"></param>
        /// <param name="appendTo"></param>
        /// <returns></returns>
        private string GetItemUniqueName(Dictionary<Guid, string> nameList, EditablePortalItem newItem, string appendTo = null)
        {
            // if the ID is already in the list, just return it
            if (nameList.ContainsKey(newItem.Id))
            {
                return nameList[newItem.Id];
            }
            else
            {
                // see if the name already exists in the list for this section
                var newName = newItem.EscapeName();

                // see if something additional has been added to ensure uniqueness (like partial URL)
                if (appendTo != null)
                {
                    newName += $" - {appendTo}";
                }
                newName = EditablePortalItem.EscapeForFileName(newName);

                var origName = newName;
                var counter = 0;
                while (nameList.Where(n => n.Value == newName).ToList().Count > 0)
                {
                    counter++;
                    newName = $"{origName} ({counter})";
                }
                // add to the list for the next round
                nameList.Add(newItem.Id, newName);
                return newName;
            }
        }

        private Dictionary<string, Dictionary<Guid, string>> GetWebSiteNameList()
        {
            var nameList = new Dictionary<string, Dictionary<Guid, string>>();

            // build out a container for the names so that we don't have clashes of folders or files.
            // add a dictionary per section.  duplicate names are ok across sectins: web page can be same as web template
            nameList.Add(WebPage.NODENAME, new Dictionary<Guid, string>());
            nameList.Add(ContentSnippet.NODENAME, new Dictionary<Guid, string>());
            nameList.Add(EntityForm.NODENAME, new Dictionary<Guid, string>());
            nameList.Add(EntityList.NODENAME, new Dictionary<Guid, string>());
            nameList.Add(WebFormStep.NODENAME, new Dictionary<Guid, string>());
            nameList.Add(WebTemplate.NODENAME, new Dictionary<Guid, string>());
            nameList.Add(WebFile.NODENAME, new Dictionary<Guid, string>());

            return nameList;
        }

        /// <summary>
        /// Helper method to load the items for export in case items already loaded have changes
        /// </summary>
        private void LoadItemsForExport()
        {
            // pull up the export options dialog for folder, search, etc.
            var expOptions = new ExportOptions();
            var result = expOptions.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            // load the portal items and export to disk
            ctvf.Enabled = false;
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading portal items...",
                Work = (bw, e) =>
                {
                    // load all the portal content again, just in case changes have been made
                    LoadPortalItems(bw, isLegacyPortal);
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    // handle any errors returned
                    this.HandlLoadPortalItemErrors(e);

                    if (e.Error == null)
                    {
                        // now that we have the items, export to the selected folder
                        Export(portalItems, expOptions.ExportFolder, isLegacyPortal,
                            expOptions.SearchText,
                            expOptions.SearchContents,
                            expOptions.ClearFolderBeforeExport);
                    }

                    ctvf.Enabled = true;
                }
            });
        }

        /// <summary>
        /// Handle the export click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void tsbExportPortalItems_Click(object sender, EventArgs args)
        {
            ExecuteMethod(LoadItemsForExport);
        }

        #endregion Export to disk
    }
}