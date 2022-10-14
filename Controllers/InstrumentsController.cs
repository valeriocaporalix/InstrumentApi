using InstrumentsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace InstrumentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAll()
        {
            WriterReader reader = new WriterReader();
            return Ok(reader.Read());
        }

        [HttpGet("getbyname/{name}")]
        public IActionResult GetByName(string name)
        {
            WriterReader reader = new WriterReader();
            List<Instruments> instruments = reader.Read();
            IEnumerable<Instruments> instrumentsByName = instruments.Where(instruments => instruments.Name == name);
            if (instrumentsByName != null)
            {
                return Ok(instrumentsByName);
            }
            return NoContent();
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            WriterReader reader = new WriterReader();
            List<Instruments> instruments = reader.Read();
            IEnumerable<Instruments> instrumentById = instruments.Where(instruments => instruments.Id == id);
            if (instrumentById != null)
            {
                return Ok(instrumentById);
            }
            return NoContent();
        }

        [HttpGet("getbybrand/{brand}")]
        public IActionResult GetByBrand(string brand)
        {
            WriterReader reader = new WriterReader();
            List<Instruments> instruments = reader.Read();
            IEnumerable<Instruments> instrumentsByBrand = instruments.Where(instruments => instruments.Brand == brand);
            if (instrumentsByBrand != null)
            {
                return Ok(instrumentsByBrand);
            }
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Instruments instrument)
        {
            WriterReader reader = new WriterReader();
            List<Instruments> instruments = reader.Read();
            int countId = instruments.MaxIdValue();

            if (instrument is not null)
            {
                instrument.Id = countId;
                WriterReader writer = new WriterReader();
                writer.Insert(JsonSerializer.Serialize(instrument));
                return Created($"{nameof(GetById)}/getbyid/{instrument.Id}",instrument);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            WriterReader reader = new WriterReader();
            List<Instruments> instruments = reader.Read();
            System.IO.File.WriteAllText("./instruments.txt", string.Empty);
            WriterReader writer = new WriterReader();
            foreach (Instruments item in instruments)
            {
                if (item.Id != id)
                {
                    writer.Insert(JsonSerializer.Serialize(item));
                }
            }        
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Instruments instrument)
        {
            WriterReader reader = new WriterReader();
            List<Instruments> instruments = reader.Read();

            WriterReader writer = new WriterReader();

            int countId = instruments.MaxIdValue();

            Instruments? instrumentToUpdate = instruments.FirstOrDefault(instruments => instruments.Id == id);


            if(instrumentToUpdate != null)
            {
                System.IO.File.WriteAllText("./instruments.txt", string.Empty);
                foreach (Instruments item in instruments)
                {
                    if (item.Id != instrumentToUpdate.Id)
                    {
                        writer.Insert(JsonSerializer.Serialize(item));
                    }
                    else
                    {
                        instrument.Id = instrumentToUpdate.Id;
                        writer.Insert(JsonSerializer.Serialize(instrument));
                    }
                }
                
                return NoContent();
            }
            else
            {
                instrument.Id = countId;
                writer.Insert(JsonSerializer.Serialize(instrument));
                return Created($"{nameof(GetById)}/getbyid/{instrument.Id}", instrument);
            }
        }
    }
}
