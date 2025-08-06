using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Метод передачи клиенту последних 10 значений из сортированного списка (в обратном порядке)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("values")]
        public IEnumerable<string> GetVal(string name)
        {
            return _csvProcessingService.LastValues(name);
        }

        // GET api/<ApiController>/5
        //[HttpGet("results/{filename}")]
        //public IEnumerable<string> GetResByName(string filename)
        //{
        //    return _csvProcessingService.ResultByName(filename);
        //}
        

        /// <summary>
        /// Метод загрузки csv-файлов на сервер и добавлению в базу данных
        /// </summary>
        /// <param name="CSV"></param>
        /// <returns></returns>
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
    }
}
