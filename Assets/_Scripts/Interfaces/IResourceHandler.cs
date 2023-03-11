using Assets._Scripts.Game;

namespace Assets._Scripts.Interfaces
{
    public interface IResourceHandler
    {
        void AddResource(Resource item);

        void RemoveResource(ResourceType type, int amountWant);

        int AmountResource(ResourceType type);


    }
}