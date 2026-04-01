namespace Ghost_Router.Engine
{
    public class OSgenerator
    {
        public static Dictionary<int, int> InitializeRandomPids(int NbrProcess)   
        {
            Dictionary<int, int> carnet = new Dictionary<int, int>();
            Random rand = new Random();

            for (int i = 1; i <= NbrProcess; i++)
            {
                carnet.Add(i, rand.Next(5, 31)); 
            }
            return carnet;
        }
    }
}