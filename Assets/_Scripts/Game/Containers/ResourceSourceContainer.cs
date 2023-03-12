using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class ResourceSourceContainer : MonoBehaviour
    {
        public List<ResourceSource> SpawnResourceSource( List<Position> positions, CinemachineVirtualCamera _camera, ResourceFactory _resourceFactory, ResourceContainer resourceContainer)
        {
            List<ResourceSource> resources = new List<ResourceSource>();
            for (int i = 0; i < positions.Count; i++)
            {
                ResourceType resourceType = positions[i].ResourceType;
                var resource = _resourceFactory.GetResource(resourceType, positions[i], _camera.transform, resourceContainer);
                resource.transform.SetParent(this.transform);
                resources.Add(resource);
            }
            return resources;
        }
    }
}