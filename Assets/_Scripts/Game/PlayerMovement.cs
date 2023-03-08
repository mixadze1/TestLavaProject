using UnityEngine;
using UnityEngine.AI;

namespace Assets._Scripts.Game
{
    public class PlayerMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Joystick _joystick;
        private IPlayerAnimationHandler _animationHandler;
        private float _speed;

        public void Initialize(Joystick joystick, float speed, IPlayerAnimationHandler animationHandler)
        {
            _animationHandler = animationHandler;
            _joystick = joystick;
            _agent = GetComponent<NavMeshAgent>();
            CalculateSpeed(speed);
        }

        public void FixedUpdater()
        {
           var movementVector = GetJoystickInfo();
            Move(movementVector);
            RotationPlayer(movementVector);
        }

        private void CalculateSpeed(float speed)
        {
            float reduceSpeed = 0.05f;
            _speed = reduceSpeed * speed;
        }

        private Vector3 GetJoystickInfo()
        {
            if (_joystick.IsActiveJoystick)
            {
                
                return _joystick.GetHorizontalInput();
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