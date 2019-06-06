using Microsoft.Xrm.Sdk;
using MscrmTools.PortalCodeEditor.AppCode;
using MscrmTools.PortalCodeEditor.AppCode.EventArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MscrmTools.PortalCodeEditor.Forms
{
    public partial class CodeTreeViewForm : DockContent
    {
        private List<EditablePortalItem> portalItems;

        public CodeTreeViewForm()
        {
            InitializeComponent();

            ToolTip tip = new ToolTip();
            tip.SetToolTip(chkSearchInContent, "Allow to search also in code content");
        }

        public event EventHandler ActionRequested;

        public event EventHandler<PortalItemSelectedEventArgs> PortalItemSelected;

        public List<EditablePortalItem> CheckedItems
        {
            get
            {
                var nodes = new List<TreeNode>();

                GetNodes(nodes, tvCodeItems, true);

                return nodes.Select(n => ((CodeItem)n.Tag).Parent).ToList();
            }
        }

        public bool IsLegacyPortal { get; set; }
        public List<Entity> Languages { get; set; }
        public IOrganizationService Service { get; set; }

        public void DisplayCodeItems(List<EditablePortalItem> items, bool isLegacyPortal)
        {
            IsLegacyPortal = isLegacyPortal;
            portalItems = items;
            tvCodeItems.Nodes.Clear();
            var rootNodes = new Dictionary<Guid, TreeNode>();

            if (isLegacyPortal)
            {
                rootNodes.Add(Guid.Empty, new TreeNode("(Not website related)"));
            }

            var searchText = txtSearch.Text.ToLower();
            var filteredItems = items.Where(i => searchText.Length == 0
                                                 || i.Name.ToLower().Contains(searchText)
                                                 || chkSearchInContent.Checked &&
                                                 i.Items.Any(i2 => i2.Content.ToLower().Contains(searchText)))
                .ToList();

            if (!filteredItems.Any() && searchText.Length > 0)
            {
                txtSearch.BackColor = Color.LightCoral;
                return;
            }

            foreach (var item in filteredItems)
            {
                item.UpdateRequired += Item_UpdateRequired;
                var websiteReference = item.WebsiteReference;

                if (websiteReference == null) { continue; }

                TreeNode parentNode;

                if (rootNodes.ContainsKey(websiteReference.Id))
                {
                    parentNode = rootNodes[websiteReference.Id];
                }
                else
                {
                    var name = websiteReference.Id == Guid.Empty ? "(Not website related)" : websiteReference.Name;
                    var rootNode = new TreeNode(name) { Tag = websiteReference };

                    rootNodes.Add(websiteReference.Id, rootNode);
                    parentNode = rootNode;

                    parentNode.Nodes.Add(new TreeNode("Web Pages") { Name = "WebPage" });

                    if (!IsLegacyPortal)
                    {
                        parentNode.Nodes.Add(new TreeNode("Content Snippets") { Name = "ContentSnippet" });
                        parentNode.Nodes.Add(new TreeNode("Entity Forms") { Name = "EntityForm" });
                        parentNode.Nodes.Add(new TreeNode("Entity Lists") { Name = "EntityList" });
                        parentNode.Nodes.Add(new TreeNode("Web Forms") { Name = "WebForm" });
                        parentNode.Nodes.Add(new TreeNode("Web Templates") { Name = "WebTemplate" });
                    }

                    parentNode.Nodes.Add(new TreeNode("Web Files") { Name = "WebFile" });
                }

                TreeNode typeNode;

                if (item is WebPage page)
                {
                    typeNode = parentNode.Nodes["WebPage"];

                    page.JavaScript.StateChanged += JavaScript_StateChanged;
                    page.Style.StateChanged += JavaScript_StateChanged;

                    TreeNode node;
                    if (page.IsRoot || page.ParentPageId == Guid.Empty)
                    {
                        node = new TreeNode(page.Name) { Tag = item };
                        typeNode.Nodes.Add(node);

                        if (isLegacyPortal)
                        {
                            var copyNode = new TreeNode("Content") { Tag = page.Copy };
                            page.Copy.Node = copyNode;
                            var scriptNode = new TreeNode("JavaScript") { Tag = page.JavaScript };
                            page.JavaScript.Node = scriptNode;
                            var styleNode = new TreeNode("Style") { Tag = page.Style };
                            page.Style.Node = styleNode;

                            node.Nodes.Add(copyNode);
                            node.Nodes.Add(scriptNode);
                            node.Nodes.Add(styleNode);
                        }
                    }
                    else
                    {
                        var parentPageNode = typeNode.Nodes.Cast<TreeNode>().FirstOrDefault(t => ((WebPage)t.Tag).Id == page.ParentPageId);
                        if (parentPageNode == null)
                        {
                            continue;
                        }

                        node = new TreeNode(page.Language) { Tag = item };

                        var copyNode = new TreeNode("Content") { Tag = page.Copy };
                        page.Copy.Node = copyNode;
                        var scriptNode = new TreeNode("JavaScript") { Tag = page.JavaScript };
                        page.JavaScript.Node = scriptNode;
                        var styleNode = new TreeNode("Style") { Tag = page.Style };
                        page.Style.Node = styleNode;

                        node.Nodes.Add(copyNode);
                        node.Nodes.Add(scriptNode);
                        node.Nodes.Add(styleNode);

                        parentPageNode.Nodes.Add(node);
                    }
                }
                else if (item is EntityForm form)
                {
                    typeNode = parentNode.Nodes["EntityForm"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Entity Forms") { Name = "EntityForm" };
                        rootNodes[form.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    form.JavaScript.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(form.Name) { Tag = form.JavaScript };
                    form.JavaScript.Node = node;

                    typeNode.Nodes.Add(node);
                }
                else if (item is EntityList list)
                {
                    typeNode = parentNode.Nodes["EntityList"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Entity Lists") { Name = "EntityList" };
                        rootNodes[list.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    list.JavaScript.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(list.Name) { Tag = list.JavaScript };
                    list.JavaScript.Node = node;

                    typeNode.Nodes.Add(node);
                }
                else if (item is WebTemplate template)
                {
                    typeNode = parentNode.Nodes["WebTemplate"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Web Templates") { Name = "WebTemplate" };
                        rootNodes[template.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    template.Code.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(template.Name) { Tag = template.Code };
                    template.Code.Node = node;

                    typeNode.Nodes.Add(node);
                }
                else if (item is WebFile file)
                {
                    typeNode = parentNode.Nodes["WebFile"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Web Files") { Name = "WebFile" };
                        rootNodes[file.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    file.Code.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(file.Name) { Tag = file.Code };
                    file.Code.Node = node;

                    typeNode.Nodes.Add(node);
                }
                else if (item is WebFormStep wfStep)
                {
                    typeNode = parentNode.Nodes["WebForm"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Web Forms") { Name = "WebForm" };
                        rootNodes[wfStep.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    wfStep.JavaScript.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(wfStep.Name) { Tag = wfStep.JavaScript };
                    wfStep.JavaScript.Node = node;

                    if (wfStep.WebFormReference != null)
                    {
                        TreeNode webFormNode;

                        if (typeNode.Nodes.ContainsKey(wfStep.WebFormReference.Name))
                        {
                            webFormNode = typeNode.Nodes[wfStep.WebFormReference.Name];
                        }
                        else
                        {
                            webFormNode = new TreeNode(wfStep.WebFormReference.Name)
                            {
                                Name = wfStep.WebFormReference.Name
                            };

                            typeNode.Nodes.Add(webFormNode);
                        }

                        webFormNode.Nodes.Add(node);
                    }
                    else
                    {
                        typeNode.Nodes.Add(node);
                    }
                }
                else if (item is ContentSnippet snippet)
                {
                    typeNode = parentNode.Nodes["ContentSnippet"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Content Snippets") { Name = "ContentSnippet" };
                        rootNodes[snippet.WebsiteReference.Id].Nodes.Add(typeNode);
                        var htmlSnippetsNode = new TreeNode("Html") { Name = "Html" };
                        var textSnippetsNode = new TreeNode("Text") { Name = "Text" };

                        typeNode.Nodes.Add(htmlSnippetsNode);
                        typeNode.Nodes.Add(textSnippetsNode);
                    }
                    else if (typeNode.Nodes["Html"] == null)
                    {
                        var htmlSnippetsNode = new TreeNode("Html") { Name = "Html" };
                        var textSnippetsNode = new TreeNode("Text") { Name = "Text" };
                        typeNode.Nodes.Add(htmlSnippetsNode);
                        typeNode.Nodes.Add(textSnippetsNode);
                    }

                    snippet.Code.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(snippet.Name) { Tag = snippet.Code };
                    snippet.Code.Node = node;

                    if (snippet.Type == "Text")
                    {
                        typeNode.Nodes["Text"].Nodes.Add(node);
                    }
                    else
                    {
                        typeNode.Nodes["Html"].Nodes.Add(node);
                    }
                }
                else
                {
                    throw new Exception($"Unsupported portal item type: {item.GetType().Name}");
                }
            }

            foreach (var node in rootNodes.Values)
            {
                ApplyCounting(node, "WebPage");
                ApplyCounting(node, "EntityForm");
                ApplyCounting(node, "EntityList");
                ApplyCounting(node, "WebTemplate");
                ApplyCounting(node, "WebFile");
                ApplyCounting(node, "WebForm");
                CountContentSnippet(node);

                tvCodeItems.Nodes.Add(node);
                tvCodeItems.Sort();
                node.Expand();
            }
        }

        private void ApplyCounting(TreeNode parentNode, string nodeName)
        {
            if (parentNode.Nodes.ContainsKey(nodeName))
            {
                var trimedName = parentNode.Nodes[nodeName].Text.Split('(')[0]?.Trim();

                parentNode.Nodes[nodeName].Text = $"{trimedName} ({parentNode.Nodes[nodeName].Nodes.Count})";
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in tvCodeItems.Nodes)
                node.Checked = chkSelectAll.Checked;
        }

        private void cmsTreeview_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == tsmiRefreshFromPortal)
            {
                var item = tvCodeItems.SelectedNode.Tag;

                if (item == null)
                {
                    return;
                }

                ActionRequested?.Invoke(this, new RefreshContentEventArgs(item));
            }
            else if (e.ClickedItem == tsmiUpdate)
            {
                var item = tvCodeItems.SelectedNode.Tag;

                if (item == null)
                {
                    return;
                }

                var portalItem = item as EditablePortalItem;
                var epi = portalItem ?? ((CodeItem)item).Parent;

                ActionRequested?.Invoke(this, new UpdatePendingChangesEventArgs(new List<EditablePortalItem> { epi }));
            }
            else if (e.ClickedItem == tsmiCreateNewItem)
            {
                var node = tvCodeItems.SelectedNode;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }

                if (tvCodeItems.SelectedNode.Name == "WebTemplate")
                {
                    var dialog = new NewWebTemplateForm(Service, node.Tag as EntityReference);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var newTemplate = new WebTemplate(dialog.Template);
                        newTemplate.Code.StateChanged += JavaScript_StateChanged;

                        var newNode = new TreeNode(newTemplate.Name) { Tag = newTemplate.Code };
                        newTemplate.Code.Node = newNode;

                        tvCodeItems.SelectedNode.Nodes.Add(newNode);
                        tvCodeItems.Sort();

                        ApplyCounting(node, "WebTemplate");
                    }
                }
                else if (tvCodeItems.SelectedNode.Name == "Html")
                {
                    var dialog = new NewContentSnippetForm(756150001, Service, node.Tag as EntityReference, Languages);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var newContentSnippet = new ContentSnippet(dialog.Template);
                        newContentSnippet.Code.StateChanged += JavaScript_StateChanged;

                        var newNode = new TreeNode(newContentSnippet.Name) { Tag = newContentSnippet.Code };
                        newContentSnippet.Code.Node = newNode;

                        tvCodeItems.SelectedNode.Nodes.Add(newNode);
                        tvCodeItems.Sort();

                        CountContentSnippet(node);
                    }
                }
                else if (tvCodeItems.SelectedNode.Name == "Text")
                {
                    var dialog = new NewContentSnippetForm(756150000, Service, node.Tag as EntityReference, Languages);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var newContentSnippet = new ContentSnippet(dialog.Template);
                        newContentSnippet.Code.StateChanged += JavaScript_StateChanged;

                        var newNode = new TreeNode(newContentSnippet.Name) { Tag = newContentSnippet.Code };
                        newContentSnippet.Code.Node = newNode;

                        tvCodeItems.SelectedNode.Nodes.Add(newNode);
                        tvCodeItems.Sort();

                        CountContentSnippet(node);
                    }
                }
            }
        }

        private void cmsTreeview_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var selectedNode = tvCodeItems.SelectedNode;
            tsmiUpdate.Visible = selectedNode.Name != "WebTemplate" && selectedNode.Name != "Html" && selectedNode.Name != "Text";
            tsmiRefreshFromPortal.Visible = selectedNode.Name != "WebTemplate" && selectedNode.Name != "Html" && selectedNode.Name != "Text";
            tsmiCreateNewItem.Visible = selectedNode.Name == "WebTemplate" || selectedNode.Name == "Html" || selectedNode.Name == "Text";

            tsmiCreateNewItem.Text = string.Format(tsmiCreateNewItem.Tag.ToString(), selectedNode.Name == "WebTemplate" ? "Web template" : "Content snippet");
        }

        private void CountContentSnippet(TreeNode parentNode)
        {
            if (parentNode.Nodes.ContainsKey("ContentSnippet"))
            {
                if (parentNode.Nodes["ContentSnippet"].Nodes.Count != 2)
                    return;

                var cSnippet = parentNode.Nodes["ContentSnippet"].Nodes[0].GetNodeCount(true);
                var cSnippet2 = parentNode.Nodes["ContentSnippet"].Nodes[1].GetNodeCount(true);

                ApplyCounting(parentNode.Nodes["ContentSnippet"], "Html");
                ApplyCounting(parentNode.Nodes["ContentSnippet"], "Text");

                var text = parentNode.Nodes["ContentSnippet"].Text.Split('(')[0].Trim();
                parentNode.Nodes["ContentSnippet"].Text = $"{text} ({cSnippet + cSnippet2})";
            }
        }

        private void GetNodes(ICollection<TreeNode> nodes, object parent, bool onlyCheckedNodes)
        {
            var tView = parent as TreeView;
            if (tView != null)
            {
                foreach (TreeNode node in tView.Nodes)
                {
                    if (onlyCheckedNodes && node.Checked || !onlyCheckedNodes)
                    {
                        var epi = node.Tag as CodeItem;
                        if (epi != null)
                        {
                            nodes.Add(node);
                        }
                    }

                    GetNodes(nodes, node, onlyCheckedNodes);
                }
            }
            else
            {
                foreach (TreeNode node in ((TreeNode)parent).Nodes)
                {
                    if (onlyCheckedNodes && node.Checked || !onlyCheckedNodes)
                        if (node.Tag is CodeItem)
                        {
                            nodes.Add(node);
                        }

                    GetNodes(nodes, node, onlyCheckedNodes);
                }
            }
        }

        private void Item_UpdateRequired(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                var pendingChangesItems = portalItems.Where(i => i.HasPendingChanges).ToList();
                if (pendingChangesItems.Any())
                {
                    lblPendingDetails.Text = string.Format(lblPendingDetails.Tag.ToString(), pendingChangesItems.Count,
                        pendingChangesItems.Count < 2 ? "" : "s",
                        pendingChangesItems.Select(p => p.WebsiteReference.Id).Distinct().Count() < 2 ? "" : "s");
                    pnlPendingChanges.Visible = true;
                }
                else
                {
                    pnlPendingChanges.Visible = false;
                }
            }));
        }

        private void JavaScript_StateChanged(object sender, EventArgs e)
        {
            Item_UpdateRequired(null, null);
        }

        private void llApplyChanges_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ActionRequested?.Invoke(this, new UpdatePendingChangesEventArgs(portalItems.Where(p => p.HasPendingChanges)));
        }

        private void llCollapseAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tvCodeItems.CollapseAll();

            foreach (TreeNode node in tvCodeItems.Nodes)
            {
                if (node.Parent == null)
                    node.Expand();
            }
        }

        private void llExpandAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tvCodeItems.ExpandAll();
        }

        private void tvCodeItems_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
        }

        private void tvCodeItems_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PortalItemSelected?.Invoke(this, new PortalItemSelectedEventArgs(e.Node.Tag as CodeItem));
        }

        private void tvCodeItems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var targetNode = tvCodeItems.GetNodeAt(e.X, e.Y);

            if (targetNode?.Tag == null && targetNode?.Name != "WebTemplate" && targetNode?.Name != "Html" && targetNode?.Name != "Text")
            {
                return;
            }

            tvCodeItems.SelectedNode = targetNode;

            cmsTreeview.Show(tvCodeItems, e.Location);
        }

        #region Search methods

        private Thread searchThread;

        private void chkSearchInContent_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.BackColor = SystemColors.Window;
            searchThread?.Abort();
            searchThread = new Thread(DisplayFiltered);
            searchThread.Start();
        }

        private void DisplayFiltered()
        {
            tvCodeItems.Invoke(new Action(() =>
            {
                tvCodeItems.Nodes.Clear();
                DisplayCodeItems(portalItems, IsLegacyPortal);
            }));
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            txtSearch.BackColor = SystemColors.Window;
            searchThread?.Abort();
            searchThread = new Thread(DisplayFiltered);
            searchThread.Start();
        }

        #endregion Search methods
    }
}