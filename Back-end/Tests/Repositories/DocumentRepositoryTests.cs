using BLL.Services;
using DAL.Data;
using DAL.Exceptions;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

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
                Date = DateTime.Now, IsArchived = false, Type = new DocumentType { Name = "Contract", Id = 1 }
            },
            new Document
            {
                DocumentId = 2, Employee = new Employee { EmployeeId = 2, Name = "Jane", Email = "jane@example.com" },
                Date = DateTime.Now.AddDays(30), IsArchived = true, Type = new DocumentType { Name = "Contract", Id = 2 }
            },
            new Document
            {
                DocumentId = 3,
                Employee = new Employee { EmployeeId = 3, Name = "Meredith", Email = "Meredith@example.com" },
                Date = DateTime.Now.AddDays(60), IsArchived = false, Type = new DocumentType { Name = "Contract", Id = 3 }
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
                (IEnumerable<object>, int) result = repository.GetPagedDocuments("John", "1", 1, 1);

                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);
                Assert.All(result.Item1, doc => Assert.False(((DocumentOverviewResponseDto)doc).IsArchived));
                Assert.All(result.Item1,
                    doc => Assert.True(((DocumentOverviewResponseDto)doc).Date <= DateTime.Now.AddDays(42)));
            }
        }

        [Fact]
        public async void GetPagedDocuments_WithArchivedDocuments_ShouldExcludeArchivedDocuments()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository repository = new DocumentRepository(context);
                (IEnumerable<object>, int) result = repository.GetPagedDocuments("John", "Contract", 1, 1);
                Assert.Empty(result.Item1.Where(doc => ((Document)doc).IsArchived));
            }
        }

        [Fact]
        public async void GetPagedDocuments_WithNoMatchingDocuments_ShouldReturnEmptyList()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository repository = new DocumentRepository(context);
                (IEnumerable<object>, int) result = repository.GetPagedDocuments("John", "Contract", 1, 1);
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
                (IEnumerable<object>, int) result =
                    repository.GetArchivedPagedDocuments("Jane", "2", 1, 1);

                Assert.Single(result.Item1);
                Assert.Equal(1, result.Item2);
                Assert.All(result.Item1, doc => Assert.True(((DocumentOverviewResponseDto)doc).IsArchived));
            }
        }

        [Fact]
        public async void GetArchivedPagedDocuments_WithNoMatchingDocuments_ShouldReturnEmptyList()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                DocumentRepository repository = new DocumentRepository(context);
                (IEnumerable<object>, int) result =
                    repository.GetArchivedPagedDocuments("John", "Contract", 1, 1);
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

                (IEnumerable<object>, Pager) result =
                    documentService.GetLongValidPagedDocuments("Meredith", "3", 1, 1);

                Assert.Single(result.Item1);
                Assert.All(result.Item1, doc => Assert.False(((DocumentOverviewResponseDto)doc).IsArchived));
                Assert.All(result.Item1,
                    doc => Assert.True(((DocumentOverviewResponseDto)doc).Date >= DateTime.Now.AddDays(42)));
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

                (IEnumerable<object>, Pager) result =
                    documentService.GetLongValidPagedDocuments("John", "Contract", 1, 1);

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

                (IEnumerable<object>, Pager) result =
                    documentService.GetLongValidPagedDocuments("John", "Contract", 1, 1);

                Assert.Empty(result.Item1);
            }
        }

        [Fact]
        public async Task GetDocumentTypes_ShouldReturnListOfDocumentTypes()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                List<DocumentType> documentTypes = new List<DocumentType>
                {
                    new DocumentType { Id = 1, Name = "Type1" },
                    new DocumentType { Id = 2, Name = "Type2" },
                };

                context.DocumentTypes.AddRange(documentTypes);
                await context.SaveChangesAsync();

                DocumentRepository yourService = new DocumentRepository(context);
                IEnumerable<DocumentType> result = await yourService.GetDocumentTypesAsync();

                Assert.NotNull(result);
                Assert.IsType<List<DocumentType>>(result);
                Assert.Equal(2, result.Count()); 
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
                DocumentResponseDto result = await repository.GetDocumentByIdAsync(1);

                Assert.NotNull(result);
                Assert.IsType<DocumentResponseDto>(result);
                Assert.Equal("SampleFile"u8.ToArray(), result.File);
                Assert.Equal("SampleType", result.FileType);
                Assert.Equal(DateTime.Now.Date, result.Date.Date);
                Assert.Equal("John", result.Employee.Name);
                Assert.Equal("Contract", result.Type.Name);
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

                DocumentResponseDto result = await repository.GetDocumentByIdAsync(999);
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
                    Type = new DocumentType { Id = 1, Name = "Contract" },
                    Employee = new Employee { Name = "John", Email = "john@example.com" }
                };

                DocumentRepository repository = new DocumentRepository(context);

                await Assert.ThrowsAsync<InputValidationException>(() => repository.CreateDocumentAsync(document));
            }
        }

        [Fact]
        public async Task AddDocument_WithInvalidEmail_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Document document = new Document
                {
                    Type = new DocumentType { Id = 1, Name = "Contract" },
                    Employee = new Employee { Name = "John", Email = "invalidemail" }
                };

                DocumentRepository repository = new DocumentRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.CreateDocumentAsync(document));
            }
        }

        [Fact]
        public async Task AddDocument_WithEmptyName_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Document document = new Document
                {
                    Type = new DocumentType { Id = 1, Name = "Contract" },
                    Employee = new Employee { Name = "", Email = "john@example.com" }
                };

                DocumentRepository repository = new DocumentRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.CreateDocumentAsync(document));
            }
        }

        [Fact]
        public async Task AddDocument_WithInvalidDate_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Document document = new Document
                {
                    Type = new DocumentType { Id = 1, Name = "Contract" },
                    Date = DateTime.Today.AddDays(-1),
                    Employee = new Employee { Name = "John", Email = "john@example.com" }
                };

                DocumentRepository repository = new DocumentRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => repository.CreateDocumentAsync(document));
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
                    Type = new DocumentType { Id = 1, Name = "Contract" },
                    Date = DateTime.Today,
                    Employee = new Employee { Email = existingEmployeeEmail, Name = "test" }
                };

                await documentRepository.CreateDocumentAsync(document);

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
                EditDocumentRequestDto editDocumentRequestDto = new EditDocumentRequestDto
                {
                    DocumentId = documentId,
                    Type = new DocumentType { Id = 5, Name = "Contract" },
                    Date = DateTime.Now.AddDays(10),
                };

                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = new DocumentType { Id = 6, Name = "Certificaat" },
                    Date = DateTime.Now,
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                await repository.UpdateDocumentAsync(editDocumentRequestDto);
                Document updatedDocument = await context.Documents.FindAsync(documentId);

                Assert.NotNull(updatedDocument);
                Assert.Equal(editDocumentRequestDto.Date, updatedDocument.Date);
                Assert.Equal(editDocumentRequestDto.Type, updatedDocument.Type);
            }
        }

        [Fact]
        public async Task UpdateDocument_WithInvalidType_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                EditDocumentRequestDto editDocumentRequestDto = new EditDocumentRequestDto
                {
                    DocumentId = documentId,
                    Type = new DocumentType { Id = 1, Name = "0" },
                    Date = DateTime.Now.AddDays(10),
                };

                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = new DocumentType { Id = 1, Name = "Certificaat" },
                    Date = DateTime.Now,
                    Employee = new Employee { Email = "test@test.nl", Name = "test" }
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                InputValidationException actualException =
                    await Assert.ThrowsAsync<InputValidationException>(() =>
                        repository.UpdateDocumentAsync(editDocumentRequestDto));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task UpdateDocument_WithEmptyDate_ShouldThrowValidationException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                EditDocumentRequestDto editDocumentRequestDto = new EditDocumentRequestDto
                {
                    DocumentId = documentId,
                    Type = new DocumentType { Id = 1, Name = "0" },
                    Date = DateTime.MinValue,
                };

                Document existingDocument = new Document
                {
                    DocumentId = documentId,
                    Type = new DocumentType { Id = 1, Name = "Certificaat" },
                    Date = DateTime.Now,
                    Employee = new Employee { Email = "test@test.nl", Name = "test" }
                };

                await context.Documents.AddAsync(existingDocument);
                await context.SaveChangesAsync();

                DocumentRepository repository = new DocumentRepository(context);

                InputValidationException actualException =
                    await Assert.ThrowsAsync<InputValidationException>(() =>
                        repository.UpdateDocumentAsync(editDocumentRequestDto));
                Assert.NotNull(actualException);
            }
        }

        [Fact]
        public async Task UpdateIsArchived_ShouldUpdateIsArchived()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                const int documentId = 1;
                CheckBoxRequestDto checkBoxRequestDto = new CheckBoxRequestDto
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

                await repository.UpdateIsArchivedAsync(checkBoxRequestDto);
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
                CheckBoxRequestDto checkBoxRequestDto = new CheckBoxRequestDto
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
                        repository.UpdateIsArchivedAsync(checkBoxRequestDto));
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

                await repository.DeleteDocumentAsync(documentId);

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
                        repository.DeleteDocumentAsync(documentId));
                Assert.NotNull(actualException);
            }
        }
    }
}