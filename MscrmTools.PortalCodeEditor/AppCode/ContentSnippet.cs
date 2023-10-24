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

        #endregion Constants

        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public ContentSnippet(Entity record, bool isEnhancedModel)
        {
            Id = record.Id;
            Code = new CodeItem(record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_value"), CodeItemType.JavaScript, false, this);
            Name = record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_name");
            WebsiteReference = record.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_websiteid") ?? new EntityReference($"{(isEnhancedModel ? "mspp" : "adx")}_website", Guid.Empty);

            Type = record.GetAttributeValue<OptionSetValue>($"{(isEnhancedModel ? "mspp" : "adx")}_type")?.Value == 756150000 ? "Text" : "Html";
            innerRecord = record;
            Items.Add(Code);
        }

        #endregion Constructor

        #region Properties

        public CodeItem Code { get; }
        public string Type { get; }

        #endregion Properties

        #region Methods

        public static List<ContentSnippet> GetItems(IOrganizationService service, ref bool isLegacyPortal, bool isEnhancedModel)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_contentsnippet")
                {
                    ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_value", $"{(isEnhancedModel ? "mspp" : "adx")}_type", $"{(isEnhancedModel ? "mspp" : "adx")}_websiteid"),
                    Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                }).Entities;

                return records.Select(record => new ContentSnippet(record, isEnhancedModel)).ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    isLegacyPortal = true;
                    var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_webtemplate")
                    {
                        ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_value", $"{(isEnhancedModel ? "mspp" : "adx")}_type", $"{(isEnhancedModel ? "mspp" : "adx")}_websiteid"),
                        Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                    }).Entities;
                    return records.Select(record => new ContentSnippet(record, isEnhancedModel)).ToList();
                }
                throw;
            }
        }

        public override string RefreshContent(CodeItem item, IOrganizationService service, bool isEnhancedModel)
        {
            var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_value"));
            innerRecord.RowVersion = record.RowVersion;
            return record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_value");
        }

        public override void Update(IOrganizationService service, bool forceUpdate, bool isEnhancedModel)
        {
            innerRecord[$"{(isEnhancedModel ? "mspp" : "adx")}_value"] = Code.Content;
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