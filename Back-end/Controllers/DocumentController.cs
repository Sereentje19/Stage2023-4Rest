using System;
using Microsoft.AspNetCore.Mvc;
using Back_end.Models;
using Back_end.Services;
using System.Reflection.Metadata;

namespace Back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;

        public DocumentController(IDocumentService ds)
        {
            documentService = ds;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var document = documentService.GetAll();
            return Ok(document);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var document = documentService.GetById(id);
            return Ok(document);
        }

        [HttpPost]
        public IActionResult Post(Models.Document doc)
        {
            documentService.Post(doc);
            return Ok(new { message = "Document created" });
        }

        [HttpPut]
        public IActionResult Put(Models.Document doc)
        {
            documentService.Put(doc);
            return Ok(new { message = "Document updated" });
        }

        [HttpDelete]
        public IActionResult Delete(Models.Document doc)
        {
            documentService.Delete(doc);
            return Ok(new { message = "Document Deleted" });
        }
    }
}