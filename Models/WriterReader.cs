using System.Text.Json;

namespace InstrumentsApi.Models
{
    public class WriterReader : IWriterReader
    {
        public void Insert(string inputText)
        {
            using (var streamWriter = File.AppendText("./instruments.txt"))
            {

                streamWriter.WriteLine(inputText);
            }
        }
        public List<Instruments> Read()
        {
            List<string> lines = new List<string>();
            List<Instruments> instruments = new List<Instruments>();
            using var streamReader = new StreamReader("./instruments.txt");
            while(!streamReader.EndOfStream)
            {
                Instruments deserialized = JsonSerializer.Deserialize<Instruments>(streamReader.ReadLine());
                instruments.Add(deserialized);
            }
            return instruments;
        }
    }
}
