using Microsoft.EntityFrameworkCore;
using RPG_BD.Data;
using RPG_BD.Middlewares;
using RPG_BD.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=items.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(conn)
);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();

    var swaggerUrl = "https://localhost:5000/swagger";
    Console.WriteLine($"\nSwagger disponível em: {swaggerUrl}");
    try
    {
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = swaggerUrl,
            UseShellExecute = true
        };
        System.Diagnostics.Process.Start(psi);
    }
    catch { }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
