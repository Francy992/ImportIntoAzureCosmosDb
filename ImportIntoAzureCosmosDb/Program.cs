using ImportIntoAzureCosmosDb.Classes;
using Microsoft.Azure.Cosmos.Table;
using Poste.UserLibraries.Classes;
using Poste.UserLibraries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

                var outputSettings = new AzureStorageSettings(false);
                CloudTable productionTable = azureStorageManager.GetTable(outputSettings.StorageConnectionString, outputSettings.StorageTableName);
                var productionEntities = await AzureStorageOperations<ScenariosTableModel>.RetrieveAllEntitiesAsync(productionTable);
                Console.WriteLine($"Lettura dallo storage di produzione completata. Totale righe da importare: {productionEntities.Count}");
                var notFoundInProduction = sviluppoEntities.Except(productionEntities).ToList();
                var partitionKeyNotFound = new List<string>();
                foreach (var sviluppo in sviluppoEntities)
                {
                    bool found = false;
                    foreach (var production in productionEntities)
                    {
                        if (sviluppo.PartitionKey == production.PartitionKey)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        partitionKeyNotFound.Add(sviluppo.PartitionKey);
                }

                Console.WriteLine($"Trovate {partitionKeyNotFound.Count} partitionKey di sviluppo non presenti in produzione:");
                foreach (var notFount in partitionKeyNotFound)
                {
                    Console.WriteLine($"{notFount}");
                }

                Console.WriteLine($"Trovate {notFoundInProduction.Count} righe di sviluppo che differiscono per qualcosa in produzione:");
                foreach (var notFount in notFoundInProduction)
                {
                    Console.WriteLine($"{notFount.PartitionKey}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
