namespace MscrmTools.PortalCodeEditor.AppCode.EventArgs
{
    internal class RefreshContentEventArgs : System.EventArgs
    {
        public RefreshContentEventArgs(object item)
        {
            Item = item;
        }
        public object Item { get; }
    }
}