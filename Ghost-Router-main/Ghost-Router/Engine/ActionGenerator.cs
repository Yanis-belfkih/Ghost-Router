using System;
using System.Collections.Generic;
using Ghost_Router.Models;

namespace Ghost_Router.Engine
{
    public class ActionGenerator
    {
        
        public const int MAX_SUSPICION = 100;
        
        public const int COST_ALLOC = 35;
        public const int COST_WRITE = 45;
        public const int COST_EXEC = 50;
        
        public const int HOP_LOCAL_PENALTY = 10;
        public const int HOP_GLOBAL_COST = 15;

        
        public List<Node> GetNeighbors(Node currentNode)
        {
            List<Node> neighbors = new List<Node>();
            
            TryAddDirectAction(currentNode, neighbors);
            TryAddEvasionAction(currentNode, neighbors);

            return neighbors;
        }

        
        private void TryAddDirectAction(Node currentNode, List<Node> neighbors)
        {
            int pid = currentNode.ActivePID;
            int suspicion = currentNode.ProcessGauges[pid];

            switch (currentNode.CurrentStep)
            {
                case 0:
                    if (suspicion + COST_ALLOC <= MAX_SUSPICION)
                        neighbors.Add(CreateNode(currentNode, 1, pid, COST_ALLOC, CalculateHCost(1), "Allocation Mémoire"));
                    break;
                case 1:
                    if (suspicion + COST_WRITE <= MAX_SUSPICION)
                        neighbors.Add(CreateNode(currentNode, 2, pid, COST_WRITE, CalculateHCost(2), "Ecriture Payload"));
                    break;
                case 2:
                    if (suspicion + COST_EXEC <= MAX_SUSPICION)
                        neighbors.Add(CreateNode(currentNode, 3, pid, COST_EXEC, CalculateHCost(3), "Execution !"));
                    break;
            }
        }

        
        private void TryAddEvasionAction(Node currentNode, List<Node> neighbors) // Process Hopping
        {
            int pid = currentNode.ActivePID;
            int suspicion = currentNode.ProcessGauges[pid];

            if (currentNode.CurrentStep < 3 && (suspicion + HOP_LOCAL_PENALTY <= MAX_SUSPICION))
            {
                int newPid = GetRandomAvailablePid(currentNode);
                int dangerRestant = CalculateHCost(currentNode.CurrentStep);
                
                Node jumpNode = CreateNode(currentNode, currentNode.CurrentStep, newPid, HOP_GLOBAL_COST, dangerRestant, $"Saut vers PID {newPid}");
                jumpNode.ProcessGauges[pid] = suspicion + HOP_LOCAL_PENALTY; // Pénalité ancien PID
                
                neighbors.Add(jumpNode);
            }
        }

        
        private int GetRandomAvailablePid(Node currentNode)
        {
            Random rand = new Random();
            List<int> pidsDisponibles = new List<int>(currentNode.ProcessGauges.Keys);
            pidsDisponibles.Remove(currentNode.ActivePID);
            return pidsDisponibles[rand.Next(pidsDisponibles.Count)];
        }

        private Node CreateNode(Node parent, int step, int pid, int costToApply, int hCost, string actionText)
        {
            Node newNode = new Node(step, pid, parent.ProcessGauges, parent.GCost + costToApply, hCost, actionText, parent);
            newNode.ProcessGauges[pid] = parent.ProcessGauges[pid] + costToApply;
            return newNode;
        }

        private int CalculateHCost(int step)
        {
            switch (step)
            {
                case 0: return 130;
                case 1: return 95;
                case 2: return 50;
                default: return 0;
            }
        }
    }
}