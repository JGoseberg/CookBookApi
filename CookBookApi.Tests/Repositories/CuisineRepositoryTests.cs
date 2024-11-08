using AutoMapper;
using CookBookApi.Mappings;
using CookBookApi.Models;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class CuisineRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;
    private IMapper _mapper;

    private readonly Cuisine _cuisine = new Cuisine
    {
        Name = "Foo"
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
    public async Task AddCuisineAsync_ValidCuisine_CuisineIsAdded()
    {
        await using var context = new CookBookContext(_options);
        
        var repository = new CuisineRepository(context, _mapper);
        
        await repository.AddCuisineAsync(_cuisine);
        
        var addedCuisine = await context.Cuisines.FirstOrDefaultAsync();

        Assert.Multiple(() =>
        {
            Assert.That(addedCuisine, Is.Not.Null);
            Assert.That(addedCuisine!.Id, Is.EqualTo(_cuisine.Id));
        });
    }

    [Test]
    public async Task AnyCuisineWithSameNameAsync_NameExists_ReturnsTrue()
    {
        await using var context = new CookBookContext(_options);

        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);
        
        var result = await repository.AnyCuisineWithSameNameAsync(_cuisine.Name);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AnyCuisineWithSameNameAsync_NameDoesNotExists_ReturnsFalse()
    {
        await using var context = new CookBookContext(_options);

        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);
        
        var result = await repository.AnyCuisineWithSameNameAsync("Bar");
        
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteCuisineAsync_ValidCuisineId_CuisineIsDeleted()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);
        
        await repository.DeleteCuisineAsync(_cuisine.Id);
        
        Assert.That(context.Cuisines.FirstOrDefault(c => c.Id == _cuisine.Id), Is.Null);
    }

    [Test]
    public async Task DeleteCuisineAsync_InvalidCuisineId_CuisineThrowsOperationException()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);
        
        await repository.DeleteCuisineAsync(404);


        Assert.Multiple(() =>
        {
            Assert.That(context.Cuisines.FirstOrDefault(c => c.Id == _cuisine.Id), Is.Not.Null);
            Assert.That(context.Cuisines.FirstOrDefault(c => c.Id == _cuisine.Id), Is.EqualTo(_cuisine));
        });
    }

    [Test]
    public async Task GetAllCuisinesAsync_ReturnsAllCuisines()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);
        
        var cuisines = await repository.GetAllCuisinesAsync();
        
        Assert.Multiple(() =>
        {
            var cuisineList = cuisines.ToList();
            Assert.That(cuisineList, Is.Not.Null);
            Assert.That(cuisineList.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public async Task GetCuisineByIdAsync_InValidCuisine_CuisineReturnsNull()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);
        
        var cuisine = await repository.GetCuisineByIdAsync(404);
        
        Assert.That(cuisine, Is.Null);
    }

    [Test]
    public async Task GetCuisinesByIdAsync_ValidCuisineId_CuisineReturnsCuisine()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);
        
        var cuisine = await repository.GetCuisineByIdAsync(_cuisine.Id);
        
        Assert.Multiple(() =>
        {
            Assert.That(cuisine!.Id, Is.EqualTo(_cuisine.Id));
            Assert.That(cuisine.Name, Is.EqualTo(_cuisine.Name));
        });
    }
    
    [Test]
    public async Task UpdateCuisineAsync_ValidCuisine_CuisineIsUpdated()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);

        var cuisineToUpdate = new Cuisine
        {
            Id = _cuisine.Id,
            Name = "Foo"
        };
        
        await repository.UpdateCuisineAsync(cuisineToUpdate);
        
        Assert.Multiple(() =>
        {
            Assert.That(context.Cuisines.FirstOrDefault(), Is.Not.Null);
            Assert.That(context.Cuisines.FirstOrDefault()!.Name, Is.EqualTo(cuisineToUpdate.Name));
        });
    }

    [Test]
    public async Task UpdateCuisineAsync_NameIsEmpty_CuisineThrowsOperationException()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();
        
        var repository = new CuisineRepository(context, _mapper);

        var cuisineToUpdate = new Cuisine
        {
            Id = _cuisine.Id,
            Name = string.Empty
        };
        
        await repository.UpdateCuisineAsync(cuisineToUpdate);
        
        Assert.That(context.Cuisines.FirstOrDefault(), Is.EqualTo(_cuisine));
    }

    [Test]
    public async Task UpdateCuisineAsync_InvalidId_CuisineReturnsNullAndNothingIsUpdated()
    {
        await using var context = new CookBookContext(_options);

        await context.Cuisines.AddAsync(_cuisine);
        await context.SaveChangesAsync();

        var repository = new CuisineRepository(context, _mapper);

        var cuisineToUpdate = new Cuisine
        {
            Id = 404,
            Name = _cuisine.Name
        };

        await repository.UpdateCuisineAsync(cuisineToUpdate);
        
        Assert.That(context.Cuisines.FirstOrDefault(), Is.EqualTo(_cuisine));
    }
}