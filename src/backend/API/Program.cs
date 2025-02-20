// API/Program.cs
using Infrastructure.Services;
using Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure RabbitMQ
builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<RabbitMQService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.UseCors("AllowReactApp");

app.Run();