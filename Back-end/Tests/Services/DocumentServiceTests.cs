﻿using BLL.Services;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using PL.Models;
using PL.Models.Requests;
using PL.Models.Responses;

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
                Type = DocumentType.Certificaat,
                IsArchived = false
            },
            new Document
            {
                DocumentId = 2,
                Date = DateTime.Now.AddDays(-5),
                Employee = new Employee { Name = "test", Email = "test@test.nl" },
                Type = DocumentType.Contract,
                IsArchived = true
            },
        };
    }

    [Fact]
    public void GetPagedDocuments_ShouldReturnPagedDocuments()
    {
        const string searchfield = "example";
        const DocumentType dropdown = DocumentType.Contract;
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
        const DocumentType dropdown = DocumentType.Contract;
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
        const DocumentType dropdown = DocumentType.Contract;
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
        const DocumentType dropdown = DocumentType.Contract;
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
        const DocumentType dropdown = DocumentType.Contract;
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
        const DocumentType dropdown = DocumentType.Contract;
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
    public void GetDocumentTypeStrings_ShouldReturnDocumentTypeStrings()
    {
        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetDocumentTypeStrings())
            .Returns(new List<string>
                { "Vog", "Contract", "Paspoort", "ID kaart", "Diploma", "Certificaat", "Lease auto" });

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        List<string> documentTypeStrings = documentService.GetDocumentTypeStrings();

        Assert.NotNull(documentTypeStrings);
        Assert.Equal(7, documentTypeStrings.Count);
        Assert.Contains("Vog", documentTypeStrings);
        Assert.Contains("Contract", documentTypeStrings);
        Assert.Contains("Paspoort", documentTypeStrings);
        Assert.Contains("ID kaart", documentTypeStrings);
        Assert.Contains("Diploma", documentTypeStrings);
        Assert.Contains("Certificaat", documentTypeStrings);
        Assert.Contains("Lease auto", documentTypeStrings);
    }

    [Fact]
    public void GetDocumentTypeStrings_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
    {
        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetDocumentTypeStrings())
            .Returns(new List<string>());

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        List<string> documentTypeStrings = documentService.GetDocumentTypeStrings();

        Assert.NotNull(documentTypeStrings);
        Assert.Empty(documentTypeStrings);
    }

    [Fact]
    public async Task GetDocumentById_ShouldReturnDocumentResponse()
    {
        const int documentId = 1;
        DocumentResponse expectedDocumentResponse = new DocumentResponse
        {
            Date = DateTime.Today.AddDays(-1),
            Employee = new Employee { Email = "test@test.nl", Name = "test" },
            Type = DocumentType.Diploma
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetDocumentById(documentId))
            .ReturnsAsync(expectedDocumentResponse);

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        DocumentResponse actualDocumentResponse = await documentService.GetDocumentById(documentId);

        Assert.NotNull(actualDocumentResponse);
        Assert.Equal(expectedDocumentResponse.Date, actualDocumentResponse.Date);
        Assert.Equal(expectedDocumentResponse.Employee, actualDocumentResponse.Employee);
        Assert.Equal(expectedDocumentResponse.Type, actualDocumentResponse.Type);
    }

    [Fact]
    public async Task GetDocumentById_WhenRepositoryReturnsNull_ShouldReturnNull()
    {
        const int documentId = 1;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.GetDocumentById(documentId))
            .ReturnsAsync((DocumentResponse)null);

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        DocumentResponse actualDocumentResponse = await documentService.GetDocumentById(documentId);

        Assert.Null(actualDocumentResponse);
    }

    [Fact]
    public async Task PostDocument_ShouldCallRepositoryAddDocument()
    {
        Document documentToPost = new Document
        {
            DocumentId = 3,
            Type = DocumentType.Not_selected,
            Date = DateTime.Today.AddDays(-1),
            Employee = new Employee { Email = "test@test.nl", Name = "test" }
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.AddDocument(It.IsAny<Document>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await documentService.PostDocument(documentToPost);

        documentRepositoryMock.Verify();
    }
    
    [Fact]
    public async Task PostDocument_ShouldHandleRepositoryException()
    {
        Document documentToPost = new Document
        {
            DocumentId = 3,
            Type = DocumentType.Not_selected,
            Date = DateTime.Today.AddDays(-1),
            Employee = new Employee { Email = "test@test.nl", Name = "test" }
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.AddDocument(It.IsAny<Document>()))
            .ThrowsAsync(new Exception("Simulated repository exception"));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => documentService.PostDocument(documentToPost));

        documentRepositoryMock.Verify(repo => repo.AddDocument(It.IsAny<Document>()), Times.Once);
    }

    [Fact]
    public async Task PutDocument_ShouldCallRepositoryUpdateDocument()
    {
        EditDocumentRequest documentToUpdate = new EditDocumentRequest
        {
            DocumentId = 3,
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.UpdateDocument(It.IsAny<EditDocumentRequest>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await documentService.PutDocument(documentToUpdate);

        documentRepositoryMock.Verify(repo => repo.UpdateDocument(It.IsAny<EditDocumentRequest>()), Times.Once);
    }

    [Fact]
    public async Task PutDocument_ShouldHandleRepositoryException()
    {
        EditDocumentRequest documentToUpdate = new EditDocumentRequest
        {
            DocumentId = 3,
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.UpdateDocument(It.IsAny<EditDocumentRequest>()))
            .ThrowsAsync(new Exception("Simulated repository exception"));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => documentService.PutDocument(documentToUpdate));

        documentRepositoryMock.Verify(repo => repo.UpdateDocument(It.IsAny<EditDocumentRequest>()), Times.Once);
    }

    [Fact]
    public async Task UpdateIsArchived_ShouldCallRepositoryUpdateIsArchived()
    {
        CheckBoxRequest checkBoxRequest = new CheckBoxRequest
        {
            DocumentId = 3,
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.UpdateIsArchived(It.IsAny<CheckBoxRequest>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await documentService.UpdateIsArchived(checkBoxRequest);

        documentRepositoryMock.Verify(repo => repo.UpdateIsArchived(It.IsAny<CheckBoxRequest>()), Times.Once);
    }

    [Fact]
    public async Task UpdateIsArchived_ShouldHandleRepositoryException()
    {
        CheckBoxRequest checkBoxRequest = new CheckBoxRequest
        {
            DocumentId = 3,
        };

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.UpdateIsArchived(It.IsAny<CheckBoxRequest>()))
            .ThrowsAsync(new Exception("Simulated repository exception"));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => documentService.UpdateIsArchived(checkBoxRequest));

        documentRepositoryMock.Verify(repo => repo.UpdateIsArchived(It.IsAny<CheckBoxRequest>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDocument_ShouldCallRepositoryDeleteDocument()
    {
        const int documentId = 3;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.DeleteDocument(It.IsAny<int>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await documentService.DeleteDocument(documentId);

        documentRepositoryMock.Verify(repo => repo.DeleteDocument(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDocument_ShouldHandleRepositoryException()
    {
        const int documentId = 3;

        Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
        documentRepositoryMock.Setup(repo => repo.DeleteDocument(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Simulated repository exception"));

        DocumentService documentService = new DocumentService(documentRepositoryMock.Object);
        await Assert.ThrowsAsync<Exception>(() => documentService.DeleteDocument(documentId));

        documentRepositoryMock.Verify(repo => repo.DeleteDocument(It.IsAny<int>()), Times.Once);
    }

    
    
}