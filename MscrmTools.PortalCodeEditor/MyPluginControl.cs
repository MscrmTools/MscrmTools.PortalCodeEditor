using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk;
using MscrmTools.PortalCodeEditor.AppCode;
using MscrmTools.PortalCodeEditor.AppCode.EventArgs;
using MscrmTools.PortalCodeEditor.Controls;
using MscrmTools.PortalCodeEditor.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MscrmTools.PortalCodeEditor
{
    public partial class MyPluginControl : PluginControlBase, IGitHubPlugin, IHelpPlugin
    {
        #region Variables

        private List<EditablePortalItem> portalItems;
        private Settings mySettings;
        private bool isLegacyPortal;

        #endregion Variables

        #region Constructor

        public MyPluginControl()
        {
            InitializeComponent();

            portalItems = new List<EditablePortalItem>();
        }

        #endregion Constructor

        #region Properties

        public string RepositoryName => "MscrmTools.PortalCodeEditor";
        public string UserName => "MscrmTools";
        public string HelpUrl => "https://github.com/MscrmTools/MscrmTools.PortalCodeEditor/wiki";

        #endregion Properties

        #region This events

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            //Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();
            }
        }

        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        #endregion This events

        #region Treeview events

        private void ctv_PortalItemSelected(object sender, PortalItemSelectedEventArgs e)
        {
            tcCodeContents.Padding = new Point(20, tcCodeContents.Padding.Y);

            if (e.Item != null)
            {
                var key = $"{e.Item.Parent.Id}{e.Item.Type}";

                TabPage tab;

                if (tcCodeContents.TabPages.ContainsKey(key))
                {
                    tab = tcCodeContents.TabPages[key];
                    tcCodeContents.SelectedTab = tab;
                }
                else
                {
                    tab = new TabPage
                    {
                        Text = e.Item.Parent.Name,
                        Name = key,
                        Tag = e.Item,
                        Width = TextRenderer.MeasureText(e.Item.Parent.Name, tcCodeContents.Font).Width + 40
                    };

                    var ce = new CodeEditorScintilla(e.Item, mySettings)
                    {
                        Dock = DockStyle.Fill
                    };

                    tab.Controls.Add(ce);
                    tcCodeContents.TabPages.Add(tab);
                    tcCodeContents.SelectedTab = tab;
                }

                ManageCodeContentToolbarDisplay(e.Item);

                lblItemSelected.Text =
                    $"{e.Item.Parent.Name} {(e.Item.Parent is WebPage ? (e.Item.Type == CodeItemType.JavaScript ? "(JavaScript)" : "(Style)") : "")}";
            }
        }

        private void ManageCodeContentToolbarDisplay(CodeItem item)
        {
            tsbBeautify.Visible = item.Type != CodeItemType.LiquidTemplate;
            tsbMinifyJS.Visible = item.Type != CodeItemType.LiquidTemplate;
        }

        private void ctv_ActionRequested(object sender, EventArgs e)
        {
            var updateItemsArgs = e as UpdatePendingChangesEventArgs;
            if (updateItemsArgs != null)
            {
                Update(updateItemsArgs.Items.ToList());
                return;
            }

            var refreshArgs = e as RefreshContentEventArgs;
            if (refreshArgs != null)
            {
                var epi = refreshArgs.Item as EditablePortalItem;
                if (epi != null)
                {
                    foreach (var item in epi.Items)
                    {
                        item.Refresh(Service);

                        foreach (var page in tcCodeContents.TabPages.Cast<TabPage>().Where(p => p.Tag == item))
                        {
                            ((CodeEditorScintilla)page.Controls[0]).RefreshContent(item.Content);
                        }
                    }
                }
                else
                {
                    var ci = refreshArgs.Item as CodeItem;
                    if (ci != null)
                    {
                        ci.Refresh(Service);

                        foreach (var page in tcCodeContents.TabPages.Cast<TabPage>().Where(p => p.Tag == ci))
                        {
                            ((CodeEditorScintilla)page.Controls[0]).RefreshContent(ci.Content);
                        }
                    }
                }
            }
        }

        #endregion Treeview events

        #region Main menu events

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbLoadItems_Click(object sender, EventArgs e)
        {
            if (tcCodeContents.TabCount > 0)
            {
                var message =
                    $"Reloading items will close the {tcCodeContents.TabCount} item{(tcCodeContents.TabCount > 1 ? "s" : "")} opened.\n\nAre you sure you want to reload items?";
                if (MessageBox.Show(this, message, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.No)
                    return;
            }

            tcCodeContents.TabPages.Clear();

            ExecuteMethod(LoadItems);
        }

        private void LoadItems()
        {
            ctv.Enabled = false;
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

                    portalItems.SelectMany(p => p.Items).ToList().ForEach(i => i.StateChanged += CodeItem_StateChanged);
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

                    ctv.DisplayCodeItems(portalItems, isLegacyPortal);
                    ctv.Enabled = true;
                }
            });
        }

        private void tsbUpdateCheckedItems_Click(object sender, EventArgs e)
        {
            Update(ctv.CheckedItems);
        }

        private void tsbSettings_Click(object sender, EventArgs e)
        {
            var sDialog = new SettingsDialog(mySettings);
            sDialog.ShowDialog(this);

            SettingsManager.Instance.Save(GetType(), mySettings);
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

        #endregion Main menu events

        #region Code content events

        private void fileMenuSave_Click(object sender, EventArgs e)
        {
            var codeItem = (CodeItem)tcCodeContents.SelectedTab.Tag;
            var codeCtrl = tcCodeContents.SelectedTab.Controls[0] as CodeEditorScintilla;
            if (codeCtrl != null)
            {
                codeCtrl.Save();
                codeItem.State = CodeItemState.Saved;

                //tcCodeContents.SelectedTab.ForeColor = Color.Blue;
                //tcCodeContents.SelectedTab.Text = $"{codeItem.Parent.Name} !";
            }
        }

        private void fileMenuUpdate_Click(object sender, EventArgs e)
        {
            fileMenuSave_Click(null, null);

            var item = tcCodeContents.SelectedTab.Tag as CodeItem;
            if (item != null)
            {
                Update(new List<EditablePortalItem> { item.Parent });
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((TabControl)(Parent).Parent).SelectedTab != Parent)
            {
                ((ToolStripDropDownItem)((ToolStrip)(((TabControl)(Parent).Parent).SelectedTab.Controls.Find("toolStripScriptContent", true)[0])).Items[2]).DropDownItems[0].PerformClick();
                return;
            }

            if (tcCodeContents.SelectedTab == null || tcCodeContents.SelectedTab.Controls.Count == 0)
                return;

            var control = (CodeEditorScintilla)tcCodeContents.SelectedTab.Controls[0];

            control.Find(false, this);
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((TabControl)(Parent).Parent).SelectedTab != Parent)
            {
                ((ToolStripDropDownItem)((ToolStrip)(((TabControl)(Parent).Parent).SelectedTab.Controls.Find("toolStripScriptContent", true)[0])).Items[2]).DropDownItems[1].PerformClick();
                return;
            }

            if (tcCodeContents.SelectedTab == null || tcCodeContents.SelectedTab.Controls.Count == 0)
                return;

            var control = (CodeEditorScintilla)tcCodeContents.SelectedTab.Controls[0];

            control.Find(true, this);
        }

        private void goToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((TabControl)(Parent).Parent).SelectedTab != Parent)
            {
                ((ToolStripDropDownItem)((ToolStrip)(((TabControl)(Parent).Parent).SelectedTab.Controls.Find("toolStripScriptContent", true)[0])).Items[2]).DropDownItems[1].PerformClick();
                return;
            }

            if (tcCodeContents.SelectedTab == null || tcCodeContents.SelectedTab.Controls.Count == 0)
                return;

            var control = (CodeEditorScintilla)tcCodeContents.SelectedTab.Controls[0];

            control.GoToLine();
        }

        private void tsbMinifyJS_Click(object sender, EventArgs e)
        {
            if (tcCodeContents.SelectedTab == null || tcCodeContents.SelectedTab.Controls.Count == 0)
                return;

            if (DialogResult.Yes ==
                MessageBox.Show(this,
                    "Are you sure you want to compress this script? After saving the compressed script, you won't be able to retrieve original content",
                    "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                ((CodeEditorScintilla)tcCodeContents.SelectedTab.Controls[0]).MinifyJs();
        }

        private void tsbBeautify_Click(object sender, EventArgs e)
        {
            if (tcCodeContents.SelectedTab == null || tcCodeContents.SelectedTab.Controls.Count == 0)
                return;

            ((CodeEditorScintilla)tcCodeContents.SelectedTab.Controls[0]).Beautify();
        }

        private void tsbGetLatestVersion_Click(object sender, EventArgs e)
        {
            if (tcCodeContents.SelectedTab == null || tcCodeContents.SelectedTab.Controls.Count == 0)
                return;

            var ci = tcCodeContents.SelectedTab.Tag as CodeItem;
            if (ci == null)
            {
                return;
            }

            ci.Refresh(Service);

            ((CodeEditorScintilla)tcCodeContents.SelectedTab.Controls[0]).RefreshContent(ci.Content);
        }

        private void tsbComment_Click(object sender, EventArgs e)
        {
            if (tcCodeContents.SelectedTab == null || tcCodeContents.SelectedTab.Controls.Count == 0)
                return;

            ((CodeEditorScintilla)tcCodeContents.SelectedTab.Controls[0]).CommentSelectedLines();
        }

        private void tsbnUncomment_Click(object sender, EventArgs e)
        {
            if (tcCodeContents.SelectedTab == null || tcCodeContents.SelectedTab.Controls.Count == 0)
                return;

            ((CodeEditorScintilla)tcCodeContents.SelectedTab.Controls[0]).UncommentSelectedLines();
        }

        #endregion Code content events

        #region Tab management

        private TabPage rightClickedTabPage;

        private void tcCodeContents_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = tcCodeContents.SelectedTab?.Tag as CodeItem;
            if (item == null)
            {
                return;
            }

            lblItemSelected.Text = $"{item.Parent.Name} {(item.Parent is WebPage ? "(" + item.Type + ")" : "")}";
            lblItemSelected.ForeColor = tcCodeContents.SelectedTab.ForeColor;
        }

        private void closeThisTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseTabs(new List<TabPage> { rightClickedTabPage });
        }

        private void closeAllTabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseTabs(tcCodeContents.TabPages.Cast<TabPage>());
        }

        private void colseAllButThisTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var list = tcCodeContents.TabPages.Cast<TabPage>().ToList();
            list.Remove(rightClickedTabPage);
            CloseTabs(list);
        }

        private void CloseTabs(IEnumerable<TabPage> tabs)
        {
            foreach (var tab in tabs)
            {
                tcCodeContents.TabPages.Remove(tab);
            }
        }

        private void tcCodeContents_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tcCodeContents.TabCount; ++i)
                {
                    if (tcCodeContents.GetTabRect(i).Contains(e.Location))
                    {
                        rightClickedTabPage = (TabPage)tcCodeContents.Controls[i];
                    }
                }
                cmsTab.Show(tcCodeContents, e.Location);
            }
        }

        #endregion Tab management

        #region Other methods

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

        private void CodeItem_StateChanged(object sender, EventArgs e)
        {
            var ci = (CodeItem)sender;

            if (tcCodeContents.SelectedTab.Tag == ci)
            {
                tcCodeContents.SelectedTab.ForeColor = ci.Node.ForeColor;
                lblItemSelected.ForeColor = ci.Node.ForeColor;

                switch (ci.State)
                {
                    case CodeItemState.None:
                        tcCodeContents.SelectedTab.Text = ci.Parent.Name;
                        tsmiSave.Enabled = false;
                        tsmiUpdate.Enabled = false;
                        break;

                    case CodeItemState.Draft:
                        tcCodeContents.SelectedTab.Text = $"{ci.Parent.Name} *";
                        tsmiSave.Enabled = true;
                        tsmiUpdate.Enabled = false;
                        break;

                    case CodeItemState.Saved:
                        tcCodeContents.SelectedTab.Text = $"{ci.Parent.Name} !";
                        tsmiSave.Enabled = false;
                        tsmiUpdate.Enabled = true;
                        break;
                }
            }
        }

        #endregion Other methods
    }
}