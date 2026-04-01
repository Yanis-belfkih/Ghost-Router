using System.Collections.Generic;
using System.Linq; 
using Ghost_Router.Models;
using System.Threading; // pour le CancelToken
using System.Runtime.CompilerServices; // pour lier le cancelToken au IAsyncEnumerable


namespace Ghost_Router.Engine
{
    public class AStarSolver
    {
        private string _configEDR;
        private HashSet<string> closedSet = new HashSet<string>(); //OPTIMISATION : HashSet est beaucoup plus rapide que List pour fouiller les codes-barres
        private ActionGenerator generator;
        private List<Node> openSet = new List<Node>();
        private List<string> timeline = new List<string>();

        public AStarSolver(string configEDR)
        {
            _configEDR = configEDR;
            generator = new ActionGenerator(_configEDR);
        }
        public async IAsyncEnumerable<Node> FindBestPath(Node startNode, [EnumeratorCancellation] CancellationToken cancelToken) // IAsyncEnumerable sert de tapis roulant qui envoie les nodes les uns apres les autres 
        {
            openSet.Add(startNode);

            while (openSet.Count > 0) 
            {
                cancelToken.ThrowIfCancellationRequested(); // si le token est annulé, on arrete la simulation , on utillise ThrowIfCancellationRequested() car on utilise CancellationToken

                openSet = openSet.OrderBy(n => n.FCost()).ToList();
                Node currentNode = openSet[0];
                openSet.Remove(currentNode);


                // Gestion du Hash
                string currentHash = currentNode.GenerateHash();
                if (closedSet.Contains(currentHash)) continue; 
                closedSet.Add(currentHash);

                yield return currentNode; // on envoie le node au client au fur est a mesure 

                if (currentNode.CurrentStep == 3) yield break;// yield break

                // On délègue le travail des voisins à une petite méthode
                ProcessNeighbors(currentNode, generator, openSet, closedSet);
            }
            yield break;// au lieu de return null on yield break car on est dans un IEnumerable
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
            timeline.Clear();
            Node? current = endNode;

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