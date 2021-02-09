using ImportIntoAzureCosmosDb.Classes;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Table;
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

            return cosmosDbService;
        }
    }
}
