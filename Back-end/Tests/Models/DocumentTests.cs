using System.ComponentModel.DataAnnotations.Schema;
using Castle.Components.DictionaryAdapter;
using PL.Models;
using FluentAssertions;


namespace Tests.Models
{

    public class DocumentTests
    {
        [Fact]
        public void Document_PropertiesSetCorrectly()
        {
            var document = new Document
            {
                DocumentId = 1,
                File = new byte[] { 1, 2, 3 },
                FileType = "PDF",
                Date = DateTime.Now,
                Employee = new Employee { EmployeeId = 1, Name = "John Doe" },
                Type = DocumentType.Contract,
                IsArchived = false
            };

            document.DocumentId.Should().Be(1);
            document.File.Should().Equal(1, 2, 3);
            document.FileType.Should().Be("PDF");
            document.Date.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(1000)); 
            document.Employee.Should().NotBeNull();
            document.Employee.EmployeeId.Should().Be(1);
            document.Type.Should().Be(DocumentType.Contract);
            document.IsArchived.Should().BeFalse();
        }

        [Fact]
        public void Document_TypeColumnHasCorrectType()
        {
            var typeProperty = typeof(Document).GetProperty(nameof(Document.Type));
            typeProperty.Should().BeDecoratedWith<ColumnAttribute>(a => a.TypeName == "nvarchar(24)");
        }

        [Fact]
        public void Document_HasKeyAttributeOnDocumentId()
        {
            var documentIdProperty = typeof(Document).GetProperty(nameof(Document.DocumentId));
            documentIdProperty.Should().BeDecoratedWith<System.ComponentModel.DataAnnotations.KeyAttribute>();
        }

        [Fact]
        public void Document_ForeignKeyAttributeOnEmployeeId()
        {
            var employeeProperty = typeof(Document).GetProperty(nameof(Document.Employee));
            employeeProperty.Should().BeDecoratedWith<ForeignKeyAttribute>(a => a.Name == "EmployeeId");
        }
    }
}