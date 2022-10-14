using System.Diagnostics.Metrics;

namespace InstrumentsApi.Models
{
    static class ExtensionMethods
    {
        public static int MaxIdValue(this List<Instruments> instruments)
        {
            int countId = 0;
            foreach (Instruments item in instruments)
            {
                if (item.Id >= countId)
                {
                    countId = item.Id + 1;
                }
            }
            return countId;
        }
    }
}
