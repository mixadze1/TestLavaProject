using UnityEngine;
using UnityEngine.AI;

namespace Assets._Scripts.Game
{
    [RequireComponent(typeof(PlayerAnimation))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Player : MonoBehaviour, IUpdater, IFixedUpdater
    {
        [SerializeField, Range(1, 10f)] private float _speed;
        private PlayerMovement _playerMovement;
        private IPlayerAnimationHandler _playerAnimationHandler;

        public void Initialize(Joystick joystick)
        {
            InitializePlayerAnimation();
            InitializePlayerMovement(joystick);       
        }

        private void InitializePlayerAnimation()
        {
            var playerAnimation  = GetComponent<PlayerAnimation>();
            _playerAnimationHandler = playerAnimation;
            playerAnimation.Initialize();
        }

        private void InitializePlayerMovement(Joystick joystick)
        {
            var playerMovement = _playerMovement = GetComponent<PlayerMovement>();
            playerMovement.Initialize(joystick, _speed, _playerAnimationHandler);
        }

        public void FixedUpdater()
        {
            _playerMovement.FixedUpdater();
        }

        public void Updater()
        {
           
        }
    }
}