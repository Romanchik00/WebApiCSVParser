using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Tasks;
using WebApiCSVParser.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiCSVParser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly CsvProcessingService _csvProcessingService;

        public DataController(CsvProcessingService csvProcessingService)
        {
            _csvProcessingService = csvProcessingService;
        }

        [HttpGet("values")]
        public IEnumerable<string> GetVal(string name)
        {
            return _csvProcessingService.LastValues(name);
        }

        // GET api/<ApiController>/5
        [HttpGet("results/{filename}")]
        public IEnumerable<string> GetResByName(string filename)
        {
            return _csvProcessingService.ResultByName(filename);
        }

        //[HttpGet("results/")]
        //public IEnumerable<string> GetResByDate(string date)
        //{
        //    return _csvProcessingService
        //}

        //[HttpGet("results/{}")]
        //public IEnumerable<string> GetResByAvgVal(string filename)
        //{
        //    return _csvProcessingService
        //}

        //[HttpGet("results/{}")]
        //public IEnumerable<string> GetResByAvgExecTime(string filename)
        //{
        //    return _csvProcessingService
        //}


        // POST api/data/up
        [HttpPost("upload")]
        public async Task<IActionResult> Post(IFormFile CSV)
        {
            var answer = await _csvProcessingService.ProcessCsvFile(CSV);
            if (answer.Equals(Results.Created()))
            { 
                return Created();
            }
            else
            {
                return BadRequest(answer);
            }
        }

        // PUT api/<ApiController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ApiController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
