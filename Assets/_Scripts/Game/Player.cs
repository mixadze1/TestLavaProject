using Assets._Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets._Scripts.Game
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ResourceDigger))]
    [RequireComponent(typeof(PlayerResourceHandler))]
    [RequireComponent(typeof(PlayerAnimation))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Player : MonoBehaviour, IUpdater, IFixedUpdater, IPlayerHandler
    {
        [SerializeField] private Transform _arrow;
        [SerializeField] private Transform _positionForItem;

        private DataPlayer _data;
        private DataResource _resource;
        private PlayerResourceHandler _resourceHandler;
        private PlayerSpotHandler _spotHandler;
        private ResourceView _resourceView;
        private IMovementHandler _movementHandler;

        private IPlayerAnimationHandler _playerAnimationHandler;

        private List<IFixedUpdater> _fixedUpdaters = new List<IFixedUpdater>();
        private List<IUpdater> _updaters = new List<IUpdater>();
        private List<IHaveSaves> _haveSaves = new List<IHaveSaves>();
        private Transform _transform;

        public void Initialize(IJoystickHandler joystickHandler, DataPlayer safeData, DataResource dataResource,  ResourceView resourceView)
        {
            _resourceView = resourceView;
            _resource = dataResource;
            _data = safeData;
            _transform = this.transform;

            InitializePlayerAnimation();
            InitializePlayerMovement(joystickHandler);
            var playerItemCollectionHandler = _resourceHandler = InitializePlayerResourceHandler();
            InitilaizeResourceDigger();
            InitializePlayerSpotHandler(joystickHandler, resourceHandler: playerItemCollectionHandler);
            InitializeSafesDependencyPlayer();
        }

        private void InitializeSafesDependencyPlayer()
        {
            Debug.Log(_data);
            this.transform.localPosition = _data.Position;

            foreach (var saves in _haveSaves)
            {
                saves.GetSaves(_data, _resource);
            }
        }

        public void FixedUpdater()
        {
            _data.SetPositionAndRotation(this.transform.localPosition);

            foreach (var fixedUpdater in _fixedUpdaters)
                fixedUpdater.FixedUpdater();
        }

        public void Updater()
        {
            foreach (var updater in _updaters)
                updater.Updater();
        }

        public Transform GetPositionForItem() => _positionForItem;

        public Transform GetTransform() => _transform;

        public Transform GetArrow() => _arrow;

        public bool IsTouchResourceSource() => _resourceHandler.IsTouch();

        public bool IsTouchSpot() => _spotHandler.IsTouch();

        private void InitilaizeResourceDigger()
        {
            var digger = GetComponent<ResourceDigger>();
            digger.Initialize(_movementHandler, _playerAnimationHandler);
            _fixedUpdaters.Add(digger);
        }

        private void InitializePlayerSpotHandler(IJoystickHandler joystickHandler, IResourceHandler resourceHandler)
        {
            var spotHandler = _spotHandler = GetComponent<PlayerSpotHandler>();
            spotHandler.Initialize(joystickHandler, resourceHandler);
            _fixedUpdaters.Add(spotHandler);
        }

        private PlayerResourceHandler InitializePlayerResourceHandler()
        {
            var itemCollection = GetComponent<PlayerResourceHandler>();
            _fixedUpdaters.Add(itemCollection);
            _haveSaves.Add(itemCollection); 
            itemCollection.Initialize(playerHandler:this, _resourceView);
            return itemCollection;
        }

        private void InitializePlayerAnimation()
        {
            var playerAnimation  = GetComponent<PlayerAnimation>();
            _playerAnimationHandler = playerAnimation;
            playerAnimation.Initialize();
        }

        private void InitializePlayerMovement(IJoystickHandler joystickHandler)
        {
            var playerMovement = GetComponent<PlayerMovement>();
            _movementHandler = playerMovement;
            _fixedUpdaters.Add(playerMovement);
            playerMovement.Initialize(joystickHandler, _playerAnimationHandler);
        }
    }
}