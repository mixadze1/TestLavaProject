using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class PositionSpawnSpot : MonoBehaviour
    {
        [SerializeField] private List<PositionSpot> _positions = new List<PositionSpot>();

        public List<PositionSpot> Positions => _positions;

        public void Initialize()
        {
            var positions = GetComponentsInChildren<PositionSpot>();
            foreach (var position in positions)
                _positions.Add(position);
        }
    }
}