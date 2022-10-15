namespace InstrumentsApi.Models
{
    public class Instruments
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Brand { get; set; }

        public string? Material { get; set; }

        public int YearOfConstruction { get; set; }

        public int Price { get; set; }

    }
}
