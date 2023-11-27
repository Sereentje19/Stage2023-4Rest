using BLL.Services;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PL.Exceptions;
using PL.Models;
using PL.Models.Requests;
using PL.Models.Responses;

namespace Tests.Repositories
{
    public class DocumentRepositoryTests
    {
        readonly List<Document> _documents = new()
        {
            new Document
            {
                DocumentId = 1, File = "SampleFile"u8.ToArray(), FileType = "SampleType",
                Employee = new Employee { EmployeeId = 1, Name = "John", Email = "john@example.com" },
                Date = DateTime.Now, IsArchived = false, Type = DocumentType.Contract
            },
            new Document
            {
                DocumentId = 2, Employee = new Employee { EmployeeId = 2, Name = "Jane", Email = "jane@example.com" },
                Date = DateTime.Now.AddDays(30), IsArchived = true, Type = DocumentType.Certificaat
            },
            new Document
            {
                DocumentId = 3,
                Employee = new Employee { EmployeeId = 3, Name = "Meredith", Email = "Meredith@example.com" },
                Date = DateTime.Now.AddDays(60), IsArchived = false, Type = DocumentType.Paspoort
            },
        };

        private DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
        }

        [Fact]
        public async void GetPagedDocuments_ShouldReturnPagedResult()
        {
            await using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);
                var result = repository.GetPagedDocuments("John", DocumentType.Contract, 1, 1);

                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);
                Assert.All(result.Item1, doc => Assert.False(((DocumentOverviewResponse)doc).IsArchived));
                Assert.All(result.Item1,
                    doc => Assert.True(((DocumentOverviewResponse)doc).Date <= DateTime.Now.AddDays(42)));
            }
        }

        [Fact]
        public async void GetPagedDocuments_WithArchivedDocuments_ShouldExcludeArchivedDocuments()
        {
            await using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var repository = new DocumentRepository(context);
                var result = repository.GetPagedDocuments("John", DocumentType.Contract, 1, 1);
                Assert.Empty(result.Item1.Where(doc => ((Document)doc).IsArchived));
            }
        }

        [Fact]
        public async void GetPagedDocuments_WithNoMatchingDocuments_ShouldReturnEmptyList()
        {
            await using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var repository = new DocumentRepository(context);
                var result = repository.GetPagedDocuments("John", DocumentType.Contract, 1, 1);
                Assert.Empty(result.Item1);
            }
        }

        [Fact]
        public async void GetArchivedPagedDocuments_ShouldReturnPagedResult()
        {
            await using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);
                var result = repository.GetArchivedPagedDocuments("Jane", DocumentType.Certificaat, 1, 1);

                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);
                Assert.All(result.Item1, doc => Assert.True(((DocumentOverviewResponse)doc).IsArchived));
            }
        }

        [Fact]
        public async void GetArchivedPagedDocuments_WithNoMatchingDocuments_ShouldReturnEmptyList()
        {
            await using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var repository = new DocumentRepository(context);
                var result = repository.GetArchivedPagedDocuments("John", DocumentType.Contract, 1, 1);
                Assert.Empty(result.Item1);
            }
        }

        [Fact]
        public async void GetLongValidPagedDocuments_ShouldReturnPagedResult()
        {
            await using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                var documentRepository = new DocumentRepository(context);
                var documentService = new DocumentService(documentRepository);

                var result = documentService.GetLongValidPagedDocuments("Meredith", DocumentType.Paspoort, 1, 1);

                Assert.Single(result.Item1);
                Assert.All(result.Item1, doc => Assert.False(((DocumentOverviewResponse)doc).IsArchived));
                Assert.All(result.Item1,
                    doc => Assert.True(((DocumentOverviewResponse)doc).Date >= DateTime.Now.AddDays(42)));
            }
        }

        [Fact]
        public void GetLongValidPagedDocuments_WithArchivedDocuments_ShouldExcludeArchivedDocuments()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                context.Documents.AddRange(_documents);
                context.SaveChanges();

                var documentRepository = new DocumentRepository(context);
                var documentService = new DocumentService(documentRepository);

                var result = documentService.GetLongValidPagedDocuments("John", DocumentType.Contract, 1, 1);

                Assert.Empty(result.Item1.Where(doc => ((Document)doc).IsArchived));
            }
        }

        [Fact]
        public void GetLongValidPagedDocuments_WithNoMatchingDocuments_ShouldReturnEmptyList()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var documentRepository = new DocumentRepository(context);
                var documentService = new DocumentService(documentRepository);

                var result = documentService.GetLongValidPagedDocuments("John", DocumentType.Contract, 1, 1);

                Assert.Empty(result.Item1);
            }
        }

        [Fact]
        public void GetDocumentTypeStrings_ShouldReturnCorrectList()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository yourClassInstance = new DocumentRepository(context);
                List<string> result = yourClassInstance.GetDocumentTypeStrings();

                Assert.NotNull(result);
                Assert.Equal(7, result.Count);

                Assert.Contains("Vog", result);
                Assert.Contains("Contract", result);
                Assert.Contains("Paspoort", result);
                Assert.Contains("ID kaart", result);
                Assert.Contains("Diploma", result);
                Assert.Contains("Certificaat", result);
                Assert.Contains("Lease auto", result);
                Assert.DoesNotContain("UndefinedType", result);
            }
        }

        [Fact]
        public void GetDocumentTypeStrings_ShouldNotContainUnderscore()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository repository = new DocumentRepository(context);
                List<string> result = repository.GetDocumentTypeStrings();
                Assert.All(result, typeString => Assert.DoesNotContain("_", typeString));
            }
        }

        [Fact]
        public async Task GetDocumentById_ShouldReturnDocumentResponse()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);
                var result = await repository.GetDocumentById(1);

                Assert.NotNull(result);
                Assert.IsType<DocumentResponse>(result);
                Assert.Equal("SampleFile"u8.ToArray(), result.File);
                Assert.Equal("SampleType", result.FileType);
                Assert.Equal(DateTime.Now.Date, result.Date.Date);
                Assert.Equal("John", result.Employee.Name);
                Assert.Equal(DocumentType.Contract, result.Type);
            }
        }

        [Fact]
        public async Task GetDocumentById_WithNonExistentId_ShouldReturnNull()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);

                var result = await repository.GetDocumentById(999);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddDocument_WithValidInput_ShouldAddDocument()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();
                Assert.NotNull(await context.Documents.FindAsync(_documents.FirstOrDefault().DocumentId));
            }
        }

        [Fact]
        public async Task AddDocument_WithInvalidType_ShouldThrowValidationException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var document = new Document
                {
                    Type = DocumentType.Not_selected,
                    Employee = new Employee { Name = "John", Email = "john@example.com" }
                };

                var repository = new DocumentRepository(context);

                await Assert.ThrowsAsync<InputValidationException>(() => repository.AddDocument(document));
            }
        }

        [Fact]
        public async Task AddDocument_WithInvalidEmail_ShouldThrowValidationException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var document = new Document
                {
                    Type = DocumentType.Contract,
                    Employee = new Employee { Name = "John", Email = "invalidemail" }
                };

                var repository = new DocumentRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.AddDocument(document));
            }
        }

        [Fact]
        public async Task UpdateDocument_WithValidInput_ShouldUpdateDocument()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                var editDocumentRequest = new EditDocumentRequest
                {
                    DocumentId = documentId,
                    Type = DocumentType.Contract,
                    Date = DateTime.Now.AddDays(10),
                };

                var existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = DocumentType.Certificaat,
                    Date = DateTime.Now,
                    Employee = new Employee()
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);

                await repository.UpdateDocument(editDocumentRequest);
                var updatedDocument = await context.Documents.FindAsync(documentId);

                Assert.NotNull(updatedDocument);
                Assert.Equal(editDocumentRequest.Date, updatedDocument.Date);
                Assert.Equal(editDocumentRequest.Type, updatedDocument.Type);
            }
        }

        [Fact]
        public async Task UpdateDocument_WithInvalidType_ShouldThrowValidationException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                var editDocumentRequest = new EditDocumentRequest
                {
                    DocumentId = documentId,
                    Type = DocumentType.Not_selected,
                    Date = DateTime.Now.AddDays(10),
                };

                var existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = DocumentType.Certificaat,
                    Date = DateTime.Now,
                    Employee = new Employee()
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);

                var actualException =
                    await Assert.ThrowsAsync<InputValidationException>(() =>
                        repository.UpdateDocument(editDocumentRequest));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task UpdateDocument_WithEmptyDate_ShouldThrowValidationException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                var editDocumentRequest = new EditDocumentRequest
                {
                    DocumentId = documentId,
                    Type = DocumentType.Contract,
                    Date = DateTime.MinValue,
                };

                var existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = DocumentType.Certificaat,
                    Date = DateTime.Now,
                    Employee = new Employee()
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);

                var actualException =
                    await Assert.ThrowsAsync<InputValidationException>(() =>
                        repository.UpdateDocument(editDocumentRequest));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task UpdateIsArchived_ShouldUpdateIsArchived()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                var checkBoxRequest = new CheckBoxRequest
                {
                    DocumentId = documentId,
                    IsArchived = true
                };

                var existingDocument = new Document
                {
                    DocumentId = documentId,
                    IsArchived = false
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);

                await repository.UpdateIsArchived(checkBoxRequest);
                var updatedDocument = await context.Documents.FindAsync(documentId);

                Assert.NotNull(updatedDocument);
                Assert.True(existingDocument.IsArchived);
            }
        }

        [Fact]
        public async Task UpdateIsArchived_WithNonExistentId_ShouldThrowNotFoundException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                var checkBoxRequest = new CheckBoxRequest
                {
                    DocumentId = 0,
                    IsArchived = true
                };

                var existingDocument = new Document
                {
                    DocumentId = documentId,
                    IsArchived = false
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);

                var actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.UpdateIsArchived(checkBoxRequest));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task DeleteDocument_ShouldDeleteDocument()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var documentId = 1;
                var existingDocument = new Document
                {
                    DocumentId = documentId,
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);

                await repository.DeleteDocument(documentId);

                var deletedDocument = await context.Documents.FindAsync(documentId);
                Assert.Null(deletedDocument); 
            }
        }
        
        [Fact]
        public async Task DeleteDocument_WithNonExistentId_ShouldThrowNotFoundException()
        {
            using (var context = new ApplicationDbContext(CreateNewOptions()))
            {
                var documentId = 0;
                var existingDocument = new Document
                {
                    DocumentId = 1,
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                var repository = new DocumentRepository(context);

                var actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.DeleteDocument(documentId));
                Assert.NotNull(actualException);
            }
        }
    }
}