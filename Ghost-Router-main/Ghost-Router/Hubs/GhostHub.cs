using Microsoft.AspNetCore.SignalR;
using Ghost_Router.Engine;
using Ghost_Router.Models;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Ghost_Router.Hubs;

public class GhostHub : Hub // elle doit heriter de Hub pour pouvoir communiquer avec le front
{
    public async Task LancerSimulation(string ConfigEDR) /// on peut ajouter les nbrds de process par ex ...
    {
        Dictionary<int, int> systemeBase = OSgenerator.InitializeRandomPids(100);
        Node depart = new Node(0, 1, systemeBase, 0, 130, "Demarrage sur PID 1", null);
        AStarSolver solveur = new AStarSolver(ConfigEDR);
        Node? victoire = null;

        
        await foreach(Node NodeExplorer in solveur.FindBestPath(depart, Context.ConnectionAborted))
        {
            victoire = NodeExplorer;
            await Clients.Caller.SendAsync("NouveauNode", NodeExplorer);
            await Task.Delay(200);
        }
        
        if(victoire != null && victoire.CurrentStep == 3){
            await Clients.Caller.SendAsync("Victoire", solveur.GetTimeline(victoire));
        }
        else{
            await Clients.Caller.SendAsync("Echec", "L'EDR m'a chicoté, je suis mort");
        }
    }
}





























