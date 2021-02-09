using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImportIntoAzureCosmosDb.Classes
{
    public class ScenariosTableModelCamelCase
    {
        [JsonProperty("timeStamp")]
        public DateTimeOffset Timestamp { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("eTag")]
        public string ETag { get; set; }
        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }
        [JsonProperty("rowKey")]
        public string RowKey { get; set; }
        [JsonProperty("changedBy")]
        public string ChangedBy { get; set; }
        [JsonProperty("definition")]
        public string Definition { get; set; }
        [JsonProperty("isCustomScenario")]
        public bool IsCustomScenario { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("channels")]
        public string Channels { get; set; }
        [JsonProperty("entities")]
        public string Entities { get; set; }
        [JsonProperty("intents")]
        public string Intents { get; set; }
        [JsonProperty("keywords")]
        public string Keywords { get; set; }
        [JsonProperty("category")]
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
