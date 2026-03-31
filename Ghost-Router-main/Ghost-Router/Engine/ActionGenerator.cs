using System.Collections.Generic;
using Ghost_Router.Models;

namespace Ghost_Router.Engine
{
    public class ActionGenerator
    {
        public List<Node> GetNeighbors(Node currentNode)
        {
            List<Node> neighbors = new List<Node>();
            
            // On note quel est le PID sur lequel on est en train de travailler
            int currentPid = currentNode.ActivePID;
            
            // On lit le score de CE PID précis dans le carnet de notes
            int currentSuspicion = currentNode.ProcessGauges[currentPid];

            // 1. Tenter d'avancer dans l'attaque (Action Directe)
            if (currentNode.CurrentStep == 0)
            {
                if (currentSuspicion + 35 <= 100)
                {
                    // a) On crée le voisin. Il reçoit le carnet actuel et va le photocopier.
                    Node nextNode = new Node(1, currentPid, currentNode.ProcessGauges, currentNode.GCost + 35, 95, "Allocation Mémoire", currentNode);
                    
                    // b) On met à jour le score DANS LE NOUVEAU CARNET photocopié !
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

            // 2. Tenter l'évasion (Process Hopping)
            if (currentNode.CurrentStep < 3)
            {
                if (currentSuspicion + 10 <= 100)
                {
                    int newPid = currentPid + 1; // On invente le prochain PID (ex: 405)
                    int remainingDanger = CalculateHCost(currentNode.CurrentStep);
                    
                    Node jumpNode = new Node(currentNode.CurrentStep, newPid, currentNode.ProcessGauges, currentNode.GCost + 15, remainingDanger, "Saut vers PID " + newPid, currentNode);
                    
                    // MISE A JOUR DU CARNET LORS D'UN SAUT :
                    // 1. On applique la pénalité sur l'ancien processus (+10)
                    jumpNode.ProcessGauges[currentPid] = currentSuspicion + 10;
                    
                    // 2. On rajoute une nouvelle ligne dans le carnet pour le nouveau PID avec un score de 0
                    jumpNode.ProcessGauges.Add(newPid, 0);

                    neighbors.Add(jumpNode);
                }
            }

            return neighbors;
        }

        private int CalculateHCost(int step)
        {
            if (step == 0) return 130; 
            if (step == 1) return 95;      
            if (step == 2) return 50;           
            return 0;
        }
    }
}