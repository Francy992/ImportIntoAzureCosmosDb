using ImportIntoAzureCosmosDb.Classes;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Poste.UserLibraries.Classes;
using Poste.UserLibraries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ImportIntoAzureCosmosDb
{
    class Program
    {
        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        async private static Task MainAsync()
        {
            try
            {
                Console.WriteLine("Inizio import");
                var azureStorageManager = new AzureStorageManager();

                // Read all entities in Input table
                var inputSettings = new AzureStorageSettings();
                CloudTable sviluppoTable = azureStorageManager.GetTable(inputSettings.StorageConnectionString, inputSettings.StorageTableName);
                var sviluppoEntities = await AzureStorageOperations<ScenariosTableModel>.RetrieveAllEntitiesAsync(sviluppoTable);
                Console.WriteLine($"Lettura dallo storage di sviluppo completata. Totale righe da importare: {sviluppoEntities.Count}");

               
                IConfiguration config = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();

                //await CreateCollectionWithoutPartitionKey(config.GetSection("CosmosDb"));
                var cosmosDbService = InitializeCosmosClientInstanceAsync(config.GetSection("CosmosDb"));
                int count = 0;
                foreach(var x in sviluppoEntities)
                {
                    var temp = new ScenariosTableModelCamelCase(x);
                    await cosmosDbService.Result.AddItemAsync(temp);
                    Thread.Sleep(30);
                    Console.WriteLine($"Inserted '{temp.PartitionKey}'. Count: {++count}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);

            CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
            Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, configurationSection.GetSection("PartitionKey").Value);


            /**
             * For create without partition key
             */
            //var documentCollection = new DocumentCollection { Id = databaseName };
            //var temp = new Microsoft.Azure.Documents.Client.DocumentClient(new Uri(account), key);

            //var document = await temp.CreateDocumentCollectionIfNotExistsAsync(
            //    account,
            //    documentCollection,
            //    new Microsoft.Azure.Documents.Client.RequestOptions { OfferThroughput = 400 }).ConfigureAwait(false);

            return cosmosDbService;
        }

        private static async Task CreateCollectionWithoutPartitionKey(IConfiguration configuration)
        {
            string databaseName = configuration.GetSection("DatabaseName").Value;
            string containerName = configuration.GetSection("ContainerName").Value;
            string account = configuration.GetSection("Account").Value;
            string key = configuration.GetSection("Key").Value;

            var documentCollection = new DocumentCollection { Id = "9a7d1a17-6343-4ff3-bc66-505d0d70b917" };
            var temp = new Microsoft.Azure.Documents.Client.DocumentClient(new Uri(account), key);

            var document = await temp.CreateDocumentCollectionIfNotExistsAsync(
                account,
                documentCollection,
                new Microsoft.Azure.Documents.Client.RequestOptions { OfferThroughput = 400 }).ConfigureAwait(false);

        }
    }
}
