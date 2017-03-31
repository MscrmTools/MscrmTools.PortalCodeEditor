using System.Drawing;
using System.Windows.Forms;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public static class Extensions
    {
        public static void ChangeNodeAndParentColor(this TreeNode node, Color newColor)
        {
            node.ForeColor = newColor;

            var parentNode = node.Parent;

            if (newColor == Color.Red)
            {
                while (parentNode != null)
                {
                    parentNode.ForeColor = newColor;
                    parentNode = parentNode.Parent;
                }
            }
            else if (newColor == Color.Blue)
            {
                bool hasRed = false;

                while (parentNode != null)
                {
                    foreach (TreeNode childNode in parentNode.Nodes)
                    {
                        if (childNode.ForeColor == Color.Red)
                        {
                            hasRed = true;
                            break;
                        }
                    }

                    if (hasRed)
                    {
                        return;
                    }

                    parentNode.ForeColor = newColor;
                    parentNode = parentNode.Parent;
                }
            }
            else if (newColor == Color.Empty)
            {
                bool areAllBlack = true;

                while (parentNode != null)
                {
                    foreach (TreeNode childNode in parentNode.Nodes)
                    {
                        if (childNode.ForeColor != Color.Empty)
                        {
                            areAllBlack = false;
                            break;
                        }
                    }

                    if (!areAllBlack)
                    {
                        return;
                    }

                    parentNode.ForeColor = newColor;
                    parentNode = parentNode.Parent;
                }
            }
        }
    }
}

