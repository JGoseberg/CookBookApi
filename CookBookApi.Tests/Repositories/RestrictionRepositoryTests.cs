using AutoMapper;
using CookBookApi.Mappings;
using CookBookApi.Models;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class RestrictionRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;
    private IMapper _mapper;

    private readonly Restriction _restriction = new Restriction()
    {
        Id = 1,
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
    public async Task AddRestrictionAsync_ValidRestriction_ShouldAddRestriction()
    {
        await using var context = new CookBookContext(_options);
        
        var repository = new RestrictionRepository(context, _mapper);
        
        await repository.AddRestrictionAsync(_restriction);
        
        var addedRestriction = await context.Restrictions.FirstOrDefaultAsync();
        
        Assert.Multiple(() =>
        {
            Assert.That(addedRestriction, Is.Not.Null);
            Assert.That(addedRestriction, Is.EqualTo(_restriction));
        });
    }

    [Test]
    public async Task AnyRestrictionWithSameNameAsync_NameExists_ShouldReturnTrue()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        var result = await repository.AnyRestrictionWithSameNameAsync(_restriction.Name);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task AnyRestrictionWithSameNameAsync_NameDoesNotExist_ShouldReturnFalse()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        var result = await repository.AnyRestrictionWithSameNameAsync("Bar");
        
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteRestrictionAsync_ValidRestrictionId_ShouldDeleteRestriction()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        await repository.DeleteRestrictionAsync(_restriction.Id);
        
        Assert.That(context.Restrictions.FirstOrDefault(r => r.Id == _restriction.Id), Is.Null);
    }

    [Test]
    public async Task DeleteRestrictionAsync_InvalidRestrictionId_ShouldNotDeleteRestriction()
    {
        var invalidId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        await repository.DeleteRestrictionAsync(invalidId);
        
        Assert.Multiple(() =>
        {
            Assert.That(context.Restrictions.FirstOrDefault(r => r.Id == _restriction.Id), Is.Not.Null);
            Assert.That(context.Restrictions.FirstOrDefault(r => r.Id == invalidId), Is.Null);
        });
    }

    [Test]
    public async Task GetAllRestrictionsAsync_ShouldReturnAllRestrictions()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        var restrictions = await repository.GetAllRestrictionsAsync();
        
        Assert.That(restrictions.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetRestrictionByIdAsync_ValidRestrictionId_ShouldReturnRestriction()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        var restriction = await repository.GetRestrictionByIdAsync(_restriction.Id);
        
        Assert.That(restriction!.Name, Is.EqualTo(_restriction.Name));
    }

    [Test]
    public async Task GetRestrictionByIdAsync_InvalidRestrictionId_ShouldReturnNull()
    {
        var invalidId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        var restriction = await repository.GetRestrictionByIdAsync(invalidId);
        
        Assert.That(restriction, Is.Null);
    }

    [Test]
    public async Task UpdateRestrictionAsync_ValidRestriction_ShouldUpdateRestriction()
    {
        var restrictionToUpdate = new Restriction
        {
            Id = _restriction.Id,
            Name = "Bar",
        };
        
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        await repository.UpdateRestrictionAsync(restrictionToUpdate);
        
        Assert.That(context.Restrictions.FirstOrDefault()!.Name, Is.EqualTo(restrictionToUpdate.Name));
    }
    
    [Test]
    public async Task UpdateRestrictionAsync_NameIsEmpty_ShouldNotUpdateRestriction()
    {
        var restrictionToUpdate = new Restriction
        {
            Id = _restriction.Id,
            Name = string.Empty,
        };
        
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        await repository.UpdateRestrictionAsync(restrictionToUpdate);
        
        Assert.That(context.Restrictions.FirstOrDefault(), Is.EqualTo(_restriction));
    }
    
    [Test]
    public async Task UpdateRestrictionAsync_InvalidRestrictionId_ShouldReturnNullAndNotUpdateRestriction()
    {
        var restrictionToUpdate = new Restriction
        {
            Id = 404,
        };
        
        await using var context = new CookBookContext(_options);
        
        await context.Restrictions.AddAsync(_restriction);
        await context.SaveChangesAsync();
        
        var repository = new RestrictionRepository(context, _mapper);
        
        await repository.UpdateRestrictionAsync(restrictionToUpdate);
        
        Assert.That(context.Restrictions.FirstOrDefault(), Is.EqualTo(_restriction));
    }
}