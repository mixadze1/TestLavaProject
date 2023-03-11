using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets._Scripts.Interfaces;

namespace Assets._Scripts.Game
{
    public class Game : MonoBehaviour, IGameHandler
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private ResourceContainer _resourceContainer;
        [SerializeField] private ResourceSourceContainer _resourceSourceContainer;
        [SerializeField] private PositionSpawnSpot _spawnSpotPosition;
        [SerializeField] private ResourceSpawnPosition _spawnResourcePositions;
        [SerializeField] private ResourceFactory _resourceFactory;
        [SerializeField] private SpotFactory _spotFactory;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private PlayerContainer _playerContainer;
        [SerializeField] private ResourceView _resourceView;

        private SavesHandler _savesHandler;
        private Player _player;
        private DataPlayer _dataPlayer;
        private DataResource _dataResource;

        private SaveData _saveData;

        private List<IUpdater> _updates = new List<IUpdater>();
        private List<IFixedUpdater> _fixedUpdater = new List<IFixedUpdater>();

        private List<ResourceSource> _allResource = new List<ResourceSource>();

        private List<Spot> _allSpot = new List<Spot>();

        private bool _isPause;

        public bool SaveGameEveryMinute;

        public bool IsEnableTutorial;

        private void Start()
        {
            Application.targetFrameRate = 60;
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
            if (_isPause)
                return;

            foreach(var fixedUpdater in _fixedUpdater)
            {
                fixedUpdater.FixedUpdater();
            }
        }

        private void StartGame()
        {
            InitializeData();
            InitializeSavesHandler();
            CreatePlayer();
            InitializeCamera();
            var positionsResource = InitializeSpawnPositionResource();
            var positionSpot = InitializeSpawnSpotPosition();

            SpawnResource(positionsResource);
            SpawnSpot(positionSpot);
            InitializeTutorial();
        }

        private void InitializeSavesHandler()
        {
            _savesHandler = GetComponent<SavesHandler>();
            _savesHandler.Initialize(_dataPlayer, _dataResource, _saveData);
            if(SaveGameEveryMinute)
                _fixedUpdater.Add(_savesHandler);
        }

        private void InitializeTutorial()
        {
            if(IsEnableTutorial && _tutorial != null)
            {
                _tutorial.Initialize(playerHandler: _player, _dataPlayer);
                _fixedUpdater.Add(_tutorial);
            }         
        }

        private void InitializeData()
        {           
            _saveData = new SaveData();

            _dataPlayer = _saveData.LoadData<DataPlayer>(_saveData.FilePlayer);
            _dataResource = _saveData.LoadData<DataResource>(_saveData.FileResource);

            if (_dataPlayer == null)
                _dataPlayer = new DataPlayer();

            if (_dataResource == null)
                _dataResource = new DataResource();
        }

        private void SpawnSpot(List<PositionSpot> position)
        {
            for(int i = 0; i < position.Count; i++)
            {
                var spot = _spotFactory.GetSpot(position[i].Type, position[i].transform.position);
                _allSpot.Add(spot);
                _fixedUpdater.Add(spot);
            }
        }

        private List<PositionSpot> InitializeSpawnSpotPosition()
        {
            _spawnSpotPosition.Initialize();
            return _spawnSpotPosition.Positions;
        }

        private void SpawnResource(List<Position> positions)
        {
            _resourceSourceContainer = GetComponentInChildren<ResourceSourceContainer>();
           var resources = _resourceSourceContainer.SpawnResourceSource(positions, _camera, _resourceFactory, _resourceContainer);
            _allResource.AddRange(resources);
            _fixedUpdater.AddRange(resources);
        }

        private List<Position> InitializeSpawnPositionResource()
        {
            _spawnResourcePositions.Initialize();
            return _spawnResourcePositions.Positions;
        }

        private void InitializeCamera()
        {
            _camera.LookAt = _player.transform;
            _camera.Follow = _player.transform;
        }

        private void CreatePlayer()
        {
            _player = Instantiate(_playerPrefab);        
            _player.transform.SetParent(_playerContainer.transform);
             _player.transform.localPosition = Vector3.zero;
            _player.Initialize(joystickHandler:_joystick, _dataPlayer, _dataResource, _resourceView);

            _fixedUpdater.Add(_player);
            _updates.Add(_player);
        }

        public void DeleteAllSaves()
        {
            _savesHandler.DeleteSaves();
        }

        public void PauseGame()
        {
            if(!_isPause)
                _isPause = true;
            else
                _isPause = false;
        }

        public void OnApplicationQuit()
        {
            SaveGame();
        }

        public void SaveGame()
        {
            _savesHandler.SaveGame();
        }

        public void OnApplicationPause()
        {
            if (_saveData == null)
                return;

            SaveGame();
        }

        public void RestartGame()
        {

        }
    }
}