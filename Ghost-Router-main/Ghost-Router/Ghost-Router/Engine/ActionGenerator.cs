using System.Collections.Generic;
using Ghost_Router.Models;

namespace Ghost_Router.Engine
{
    public class ActionGenerator
    {
        public List<Node> GetNeighbors(Node currentNode)
        {
            const int MAX_SUSPICION = 100;
            const int COST_ALLOCATION = 35;
            const int COST_WRITING = 45;
            const int COST_EXECUTION = 50;
            const int COST_JUMP = 10;
            const int COST_GLOBAL_SUSPICION = 15;
            const int HCOST_STEP_0 = 95; 
            const int HCOST_STEP_1 = 50; 
            const int HCOST_STEP_2 = 0; 
            List<Node> neighbors = new List<Node>();

            if (currentNode.CurrentStep == 0)
            {
                int newSuspicion = currentNode.LocalSuspicion + COST_ALLOCATION;
                if (newSuspicion <= MAX_SUSPICION)
                {
                    Node nextNode = new Node(1, currentNode.ActivePID, newSuspicion, currentNode.GCost + COST_ALLOCATION, HCOST_STEP_0, "Allocation Mémoire", currentNode);
                    neighbors.Add(nextNode);
                }
            }

            if (currentNode.CurrentStep == 1)
            {
                int newSuspicion = currentNode.LocalSuspicion + COST_WRITING;
                if (newSuspicion <= MAX_SUSPICION)
                {
                    Node nextNode = new Node(2, currentNode.ActivePID, newSuspicion, currentNode.GCost + COST_WRITING, HCOST_STEP_1, "Ecriture Payload", currentNode);
                    neighbors.Add(nextNode);
                }
            }

            if (currentNode.CurrentStep == 2)
            {
                int newSuspicion = currentNode.LocalSuspicion + COST_EXECUTION;
                if (newSuspicion <= MAX_SUSPICION)
                {
                    Node nextNode = new Node(3, currentNode.ActivePID, newSuspicion, currentNode.GCost + COST_EXECUTION, HCOST_STEP_2, "Execution !", currentNode);
                    neighbors.Add(nextNode);
                }
            }

            if (currentNode.CurrentStep < 3)
            {
                int oldPidSuspicion = currentNode.LocalSuspicion + COST_JUMP;
                
                if (oldPidSuspicion <= MAX_SUSPICION)
                {
                    int newPid = currentNode.ActivePID + 1; 
                    int remainingDanger = CalculateHCost(currentNode.CurrentStep);
                    
                    Node jumpNode = new Node(currentNode.CurrentStep, newPid, 0, currentNode.GCost + COST_GLOBAL_SUSPICION, remainingDanger, "Saut vers PID " + newPid, currentNode);
                    neighbors.Add(jumpNode);
                }
            }

            return neighbors;
        }

        private int CalculateHCost(int step)
        {
            if (step == 0) 
            {
                return 35 + 45 + 50; 
            }
            else if (step == 1) 
            {
                return 45 + 50;      
            }
            else if (step == 2) 
            {
                return 50;           
            }
            return 0;
        }
    }
}