using CMS.Application;
using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;

namespace CMS.Application.UnitTests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_Should_RegisterMediatRServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IMediator));
    }

    [Fact]
    public void AddApplication_Should_RegisterValidators()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        var validatorDescriptors = services.Where(s => 
            s.ServiceType.IsGenericType && 
            s.ServiceType.GetGenericTypeDefinition() == typeof(IValidator<>));
        
        validatorDescriptors.Should().NotBeEmpty("because validators should be registered");
    }

    [Fact]
    public void AddApplication_Should_RegisterMapper()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IMapper));
    }

    [Fact]
    public void AddApplication_Should_RegisterMapsterConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(Mapster.TypeAdapterConfig));
    }

    [Fact]
    public void AddApplication_Should_ReturnServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddApplication();

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddApplication_Should_RegisterMapperAsScoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        var mapperDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IMapper));
        mapperDescriptor.Should().NotBeNull();
        mapperDescriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddApplication_Should_RegisterMapsterConfigAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        var configDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(Mapster.TypeAdapterConfig));
        configDescriptor.Should().NotBeNull();
        configDescriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddApplication_Should_RegisterServicesFromAssembly()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        services.Should().NotBeEmpty("because services should be registered from the assembly");
        services.Count.Should().BeGreaterThan(10, "because multiple services should be registered");
    }
}
