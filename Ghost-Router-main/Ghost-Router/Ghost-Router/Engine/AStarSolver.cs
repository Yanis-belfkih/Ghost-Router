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
            List<Node> closeSet = new List<Node>();

            ActionGenerator generator = new ActionGenerator();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                openSet = openSet.OrderBy(n => n.Fcost()).ToList();
                Node currentNode = openSet[0];
                
                openSet.Remove(currentNode);
                closeSet.Add(currentNode);

                if(currentNode.CurrentStep == 3)
                {
                    return currentNode;
                }

                List<Node> neighbors = generator.GetNeighbors(currentNode);

                foreach (Node neighbor in neighbors)
                {
                    openSet.Add(neighbor);
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
                string phrase = $"Etape {current.CurrentStep} | PID : {current.ActivePID} | Suscpicion : {current.LocalSuspicion}/100 | Bruit Global : {current.GCost} | Action : {current.ActionTaken}";
                
                timeline.Add(phrase);

                current = current.Parent;
            }
        
            timeline.Reverse();
            return timeline;
        }
    }
}