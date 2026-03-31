using System.Collections.Generic;
using System.Linq; 
using Ghost_Router.Models;

namespace Ghost_Router.Engine
{
    public class AStarSolver
    {
        public Node FindBestPath(Node startNode)
        {
            List<Node> openSet = new List<Node>();
            
            // OPTIMISATION : HashSet est beaucoup plus rapide que List pour fouiller les codes-barres
            HashSet<string> closedSet = new HashSet<string>(); 
            
            ActionGenerator generator = new ActionGenerator();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                openSet = openSet.OrderBy(n => n.FCost()).ToList();
                Node currentNode = openSet[0];
                openSet.Remove(currentNode);

                // Gestion du Hash
                string currentHash = currentNode.GenerateHash();
                if (closedSet.Contains(currentHash)) continue; 
                closedSet.Add(currentHash);

                // Condition de victoire
                if (currentNode.CurrentStep == 3) return currentNode;

                // On délègue le travail des voisins à une petite méthode
                ProcessNeighbors(currentNode, generator, openSet, closedSet);
            }
            return null; 
        }

        private void ProcessNeighbors(Node currentNode, ActionGenerator generator, List<Node> openSet, HashSet<string> closedSet)
        {
            List<Node> neighbors = generator.GetNeighbors(currentNode);
            foreach (Node neighbor in neighbors)
            {
                if (!closedSet.Contains(neighbor.GenerateHash()))
                {
                    openSet.Add(neighbor);
                }
            }
        }

        public List<string> GetTimeline(Node endNode)
        {
            List<string> timeline = new List<string>();
            Node current = endNode;

            while (current != null)
            {
                timeline.Add($"Etape {current.CurrentStep} | PID Actif: {current.ActivePID} | Suspicion: {current.ProcessGauges[current.ActivePID]}/100 | Bruit Global: {current.GCost} | Action: {current.ActionTaken}");
                current = current.Parent;
            }

            timeline.Reverse(); 
            return timeline;
        }
    }
}