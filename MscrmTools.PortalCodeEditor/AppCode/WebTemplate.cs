using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public class WebTemplate : EditablePortalItem
    {
        #region Constants
        public const string NODEKEY = "WebTemplate";
        public const string NODENAME = "Web Templates";
        #endregion

        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public WebTemplate(Entity record)
        {
            Id = record.Id;
            Code = new CodeItem(record.GetAttributeValue<string>("adx_source"), CodeItemType.LiquidTemplate, false, this);
            Name = record.GetAttributeValue<string>("adx_name");
            WebsiteReference = record.GetAttributeValue<EntityReference>("adx_websiteid") ??
                               new EntityReference("adx_website", Guid.Empty);

            innerRecord = record;

            Items.Add(Code);
        }

        #endregion Constructor

        #region Properties

        public CodeItem Code { get; }

        #endregion Properties

        #region Methods

        public static List<WebTemplate> GetItems(IOrganizationService service, ref bool isLegacyPortal)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression("adx_webtemplate")
                {
                    ColumnSet = new ColumnSet("adx_name", "adx_source", "adx_websiteid"),
                    Orders = { new OrderExpression("adx_name", OrderType.Ascending) }
                }).Entities;

                return records.Select(record => new WebTemplate(record)).ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    isLegacyPortal = true;

                    var records = service.RetrieveMultiple(new QueryExpression("adx_webtemplate")
                    {
                        ColumnSet = new ColumnSet("adx_name", "adx_source"),
                        Orders = { new OrderExpression("adx_name", OrderType.Ascending) }
                    }).Entities;
                    return records.Select(record => new WebTemplate(record)).ToList();
                }
                throw;
            }
        }

        public override void Update(IOrganizationService service, bool forceUpdate = false)
        {
            innerRecord["adx_source"] = Code.Content;

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

        public override string RefreshContent(CodeItem item, IOrganizationService service)
        {
            var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                new ColumnSet("adx_source"));

            innerRecord.RowVersion = record.RowVersion;

            return record.GetAttributeValue<string>("adx_source");
        }

        /// <summary>
        /// Write the contents of the code object to disk
        /// </summary>
        /// <param name="path"></param>
        public override void WriteContent(string path)
        {
            var filePath = Path.Combine(path, $"template.liquid");

            Code?.WriteCodeItem(filePath);
        }

        #endregion Methods
    }
}