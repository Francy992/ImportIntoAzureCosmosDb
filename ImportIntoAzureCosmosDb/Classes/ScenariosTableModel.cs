using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportIntoAzureCosmosDb.Classes
{
    public class ScenariosTableModel : TableEntity
    {
        public string ChangedBy { get; set; }
        public string Definition { get; set; }
        [IgnoreProperty]
        public List<DefinitionModel> DefinitionModel 
        { 
            get
            {
                return JsonConvert.DeserializeObject<List<DefinitionModel>>(Definition);
            }

            private set { }
        }

        public bool IsCustomScenario { get; set; }
        public string Status { get; set; }
        public string Channels { get; set; }
        public string Entities { get; set; }
        public string Intents { get; set; }
        public string Keywords { get; set; }
        public string Category { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            string x;
            if (PartitionKey == "V2_Info_Notifica_Mazzetti")
                x = "TEst";

            var castObj = (ScenariosTableModel)obj;

            return PartitionKey == castObj.PartitionKey &&
                DefinitionModelListAreEquals(DefinitionModel, castObj.DefinitionModel) && 
                IsCustomScenario == castObj.IsCustomScenario &&
                Status == castObj.Status &&
                Channels == castObj.Channels &&
                Entities == castObj.Entities &&
                Intents == castObj.Intents &&
                Keywords == castObj.Keywords &&
                Category == castObj.Category;
        }

        private bool DefinitionModelListAreEquals(List<DefinitionModel> definitionModel1, List<DefinitionModel> definitionModel2)
        {
            foreach (var defModel1 in definitionModel1)
            {
                bool found = false;
                foreach (var defModel2 in definitionModel2)
                {
                    if (defModel1.Equals(defModel2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + PartitionKey.GetHashCode();
            hash = (hash * 7) + IsCustomScenario.GetHashCode();
            hash = (hash * 7) + Status?.GetHashCode() ?? 0;
            hash = (hash * 7) + Channels?.GetHashCode() ?? 0;
            hash = (hash * 7) + Entities?.GetHashCode() ?? 0;
            hash = (hash * 7) + Intents?.GetHashCode() ?? 0;
            hash = (hash * 7) + Keywords?.GetHashCode() ?? 0;
            hash = (hash * 7) + Category?.GetHashCode() ?? 0;
            return hash;
        }
    }
}
