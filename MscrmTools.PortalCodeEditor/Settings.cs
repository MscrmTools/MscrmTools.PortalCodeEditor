namespace MscrmTools.PortalCodeEditor
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    public class Settings
    {
        public bool ForceUpdate { get; set; }
        public bool ObfuscateJavascript { get; set; }
        public bool RemoveCssComments { get; set; }
    }
}
