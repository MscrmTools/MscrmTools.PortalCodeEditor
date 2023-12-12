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
    public class WebFormStep : EditablePortalItem
    {
        #region Constants

        public const string NODEKEY = "WebForm";
        public const string NODENAME = "Web Forms";

        #endregion Constants

        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public WebFormStep(Entity record, bool isEnhancedModel)
        {
            Id = record.Id;
            JavaScript = new CodeItem(record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript"), CodeItemType.JavaScript, false, this);
            Name = record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_name");
            WebFormReference = record.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_webform");

            WebsiteReference = record.GetAliasedValue<EntityReference>("webform", $"{(isEnhancedModel ? "mspp" : "adx")}_websiteid") ??
                                   new EntityReference($"{(isEnhancedModel ? "mspp" : "adx")}_website", Guid.Empty);

            innerRecord = record;
            Items.Add(JavaScript);
        }

        #endregion Constructor

        #region Properties

        public CodeItem JavaScript { get; }

        public EntityReference WebFormReference { get; }

        #endregion Properties

        #region Methods

        public static List<WebFormStep> GetItems(IOrganizationService service, bool isEnhancedModel)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_webformstep")
                {
                    ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript", $"{(isEnhancedModel ? "mspp" : "adx")}_webform"),
                    Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) },
                    LinkEntities =
                {
                    new LinkEntity
                    {
                        LinkFromEntityName = $"{(isEnhancedModel ? "mspp": "adx")}_webformstep",
                        LinkFromAttributeName = $"{(isEnhancedModel ? "mspp_webform" : "adx_webform")}",
                        LinkToAttributeName = $"{(isEnhancedModel ? "mspp": "adx")}_webformid",
                        LinkToEntityName = $"{(isEnhancedModel ? "mspp": "adx")}_webform",
                        Columns = new ColumnSet($"{(isEnhancedModel ? "mspp": "adx")}_websiteid"),
                        EntityAlias = "webform"
                    }
                }
                }).Entities;

                if (isEnhancedModel)
                {
                    var webforms = service.RetrieveMultiple(new QueryExpression("mspp_webform")
                    {
                        ColumnSet = new ColumnSet("mspp_websiteid", "msp_webformid")
                    }).Entities;

                    foreach (var record in records)
                    {
                        record["webform.mspp_websiteid"] = new AliasedValue("webform", "mspp_websiteid", webforms.First(wf => wf.Id == record.GetAttributeValue<EntityReference>("mspp_webform").Id).GetAttributeValue<EntityReference>("mspp_websiteid"));
                    }
                }

                return records.Select(record => new WebFormStep(record, isEnhancedModel)).ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_webformstep")
                    {
                        ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript", $"{(isEnhancedModel ? "mspp" : "adx")}_webform"),
                        Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                    }).Entities;

                    return records.Select(record => new WebFormStep(record, isEnhancedModel)).ToList();
                }
                throw;
            }
        }

        public override string RefreshContent(CodeItem item, IOrganizationService service, bool isEnhancedModel)
        {
            var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript"));

            innerRecord.RowVersion = record.RowVersion;

            return record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript");
        }

        public override void Update(IOrganizationService service, bool forceUpdate, bool isEnhancedModel)
        {
            innerRecord[$"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript"] = JavaScript.Content;

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