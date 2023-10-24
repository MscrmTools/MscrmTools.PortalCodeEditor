using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public abstract class EditablePortalItem
    {
        #region Variables

        private bool hasPendingChanges;

        #endregion Variables

        #region Constructor

        protected EditablePortalItem()
        {
            Items = new List<CodeItem>();
        }

        #endregion Constructor

        #region Events

        public event EventHandler UpdateRequired;

        #endregion Events

        #region Properties

        public bool HasPendingChanges
        {
            get { return hasPendingChanges; }
            set
            {
                hasPendingChanges = value;
                UpdateRequired?.Invoke(this, new System.EventArgs());
            }
        }

        public Guid Id { get; protected set; }
        public List<CodeItem> Items { get; }
        public string Name { get; protected set; }
        public EntityReference WebsiteReference { get; protected set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Ensure this name is ok for a file or folder
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>>
        /// <returns></returns>
        public static string EscapeForFileName(string str)
        {
            if (str == null) return null;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.Trim();
        }

        /// <summary>
        /// Escape the name so it can be used for file or folder
        /// </summary>
        /// <returns></returns>
        public string EscapeName()
        {
            return EscapeForFileName(Name);
        }

        public abstract string RefreshContent(CodeItem item, IOrganizationService service, bool isEnhancedModel);

        public abstract void Update(IOrganizationService service, bool forceUpdate, bool isEnhancedModel);

        /// <summary>
        /// Write the contents of the code object to disk
        /// </summary>
        /// <param name="path">Path to which the content will be written</param>
        public abstract void WriteContent(string path);

        #endregion Methods
    }
}