using AutoMapper;
using CookBookApi.DTOs.MeasurementUnit;
using CookBookApi.Mappings;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class MeasurementUnitRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;
    private IMapper _mapper;

    private readonly MeasurementUnit _measurementUnit = new MeasurementUnit
    {
        Name = "Foo",
        Abbreviation = "Bar",
    };

    [SetUp]
    public void SetUp()
    {
        _options = new DbContextOptionsBuilder<CookBookContext>()
            .UseInMemoryDatabase(databaseName: "CookBook")
            .Options;

        _mapper = MapperTestConfig.InitializeAutoMapper();
    }

    [TearDown]
    public void TearDown()
    {
        using var context = new CookBookContext(_options);
        context.Database.EnsureDeleted();
    }

    [Test]
    public async Task AddMeasurementUnitAsync_ValidMeasurementUnit_MeasurementUnitAdded()
    {
        await using var context = new CookBookContext(_options);
        
        var repository = new MeasurementunitRepository(context, _mapper);
        
        await repository.AddMeasurementUnitAsync(_measurementUnit);
        
        var addedMeasurementUnit = await context.MeasurementUnits.FindAsync(_measurementUnit.Id);
        
        Assert.That(context.MeasurementUnits.Count(), Is.EqualTo(1));
        Assert.That(addedMeasurementUnit, Is.EqualTo(_measurementUnit));
    }

    [Test]
    public async Task AnyMeasurementUnitWithSameName_MeasurementUnitExists_ReturnsTrue()
    {
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);
        
        var result = await repository.AnyMeasurementUnitWithSameNameAsync(_measurementUnit.Name);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AnyMeasurementUnitWithSameName_MeasurementUnitDoesNotExist_ReturnsFalse()
    {
        var measurementUnitName = "Bar";
        
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);
        
        var result = await repository.AnyMeasurementUnitWithSameNameAsync(measurementUnitName);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteMeasurementUnitAsync_ValidMeasurementUnit_MeasurementUnitDeleted()
    {
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);
        
        await repository.DeleteMeasurementUnitAsync(_measurementUnit.Id);
        
        Assert.That(context.MeasurementUnits.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task DeleteMeasurementUnitAsync_InvalidMeasurementUnit_MeasurementUnitNotDeleted()
    {
        var measurementUnitId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);
        
        await repository.DeleteMeasurementUnitAsync(measurementUnitId);
        
        Assert.Multiple(() =>
        {
            Assert.That(context.MeasurementUnits.Count(), Is.EqualTo(1));
            Assert.That(context.MeasurementUnits.FirstOrDefault(mu => mu.Id == _measurementUnit.Id), Is.EqualTo(_measurementUnit));
        });
    }

    [Test]
    public async Task GetAllMeasurementUnitsAsync_ReturnsAllMeasurementUnits()
    {
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);

        var measurementUnits = await repository.GetAllMeasurementunitsAsync();
        
        Assert.That(measurementUnits.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetMeasurementUnitByIdAsync_ValidMeasurementUnitId_ReturnsMeasurementUnit()
    {
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);
        
        var measurementUnit = await repository.GetMeasurementUnitByIdAsync(_measurementUnit.Id);
        
        Assert.That(measurementUnit, Is.Not.Null);
    }

    [Test]
    public async Task GetMeasurementUnitByIdAsync_InvalidMeasurementUnitId_ReturnsNull()
    {
        var invalidMeasurementUnitId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);
        
        var measurementUnit = await repository.GetMeasurementUnitByIdAsync(invalidMeasurementUnitId);
        
        Assert.That(measurementUnit, Is.Null);
    }

    [Test]
    public async Task UpdateMeasurementUnitAsync_ValidMeasurementUnit_MeasurementUnitIsUpdated()
    {
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);

        var measurementUnitToUpdate = new MeasurementUnit
        {
            Id = _measurementUnit.Id,
            Name = "Bar",
            Abbreviation = "Foo",
        };

        await repository.UpdateMeasurementUnitAsync(measurementUnitToUpdate);
        
        Assert.Multiple(() => 
        { 
            Assert.That(context.MeasurementUnits.Count(), Is.EqualTo(1)); 
            Assert.That(context.MeasurementUnits.FirstOrDefault(mu => mu.Id == measurementUnitToUpdate.Id).Name, 
                Is.EqualTo(measurementUnitToUpdate.Name));
            Assert.That(context.MeasurementUnits.FirstOrDefault(mu => mu.Id == measurementUnitToUpdate.Id).Abbreviation,
                Is.EqualTo(measurementUnitToUpdate.Abbreviation));
            Assert.That(context.MeasurementUnits.FirstOrDefault(mu => mu.Id == measurementUnitToUpdate.Id).Id,
                Is.EqualTo(_measurementUnit.Id));
        });
    }

    [Test]
    public async Task UpdateMeasurementUnitAsync_NameIsEmpty_MeasurementUnitIsNotUpdated()
    {
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);

        var measurementUnitToUpdate = new MeasurementUnit
        {
            Id = _measurementUnit.Id,
            Name = string.Empty,
            Abbreviation = "Foo",
        };

        await repository.UpdateMeasurementUnitAsync(measurementUnitToUpdate);
        
        Assert.That(context.MeasurementUnits.FirstOrDefault(), Is.EqualTo(_measurementUnit));
    }

    [Test]
    public async Task UpdateMeasurementUnitAsync_AbbreviationIsEmpty_MeasurementUnitIsNotUpdated()
    {
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);

        var measurementUnitToUpdate = new MeasurementUnit
        {
            Id = _measurementUnit.Id,
            Name = "Bar",
            Abbreviation = string.Empty,
        };

        await repository.UpdateMeasurementUnitAsync(measurementUnitToUpdate);
        
        Assert.That(context.MeasurementUnits.FirstOrDefault(), Is.EqualTo(_measurementUnit));
    }

    [Test]
    public async Task UpdateMeasurementUnitAsync_MeasurementUnitDoesNotExist_MeasurementUnitIsNotUpdated()
    {
        await using var context = new CookBookContext(_options);
        
        await context.MeasurementUnits.AddAsync(_measurementUnit);
        await context.SaveChangesAsync();
        
        var repository = new MeasurementunitRepository(context, _mapper);

        var measurementUnitToUpdate = new MeasurementUnit
        {
            Id = 404,
            Name = "Bar",
            Abbreviation = "Foo",
        };

        await repository.UpdateMeasurementUnitAsync(measurementUnitToUpdate);
        
        Assert.That(context.MeasurementUnits.FirstOrDefault(), Is.EqualTo(_measurementUnit));
    }
}