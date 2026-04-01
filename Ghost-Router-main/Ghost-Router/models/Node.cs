using System.Collections.Generic;
using System.Linq;

namespace Ghost_Router.Models
{
    public class Node
    {
        public int CurrentStep { get; set; } // private public get private set
        public int ActivePID { get; set; }
        public Dictionary<int, int> ProcessGauges { get; set; } 
        public int GCost { get; set; } 
        public int HCost { get; set; } 
        public string ActionTaken { get; set; }
        public Node? Parent { get; set; }

        public int FCost() => GCost + HCost;

        public Node(int step, int pid, Dictionary<int, int> oldGauges, int gCost, int hCost, string action, Node? parentNode)
        {
            CurrentStep = step;
            ActivePID = pid;
            ProcessGauges = new Dictionary<int, int>(oldGauges);
            GCost = gCost;
            HCost = hCost;
            ActionTaken = action;
            Parent = parentNode;
        }

        public string GenerateHash()
        {
            return $"Step{CurrentStep}_Active{ActivePID}_[{GetSortedGaugesText()}]";
        }

        private string GetSortedGaugesText()
        {
            List<int> pidsTries = ProcessGauges.Keys.OrderBy(pid => pid).ToList();
            string jaugesTexte = "";
            
            foreach (int pid in pidsTries)
            {
                jaugesTexte += $"{pid}:{ProcessGauges[pid]}|";
            }
            
            if (jaugesTexte.EndsWith("|")) 
            {
                jaugesTexte = jaugesTexte.Substring(0, jaugesTexte.Length - 1);
            }
            
            return jaugesTexte;
        }
    }
}