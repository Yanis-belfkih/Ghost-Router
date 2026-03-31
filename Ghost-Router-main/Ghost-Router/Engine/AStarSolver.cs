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
            List<string> closedSet = new List<string>(); // Archives des Hashs
            
            ActionGenerator generator = new ActionGenerator();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                openSet = openSet.OrderBy(n => n.FCost()).ToList();
                Node currentNode = openSet[0];

                openSet.Remove(currentNode);

                // Vérification du Hash
                string currentHash = currentNode.GenerateHash();
                if (closedSet.Contains(currentHash))
                {
                    continue; 
                }
                closedSet.Add(currentHash);

                // Condition de victoire
                if (currentNode.CurrentStep == 3)
                {
                    return currentNode;
                }

                List<Node> neighbors = generator.GetNeighbors(currentNode);

                foreach (Node neighbor in neighbors)
                {
                    if (!closedSet.Contains(neighbor.GenerateHash()))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
            return null; 
        }

        public List<string> GetTimeline(Node endNode)
        {
            List<string> timeline = new List<string>();
            Node current = endNode;

            while (current != null)
            {
                string phrase = $"Etape {current.CurrentStep} | PID Actif: {current.ActivePID} | Suspicion locale: {current.ProcessGauges[current.ActivePID]}/100 | Bruit Global: {current.GCost} | Action: {current.ActionTaken}";
                timeline.Add(phrase);
                current = current.Parent;
            }

            timeline.Reverse(); 
            return timeline;
        }
    }
}