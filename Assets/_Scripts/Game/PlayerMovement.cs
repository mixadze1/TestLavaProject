using Assets._Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Assets._Scripts.Game
{
    public class PlayerMovement : MonoBehaviour, IFixedUpdater, IMovementHandler
    {
        [SerializeField, Range(1, 10f)] private float _speed;
        private NavMeshAgent _agent;
        private IJoystickHandler _joystickHandler;
        private IPlayerAnimationHandler _animationHandler;


        public void Initialize(IJoystickHandler joystickHandler, IPlayerAnimationHandler animationHandler)
        {
            _animationHandler = animationHandler;
            _joystickHandler = joystickHandler;
            _agent = GetComponent<NavMeshAgent>();
            CalculateSpeed(_speed);
        }

        public void FixedUpdater()
        {
            var movementVector = GetJoystickInfo();
            Move(movementVector);
            RotationPlayer(movementVector);
        }

        public bool IsMovement()
        {
            if (_joystickHandler.IsJoystickEnable())
                return false;

            return true;
        }

        private void CalculateSpeed(float speed)
        {
            float reduceSpeed = 0.05f;
            _speed = reduceSpeed * speed;
        }

        private Vector3 GetJoystickInfo()
        {
            if (_joystickHandler.IsJoystickEnable())
            {             
                return _joystickHandler.GetHorizontalInput();
            }
            return Vector3.zero;
        }

        private void Move(Vector3 movementVector)
        {                        
            if(movementVector.magnitude < 0.01f)
            {
                _animationHandler.SetWalking(false);
                return;
            }
               
            _animationHandler.SetWalking(true);
            movementVector *= _speed;
            _agent.Move(movementVector);
        }

        private void RotationPlayer(Vector3 movement)
        {
            if (movement == Vector3.zero)
                return;

            var rotation = Quaternion.LookRotation(movement);
            float offsetRotation = 8f;
            var needRotation = Quaternion.SlerpUnclamped(this.transform.rotation, rotation, offsetRotation * Time.fixedDeltaTime);
            transform.rotation = needRotation;
        }
    }
}