using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImportIntoAzureCosmosDb.Classes
{
    public class ScenariosTableModelCamelCase
    {
        [JsonProperty("SourceTimeStamp")]
        public DateTimeOffset Timestamp { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("SourceETag")]
        public string ETag { get; set; }
        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }
        [JsonProperty("RowKey")]
        public string RowKey { get; set; }
        [JsonProperty("ChangedBy")]
        public string ChangedBy { get; set; }
        [JsonProperty("Definition")]
        public string Definition { get; set; }
        [JsonProperty("IsCustomScenario")]
        public bool IsCustomScenario { get; set; }
        [JsonProperty("Status")]
        public string Status { get; set; }
        [JsonProperty("Channels")]
        public string Channels { get; set; }
        [JsonProperty("Entities")]
        public string Entities { get; set; }
        [JsonProperty("Intents")]
        public string Intents { get; set; }
        [JsonProperty("Keywords")]
        public string Keywords { get; set; }
        [JsonProperty("Category")]
        public string Category { get; set; }

        public ScenariosTableModelCamelCase() { }

        public ScenariosTableModelCamelCase(ScenariosTableModel item)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = item.Timestamp;
            ETag = item.ETag;
            PartitionKey = item.PartitionKey;
            RowKey = item.RowKey;
            ChangedBy = item.ChangedBy;
            Definition = item.Definition;
            IsCustomScenario = item.IsCustomScenario;
            Status = item.Status;
            Channels = item.Channels;
            Entities = item.Entities;
            Intents = item.Intents;
            Keywords = item.Keywords;
            Category = item.Category;
        }
    }
}
