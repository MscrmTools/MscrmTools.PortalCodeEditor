using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public class ContentSnippet : EditablePortalItem
    {
        #region Constants
        public const string NODEKEY = "ContentSnippet";
        public const string NODENAME = "Content Snippets";
        #endregion

        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public ContentSnippet(Entity record)
        {
            Id = record.Id;
            Code = new CodeItem(record.GetAttributeValue<string>("adx_value"), CodeItemType.JavaScript, false, this);
            Name = record.GetAttributeValue<string>("adx_name");
            WebsiteReference = record.GetAttributeValue<EntityReference>("adx_websiteid") ?? new EntityReference("adx_website", Guid.Empty);

            Type = record.GetAttributeValue<OptionSetValue>("adx_type")?.Value == 756150000 ? "Text" : "Html";
            innerRecord = record;
            Items.Add(Code);
        }

        #endregion Constructor

        #region Properties

        public CodeItem Code { get; }
        public string Type { get; }

        #endregion Properties

        #region Methods

        public static List<ContentSnippet> GetItems(IOrganizationService service, ref bool isLegacyPortal)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression("adx_contentsnippet")
                {
                    ColumnSet = new ColumnSet("adx_name", "adx_value", "adx_type", "adx_websiteid"),
                    Orders = { new OrderExpression("adx_name", OrderType.Ascending) }
                }).Entities;

                return records.Select(record => new ContentSnippet(record)).ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    isLegacyPortal = true;
                    var records = service.RetrieveMultiple(new QueryExpression("adx_webtemplate")
                    {
                        ColumnSet = new ColumnSet("adx_name", "adx_value", "adx_type", "adx_websiteid"),
                        Orders = { new OrderExpression("adx_name", OrderType.Ascending) }
                    }).Entities;
                    return records.Select(record => new ContentSnippet(record)).ToList();
                }
                throw;
            }
        }

        public override string RefreshContent(CodeItem item, IOrganizationService service)
        {
            var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                new ColumnSet("adx_value"));
            innerRecord.RowVersion = record.RowVersion;
            return record.GetAttributeValue<string>("adx_value");
        }

        public override void Update(IOrganizationService service, bool forceUpdate = false)
        {
            innerRecord["adx_value"] = Code.Content;
            var updateRequest = new UpdateRequest
            {
                ConcurrencyBehavior = forceUpdate ? ConcurrencyBehavior.AlwaysOverwrite : ConcurrencyBehavior.IfRowVersionMatches,
                Target = innerRecord
            };
            service.Execute(updateRequest);
            var updatedRecord = service.Retrieve(innerRecord.LogicalName, innerRecord.Id, new ColumnSet());
            innerRecord.RowVersion = updatedRecord.RowVersion;
            Code.State = CodeItemState.None;
            HasPendingChanges = false;
        }
        /// <summary>
        /// Write the code item Content to disk
        /// </summary>
        /// <param name="path"></param>
        public override void WriteContent(string path)
        {
            var ext = (Type == "Text") ? "txt" : "html";
            var currentPath = Path.Combine(path, $"{EscapeName()}.{ext}");
            Code.WriteCodeItem(currentPath);
        }

        #endregion Methods
    }
}