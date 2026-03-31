using Ghost_Router.Models;
using Ghost_Router.Engine;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ==========================================
// --- LANCEMENT DE L'APPLICATION ---
// ==========================================

RunSimulation(); // On appelle notre méthode propre

app.Run();

// ==========================================
// --- SOUS-METHODES DE SIMULATION ---
// ==========================================

void RunSimulation()
{
    Console.WriteLine("=================================");
    Console.WriteLine("Lancement de l'IA Ghost-Router...");
    Console.WriteLine("=================================");

    // On initialise le système de manière propre
    Dictionary<int, int> systemeBase = InitializeRandomPids(100);
    
    // On commence sur le PID 1
    Node depart = new Node(0, 1, systemeBase, 0, 130, "Demarrage sur PID 1", null);
    
    AStarSolver solveur = new AStarSolver();
    Node victoire = solveur.FindBestPath(depart);

    DisplayResults(solveur, victoire);
}

// Méthode pour générer les 100 PID avec des scores aléatoires
Dictionary<int, int> InitializeRandomPids(int count)
{
    Dictionary<int, int> carnet = new Dictionary<int, int>();
    Random generateurAleatoire = new Random();

    for (int i = 1; i <= count; i++)
    {
        carnet.Add(i, generateurAleatoire.Next(5, 31)); // Entre 5 et 30 inclus
    }
    return carnet;
}

// Méthode pour afficher le rapport de la Timeline
void DisplayResults(AStarSolver solveur, Node victoire)
{
    if (victoire != null)
    {
        Console.WriteLine("SUCCES ! Le chemin a ete trouve !");
        foreach(string ligne in solveur.GetTimeline(victoire))
        {
            Console.WriteLine(ligne);
        }
    }
    else
    {
        Console.WriteLine("ECHEC : Aucun chemin furtif n'est possible.");
    }
    Console.WriteLine("=================================");
}