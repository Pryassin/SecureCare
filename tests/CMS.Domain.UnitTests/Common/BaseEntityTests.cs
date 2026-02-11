using CMS.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CMS.Domain.UnitTests.Common;

public class BaseEntityTests
{
    // Create a concrete test class since BaseEntity is abstract
    private class TestEntity : BaseEntity
    {
        public TestEntity() : base() { }
        public TestEntity(Guid id) : base(id) { }
    }

    [Fact]
    public void Constructor_WithEmptyGuid_Should_GenerateNewId()
    {
        // Act
        var entity = new TestEntity(Guid.Empty);

        // Assert
        entity.Id.Should().NotBe(Guid.Empty);
        entity.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_WithSpecificGuid_Should_UseProvidedId()
    {
        // Arrange
        var specificId = Guid.NewGuid();

        // Act
        var entity = new TestEntity(specificId);

        // Assert
        entity.Id.Should().Be(specificId);
    }

    [Fact]
    public void ParameterlessConstructor_Should_GenerateNewId()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        entity.Id.Should().NotBe(Guid.Empty);
        entity.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void MultipleEntities_Should_HaveDifferentIds()
    {
        // Act
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();
        var entity3 = new TestEntity(Guid.Empty);

        // Assert
        entity1.Id.Should().NotBe(entity2.Id);
        entity2.Id.Should().NotBe(entity3.Id);
        entity1.Id.Should().NotBe(entity3.Id);
    }

    [Fact]
    public void Id_Should_BeReadOnly()
    {
        // Arrange
        var entity = new TestEntity();
        var originalId = entity.Id;

        // Assert - Id should have a protected setter, so it can't be changed from outside
        entity.Id.Should().Be(originalId);
        
        // Verify the property is read-only from external code
        var idProperty = typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id));
        idProperty.Should().NotBeNull();
        idProperty!.SetMethod.Should().NotBeNull();
        idProperty.SetMethod!.IsFamily.Should().BeTrue(); // protected
    }
}
