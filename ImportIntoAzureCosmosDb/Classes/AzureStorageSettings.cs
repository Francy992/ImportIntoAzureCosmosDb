using Microsoft.Extensions.Configuration;
using Poste.UserLibraries.Classes;
using System;

namespace ImportIntoAzureCosmosDb.Classes
{
    public class AzureStorageSettings : IAzureStorageSettings
    {
        public string StorageConnectionString { get; set; }
        public string StorageTableName { get; set; }
        public string PartitionKey { get; set; }

        public AzureStorageSettings(bool input = true)
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

            if (input)
            {
                StorageConnectionString = config.GetSection("Input").GetSection("AzureWebStore").Value;
                StorageTableName = config.GetSection("Input").GetSection("TableName").Value;
            }
            else
            {
                StorageConnectionString = config.GetSection("Output").GetSection("AzureWebStore").Value;
                StorageTableName = config.GetSection("Output").GetSection("TableName").Value;
            }
            PartitionKey = "";
        }
    }
}
