using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MscrmTools.PortalCodeEditor.AppCode
{
    public class WebFile : EditablePortalItem
    {
        #region Constants

        public const string NODEKEY = "WebFile";
        public const string NODENAME = "Web Files";

        #endregion Constants

        #region Variables

        private readonly Entity innerRecord;

        #endregion Variables

        #region Constructor

        public WebFile(Entity record, bool isEnhancedModel, string content = null)
        {
            var ext = record.GetAttributeValue<string>(isEnhancedModel ? "mspp_partialurl" : "filename").ToLower().Split('.').Last();
            Id = record.Id;

            if (isEnhancedModel)
            {
                Name = record.GetAttributeValue<string>("mspp_name");
                WebsiteReference = record.GetAttributeValue<EntityReference>("mspp_websiteid") ?? new EntityReference($"mspp_website", Guid.Empty);
                Code = new CodeItem(content, ext == "js" ? CodeItemType.JavaScript : CodeItemType.Style, false, this);
            }
            else
            {
                Code = new CodeItem(record.GetAttributeValue<string>("documentbody"), ext == "js" ? CodeItemType.JavaScript : CodeItemType.Style, true, this);
                Name = record.GetAttributeValue<AliasedValue>($"webfile.{ (isEnhancedModel ? "mspp" : "adx")}_name")?.Value.ToString() ?? record.GetAttributeValue<string>("filename");
                WebsiteReference = (EntityReference)record.GetAttributeValue<AliasedValue>($"webfile.{ (isEnhancedModel ? "mspp" : "adx")}_websiteid")?.Value ?? new EntityReference($"{(isEnhancedModel ? "mspp" : "adx")}_website", Guid.Empty);
            }
            innerRecord = record;

            Items.Add(Code);
        }

        #endregion Constructor

        #region Properties

        public CodeItem Code { get; }

        #endregion Properties

        #region Methods

        public static List<WebFile> GetItems(IOrganizationService service, bool isEnhancedModel)
        {
            List<WebFile> records = new List<WebFile>();

            if (!isEnhancedModel)
            {
                var tmpRecords = service.RetrieveMultiple(new QueryExpression("annotation")
                {
                    ColumnSet = new ColumnSet("filename", "documentbody", "objectid", "modifiedon"),
                    LinkEntities =
                    {
                        new LinkEntity
                        {
                            EntityAlias = "webfile",
                            LinkFromEntityName = "annotation",
                            LinkFromAttributeName = "objectid",
                            LinkToAttributeName = $"{(isEnhancedModel ? "mspp": "adx")}_webfileid",
                            LinkToEntityName = $"{(isEnhancedModel ? "mspp": "adx")}_webfile",
                            Columns = new ColumnSet($"{(isEnhancedModel ? "mspp": "adx")}_websiteid", $"{(isEnhancedModel ? "mspp": "adx")}_name"),
                            Orders = {new OrderExpression($"{(isEnhancedModel ? "mspp": "adx")}_name", OrderType.Ascending)}
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

                records = tmpRecords.Select(record => new WebFile(record, isEnhancedModel))
                    .OrderByDescending(e => e.innerRecord.GetAttributeValue<DateTime>("modifiedon")).ToList()
                    .GroupBy(e => e.innerRecord.GetAttributeValue<EntityReference>("objectid").Id.ToString()).ToList()
                    .Select(g => g.First())
                    .ToList();
            }
            else
            {
                var tmpRecords = service.RetrieveMultiple(new QueryExpression("mspp_webfile")
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression
                    {
                        Filters =
                        {
                            new FilterExpression(LogicalOperator.Or)
                            {
                                Conditions =
                                {
                                    new ConditionExpression("mspp_partialurl", ConditionOperator.EndsWith, "js"),
                                    new ConditionExpression("mspp_partialurl", ConditionOperator.EndsWith, "css"),
                                    new ConditionExpression("mspp_partialurl", ConditionOperator.EndsWith, "json")
                                }
                            }
                        }
                    }
                }).Entities;

                foreach (var record in tmpRecords)
                {
                    var content = GetEnhancedContent(record.Id, service);

                    records.Add(new WebFile(record, isEnhancedModel, content));
                }
            }

            return records;
        }

        public override string RefreshContent(CodeItem item, IOrganizationService service, bool isEnhancedModel)
        {
            if (isEnhancedModel)
            {
                return GetEnhancedContent(item.Parent.Id, service);
            }
            else
            {
                var record = service.Retrieve(innerRecord.LogicalName, innerRecord.Id,
                   new ColumnSet("documentbody"));

                innerRecord.RowVersion = record.RowVersion;

                var data = Encoding.UTF8.GetString(Convert.FromBase64String(record.GetAttributeValue<string>("documentbody")));

                return data;
            }
        }

        public override void Update(IOrganizationService service, bool forceUpdate, bool isEnhancedModel)
        {
            if (isEnhancedModel)
            {
                var request = new InitializeFileBlocksUploadRequest
                {
                    Target = new EntityReference("powerpagecomponent", innerRecord.Id),
                    FileAttributeName = "filecontent",
                    FileName = Name
                };
                var response = (InitializeFileBlocksUploadResponse)service.Execute(request);

                var data = Convert.FromBase64String(Code.EncodedContent);
                // to store different block id in case of chunking           
                var lstBlock = new List<string>();

                // 4194304 = 4 MB
                for (int i = 0; i <= data.Length / 4194304; i++)
                {
                    var blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                    lstBlock.Add(blockId);

                    var uploadBlockRequest = new UploadBlockRequest()
                    {
                        BlockId = blockId,
                        BlockData = data.Skip(i * 4194304).Take(4194304).ToArray(),
                        FileContinuationToken = response.FileContinuationToken
                    };

                    service.Execute(uploadBlockRequest);
                }

                var commitFileBlocksUploadRequest = new CommitFileBlocksUploadRequest
                {
                    FileContinuationToken = response.FileContinuationToken,
                    FileName = Name,
                    MimeType = System.Web.MimeMapping.GetMimeMapping(Name),
                    BlockList = lstBlock.ToArray()

                };

               service.Execute(commitFileBlocksUploadRequest);


            }
            else
            {
                innerRecord["documentbody"] = Code.EncodedContent;

                var recordToUpdate = new Entity(innerRecord.LogicalName)
                {
                    Id = innerRecord.Id,
                    RowVersion = innerRecord.RowVersion
                };
                recordToUpdate["documentbody"] = innerRecord["documentbody"];

                if (Code.Type == CodeItemType.Style)
                {
                    recordToUpdate["mimetype"] = "text/css";
                }
                else if (Code.Type == CodeItemType.JavaScript)
                {
                    recordToUpdate["mimetype"] = "application/javascript";
                }

                var updateRequest = new UpdateRequest
                {
                    ConcurrencyBehavior = forceUpdate ? ConcurrencyBehavior.AlwaysOverwrite : ConcurrencyBehavior.IfRowVersionMatches,
                    Target = recordToUpdate
                };

                service.Execute(updateRequest);

                var updatedRecord = service.Retrieve(innerRecord.LogicalName, innerRecord.Id, new ColumnSet());
                innerRecord.RowVersion = updatedRecord.RowVersion;
            }

            Code.State = CodeItemState.None;
            HasPendingChanges = false;
        }

        /// <summary>
        /// Write the contents of the code object to disk
        /// </summary>
        /// <param name="path"></param>
        public override void WriteContent(string path)
        {
            var filePath = Path.Combine(path, Name);

            Code?.WriteCodeItem(filePath);
        }

        private static string GetEnhancedContent(Guid recordId, IOrganizationService service)
        {
            var request = new InitializeFileBlocksDownloadRequest
            {
                Target = new EntityReference("powerpagecomponent", recordId),
                FileAttributeName = "filecontent"
            };

            var response = (InitializeFileBlocksDownloadResponse)service.Execute(request);

            var dlRequest = new DownloadBlockRequest
            {
                Offset = 0,
                BlockLength = response.FileSizeInBytes,
                FileContinuationToken = response.FileContinuationToken
            };

            var dlResponse = (DownloadBlockResponse)service.Execute(dlRequest);
            string content;
            using (var stream = new StreamReader(new MemoryStream(dlResponse.Data), true))
            {
                return stream.ReadToEnd();
            }
        }

        #endregion Methods
    }
}