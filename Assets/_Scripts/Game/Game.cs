using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace Assets._Scripts.Game
{
    public class Game : MonoBehaviour, IGameHandler
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Player _player;

        private List<IUpdater> _updates = new List<IUpdater>();
        private List<IFixedUpdater> _fixedUpdater = new List<IFixedUpdater>();

        private void Start()
        {
            StartGame();
        }

        private void Update()
        { 
            foreach(var updater in _updates)
            {
                updater.Updater();
            }
        }

        private void FixedUpdate()
        {
            foreach(var fixedUpdater in _fixedUpdater)
            {
                fixedUpdater.FixedUpdater();
            }
        }

        private void StartGame()
        {
            InitializePlayer();
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            _camera.LookAt = _player.transform;
            _camera.Follow = _player.transform;
        }

        private void InitializePlayer()
        {
            _player.Initialize(_joystick);

            _fixedUpdater.Add(_player);
            _updates.Add(_player);
        }

        public void RestartGame()
        {

        }
    }
}