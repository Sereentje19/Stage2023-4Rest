using System.Reflection;
using BLL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;

namespace Tests.Controllers;

public class DocumentControllerTests
{
    
    [Fact]
    public void GetFilteredPagedDocuments_ReturnsOkResultWithPagedDocuments()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        documentServiceMock.Setup(s => s.GetPagedDocuments(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((GetSampleDocuments(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        DocumentController controller = new DocumentController(documentServiceMock.Object);
        IActionResult result = controller.GetFilteredPagedDocuments("search", "dropdown", 1, 5);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        object documents = response.GetType().GetProperty("Documents")?.GetValue(response);

        Assert.NotNull(documents);
        object pager = response.GetType().GetProperty("Pager")?.GetValue(response);

        Assert.NotNull(pager);
        documentServiceMock.Verify(s => s.GetPagedDocuments(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    
    [Fact]
    public void GetArchivedPagedDocuments_ReturnsOkResultWithPagedDocuments()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        documentServiceMock.Setup(s => s.GetArchivedPagedDocuments(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((GetSampleDocuments(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        DocumentController controller = new DocumentController(documentServiceMock.Object);
        IActionResult result = controller.GetArchivedPagedDocuments("search", "dropdown", 1, 5);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        object documents = response.GetType().GetProperty("Documents")?.GetValue(response);

        Assert.NotNull(documents);
        object pager = response.GetType().GetProperty("Pager")?.GetValue(response);

        Assert.NotNull(pager);
        documentServiceMock.Verify(s => s.GetArchivedPagedDocuments(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

   
    [Fact]
    public void GetLongValidPagedDocuments_ReturnsOkResultWithPagedDocuments()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        documentServiceMock.Setup(s => s.GetLongValidPagedDocuments(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((GetSampleDocuments(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        DocumentController controller = new DocumentController(documentServiceMock.Object);
        IActionResult result = controller.GetLongValidPagedDocuments("search", "dropdown", 1, 5);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        object documents = response.GetType().GetProperty("Documents")?.GetValue(response);

        Assert.NotNull(documents);
        object pager = response.GetType().GetProperty("Pager")?.GetValue(response);

        Assert.NotNull(pager);
        documentServiceMock.Verify(s => s.GetLongValidPagedDocuments(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }


    private static IEnumerable<object> GetSampleDocuments()
    {
        return Enumerable.Range(1, 10).Select(i => new { Id = i, Title = $"Document {i}" });
    }

    
    [Fact]
    public async Task GetDocumentTypesAsync_ReturnsOkResultWithDocumentTypes()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();

        IEnumerable<DocumentType> expectedDocumentTypes = new List<DocumentType>
        {
            new DocumentType { Id = 1, Name = "Type1" },
            new DocumentType { Id = 2, Name = "Type2" },
        };

        documentServiceMock.Setup(s => s.GetDocumentTypesAsync())
            .ReturnsAsync(expectedDocumentTypes);

        DocumentController controller = new DocumentController(documentServiceMock.Object);

        IActionResult result = await controller.GetDocumentTypesAsync();

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        IEnumerable<DocumentType> documentTypes = Assert.IsAssignableFrom<IEnumerable<DocumentType>>(okResult.Value);

        Assert.Equal(expectedDocumentTypes.Count(), documentTypes.Count());
    }


    [Fact]
    public async Task GetDocumentByIdAsync_ReturnsOkResultWithDocument()
    {
        const int documentId = 1;
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();

        DocumentResponseDto expectedDocument = new DocumentResponseDto
        {
            Date = DateTime.Now,
            Type = new DocumentType { Name = "Contract" },
            Employee = new Employee { Name = "Serena", EmployeeId = 1, Email = "Serena@" }
        };

        documentServiceMock.Setup(s => s.GetDocumentByIdAsync(documentId))
            .ReturnsAsync(expectedDocument);

        DocumentController controller = new DocumentController(documentServiceMock.Object);

        IActionResult result = await controller.GetDocumentByIdAsync(documentId);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        DocumentResponseDto document = Assert.IsType<DocumentResponseDto>(okResult.Value);

        Assert.Equal(expectedDocument.Employee, document.Employee);
    }

    [Fact]
    public async Task CreateDocumentAsync_ReturnsOkResult()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        Mock<IFormFile> fileMock = new Mock<IFormFile>();
        DocumentResponseDto document = new DocumentResponseDto
        {
            Type = new DocumentType { Name = "SampleType" },
            Date = DateTime.Now,
            Employee = new Employee { Email = "test@example.com", Name = "John Doe" },
            FileType = "SampleFileType"
        };

        DocumentController controller = new DocumentController(documentServiceMock.Object);
        IActionResult result = await controller.CreateDocumentAsync(fileMock.Object, document);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        dynamic response = okResult.Value;

        dynamic messageProperty = response.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);

        dynamic messageValue = messageProperty.GetValue(response, null);
        Assert.Equal("Document toegevoegd.", messageValue);

        documentServiceMock.Verify(s => s.CreateDocumentAsync(It.IsAny<Document>()), Times.Once);
    }


    [Fact]
    public async Task CreateDocumentAsync_WithFile_ReturnsOkResult()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        Mock<IFormFile> fileMock = new Mock<IFormFile>();
        DocumentResponseDto document = new DocumentResponseDto
        {
            Type = new DocumentType { Name = "SampleType" },
            Date = DateTime.Now,
            Employee = new Employee { Email = "test@example.com", Name = "John Doe" },
            FileType = "SampleFileType"
        };

        DocumentController controller = new DocumentController(documentServiceMock.Object);

        IActionResult result = await controller.CreateDocumentAsync(fileMock.Object, document);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        PropertyInfo messageProperty = response.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);

        object messageValue = messageProperty.GetValue(response, null);
        Assert.Equal("Document toegevoegd.", messageValue);

        documentServiceMock.Verify(s => s.CreateDocumentAsync(It.IsAny<Document>()), Times.Once);
        fileMock.Verify(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDocumentAsync_ReturnsOkResult()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        EditDocumentRequestDto documentToUpdate = new EditDocumentRequestDto
        {
            DocumentId = 1,
        };

        DocumentController controller = new DocumentController(documentServiceMock.Object);

        IActionResult result = await controller.UpdateDocumentAsync(documentToUpdate);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);

        string message = response.GetType().GetProperty("message")?.GetValue(response)?.ToString();

        Assert.NotNull(message);
        Assert.Equal("Document geupdate.", message);

        documentServiceMock.Verify(s => s.UpdateDocumentAsync(It.IsAny<EditDocumentRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateIsArchivedAsync_ReturnsOkResult()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        CheckBoxRequestDto checkBoxRequest = new CheckBoxRequestDto
        {
            DocumentId = 1,
            IsArchived = true
        };

        DocumentController controller = new DocumentController(documentServiceMock.Object);
        IActionResult result = await controller.UpdateIsArchivedAsync(checkBoxRequest);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        string message = response.GetType().GetProperty("message")?.GetValue(response)?.ToString();

        Assert.NotNull(message);
        Assert.Equal("Document geupdate.", message);

        documentServiceMock.Verify(s => s.UpdateIsArchivedAsync(It.IsAny<CheckBoxRequestDto>()), Times.Once);
    }
    
    [Fact]
    public async Task DeleteDocumentAsync_ReturnsOkResult()
    {
        Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        const int documentId = 1; 

        DocumentController controller = new DocumentController(documentServiceMock.Object);
        IActionResult result = await controller.DeleteDocumentAsync(documentId);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        object response = okResult.Value;

        Assert.NotNull(response);
        string message = response.GetType().GetProperty("message")?.GetValue(response)?.ToString();

        Assert.NotNull(message);
        Assert.Equal("Document verwijderd.", message);

        documentServiceMock.Verify(s => s.DeleteDocumentAsync(It.IsAny<int>()), Times.Once);
    }

    
}