namespace Ghost_Router.Models
{
    public class Node
    {
        public int CurrentStep {get ; set;}
        public int ActivePID {get; set;}

        public Dictionary<int, int> ProcessGauges { get; set; }

        public int GCost {get; set;}
        public int HCost {get; set;}

        public string ActionTaken {get; set;}
        public Node Parent {get; set;}

        
        public Node(int step, int pid, Dictionary<int, int> oldGauges, int gCost, int hCost, string action, Node parentNode)
        {
            this.CurrentStep = step;
            this.ActivePID = pid;
            
            this.ProcessGauges = new Dictionary<int, int>(oldGauges);

            this.GCost = gCost;
            this.HCost = hCost;
            
            this.ActionTaken = action;
            this.Parent = parentNode;
        }

        public int Fcost(){
            return GCost + HCost;
        }

        public string GenerateHash()
        {
            string codeBarres = $"Step{CurrentStep}_Active{ActivePID}_[";
            
            List<int> pidsTries = ProcessGauges.Keys.OrderBy(pid => pid).ToList();

            string jaugesTexte = "";
            foreach (int pid in pidsTries)
            {
                int suspicion = ProcessGauges[pid];
                jaugesTexte += $"{pid}:{suspicion}|"; 
            }

            if (jaugesTexte.EndsWith("|")) 
            {
                jaugesTexte = jaugesTexte.Substring(0, jaugesTexte.Length - 1);
            }

            codeBarres += jaugesTexte + "]";

            return codeBarres;
        }
    }
}