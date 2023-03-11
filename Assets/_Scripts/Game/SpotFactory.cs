using System;
using UnityEngine;

namespace Assets._Scripts.Game
{
    [CreateAssetMenu]
    public class SpotFactory : GameObjectFactory
    {
        [Serializable]
        public class SpotConfig
        {
            public Spot PrefabSpot;

            [Range(1f, 10f)]
            public float TimeCreateResource;
            [Range(0.1f, 5f)] 
            public float TimeDelayGiveResource;
            [Range(1, 10)] 
            public int ResourceNeed;
            [Range(1, 25)] 
            public int ResourceCreate;
            [Range(0, 1f)]
            public float RandomRangeNeedResource;
            [Range(0, 5f)]
            public float RandomRangeCreateResource;

            public ResourceType TypeResourceNeed;
            public ResourceType TypeResourceCreate;

            public Material MaterialFrom;
            public Material MaterialTo;
            public ParticleSystem Particle;
        }
     
        [SerializeField] private SpotConfig _treesToMetal, _metalToCrystal, _crystalToTrees;

        public Spot GetSpot(SpotType type, Vector3 position, SpotContainer spotContainer)
        {
            var config = GetConfig(type);
            Spot spot = CreateGameObjectInstance(config.PrefabSpot);
            spot.Initialize(config, position, spotContainer);
            return spot;
        }

        private SpotConfig GetConfig(SpotType type)
        {
            switch (type)
            {
                case SpotType.TreesToMetal:
                    return _treesToMetal;
                case SpotType.MetalToCrystal:
                    return _metalToCrystal;
                case SpotType.CrystalToTrees:
                    return _crystalToTrees;
            }
            return _treesToMetal;
        }
    }
}

public enum SpotType
{
    TreesToMetal,
    MetalToCrystal,
    CrystalToTrees
}