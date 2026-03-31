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
            
            // MODIFICATION : Le closedSet devient une liste de TEXTE (les Hash)
            List<string> closedSet = new List<string>(); 
            
            ActionGenerator generator = new ActionGenerator();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                openSet = openSet.OrderBy(n => n.Fcost()).ToList();
                Node currentNode = openSet[0];

                openSet.Remove(currentNode);

                // --- UTILISATION DU HASH ICI ---
                // 1. On génère le code-barres de la situation actuelle
                string currentHash = currentNode.GenerateHash();

                // 2. Si on a déjà archivé ce code-barres, on ignore ce nœud et on passe au suivant
                if (closedSet.Contains(currentHash))
                {
                    continue; // "Continue" dit à la boucle : "Saute cette étape et recommence au début"
                }

                // 3. Sinon, on archive ce nouveau code-barres
                closedSet.Add(currentHash);
                // ------------------------------

                if (currentNode.CurrentStep == 3)
                {
                    return currentNode;
                }

                List<Node> neighbors = generator.GetNeighbors(currentNode);

                foreach (Node neighbor in neighbors)
                {
                    // On ne rajoute le voisin que s'il n'a pas déjà été exploré
                    if (!closedSet.Contains(neighbor.GenerateHash()))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
            return null!; 
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