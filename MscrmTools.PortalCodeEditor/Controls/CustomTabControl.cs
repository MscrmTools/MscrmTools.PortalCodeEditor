using System.Drawing;
using System.Windows.Forms;
using MscrmTools.PortalCodeEditor.AppCode;

namespace MscrmTools.PortalCodeEditor.Controls
{
    public class CustomTabControl : TabControl
    {
        private const int LEADING_SPACE = 12;
        private const int CLOSE_AREA = 15;

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            var ci = TabPages[e.Index].Tag as CodeItem;
            if (ci == null)
            {
                base.OnDrawItem(e);
                return;
            }

            if (e.Index == SelectedIndex)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.DarkGray), e.Bounds);
            }

            var color = ci.State == CodeItemState.Draft
                ? Color.Red
                : ci.State == CodeItemState.Saved ? Color.Blue : Color.Black;

            //This code will render a "x" mark at the end of the Tab caption.
            e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - CLOSE_AREA, e.Bounds.Top + 4);
            //e.Graphics.DrawString(TabPages[e.Index].Text, e.Font, new SolidBrush(color), e.Bounds.Left + LEADING_SPACE, e.Bounds.Top + 4);
            e.Graphics.DrawString(ci.Parent.Name, e.Font, new SolidBrush(color), e.Bounds.Left + LEADING_SPACE, e.Bounds.Top + 4);
            e.DrawFocusRectangle();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            RectangleF tabTextArea = GetTabRect(SelectedIndex);
            tabTextArea = new RectangleF(tabTextArea.X + tabTextArea.Width - CLOSE_AREA, tabTextArea.Y, 13, 13);
            Point pt = new Point(e.X, e.Y);
            if (tabTextArea.Contains(pt))
            {
                var wr = SelectedTab.Tag as CodeItem;
                if (wr == null)
                {
                    TabPages.Remove(SelectedTab);
                    return;
                }

                if (wr.State == CodeItemState.Draft)
                {
                    //if (Options.Instance.AutoSaveWhenLeaving)
                    //{
                    //    wr.Save();
                    //}
                    //else
                    {
                        var message = "You did not save your changes. Are you sure you want to close this tab?";
                        if (
                            MessageBox.Show(this, message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                            DialogResult.No)
                        {
                            return;
                        }
                        //else
                        //{
                        //    wr.CancelChange();
                        //}
                        //}
                    }

                    TabPages.Remove(SelectedTab);
                }
                else
                {
                    TabPages.Remove(SelectedTab);
                }
            }
        }
    }
}