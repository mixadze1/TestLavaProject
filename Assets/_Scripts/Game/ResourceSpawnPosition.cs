using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class ResourceSpawnPosition : MonoBehaviour
    {
        [SerializeField] private List<Position> _positions = new List<Position>();

        public List<Position> Positions => _positions;

        public void Initialize()
        {
            var positions = GetComponentsInChildren<Position>();
            foreach (var position in positions)
                _positions.Add(position);
        }
    }
}