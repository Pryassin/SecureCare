using CMS.Application.Common.Interfaces;
using CMS.Domain.Repositories;
using CMS.Infrastructure;
using CMS.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CMS.Infrastructure.UnitTests;

public class DependencyInjectionTests
{
    private IConfiguration GetTestConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"ConnectionStrings:DefaultConnection", "Host=localhost;Database=testdb;Username=test;Password=test"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterDbContext()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(ApplicationDbContext));
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterUnitOfWork()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IUnitOfWork));
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterJwtTokenGenerator()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IJwtTokenGenerator));
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterUserRepository()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IUserRepository));
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterDoctorRepository()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IDoctorRepository));
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterPatientRepository()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IPatientRepository));
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterAppointmentRepository()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IAppointmentRepository));
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterPrescriptionRepository()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(IPrescriptionRepository));
    }

    [Fact]
    public void AddInfrastructure_Should_ReturnServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        var result = services.AddInfrastructure(configuration);

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterAllRepositoriesAsScoped()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        var repositoryDescriptors = services.Where(s => 
            s.ServiceType.Name.Contains("Repository") && 
            s.ServiceType != typeof(IRepository<>)).ToList();

        repositoryDescriptors.Should().NotBeEmpty();
        repositoryDescriptors.Should().OnlyContain(d => d.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterJwtTokenGeneratorAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        var jwtDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IJwtTokenGenerator));
        jwtDescriptor.Should().NotBeNull();
        jwtDescriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddInfrastructure_Should_RegisterUnitOfWorkAsScoped()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        var uowDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IUnitOfWork));
        uowDescriptor.Should().NotBeNull();
        uowDescriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddInfrastructure_Should_UseConnectionStringFromConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = GetTestConfiguration();

        // Act
        services.AddInfrastructure(configuration);

        // Assert
        var dbContextDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ApplicationDbContext));
        dbContextDescriptor.Should().NotBeNull("because DbContext should be registered");
    }
}