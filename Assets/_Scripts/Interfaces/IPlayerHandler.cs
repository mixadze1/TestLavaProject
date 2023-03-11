using UnityEngine;

namespace Assets._Scripts.Interfaces
{
    public interface IPlayerHandler
    {
        Transform GetTransform();
        Transform GetPositionForItem();
        Transform GetArrow();
        bool IsTouchResourceSource();
        bool IsTouchSpot();
    }
}