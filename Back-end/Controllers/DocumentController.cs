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
        private readonly NotificationContext context;
        private readonly IDocumentService documentService;

        public DocumentController(NotificationContext nc, IDocumentService ds)
        {
            context = nc;
            documentService = ds;
        }


        [HttpGet(Name = "GetDocument")]
        public IActionResult Get()
        {
            var document = context.Documents.Find(1);
            return Ok(document);
        }

        [HttpPost(Name = "PostDocument")]
        public IActionResult Post(Document doc)
        {
            documentService.Post(doc);
            return Ok(new { message = "User created" });
        }
    }
}