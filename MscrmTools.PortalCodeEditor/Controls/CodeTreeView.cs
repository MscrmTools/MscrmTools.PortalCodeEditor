using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MscrmTools.PortalCodeEditor.AppCode;
using MscrmTools.PortalCodeEditor.AppCode.EventArgs;

namespace MscrmTools.PortalCodeEditor.Controls
{
    public partial class CodeTreeView : UserControl
    {
        private List<EditablePortalItem> portalItems;

        public List<EditablePortalItem> CheckedItems
        {
            get
            {
                var nodes = new List<TreeNode>();

                GetNodes(nodes, tvCodeItems, true);

                return nodes.Select(n => ((CodeItem)n.Tag).Parent).ToList();
            }
        }

        public object SelectedItem => tvCodeItems.SelectedNode?.Tag;

        public bool IsLegacyPortal { get; set; }

        public CodeTreeView()
        {
            InitializeComponent();

            ToolTip tip = new ToolTip();
            tip.SetToolTip(chkSearchInContent, "Allow to search also in code content");
        }

        public event EventHandler<PortalItemSelectedEventArgs> PortalItemSelected;

        public event EventHandler ActionRequested;

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
                    var rootNode = new TreeNode(name);

                    rootNodes.Add(websiteReference.Id, rootNode);

                    parentNode = rootNode;

                    parentNode.Nodes.Add(new TreeNode("Web Pages") { Name = "WebPage" });

                    if (!IsLegacyPortal)
                    {
                        parentNode.Nodes.Add(new TreeNode("Entity Forms") { Name = "EntityForm" });
                        parentNode.Nodes.Add(new TreeNode("Entity Lists") { Name = "EntityList" });
                        parentNode.Nodes.Add(new TreeNode("Web Forms") { Name = "WebForm" });
                        parentNode.Nodes.Add(new TreeNode("Web Templates") { Name = "WebTemplate" });
                    }

                    parentNode.Nodes.Add(new TreeNode("Web Files") { Name = "WebFile" });
                }

                TreeNode typeNode;

                if (item is WebPage)
                {
                    typeNode = parentNode.Nodes["WebPage"];

                    WebPage page = (WebPage)item;
                    page.JavaScript.StateChanged += JavaScript_StateChanged;
                    page.Style.StateChanged += JavaScript_StateChanged;

                    TreeNode node;
                    if (page.IsRoot || page.ParentPageId == Guid.Empty)
                    {
                        node = new TreeNode(page.Name) { Tag = item };
                        typeNode.Nodes.Add(node);

                        if (isLegacyPortal)
                        {
                            var scriptNode = new TreeNode("JavaScript") { Tag = page.JavaScript };
                            page.JavaScript.Node = scriptNode;
                            var styleNode = new TreeNode("Style") { Tag = page.Style };
                            page.Style.Node = styleNode;

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

                        var scriptNode = new TreeNode("JavaScript") { Tag = page.JavaScript };
                        page.JavaScript.Node = scriptNode;
                        var styleNode = new TreeNode("Style") { Tag = page.Style };
                        page.Style.Node = styleNode;

                        node.Nodes.Add(scriptNode);
                        node.Nodes.Add(styleNode);

                        parentPageNode.Nodes.Add(node);
                    }
                }
                else if (item is EntityForm)
                {
                    typeNode = parentNode.Nodes["EntityForm"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Entity Forms") { Name = "EntityForm" };
                        rootNodes[item.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    EntityForm form = (EntityForm)item;
                    form.JavaScript.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(form.Name) { Tag = form.JavaScript };
                    form.JavaScript.Node = node;

                    typeNode.Nodes.Add(node);
                }
                else if (item is EntityList)
                {
                    typeNode = parentNode.Nodes["EntityList"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Entity Lists") { Name = "EntityList" };
                        rootNodes[item.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    EntityList list = (EntityList)item;
                    list.JavaScript.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(list.Name) { Tag = list.JavaScript };
                    list.JavaScript.Node = node;

                    typeNode.Nodes.Add(node);
                }
                else if (item is WebTemplate)
                {
                    typeNode = parentNode.Nodes["WebTemplate"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Web Templates") { Name = "WebTemplate" };
                        rootNodes[item.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    WebTemplate template = (WebTemplate)item;
                    template.Code.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(template.Name) { Tag = template.Code };
                    template.Code.Node = node;

                    typeNode.Nodes.Add(node);
                }
                else if (item is WebFile)
                {
                    typeNode = parentNode.Nodes["WebFile"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Web Files") { Name = "WebFile" };
                        rootNodes[item.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    WebFile file = (WebFile)item;
                    file.Code.StateChanged += JavaScript_StateChanged;

                    var node = new TreeNode(file.Name) { Tag = file.Code };
                    file.Code.Node = node;

                    typeNode.Nodes.Add(node);
                }
                else if (item is WebFormStep)
                {
                    typeNode = parentNode.Nodes["WebForm"];

                    if (typeNode == null)
                    {
                        typeNode = new TreeNode("Web Forms") { Name = "WebForm" };
                        rootNodes[item.WebsiteReference.Id].Nodes.Add(typeNode);
                    }

                    WebFormStep wfStep = (WebFormStep)item;
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

                tvCodeItems.Nodes.Add(node);
                node.Expand();
            }
        }

        private void ApplyCounting(TreeNode parentNode, string nodeName)
        {
            if (parentNode.Nodes.ContainsKey(nodeName))
            {
                parentNode.Nodes[nodeName].Text += $" ({parentNode.Nodes[nodeName].Nodes.Count})";
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

        private void JavaScript_StateChanged(object sender, EventArgs e)
        {
            Item_UpdateRequired(null, null);
        }

        private void Item_UpdateRequired(object sender, EventArgs e)
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
        }

        private void tvCodeItems_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PortalItemSelected?.Invoke(this, new PortalItemSelectedEventArgs(e.Node.Tag as CodeItem));
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in tvCodeItems.Nodes)
                node.Checked = chkSelectAll.Checked;
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

        private void tvCodeItems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var targetNode = tvCodeItems.GetNodeAt(e.X, e.Y);

            if (targetNode?.Tag == null)
            {
                return;
            }

            tvCodeItems.SelectedNode = targetNode;

            cmsTreeview.Show(tvCodeItems, e.Location);
        }

        private void tvCodeItems_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var targetNode = tvCodeItems.GetNodeAt(e.X, e.Y);

            if (targetNode?.Tag == null)
            {
                return;
            }

            PortalItemSelected?.Invoke(this, new PortalItemSelectedEventArgs(targetNode.Tag as CodeItem));
        }

        #region Search methods

        private Thread searchThread;

        private void txtSearch_TextChanged(object sender, EventArgs e)
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

        private void chkSearchInContent_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.BackColor = SystemColors.Window;
            searchThread?.Abort();
            searchThread = new Thread(DisplayFiltered);
            searchThread.Start();
        }

        #endregion Search methods

        private void llApplyChanges_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ActionRequested?.Invoke(this, new UpdatePendingChangesEventArgs(portalItems.Where(p => p.HasPendingChanges)));
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void refreshFromPortalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = tvCodeItems.SelectedNode.Tag;

            if (item == null)
            {
                return;
            }

            ActionRequested?.Invoke(this, new RefreshContentEventArgs(item));
        }
    }
}