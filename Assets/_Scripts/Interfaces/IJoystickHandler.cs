using UnityEngine;

namespace Assets._Scripts.Interfaces
{
    public interface IJoystickHandler
    {
        bool IsJoystickEnable();
        Vector3 GetHorizontalInput();
    }
}