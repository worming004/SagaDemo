var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

builder.Services.AddHttpLogging(log =>
{
    log.RequestBodyLogLimit = 80000;
    log.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestBody;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHttpLogging();

app.MapControllers();

app.Run();
