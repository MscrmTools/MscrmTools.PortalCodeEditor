using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk;

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

        public static T GetAliasedValue<T>(this Entity record, string alias, string attribute)
        {
            var aliasedValue = record.GetAttributeValue<AliasedValue>($"{alias}.{attribute}");
            if (aliasedValue == null)
            {
                return default(T);
            }

            if (aliasedValue.Value is T)
            {
                return (T)aliasedValue.Value;
            }

            throw new Exception($"{alias}.{attribute} is not of type {typeof(T)}");
        }
    }
}