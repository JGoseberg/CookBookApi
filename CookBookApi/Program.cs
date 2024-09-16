
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CookBookContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CookBookContext")));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();/*.AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);*/
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IRecipeIngredientRepository,  RecipeIngredientRepository>();
builder.Services.AddScoped<ICuisineRepository, CuisineRepository>();
builder.Services.AddScoped<IRestrictionRepository, RestrictionRepository>();
builder.Services.AddScoped<IMeasurementUnitRepository, MeasurementunitRepository>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<CookBookContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
