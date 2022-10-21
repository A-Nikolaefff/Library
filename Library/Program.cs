using System.Text.Json.Serialization;
using Library;
using Library.Services;
using Library.Storage;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<LibraryContext>();
builder.Services.AddServices();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, configuration) => configuration
    .WriteTo.Console()
    );

var app = builder.Build();


app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API V1");

});

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();


app.Run();