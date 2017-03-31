using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public class WebFile : EditablePortalItem
    {
        #region Variables

        private readonly Entity innerRecord;

        #endregion

        #region Constructor

        public WebFile(Entity record)
        {
            var ext = record.GetAttributeValue<string>("filename").ToLower().Split('.').Last();

            Code = new CodeItem(record.GetAttributeValue<string>("documentbody"), ext == "js" ? CodeItemType.JavaScript : CodeItemType.Style, true, this);
            Name = record.GetAttributeValue<AliasedValue>("webfile.adx_name").Value.ToString();
            WebsiteReference = (EntityReference) record.GetAttributeValue<AliasedValue>("webfile.adx_websiteid").Value;

            innerRecord = record;

            Items.Add(Code);
        }

        #endregion

        #region Properties

        public CodeItem Code { get; }

        #endregion

        #region Methods

        public static List<WebFile> GetItems(IOrganizationService service)
        {
            var records = service.RetrieveMultiple(new QueryExpression("annotation")
            {
                ColumnSet = new ColumnSet("filename", "documentbody"),
                LinkEntities =
                {
                    new LinkEntity
                    {
                        EntityAlias = "webfile",
                        LinkFromEntityName = "annotation",
                        LinkFromAttributeName = "objectid",
                        LinkToAttributeName = "adx_webfileid",
                        LinkToEntityName = "adx_webfile",
                        Columns = new ColumnSet("adx_websiteid", "adx_name"),
                        Orders = {new OrderExpression("adx_name", OrderType.Ascending)}
                    }
                },
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("documentbody", ConditionOperator.NotNull),

                    },
                    Filters =
                    {
                        new FilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                            {
                                new ConditionExpression("filename", ConditionOperator.EndsWith, "js"),
                                new ConditionExpression("filename", ConditionOperator.EndsWith, "css")
                            }
                        }
                    }
                }
            }).Entities;

            return records.Select(record => new WebFile(record)).ToList();
        }
        public override void Update(IOrganizationService service, bool forceUpdate = false)
        {
            innerRecord["documentbody"] = Code.EncodedContent;

            var recordToUpdate = new Entity(innerRecord.LogicalName)
            {
                Id = innerRecord.Id,
                RowVersion = innerRecord.RowVersion
            };
            recordToUpdate["documentbody"] = innerRecord["documentbody"];

            var updateRequest = new UpdateRequest
            {
                ConcurrencyBehavior = forceUpdate ? ConcurrencyBehavior.AlwaysOverwrite : ConcurrencyBehavior.IfRowVersionMatches,
                Target = recordToUpdate
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
               new ColumnSet("documentbody"));

            innerRecord.RowVersion = record.RowVersion;

            var data = Encoding.UTF8.GetString(Convert.FromBase64String(record.GetAttributeValue<string>("documentbody")));

            return data;
        }

        #endregion
    }
}
