using Assets._Scripts.Interfaces;
using System;
using UnityEngine;

namespace Assets._Scripts.Game
{
    [CreateAssetMenu]
    public class ResourceFactory : GameObjectFactory, IResourceFactoryHandler
    {
        [Serializable]
        public class ResourceConfig
        {
            [Header("ResourceSourceSetting")]
            public ResourceSource PrefabResourceSource;
            [Range(1f, 3f)] 
            public float RangeCreateResource;
            [Range(0.1f, 1.5f)]
            public float ScalerDamage;
            [Range(0.1f, 3f)]
            public float TimeDamage;
            [Range(1, 15)]
            public int AmountCreateResource;
            [Range(0.1f, 5f)]
            public float DelayRecovery;

            [Header("ResourceSetting")]
            [Range(1f, 10f)]
            public float ResourceJumpPower;
            [Range(1f, 5f)]
            public float ResourceTimeMove;

            public float Health;
            public float RecoveryHealthPerSecond;
          
            public ResourceType Type;
            public Material Material;
            public ParticleSystem Particle;
        }

        [SerializeField]
        private ResourceConfig _crystal, _trees, _metal;

        public ResourceSource GetResource(ResourceType type, Position position, Transform camera, ResourceContainer resourceContainer)
        {
            var config = GetConfig(type);
            var resource = CreateGameObjectInstance(config.PrefabResourceSource);
            resource.Initialize(config, position, camera, resourceContainer);
            return resource;
        }

        private ResourceConfig GetConfig(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Trees:
                    return _trees;
                case ResourceType.Metal:
                    return _metal;
                case ResourceType.Crystal:
                    return _crystal;
            }
            return _trees;
        }

        public Material GetMaterialType(ResourceType type)
        {
            var config = GetConfig(type);
            return config.Material;
        }
    }

    public enum ResourceType
    {
        Trees = 0,
        Crystal = 1,
        Metal = 2
    }
}