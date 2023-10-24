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
    public class WebPage : EditablePortalItem
    {
        #region Constants

        public const string NODEKEY = "WebPage";
        public const string NODENAME = "Web Pages";

        #endregion Constants

        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public WebPage(Entity record, bool isLegacyPortal, bool isEnhancedModel)
        {
            Id = record.Id;

            Copy = new CodeItem(record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : $"{(isEnhancedModel ? "mspp" : "adx")}")}_copy"), CodeItemType.LiquidTemplate, false, this);
            JavaScript = new CodeItem(record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_customjavascript"), CodeItemType.JavaScript, false, this);
            Style = new CodeItem(record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_customcss"), CodeItemType.Style, false, this);
            IsRoot = record.GetAttributeValue<bool>($"{(isEnhancedModel ? "mspp" : "adx")}_isroot");

            PartialUrl = record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_partialurl");

            ParentPageId = record.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_rootwebpageid")?.Id ?? Guid.Empty;
            Language = record.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_webpagelanguageid")?.Name ?? "no language";
            Name = $"{record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_name")}{(IsRoot || isLegacyPortal ? "" : " (" + Language + ")")}";

            WebsiteReference = record.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_websiteid") ??
                               new EntityReference($"{(isEnhancedModel ? "mspp" : "adx")}_website", Guid.Empty);

            innerRecord = record;

            Items.Add(Copy);
            Items.Add(JavaScript);
            Items.Add(Style);
        }

        #endregion Constructor

        #region Properties

        public CodeItem Copy { get; }
        public bool IsRoot { get; }
        public CodeItem JavaScript { get; }
        public string Language { get; }
        public Guid ParentPageId { get; }
        public string PartialUrl { get; }
        public CodeItem Style { get; }

        #endregion Properties

        #region Methods

        public static List<WebPage> GetItems(IOrganizationService service, bool isEnhancedModel)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_webpage")
                {
                    ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_customjavascript", $"{(isEnhancedModel ? "mspp" : "adx")}_customcss", $"{(isEnhancedModel ? "mspp" : "adx")}_websiteid", $"{(isEnhancedModel ? "mspp" : "adx")}_webpagelanguageid", $"{(isEnhancedModel ? "mspp" : "adx")}_rootwebpageid", $"{(isEnhancedModel ? "mspp" : "adx")}_isroot", $"{(isEnhancedModel ? "mspp" : "adx")}_partialurl", $"{(isEnhancedModel ? "mspp" : "adx")}_copy"),
                    Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_isroot", OrderType.Descending), new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                }).Entities;

                return records.Select(record => new WebPage(record, false, isEnhancedModel))
                    .OrderByDescending(e => e.innerRecord.GetAttributeValue<bool>($"{(isEnhancedModel ? "mspp" : "adx")}_isroot"))
                    .ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_webpage")
                    {
                        ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_customjavascript", $"{(isEnhancedModel ? "mspp" : "adx")}_customcss", $"{(isEnhancedModel ? "mspp" : "adx")}_websiteid", $"{(isEnhancedModel ? "mspp" : "adx")}_partialurl"),
                        Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                    }).Entities;

                    return records.Select(record => new WebPage(record, true, isEnhancedModel)).ToList();
                }
                throw;
            }
        }

        public override string RefreshContent(CodeItem item, IOrganizationService service, bool isEnhancedModel)
        {
            var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                new ColumnSet(item.Type == CodeItemType.JavaScript ? $"{(isEnhancedModel ? "mspp" : "adx")}_customjavascript" : item.Type == CodeItemType.Style ? $"{(isEnhancedModel ? "mspp" : "adx")}_customcss" : $"{(isEnhancedModel ? "mspp" : "adx")}_copy"));

            innerRecord.RowVersion = record.RowVersion;

            return item.Type == CodeItemType.JavaScript
                ? record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_customjavascript")
                : item.Type == CodeItemType.Style
                    ? record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_customcss")
                    : record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_copy");
        }

        public override void Update(IOrganizationService service, bool forceUpdate, bool isEnhancedModel)
        {
            innerRecord[$"{(isEnhancedModel ? "mspp" : "adx")}_copy"] = Copy.Content;
            innerRecord[$"{(isEnhancedModel ? "mspp" : "adx")}_customjavascript"] = JavaScript.Content;
            innerRecord[$"{(isEnhancedModel ? "mspp" : "adx")}_customcss"] = Style.Content;

            var updateRequest = new UpdateRequest
            {
                ConcurrencyBehavior = forceUpdate ? ConcurrencyBehavior.AlwaysOverwrite : ConcurrencyBehavior.IfRowVersionMatches,
                Target = innerRecord
            };

            service.Execute(updateRequest);

            var updatedRecord = service.Retrieve(innerRecord.LogicalName, innerRecord.Id, new ColumnSet());
            innerRecord.RowVersion = updatedRecord.RowVersion;

            Copy.State = CodeItemState.None;
            JavaScript.State = CodeItemState.None;
            Style.State = CodeItemState.None;
            HasPendingChanges = false;
        }

        /// <summary>
        /// Write the contents of the code object to disk
        /// </summary>
        /// <param name="path"></param>
        public override void WriteContent(string path)
        {
            var filePath = Path.Combine(path, "Content.html");
            Copy?.WriteCodeItem(filePath);

            filePath = Path.Combine(path, "JavaScript.js");
            JavaScript?.WriteCodeItem(filePath);

            filePath = Path.Combine(path, "Style.css");
            Style?.WriteCodeItem(filePath);
        }

        #endregion Methods
    }
}