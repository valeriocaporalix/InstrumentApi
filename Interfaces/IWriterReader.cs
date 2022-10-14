using System.Diagnostics.Metrics;

namespace InstrumentsApi.Models
{
    public interface IWriterReader
    {
        public void Insert(string input);

        public List<Instruments> Read();
    }
}
