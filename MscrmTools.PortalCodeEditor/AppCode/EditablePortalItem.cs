using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

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

        public EntityReference WebsiteReference { get; protected set; }
        public string Name { get; protected set; }
        public List<CodeItem> Items { get; }
        public Guid Id { get; protected set; }

        public bool HasPendingChanges
        {
            get { return hasPendingChanges; }
            set
            {
                hasPendingChanges = value;
                UpdateRequired?.Invoke(this, new System.EventArgs());
            }
        }

        #endregion Properties

        #region Methods

        public abstract void Update(IOrganizationService service, bool forceUpdate = false);

        public abstract string RefreshContent(CodeItem item, IOrganizationService service);

        #endregion Methods
    }
}