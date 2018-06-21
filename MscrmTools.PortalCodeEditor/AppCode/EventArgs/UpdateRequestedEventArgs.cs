namespace MscrmTools.PortalCodeEditor.AppCode.EventArgs
{
    public class UpdateRequestedEventArgs : System.EventArgs
    {
        public UpdateRequestedEventArgs(CodeItem item)
        {
            Item = item;
        }

        public CodeItem Item { get; }
    }
}