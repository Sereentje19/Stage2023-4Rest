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
        private readonly List<Document> _documents = new()
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

        private static DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async void GetPagedDocuments_ShouldReturnPagedResult()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);
                (IEnumerable<object>, int) result = repository.GetPagedDocuments("John", DocumentType.Contract, 1, 1);

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
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository repository = new DocumentRepository(context);
                (IEnumerable<object>, int) result = repository.GetPagedDocuments("John", DocumentType.Contract, 1, 1);
                Assert.Empty(result.Item1.Where(doc => ((Document)doc).IsArchived));
            }
        }

        [Fact]
        public async void GetPagedDocuments_WithNoMatchingDocuments_ShouldReturnEmptyList()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository repository = new DocumentRepository(context);
                (IEnumerable<object>, int) result = repository.GetPagedDocuments("John", DocumentType.Contract, 1, 1);
                Assert.Empty(result.Item1);
            }
        }

        [Fact]
        public async void GetArchivedPagedDocuments_ShouldReturnPagedResult()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);
                (IEnumerable<object>, int) result = repository.GetArchivedPagedDocuments("Jane", DocumentType.Certificaat, 1, 1);

                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);
                Assert.All(result.Item1, doc => Assert.True(((DocumentOverviewResponse)doc).IsArchived));
            }
        }

        [Fact]
        public async void GetArchivedPagedDocuments_WithNoMatchingDocuments_ShouldReturnEmptyList()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository repository = new DocumentRepository(context);
                (IEnumerable<object>, int) result = repository.GetArchivedPagedDocuments("John", DocumentType.Contract, 1, 1);
                Assert.Empty(result.Item1);
            }
        }

        [Fact]
        public async void GetLongValidPagedDocuments_ShouldReturnPagedResult()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                DocumentRepository documentRepository = new DocumentRepository(context);
                DocumentService documentService = new DocumentService(documentRepository);

                (IEnumerable<object>, Pager) result = documentService.GetLongValidPagedDocuments("Meredith", DocumentType.Paspoort, 1, 1);

                Assert.Single(result.Item1);
                Assert.All(result.Item1, doc => Assert.False(((DocumentOverviewResponse)doc).IsArchived));
                Assert.All(result.Item1,
                    doc => Assert.True(((DocumentOverviewResponse)doc).Date >= DateTime.Now.AddDays(42)));
            }
        }

        [Fact]
        public void GetLongValidPagedDocuments_WithArchivedDocuments_ShouldExcludeArchivedDocuments()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                context.Documents.AddRange(_documents);
                context.SaveChanges();

                DocumentRepository documentRepository = new DocumentRepository(context);
                DocumentService documentService = new DocumentService(documentRepository);

                (IEnumerable<object>, Pager) result = documentService.GetLongValidPagedDocuments("John", DocumentType.Contract, 1, 1);

                Assert.Empty(result.Item1.Where(doc => ((Document)doc).IsArchived));
            }
        }

        [Fact]
        public void GetLongValidPagedDocuments_WithNoMatchingDocuments_ShouldReturnEmptyList()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository documentRepository = new DocumentRepository(context);
                DocumentService documentService = new DocumentService(documentRepository);

                (IEnumerable<object>, Pager) result = documentService.GetLongValidPagedDocuments("John", DocumentType.Contract, 1, 1);

                Assert.Empty(result.Item1);
            }
        }

        [Fact]
        public void GetDocumentTypeStrings_ShouldReturnCorrectList()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
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
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository repository = new DocumentRepository(context);
                List<string> result = repository.GetDocumentTypeStrings();
                Assert.All(result, typeString => Assert.DoesNotContain("_", typeString));
            }
        }

        [Fact]
        public async Task GetDocumentById_ShouldReturnDocumentResponse()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);
                DocumentResponse result = await repository.GetDocumentById(1);

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
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                DocumentResponse result = await repository.GetDocumentById(999);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddDocument_WithValidInput_ShouldAddDocument()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                await context.Documents.AddRangeAsync(_documents);
                await context.SaveChangesAsync();
                Assert.NotNull(await context.Documents.FindAsync(_documents.FirstOrDefault().DocumentId));
            }
        }

        [Fact]
        public async Task AddDocument_WithInvalidType_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Document document = new Document
                {
                    Type = DocumentType.Not_selected,
                    Employee = new Employee { Name = "John", Email = "john@example.com" }
                };

                DocumentRepository repository = new DocumentRepository(context);

                await Assert.ThrowsAsync<InputValidationException>(() => repository.AddDocument(document));
            }
        }

        [Fact]
        public async Task AddDocument_WithInvalidEmail_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Document document = new Document
                {
                    Type = DocumentType.Contract,
                    Employee = new Employee { Name = "John", Email = "invalidemail" }
                };

                DocumentRepository repository = new DocumentRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.AddDocument(document));
            }
        }
        
        [Fact]
        public async Task AddDocument_WithEmptyName_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Document document = new Document
                {
                    Type = DocumentType.Contract,
                    Employee = new Employee { Name = "", Email = "john@example.com" }
                };

                DocumentRepository repository = new DocumentRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.AddDocument(document));
            }
        }
        
        [Fact]
        public async Task AddDocument_WithInvalidDate_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Document document = new Document
                {
                    Type = DocumentType.Contract,
                    Date = DateTime.Today.AddDays(-1),
                    Employee = new Employee { Name = "John", Email = "john@example.com" }
                };

                DocumentRepository repository = new DocumentRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.AddDocument(document));
            }
        }
        
        [Fact]
        public async Task AddDocument_ShouldSetExistingEmployeeWhenFound()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const string existingEmployeeEmail = "existing@example.com";
                Employee existingEmployee = new Employee { Email = existingEmployeeEmail, Name = "test" };
                context.Employees.Add(existingEmployee);
                await context.SaveChangesAsync();

                DocumentRepository documentRepository = new DocumentRepository(context);

                Document document = new Document
                {
                    Type = DocumentType.Contract,
                    Date = DateTime.Today,
                    Employee = new Employee { Email = existingEmployeeEmail, Name = "test" }
                };

                await documentRepository.AddDocument(document);

                Assert.NotNull(document.Employee);
                Assert.Equal(existingEmployee, document.Employee);
            }
        }



        [Fact]
        public async Task UpdateDocument_WithValidInput_ShouldUpdateDocument()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                EditDocumentRequest editDocumentRequest = new EditDocumentRequest
                {
                    DocumentId = documentId,
                    Type = DocumentType.Contract,
                    Date = DateTime.Now.AddDays(10),
                };

                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = DocumentType.Certificaat,
                    Date = DateTime.Now,
                    Employee = new Employee { Email = "test@test.nl", Name = "test" }
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                await repository.UpdateDocument(editDocumentRequest);
                Document updatedDocument = await context.Documents.FindAsync(documentId);

                Assert.NotNull(updatedDocument);
                Assert.Equal(editDocumentRequest.Date, updatedDocument.Date);
                Assert.Equal(editDocumentRequest.Type, updatedDocument.Type);
            }
        }

        [Fact]
        public async Task UpdateDocument_WithInvalidType_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                EditDocumentRequest editDocumentRequest = new EditDocumentRequest
                {
                    DocumentId = documentId,
                    Type = DocumentType.Not_selected,
                    Date = DateTime.Now.AddDays(10),
                };

                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = DocumentType.Certificaat,
                    Date = DateTime.Now,
                    Employee = new Employee { Email = "test@test.nl", Name = "test" }
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                InputValidationException actualException =
                    await Assert.ThrowsAsync<InputValidationException>(() =>
                        repository.UpdateDocument(editDocumentRequest));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task UpdateDocument_WithEmptyDate_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                EditDocumentRequest editDocumentRequest = new EditDocumentRequest
                {
                    DocumentId = documentId,
                    Type = DocumentType.Contract,
                    Date = DateTime.MinValue,
                };

                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = DocumentType.Certificaat,
                    Date = DateTime.Now,
                    Employee = new Employee { Email = "test@test.nl", Name = "test" }
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                InputValidationException actualException =
                    await Assert.ThrowsAsync<InputValidationException>(() =>
                        repository.UpdateDocument(editDocumentRequest));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task UpdateIsArchived_ShouldUpdateIsArchived()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                CheckBoxRequest checkBoxRequest = new CheckBoxRequest
                {
                    DocumentId = documentId,
                    IsArchived = true
                };

                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                    IsArchived = false
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                await repository.UpdateIsArchived(checkBoxRequest);
                Document updatedDocument = await context.Documents.FindAsync(documentId);

                Assert.NotNull(updatedDocument);
                Assert.True(existingDocument.IsArchived);
            }
        }

        [Fact]
        public async Task UpdateIsArchived_WithNonExistentId_ShouldThrowNotFoundException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                CheckBoxRequest checkBoxRequest = new CheckBoxRequest
                {
                    DocumentId = 0,
                    IsArchived = true
                };

                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                    IsArchived = false
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                NotFoundException actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.UpdateIsArchived(checkBoxRequest));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task DeleteDocument_ShouldDeleteDocument()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                int documentId = 1;
                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                await repository.DeleteDocument(documentId);

                Document deletedDocument = await context.Documents.FindAsync(documentId);
                Assert.Null(deletedDocument);
            }
        }

        [Fact]
        public async Task DeleteDocument_WithNonExistentId_ShouldThrowNotFoundException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                int documentId = 0;
                Document existingDocument = new Document
                {
                    DocumentId = 1,
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                NotFoundException actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.DeleteDocument(documentId));
                Assert.NotNull(actualException);
            }
        }
    }
}