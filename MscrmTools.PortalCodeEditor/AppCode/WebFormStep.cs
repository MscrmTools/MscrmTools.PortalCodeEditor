using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public class WebFormStep : EditablePortalItem
    {
        #region Constants
        public const string NODEKEY = "WebForm";
        public const string NODENAME = "Web Forms";
        #endregion
        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public WebFormStep(Entity record)
        {
            Id = record.Id;
            JavaScript = new CodeItem(record.GetAttributeValue<string>("adx_registerstartupscript"), CodeItemType.JavaScript, false, this);
            Name = record.GetAttributeValue<string>("adx_name");
            WebFormReference = record.GetAttributeValue<EntityReference>("adx_webform");

            var websiteReference = record.GetAliasedValue<EntityReference>("webform", "adx_websiteid");
            if (websiteReference != null)
            {
                WebsiteReference = websiteReference;
            }
            else
            {
                WebsiteReference = new EntityReference("adx_websiteid", Guid.Empty);
            }

            innerRecord = record;
            Items.Add(JavaScript);
        }

        #endregion Constructor

        #region Properties

        public CodeItem JavaScript { get; }

        public EntityReference WebFormReference { get; }

        #endregion Properties

        #region Methods

        public static List<WebFormStep> GetItems(IOrganizationService service)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression("adx_webformstep")
                {
                    ColumnSet = new ColumnSet("adx_name", "adx_registerstartupscript", "adx_webform"),
                    Orders = { new OrderExpression("adx_name", OrderType.Ascending) },
                    LinkEntities =
                {
                    new LinkEntity
                    {
                        LinkFromEntityName = "adx_webformstep",
                        LinkFromAttributeName = "adx_webform",
                        LinkToAttributeName = "adx_webformid",
                        LinkToEntityName = "adx_webform",
                        Columns = new ColumnSet("adx_websiteid"),
                        EntityAlias = "webform"
                    }
                }
                }).Entities;

                return records.Select(record => new WebFormStep(record)).ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    var records = service.RetrieveMultiple(new QueryExpression("adx_webformstep")
                    {
                        ColumnSet = new ColumnSet("adx_name", "adx_registerstartupscript", "adx_webform"),
                        Orders = { new OrderExpression("adx_name", OrderType.Ascending) }
                    }).Entities;

                    return records.Select(record => new WebFormStep(record)).ToList();
                }
                throw;
            }
        }

        public override void Update(IOrganizationService service, bool forceUpdate = false)
        {
            innerRecord["adx_registerstartupscript"] = JavaScript.Content;

            var updateRequest = new UpdateRequest
            {
                ConcurrencyBehavior = forceUpdate ? ConcurrencyBehavior.AlwaysOverwrite : ConcurrencyBehavior.IfRowVersionMatches,
                Target = innerRecord
            };

            service.Execute(updateRequest);

            var updatedRecord = service.Retrieve(innerRecord.LogicalName, innerRecord.Id, new ColumnSet());
            innerRecord.RowVersion = updatedRecord.RowVersion;

            JavaScript.State = CodeItemState.None;
            HasPendingChanges = false;
        }

        public override string RefreshContent(CodeItem item, IOrganizationService service)
        {
            var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                new ColumnSet("adx_registerstartupscript"));

            innerRecord.RowVersion = record.RowVersion;

            return record.GetAttributeValue<string>("adx_registerstartupscript");
        }

        /// <summary>
        /// Write the contents of the code object to disk
        /// </summary>
        /// <param name="path"></param>
        public override void WriteContent(string path)
        {
            var filePath = Path.Combine(path, $"JavaScript.js");

            JavaScript?.WriteCodeItem(filePath);
        }
        #endregion Methods
    }
}