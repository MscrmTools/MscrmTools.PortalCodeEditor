using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public class WebPage : EditablePortalItem
    {
        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public WebPage(Entity record)
        {
            JavaScript = new CodeItem(record.GetAttributeValue<string>("adx_customjavascript"), CodeItemType.JavaScript,
                false, this);
            Style = new CodeItem(record.GetAttributeValue<string>("adx_customcss"), CodeItemType.Style, false, this);

            Name =
                $"{record.GetAttributeValue<string>("adx_name")} {(record.Contains("adx_webpagelanguageid") ? "(" + record.GetAttributeValue<EntityReference>("adx_webpagelanguageid").Name + ")" : "")}";

            WebsiteReference = record.GetAttributeValue<EntityReference>("adx_websiteid");

            innerRecord = record;

            Items.Add(JavaScript);
            Items.Add(Style);
        }

        #endregion Constructor

        #region Properties

        public CodeItem JavaScript { get; }
        public CodeItem Style { get; }

        #endregion Properties

        #region Methods

        public static List<WebPage> GetItems(IOrganizationService service)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression("adx_webpage")
                {
                    ColumnSet = new ColumnSet("adx_name", "adx_customjavascript", "adx_customcss", "adx_websiteid", "adx_webpagelanguageid"),
                    Orders = { new OrderExpression("adx_name", OrderType.Ascending) }
                }).Entities;

                return records.Select(record => new WebPage(record)).ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    var records = service.RetrieveMultiple(new QueryExpression("adx_webpage")
                    {
                        ColumnSet = new ColumnSet("adx_name", "adx_customjavascript", "adx_customcss", "adx_websiteid"),
                        Orders = { new OrderExpression("adx_name", OrderType.Ascending) }
                    }).Entities;

                    return records.Select(record => new WebPage(record)).ToList();
                }
                throw;
            }
        }

        public override void Update(IOrganizationService service, bool forceUpdate = false)
        {
            innerRecord["adx_customjavascript"] = JavaScript.Content;
            innerRecord["adx_customcss"] = Style.Content;

            var updateRequest = new UpdateRequest
            {
                ConcurrencyBehavior = forceUpdate ? ConcurrencyBehavior.AlwaysOverwrite : ConcurrencyBehavior.IfRowVersionMatches,
                Target = innerRecord
            };

            service.Execute(updateRequest);

            var updatedRecord = service.Retrieve(innerRecord.LogicalName, innerRecord.Id, new ColumnSet());
            innerRecord.RowVersion = updatedRecord.RowVersion;

            JavaScript.State = CodeItemState.None;
            Style.State = CodeItemState.None;
            HasPendingChanges = false;
        }

        public override string RefreshContent(CodeItem item, IOrganizationService service)
        {
            var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                new ColumnSet(item.Type == CodeItemType.JavaScript ? "adx_customjavascript" : "adx_customcss"));

            innerRecord.RowVersion = record.RowVersion;

            return item.Type == CodeItemType.JavaScript
                ? record.GetAttributeValue<string>("adx_customjavascript")
                : record.GetAttributeValue<string>("adx_customcss");
        }

        #endregion Methods
    }
}