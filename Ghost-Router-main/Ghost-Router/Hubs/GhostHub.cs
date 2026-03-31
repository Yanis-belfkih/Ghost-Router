using Microsoft.AspNetCore.SignalR;
using Ghost_Router.Engine;
using Ghost_Router.Models;
using System.Threading;
using System.Runtime.CompilerServices;


public class GhostHub : Hub // elle doit heriter de Hub pour pouvoir communiquer avec le front
{
    public async Task LancerSimulation(string ConfigEDR) /// on peut ajouter les nbrds de process par ex ...
    {
        Node depart = new Node(0, 1, systemeBase, 0, 130, "Demarrage sur PID 1", null);
        AStarSolver solveur = new AStarSolver();
        Node DernierNode = null;

        
    }
}





























