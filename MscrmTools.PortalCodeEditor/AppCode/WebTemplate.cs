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
    public class WebTemplate : EditablePortalItem
    {
        #region Constants

        public const string NODEKEY = "WebTemplate";
        public const string NODENAME = "Web Templates";

        #endregion Constants

        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public WebTemplate(Entity record, bool isEnhancedModel)
        {
            Id = record.Id;
            Code = new CodeItem(record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_source"), CodeItemType.LiquidTemplate, false, this);
            Name = record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_name");
            WebsiteReference = record.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_websiteid") ??
                               new EntityReference($"{(isEnhancedModel ? "mspp" : "adx")}_website", Guid.Empty);

            innerRecord = record;

            Items.Add(Code);
        }

        #endregion Constructor

        #region Properties

        public CodeItem Code { get; }

        #endregion Properties

        #region Methods

        public static List<WebTemplate> GetItems(IOrganizationService service, ref bool isLegacyPortal, bool isEnhancedModel)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_webtemplate")
                {
                    ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_source", $"{(isEnhancedModel ? "mspp" : "adx")}_websiteid"),
                    Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                }).Entities;

                return records.Select(record => new WebTemplate(record, isEnhancedModel)).ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    isLegacyPortal = true;

                    var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_webtemplate")
                    {
                        ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_source"),
                        Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                    }).Entities;
                    return records.Select(record => new WebTemplate(record, isEnhancedModel)).ToList();
                }
                throw;
            }
        }

        public override string RefreshContent(CodeItem item, IOrganizationService service, bool isEnhancedModel)
        {
            var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_source"));

            innerRecord.RowVersion = record.RowVersion;

            return record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_source");
        }

        public override void Update(IOrganizationService service, bool forceUpdate, bool isEnhancedModel)
        {
            innerRecord[$"{(isEnhancedModel ? "mspp" : "adx")}_source"] = Code.Content;

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