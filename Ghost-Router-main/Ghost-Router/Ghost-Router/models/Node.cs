namespace Ghost_Router.Models
{
    public class Node
    {
        public int CurrentStep {get ; set;}
        public int ActivePID {get; set;}
        public int LocalSuspicion {get; set;}

        public int GCost {get; set;}
        public int HCost {get; set;}

        public string ActionTaken {get; set;}
        public Node Parent {get; set;}

        public int Fcost(){
            return GCost + HCost;
        }
        public Node(int step, int pid, int suspicion, int gCost, int hCost, string action, Node parentNode)
        {
            this.CurrentStep = step;
            this.ActivePID = pid;
            this.LocalSuspicion = suspicion;
            
            this.GCost = gCost;
            this.HCost = hCost;
            
            this.ActionTaken = action;
            this.Parent = parentNode;
        }
    }
}