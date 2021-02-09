using System;
using System.Collections.Generic;
using System.Text;

namespace ImportIntoAzureCosmosDb.Classes
{
    public class DefinitionModel
    {
        public string Type { get; set; }
        public string Q { get; set; }
        public string Telemetryid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Sub { get; set; }
        public string Nextq { get; set; }
        public string Node { get; set; }
        public string ImageUrl { get; set; }
        public string Thumbnail { get; set; }
        public string Speak { get; set; }
        public string Options { get; set; }
        public string Langdet { get; set; }
        public string Bypass { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var castObj = (DefinitionModel)obj;
            string q1 = Q?.Replace(" ", "") ?? "";
            string q2 = castObj.Q?.Replace(" ", "") ?? "";

            string nextq1 = Nextq?.Replace(" ", "") ?? "";
            string nextq2 = castObj.Nextq?.Replace(" ", "") ?? "";

            int distanceQ, distanceNextQ;

            distanceQ = Distance(q1, q2);
            distanceNextQ = Distance(nextq1, nextq2);

            if (distanceQ == -1 || distanceNextQ == -1)
                return false; //Exception in calculate distance, difference into lenght str
            else if (distanceQ > 5 || distanceNextQ > 5) // too much distance
                return false;
            else
                return Type == castObj.Type &&
                        Sub == castObj.Sub;
        }
        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Type?.GetHashCode() ?? 0;
            hash = (hash * 7) + Sub?.GetHashCode() ?? 0;
            return hash;
        }

        public int Distance(string firstStrand, string secondStrand)
        {
            int count = 0;

            if (firstStrand.Length > secondStrand.Length || firstStrand.Length < secondStrand.Length) { return -1; }
            else if (firstStrand.Length == 0 || secondStrand.Length == 0) { count = 0; }
            else if (firstStrand == secondStrand) { count = 0; }
            else
            {
                for (int i = 0; i < firstStrand.Length; i++)
                {
                    if (firstStrand[i] != secondStrand[i])
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }

}
