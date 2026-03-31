using Ghost_Router.Models;
using Ghost_Router.Engine;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
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

// ==========================================
// --- NOTRE TEST GHOST-ROUTER ---
// ==========================================
Console.WriteLine("=================================");
Console.WriteLine("Lancement de l'IA Ghost-Router...");
Console.WriteLine("=================================");

Dictionary<int, int> carnetDepart = new Dictionary<int, int>();

Random generateurAleatoire = new Random();
 
for (int i = 1; i <= 100; i++)
{
    int suspicionAleatoire = generateurAleatoire.Next(5, 31);
    
    carnetDepart.Add(i, suspicionAleatoire);
}

Node depart = new Node(0, 1, carnetDepart, 0, 130, "Demarrage de l'attaque sur PID 1", null);

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
// ==========================================

app.Run();