namespace Assets._Scripts.Game
{
    public class DataResource 
    {
        public int Metal;
        public int Crystal;
        public int Trees;

        public void SetResource(ResourceType type, int amount)
        {
            switch(type)
            {
                case ResourceType.Metal:
                    Metal = amount;
                    break;
                case ResourceType.Crystal:
                    Crystal = amount;
                    break;
                case ResourceType.Trees:
                    Trees = amount;
                    break;
            }
        }
    }
}