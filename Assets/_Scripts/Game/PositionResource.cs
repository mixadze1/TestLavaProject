using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class PositionResource : MonoBehaviour
    {
        [SerializeField] private List<Position> _positionSpawn;

        public void Initialize()
        {
            var positions = GetComponentsInChildren<Position>();
            foreach (var position in positions)
                _positionSpawn.Add(position);
        }

    }
}