using System;

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
        private string liquidObjectColor;
        private string liquidTagColor;

        public event EventHandler OnColorChanged;

        public bool ForceUpdate { get; set; }

        public string LiquidObjectColor
        {
            get
            {
                return liquidObjectColor;
            }
            set
            {
                liquidObjectColor = value;
                OnColorChanged?.Invoke(this, new EventArgs());
            }
        }

        public string LiquidTagColor
        {
            get
            {
                return liquidTagColor;
            }
            set
            {
                liquidTagColor = value;
                OnColorChanged?.Invoke(this, new EventArgs());
            }
        }

        public bool ObfuscateJavascript { get; set; }
        public bool RemoveCssComments { get; set; }
        public bool UseEnhancedDataModel { get; set; }
    }
}