using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportIntoAzureCosmosDb.Classes
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(ScenariosTableModelCamelCase item)
        {
            await this._container.CreateItemAsync<ScenariosTableModelCamelCase>(item, PartitionKey.None);
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<ScenariosTableModel>(id, new PartitionKey(id));
        }

        public async Task<ScenariosTableModelCamelCase> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<ScenariosTableModelCamelCase> response = await this._container.ReadItemAsync<ScenariosTableModelCamelCase>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<ScenariosTableModelCamelCase>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<ScenariosTableModelCamelCase>(new QueryDefinition(queryString));
            List<ScenariosTableModelCamelCase> results = new List<ScenariosTableModelCamelCase>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, ScenariosTableModelCamelCase item)
        {
            await this._container.UpsertItemAsync<ScenariosTableModelCamelCase>(item, new PartitionKey(id));
        }
    }
}
