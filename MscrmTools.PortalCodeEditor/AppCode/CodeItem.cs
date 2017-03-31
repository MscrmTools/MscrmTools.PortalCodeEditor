using Microsoft.Xrm.Sdk;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public enum CodeItemState
    {
        None,
        Draft,
        Saved
    }

    public enum CodeItemType
    {
        JavaScript,
        Style, 
        LiquidTemplate
    }

    public class CodeItem
    {
        #region Variables

        private string content;

        private string encodedContent;

        private CodeItemState state;

        #endregion

        #region Constructor

        public CodeItem(string data, CodeItemType type, bool isEncoded, EditablePortalItem parent)
        {
            Type = type;
            state = CodeItemState.None;
            Parent = parent;

            if (string.IsNullOrEmpty(data))
            {
                content = string.Empty;
                return;
            }

            if (isEncoded)
            {
                EncodedContent = data;
                content = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            }
            else
            {
                content = data;
            }
        }

        #endregion

        #region Properties

        public string EncodedContent
        {
            set { encodedContent = value; }
            get
            {
                encodedContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
                return encodedContent;
            }
        } 

        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                Parent.HasPendingChanges = true;
            }
        }

        public bool IsEncoded { get; set; }

        public CodeItemState State
        {
            get { return state; }

            set
            {
                state = value;

                switch (state)
                {
                    case CodeItemState.Draft:
                    {
                        Node.ChangeNodeAndParentColor(Color.Red);
                    }
                        break;
                    case CodeItemState.Saved:
                    {
                        Node.ChangeNodeAndParentColor(Color.Blue);

                        Parent.HasPendingChanges = true;
                    }
                        break;
                    case CodeItemState.None:
                    {
                        Node.ChangeNodeAndParentColor(Color.Empty);

                        if (Parent.Items.All(i => i.State != CodeItemState.Saved))
                        {
                            Parent.HasPendingChanges = false;
                        }
                    }
                        break;
                }

                StateChanged?.Invoke(this, new System.EventArgs());
            }
            
        }
        public CodeItemType Type { get; set; }
        public TreeNode Node { get; set; }
        public EditablePortalItem Parent { get; }

        #endregion

        #region Events

        public event EventHandler StateChanged;

        #endregion

        #region Methods

        public void Refresh(IOrganizationService service)
        {
            content = Parent.RefreshContent(this, service);
            State = CodeItemState.None;
        }

        #endregion
    }
}
