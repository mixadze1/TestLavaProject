using UnityEngine;

namespace Assets._Scripts.Game
{
    public abstract class GameObjectFactory : ScriptableObject
    {
        protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
        {
            T instance = Instantiate(prefab);
            return instance;
        }
    }
}