using Microsoft.EntityFrameworkCore;
using RPG_BD.Data;
using RPG_BD.Models;

var builder = WebApplication.CreateBuilder(args);

// load configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Add services
builder.Services.AddControllers().AddJsonOptions(opts => {
    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=items.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(conn));

var app = builder.Build();

// If run with --cli, run a simple CLI CRUD and exit.
if (args.Contains("--cli"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    Console.WriteLine("CLI mode - CRUD básico. Digite: list | add | get {id} | update {id} | delete {id} | exit");
    while (true)
    {
        Console.Write("> ");
        var line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line)) continue;
        var parts = line.Split(' ', 2);
        var cmd = parts[0].ToLowerInvariant();
        if (cmd == "exit") break;
        if (cmd == "list")
        {
            var list = db.Items.OrderBy(i => i.Name).ToList();
            foreach (var it in list) Console.WriteLine($"{it.Id}: {it.Name} | {it.Rarity} | {it.Price:C}"); 
            continue;
        }
        if (cmd == "add" && parts.Length == 2)
        {
            var pieces = parts[1].Split(';');
            if (pieces.Length < 3) { Console.WriteLine("Formato: add Name;Rarity;Price"); continue; }
            if (!Enum.TryParse<Rarity>(pieces[1], true, out var rarity)) { Console.WriteLine("Rarity inválida."); continue; }
            if (!decimal.TryParse(pieces[2], out var price)) { Console.WriteLine("Preço inválido."); continue; }
            var it = new Item { Name = pieces[0].Trim(), Rarity = rarity, Price = price };
            db.Items.Add(it);
            db.SaveChanges();
            Console.WriteLine($"Criado Id={it.Id}"); continue;
        }
        if ((cmd == "get" || cmd=="update" || cmd=="delete") && parts.Length==2 && int.TryParse(parts[1], out var id))
        {
            var it = db.Items.Find(id);
            if (it == null) { Console.WriteLine("Não encontrado"); continue; }
            if (cmd == "get") { Console.WriteLine($"{it.Id}: {it.Name} | {it.Rarity} | {it.Price:C}"); continue; }
            if (cmd == "delete") { db.Items.Remove(it); db.SaveChanges(); Console.WriteLine("Removido"); continue; }
            if (cmd == "update")
            {
                Console.Write("Novo nome: "); var nm = Console.ReadLine() ?? it.Name;
                Console.Write("Nova rarity: "); var r = Console.ReadLine() ?? it.Rarity.ToString();
                Console.Write("Novo preço: "); var p = Console.ReadLine() ?? it.Price.ToString();
                if (Enum.TryParse<Rarity>(r, true, out var rr)) it.Rarity = rr;
                if (decimal.TryParse(p, out var pp)) it.Price = pp;
                it.Name = nm;
                db.SaveChanges();
                Console.WriteLine("Atualizado");
                continue;
            }
        }

        Console.WriteLine("Comando desconhecido.");
    }
    return;
}

// normal API startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Create database if not exists. For the assignment you can later use migrations.
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
