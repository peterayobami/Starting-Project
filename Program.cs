using Microsoft.OpenApi.Models;
using Starting_Project;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CP Program APIs",
        Version = "v1",
        Description = "The APIs for CP Programs Test"
    });

    // Include the XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Add example filters
    options.ExampleFilters();
});

// Add swagger examples
builder.Services.AddSwaggerExamplesFromAssemblyOf<ProgramCredentialsModelExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<UpdateProgramCredentialsModelExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApplicationCredentialsModelExample>();

// Add controllers
builder.Services.AddControllers();

// Configure services
builder.Services.AddDatabaseConfiguration(builder.Configuration)
    .AddApplicationLogger()
    .AddDomainServices();

var app = builder.Build();

// Ensure database is created
await app.CreateApplicationDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CP Programs APIs v1");
    });
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();
