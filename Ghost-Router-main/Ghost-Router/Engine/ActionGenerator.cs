using System.Collections.Generic;
using Ghost_Router.Models;

namespace Ghost_Router.Engine
{
    public class ActionGenerator
    {
        public List<Node> GetNeighbors(Node currentNode)
        {
            List<Node> neighbors = new List<Node>();
            
            int currentPid = currentNode.ActivePID;
            int currentSuspicion = currentNode.ProcessGauges[currentPid];

            if (currentNode.CurrentStep == 0)
            {
                if (currentSuspicion + 35 <= 100)
                {
                    Node nextNode = new Node(1, currentPid, currentNode.ProcessGauges, currentNode.GCost + 35, 95, "Allocation Mémoire", currentNode);
                    nextNode.ProcessGauges[currentPid] = currentSuspicion + 35;
                    neighbors.Add(nextNode);
                }
            }

            if (currentNode.CurrentStep == 1)
            {
                if (currentSuspicion + 45 <= 100)
                {
                    Node nextNode = new Node(2, currentPid, currentNode.ProcessGauges, currentNode.GCost + 45, 50, "Ecriture Payload", currentNode);
                    nextNode.ProcessGauges[currentPid] = currentSuspicion + 45;
                    neighbors.Add(nextNode);
                }
            }

            if (currentNode.CurrentStep == 2)
            {
                if (currentSuspicion + 50 <= 100)
                {
                    Node nextNode = new Node(3, currentPid, currentNode.ProcessGauges, currentNode.GCost + 50, 0, "Execution !", currentNode);
                    nextNode.ProcessGauges[currentPid] = currentSuspicion + 50;
                    neighbors.Add(nextNode);
                }
            }

            if (currentNode.CurrentStep < 3)
            {
                if (currentSuspicion + 10 <= 100)
                {
                    Random rand = new Random();
                    
                    List<int> pidsDisponibles = new List<int>(currentNode.ProcessGauges.Keys);
                    
                    pidsDisponibles.Remove(currentPid);
                    
                    int indexAleatoire = rand.Next(pidsDisponibles.Count);
                    
                    int newPid = pidsDisponibles[indexAleatoire];

                    int remainingDanger = CalculateHCost(currentNode.CurrentStep);
                    
                    Node jumpNode = new Node(currentNode.CurrentStep, newPid, currentNode.ProcessGauges, currentNode.GCost + 15, remainingDanger, "Saut vers PID " + newPid, currentNode);
                    
                    jumpNode.ProcessGauges[currentPid] = currentSuspicion + 10;

                    neighbors.Add(jumpNode);
                }
            }

            return neighbors;
        }

        private int CalculateHCost(int step)
        {
            switch(step)
            {
                case 0: return 130;
                
                case 1: return 95;

                case 2: return 50;

                default : return 0;
            }
            
        }
    }
}