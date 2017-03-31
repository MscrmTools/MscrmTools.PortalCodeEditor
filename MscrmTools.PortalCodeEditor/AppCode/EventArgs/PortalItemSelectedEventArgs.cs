namespace MscrmTools.PortalCodeEditor.AppCode.EventArgs
{
    public class PortalItemSelectedEventArgs
    {
        public PortalItemSelectedEventArgs(CodeItem item)
        {
            Item = item;
        }

        public CodeItem Item { get; set; }
    }
}