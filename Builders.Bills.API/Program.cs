using Builders.Bills.Database;
using Builders.Bills.Services.BillCalculator;
using Builders.Bills.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: ExcludeFromCodeCoverage]
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(
    opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Builders Boll API", Version = "v1" });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        opt.IncludeXmlComments(xmlPath);
    });
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IBillCalculatorService, BillCalculatorService>();
builder.Services.AddDbContext<BillsDbContext>(c => c.UseSqlite(builder.Configuration.GetConnectionString("billsDb")));

var app = builder.Build();

//create dbstructure
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<BillsDbContext>();
    dbContext.Database.Migrate();
}

//disable swagger on real prod

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();