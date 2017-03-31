using System.Collections.Generic;

namespace MscrmTools.PortalCodeEditor.AppCode.EventArgs
{
    internal class UpdatePendingChangesEventArgs : System.EventArgs
    {
        public UpdatePendingChangesEventArgs(IEnumerable<EditablePortalItem> items)
        {
            Items = items;
        }

        public IEnumerable<EditablePortalItem> Items { get; }
    }
}