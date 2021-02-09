using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImportIntoAzureCosmosDb.Classes
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<ScenariosTableModelCamelCase>> GetItemsAsync(string query);
        Task<ScenariosTableModelCamelCase> GetItemAsync(string id);
        Task AddItemAsync(ScenariosTableModelCamelCase item);
        Task UpdateItemAsync(string id, ScenariosTableModelCamelCase item);
        Task DeleteItemAsync(string id);
    }
}
