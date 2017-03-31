using System.Drawing;
using System.Windows.Forms;

namespace MscrmTools.PortalCodeEditor.AppCode.EventArgs
{
    public class ContextMenuRequestedEventArgs
    {
        public ContextMenuRequestedEventArgs(TreeNode selectedNode, Point location)
        {
            SelectedNode = selectedNode;
            Location = location;
        }

        public Point Location { get; set; }
        public TreeNode SelectedNode { get; set; }
    }
}