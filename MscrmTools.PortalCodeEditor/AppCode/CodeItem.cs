using Microsoft.Xrm.Sdk;
using System;
using System.Drawing;
using System.IO;
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

        #endregion Variables

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
                encodedContent = data;
                // use StreamReader to auto-detect encoding
                using (var stream = new StreamReader(new MemoryStream(Convert.FromBase64String(data)), true))
                {
                    content = stream.ReadToEnd();
                }
            }
            else
            {
                content = data;
            }
        }

        #endregion Constructor

        #region Properties

        public string Content
        {
            get => content;
            set
            {
                content = value;
                Parent.HasPendingChanges = true;
            }
        }

        public string EncodedContent
        {
            set => encodedContent = value;
            get
            {
                encodedContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
                return encodedContent;
            }
        }

        public bool IsEncoded { get; set; }

        public TreeNode Node { get; set; }

        public EditablePortalItem Parent { get; }

        public CodeItemState State
        {
            get => state;

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

        #endregion Properties

        #region Events

        public event EventHandler StateChanged;

        #endregion Events

        #region Methods

        public void Refresh(IOrganizationService service)
        {
            content = Parent.RefreshContent(this, service);
            State = CodeItemState.None;
        }

        #endregion Methods
    }
}