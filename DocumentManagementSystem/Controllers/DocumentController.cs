using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagementSystem.DTOs;
using DocumentManagementSystem.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IStorageService _storageService;

        public DocumentController(IRepository repository, IStorageService storageService)
        {
            _repository = repository;
            _storageService = storageService;
        }

        [HttpGet]
        public ActionResult<IList<DocumentDto>> GetDocuments()
        {
            return Ok();
        }

        [HttpGet("{location}", Name = "Download")]
        public ActionResult Download(string location)
        {

            return Ok();
        }

        [HttpPost]
        public ActionResult<bool> Upload([FromForm] IFormFile file)
        {
            return Created("", true);
        }

        [HttpPut("id")]
        public ActionResult UpdateDoc(string id, DocumentDto doc)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDoc(string id)
        {
            return NoContent();
        }
    }
}