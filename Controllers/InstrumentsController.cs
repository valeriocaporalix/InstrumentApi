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
        private IReader _reader = new WriterReader();
        private IWriter _writer = new WriterReader();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_reader.Read());
        }

        [HttpGet("getbyname/{name}")]
        public IActionResult GetByName(string name)
        {
            List<Instruments> instruments = _reader.Read();
            IEnumerable<Instruments> instrumentsByName = instruments.Where(instrument => instrument.Name == name);
            if (instrumentsByName != null)
            {
                return Ok(instrumentsByName);
            }
            return NoContent();
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            List<Instruments> instruments = _reader.Read();
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
            List<Instruments> instruments = _reader.Read();
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
            List<Instruments> instruments = _reader.Read();
            int countId = instruments.MaxIdValue();

            if (instrument is not null)
            {
                instrument.Id = countId;
                _writer.Insert(JsonSerializer.Serialize(instrument));
                return Created($"{nameof(GetById)}/getbyid/{instrument.Id}",instrument);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<Instruments> instruments = _reader.Read();
            System.IO.File.WriteAllText("./instruments.txt", string.Empty);

            foreach (Instruments item in instruments)
            {
                if (item.Id != id)
                {
                    _writer.Insert(JsonSerializer.Serialize(item));
                }
            }        
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Instruments instrument)
        {

            List<Instruments> instruments = _reader.Read();

            int countId = instruments.MaxIdValue();

            Instruments? instrumentToUpdate = instruments.FirstOrDefault(instruments => instruments.Id == id);


            if(instrumentToUpdate != null)
            {
                System.IO.File.WriteAllText("./instruments.txt", string.Empty);
                foreach (Instruments item in instruments)
                {
                    if (item.Id != instrumentToUpdate.Id)
                    {
                        _writer.Insert(JsonSerializer.Serialize(item));
                    }
                    else
                    {
                        instrument.Id = instrumentToUpdate.Id;
                        _writer.Insert(JsonSerializer.Serialize(instrument));
                    }
                }
                
                return NoContent();
            }
            else
            {
                instrument.Id = countId;
                _writer.Insert(JsonSerializer.Serialize(instrument));
                return Created($"{nameof(GetById)}/getbyid/{instrument.Id}", instrument);
            }
        }
    }
}
