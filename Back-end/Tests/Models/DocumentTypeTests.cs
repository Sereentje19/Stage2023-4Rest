using FluentAssertions;
using PL.Models;

namespace Tests.Models
{

    public class DocumentTypeTests
    {
        [Fact]
        public void Enum_Contains_All_Expected_Values()
        {
            var expectedValues = new[]
            {
                DocumentType.Not_selected,
                DocumentType.Vog,
                DocumentType.Contract,
                DocumentType.Paspoort,
                DocumentType.ID_kaart,
                DocumentType.Diploma,
                DocumentType.Certificaat,
                DocumentType.Lease_auto
            };

            var enumValues = (DocumentType[])Enum.GetValues(typeof(DocumentType));
            enumValues.Should().BeEquivalentTo(expectedValues);
        }

        [Theory]
        [InlineData(DocumentType.Not_selected)]
        [InlineData(DocumentType.Vog)]
        [InlineData(DocumentType.Contract)]
        [InlineData(DocumentType.Paspoort)]
        [InlineData(DocumentType.ID_kaart)]
        [InlineData(DocumentType.Diploma)]
        [InlineData(DocumentType.Certificaat)]
        [InlineData(DocumentType.Lease_auto)]
        public void Enum_Has_Valid_Names(DocumentType value)
        {
            value.ToString().Should().NotBeNullOrEmpty();
        }

    }
}