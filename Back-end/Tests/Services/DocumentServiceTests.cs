using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Services;

public class DocumentServiceTests
{
    private static IEnumerable<object> GetSampleDocuments()
    {
        return new List<object>
        {
            new Document
            {
                DocumentId = 1,
                Date = DateTime.Now,
                Employee = new Employee { Name = "test", Email = "test@test.nl" },
                Type = new DocumentType { Id = 1, Name = "Contract" },
                IsArchived = false
            },
            new Document
            {
                DocumentId = 2,
                Date = DateTime.Now.AddDays(-5),
                Employee = new Employee { Name = "test", Email = "test@test.nl" },
                Type = new DocumentType { Id = 1, Name = "Contract" },
                IsArchived = true
            },
        };
    }

    [Fact]
    public void GetPagedDocuments_ShouldReturnPagedDocuments()
    {
        const string searchfield = "example";
        const string dropdown = "Contract";
        const int page = 1;
        const int pageSize = 5;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetPagedDocuments(searchfield, dropdown, page, pageSize))
            .Returns((GetSampleDocuments(), 2));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        (IEnumerable<object> documentList, Pager pager) =
            documentService.GetPagedDocuments(searchfield, dropdown, page, pageSize);

        Assert.NotNull(documentList);
        Assert.Equal(2, pager.TotalItems);
    }

    [Fact]
    public void GetPagedDocuments_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
    {
        const string searchfield = "example";
        const string dropdown = "Contract";
        const int page = 1;
        const int pageSize = 5;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetPagedDocuments(searchfield, dropdown, page, pageSize))
            .Returns((new List<object>(), 0));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        (IEnumerable<object> documentList, Pager pager) =
            documentService.GetPagedDocuments(searchfield, dropdown, page, pageSize);

        Assert.NotNull(documentList);
        Assert.Equal(0, pager.TotalItems);
    }

    [Fact]
    public void GetArchivedPagedDocuments_ShouldReturnPagedDocuments()
    {
        const string searchfield = "example";
        const string dropdown = "Contract";
        const int page = 1;
        const int pageSize = 10;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize))
            .Returns((GetSampleDocuments(), 2));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        (IEnumerable<object> documentList, Pager pager) =
            documentService.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize);

        Assert.NotNull(documentList);
        Assert.Equal(2, pager.TotalItems);
    }

    [Fact]
    public void GetArchivedPagedDocuments_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
    {
        const string searchfield = "example";
        const string dropdown = "Contract";
        const int page = 1;
        const int pageSize = 10;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize))
            .Returns((new List<object>(), 0));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        (IEnumerable<object> documentList, Pager pager) =
            documentService.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize);

        Assert.NotNull(documentList);
        Assert.Equal(0, pager.TotalItems);
    }

    [Fact]
    public void GetLongValidPagedDocuments_ShouldReturnPagedDocuments()
    {
        const string searchfield = "example";
        const string dropdown = "Contract";
        const int page = 1;
        const int pageSize = 10;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetLongValidPagedDocuments(searchfield, dropdown, page, pageSize))
            .Returns((GetSampleDocuments(), 2));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        (IEnumerable<object> documentList, Pager pager) =
            documentService.GetLongValidPagedDocuments(searchfield, dropdown, page, pageSize);

        Assert.NotNull(documentList);
        Assert.Equal(2, pager.TotalItems);
    }

    [Fact]
    public void GetLongValidPagedDocuments_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
    {
        const string searchfield = "example";
        const string dropdown = "Contract";
        const int page = 1;
        const int pageSize = 10;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetLongValidPagedDocuments(searchfield, dropdown, page, pageSize))
            .Returns((new List<object>(), 0));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        (IEnumerable<object> documentList, Pager pager) =
            documentService.GetLongValidPagedDocuments(searchfield, dropdown, page, pageSize);

        Assert.Empty(documentList);
        Assert.Equal(0, pager.TotalItems);
    }

    [Fact]
    public async Task GetDocumentTypes_ShouldReturnListOfDocumentTypes()
    {
        Mock<IDocumentRepository> mockRepository = new Mock<IDocumentRepository>();
        DocumentService documentService = new DocumentService(mockRepository.Object);

        List<DocumentType> documentTypes = new List<DocumentType>
        {
            new DocumentType { Id = 1, Name = "Type1" },
            new DocumentType { Id = 2, Name = "Type2" },
        };

        mockRepository.Setup(repo => repo.GetDocumentTypesAsync())
            .ReturnsAsync(documentTypes);

        IEnumerable<DocumentType> result = await documentService.GetDocumentTypesAsync();

        Assert.NotNull(result);
        Assert.IsType<List<DocumentType>>(result);
        Assert.Equal(2, result.Count()); 
    }


    [Fact]
    public async Task GetDocumentById_ShouldReturnDocumentResponse()
    {
        const int documentId = 1;
        DocumentResponseDto expectedDocumentResponseDto = new DocumentResponseDto
        {
            Date = DateTime.Today.AddDays(-1),
            Employee = new Employee { Email = "test@test.nl", Name = "test" },
            Type = new DocumentType { Id = 1, Name = "Laptop" }
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetDocumentByIdAsync(documentId))
            .ReturnsAsync(expectedDocumentResponseDto);

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        DocumentResponseDto actualDocumentResponseDto = await documentService.GetDocumentByIdAsync(documentId);

        Assert.NotNull(actualDocumentResponseDto);
        Assert.Equal(expectedDocumentResponseDto.Date, actualDocumentResponseDto.Date);
        Assert.Equal(expectedDocumentResponseDto.Employee, actualDocumentResponseDto.Employee);
        Assert.Equal(expectedDocumentResponseDto.Type, actualDocumentResponseDto.Type);
    }

    [Fact]
    public async Task GetDocumentById_WhenRepositoryReturnsNull_ShouldReturnNull()
    {
        const int documentId = 1;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetDocumentByIdAsync(documentId))
            .ReturnsAsync((DocumentResponseDto)null);

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        DocumentResponseDto actualDocumentResponseDto = await documentService.GetDocumentByIdAsync(documentId);

        Assert.Null(actualDocumentResponseDto);
    }

    [Fact]
    public async Task PostDocument_ShouldCallRepositoryAddDocument()
    {
        Document documentToPost = new Document
        {
            DocumentId = 3,
            Type = new DocumentType { Id = 1, Name = "Laptop" },
            Date = DateTime.Today.AddDays(-1),
            Employee = new Employee { Email = "test@test.nl", Name = "test" }
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.CreateDocumentAsync(It.IsAny<Document>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await documentService.CreateDocumentAsync(documentToPost);

        documentRepositoryMock.Verify();
    }

    [Fact]
    public async Task PostDocument_ShouldHandleRepositoryException()
    {
        Document documentToPost = new Document
        {
            DocumentId = 3,
            Type = new DocumentType { Id = 1, Name = "Laptop" },
            Date = DateTime.Today.AddDays(-1),
            Employee = new Employee { Email = "test@test.nl", Name = "test" }
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.CreateDocumentAsync(It.IsAny<Document>()))
            .ThrowsAsync(new Exception("Simulated repository exception"));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => documentService.CreateDocumentAsync(documentToPost));

        documentRepositoryMock.Verify(repo => repo.CreateDocumentAsync(It.IsAny<Document>()), Times.Once);
    }

    [Fact]
    public async Task PutDocument_ShouldCallRepositoryUpdateDocument()
    {
        EditDocumentRequestDto documentToUpdate = new EditDocumentRequestDto
        {
            DocumentId = 3,
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.UpdateDocumentAsync(It.IsAny<EditDocumentRequestDto>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await documentService.UpdateDocumentAsync(documentToUpdate);

        documentRepositoryMock.Verify(repo => repo.UpdateDocumentAsync(It.IsAny<EditDocumentRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task PutDocument_ShouldHandleRepositoryException()
    {
        EditDocumentRequestDto documentToUpdate = new EditDocumentRequestDto
        {
            DocumentId = 3,
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.UpdateDocumentAsync(It.IsAny<EditDocumentRequestDto>()))
            .ThrowsAsync(new Exception("Simulated repository exception"));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => documentService.UpdateDocumentAsync(documentToUpdate));

        documentRepositoryMock.Verify(repo => repo.UpdateDocumentAsync(It.IsAny<EditDocumentRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateIsArchived_ShouldCallRepositoryUpdateIsArchived()
    {
        CheckBoxRequestDto checkBoxRequestDto = new CheckBoxRequestDto
        {
            DocumentId = 3,
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.UpdateIsArchivedAsync(It.IsAny<CheckBoxRequestDto>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await documentService.UpdateIsArchivedAsync(checkBoxRequestDto);

        documentRepositoryMock.Verify(repo => repo.UpdateIsArchivedAsync(It.IsAny<CheckBoxRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateIsArchived_ShouldHandleRepositoryException()
    {
        CheckBoxRequestDto checkBoxRequestDto = new CheckBoxRequestDto
        {
            DocumentId = 3,
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.UpdateIsArchivedAsync(It.IsAny<CheckBoxRequestDto>()))
            .ThrowsAsync(new Exception("Simulated repository exception"));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => documentService.UpdateIsArchivedAsync(checkBoxRequestDto));

        documentRepositoryMock.Verify(repo => repo.UpdateIsArchivedAsync(It.IsAny<CheckBoxRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDocument_ShouldCallRepositoryDeleteDocument()
    {
        const int documentId = 3;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.DeleteDocumentAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await documentService.DeleteDocumentAsync(documentId);

        documentRepositoryMock.Verify(repo => repo.DeleteDocumentAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDocument_ShouldHandleRepositoryException()
    {
        const int documentId = 3;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.DeleteDocumentAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Simulated repository exception"));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => documentService.DeleteDocumentAsync(documentId));

        documentRepositoryMock.Verify(repo => repo.DeleteDocumentAsync(It.IsAny<int>()), Times.Once);
    }
}