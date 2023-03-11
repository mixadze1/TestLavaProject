using Assets._Scripts.Game;
using UnityEngine;

namespace Assets._Scripts.Interfaces
{
    public interface IResourceFactoryHandler
    {
        Material GetMaterialType(ResourceType type);

    }
}