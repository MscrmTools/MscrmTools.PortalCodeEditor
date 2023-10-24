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
    public class EntityList : EditablePortalItem
    {
        #region Constants

        public const string NODEKEY = "EntityList";
        public const string NODENAME = "Entity Lists";

        #endregion Constants

        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public EntityList(Entity record, bool isEnhancedModel)
        {
            Id = record.Id;
            JavaScript = new CodeItem(record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript"), CodeItemType.JavaScript, false, this);
            Name = record.GetAttributeValue<string>($"{(isEnhancedModel ? "mspp" : "adx")}_name");
            WebsiteReference = record.GetAttributeValue<EntityReference>($"{(isEnhancedModel ? "mspp" : "adx")}_websiteid") ??
                               new EntityReference($"{(isEnhancedModel ? "mspp" : "adx")}_website", Guid.Empty);

            innerRecord = record;
            Items.Add(JavaScript);
        }

        #endregion Constructor

        #region Properties

        public CodeItem JavaScript { get; }

        #endregion Properties

        #region Methods

        public static List<EntityList> GetItems(IOrganizationService service, ref bool isLegacyPortal, bool isEnhancedModel)
        {
            try
            {
                var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_entitylist")
                {
                    ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript", $"{(isEnhancedModel ? "mspp" : "adx")}_websiteid"),
                    Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                }).Entities;

                return records.Select(record => new EntityList(record, isEnhancedModel)).ToList();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail.ErrorCode == -2147217149)
                {
                    isLegacyPortal = true;

                    var records = service.RetrieveMultiple(new QueryExpression($"{(isEnhancedModel ? "mspp" : "adx")}_entitylist")
                    {
                        ColumnSet = new ColumnSet($"{(isEnhancedModel ? "mspp" : "adx")}_name", $"{(isEnhancedModel ? "mspp" : "adx")}_registerstartupscript"),
                        Orders = { new OrderExpression($"{(isEnhancedModel ? "mspp" : "adx")}_name", OrderType.Ascending) }
                    }).Entities;

                    return records.Select(record => new EntityList(record, isEnhancedModel)).ToList();
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
            var filePath = Path.Combine(path, "JavaScript.js");

            JavaScript?.WriteCodeItem(filePath);
        }

        #endregion Methods
    }
}