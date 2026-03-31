using Ghost_Router.Models;
using Ghost_Router.Engine;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// --- NOTRE TEST GHOST-ROUTER ---
Console.WriteLine("=================================");
Console.WriteLine("Lancement de l'IA Ghost-Router...");
Console.WriteLine("=================================");

// On fabrique le tout premier carnet
Dictionary<int, int> carnetDepart = new Dictionary<int, int>();
carnetDepart.Add(404, 0); // Le PID 404 commence à 0 points

// On donne le carnet au Nœud (C'est le 3ème ingrédient)
Node depart = new Node(0, 404, carnetDepart, 0, 130, "Demarrage de l'attaque", null);


AStarSolver solveur = new AStarSolver();

Node victoire = solveur.FindBestPath(depart);

if (victoire != null)
{
    Console.WriteLine("SUCCES ! Le chemin a ete trouve !");
    Console.WriteLine("Voici la chronologie des actions :");
    
    List<string> chronologie = solveur.GetTimeline(victoire);
    
    foreach(string ligne in chronologie)
    {
        Console.WriteLine(ligne);
    }
}
else
{
    Console.WriteLine("ECHEC : Aucun chemin furtif n'est possible.");
}
Console.WriteLine("=================================");

app.Run();