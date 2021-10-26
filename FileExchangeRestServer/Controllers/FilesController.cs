using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileExchangeRestServer.Managers;
using FileExchangeRestServer.Models;
using FileExchangeSharedClasses;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileExchangeRestServer.Controllers
{
    [EnableCors("AllOrigins")]
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesManager _manager;

        public FilesController(IFilesManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var files = _manager.GetAllFileNames();
            return Ok(files);
        }
        
        [HttpGet("{fileName}")]
        public ActionResult<string> Get(string fileName)
        {
            var json = _manager.GetPeersByFileName(fileName);
            if (string.IsNullOrEmpty(json)) return NotFound();
            return Ok(json);
        }
        
        [HttpPost("{fileName}")]
        public async Task<ActionResult<StoredFile>> Post(string fileName, [FromBody] FileEndPoint endPoint)
        {
            var file = await _manager.AddFileRecordAsync(fileName, endPoint);
            return Ok(file);
        }
        
        [HttpPut("{fileName}")]
        public async Task<ActionResult<StoredFile>> Put(string fileName, [FromBody] FileEndPoint endPoint)
        {
            var file = await _manager.DeleteEndpointFromRecord(fileName, endPoint);
            if (file is null) return NoContent();
            return Ok(file);
        }

    }
}
