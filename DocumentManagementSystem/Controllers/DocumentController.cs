using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using DocumentManagementSystem.DTOs;
using DocumentManagementSystem.Interfaces;
using DocumentManagementSystem.Model;
using DocumentManagementSystem.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private static int _displayOrder = 0;

        public DocumentController(IRepository repository, IStorageService storageService, IMapper mapper)
        {
            _repository = repository;
            _storageService = storageService;
            _mapper = mapper;
            _displayOrder = Helper.GetLatestDisplayOrder(repository) ?? 0;
        }

        [HttpGet]
        public ActionResult<IList<DocumentDto>> GetDocuments()
        {
            var docs = _repository.GetDocs().OrderBy(x => x.DisplayOrder);

            if (!docs.Any())
                return NotFound();

            return Ok(_mapper.Map<IList<DocumentDto>>(docs));
        }

        [HttpGet("{location}", Name = "Download")]
        public ActionResult Download(string location)
        {

            if (string.IsNullOrEmpty(location))
                return BadRequest();

            var decodedUrl = HttpUtility.UrlDecode(location);

            var fileId = _repository.GetDocs()
                            .Where(d => string.Compare(d.Location, decodedUrl, true) == 0)
                            .FirstOrDefault()?.DocId;

            if (string.IsNullOrEmpty(fileId))
                return NotFound();

            var file = _storageService.DownloadFile(fileId);

            if (file == null)
                return NotFound();

            return File(file, "application/pdf");
        }

        [HttpPost]
        public ActionResult<bool> Upload([FromForm] PdfDocument pdf)
        {
            var fileId = string.Empty;
            if (ModelState.IsValid)
            {
                var doc = pdf.Doc;
                if (doc == null || doc.Length < 1)
                    return BadRequest();

                using (var stream = doc.OpenReadStream())
                {
                    fileId = _storageService.UploadFile(stream);
                }
                if (fileId == Guid.Empty.ToString())
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Upload a file.");
                _repository.AddDoc(new Document
                {
                    DocId = fileId,
                    DisplayOrder = ++_displayOrder,
                    FileSize = doc.Length,
                    Location = _storageService.GetFilePath(fileId),
                    Name = doc.FileName
                });
                _repository.Save();
            }
            return Created(_storageService.GetFilePath(fileId), true);
        }

        [HttpPatch("{id}")]
        public ActionResult UpdateDoc(string id, JsonPatchDocument<DocumentDto> patchDoc)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDoc(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var doc = _repository.GetDoc(id);

            if (doc == null || string.IsNullOrEmpty(doc?.DocId))
                return NotFound();

            if (_storageService.DeleteFile(doc?.DocId))
                _repository.DeleteDoc(doc);

            _repository.Save();

            return NoContent();
        }
    }
}