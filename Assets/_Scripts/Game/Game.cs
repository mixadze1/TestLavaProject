using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets._Scripts.Interfaces;
using UnityEngine.AI;

namespace Assets._Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navMesh;
        [SerializeField] private LevelView _levelView;
        [SerializeField] private LevelHandler _levelHnadler;  
        [SerializeField] private ResourceFactory _resourceFactory;
        [SerializeField] private SpotFactory _spotFactory;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Player _playerPrefab;  
        [SerializeField] private ResourceView _resourceView;

        private Tutorial _tutorial;
        private PlayerContainer _playerContainer;
        private SpotContainer _spotContainer;
        private ResourceContainer _resourceContainer;
        private ResourceSourceContainer _resourceSourceContainer;

        private ResourceSpawnPosition _spawnResourcePositions;
        private PositionSpawnSpot _spawnSpotPosition;

        private Level _level;
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
            StartNewGame();
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

        private void StartNewGame()
        {
            InitializeData();
            InitializeSavesHandler();
            InitializeCountLevel();
            CreateLevel();
            BuildNavMesh();
            CreatePlayer();
            InitializeCamera();
            var positionsResource = InitializeSpawnPositionResource();
            var positionSpot = InitializeSpawnSpotPosition();
            SpawnResource(positionsResource);
            SpawnSpot(positionSpot);
            InitializeTutorial();
        }

        private void RestartGame()
        {
            DestroyImmediate(_level.gameObject);
            _updates.Clear();
            _fixedUpdater.Clear();
            _allResource.Clear();
            _allSpot.Clear();
        }

        private void BuildNavMesh()
        {
            _navMesh.BuildNavMesh();
            _navMesh.transform.position = new Vector3(_navMesh.transform.position.x, _navMesh.transform.position.y - 0.01f, _navMesh.transform.position.z);
        }

        private void CreateLevel()
        {
            _level = _levelHnadler.CreateLevel(_dataPlayer.Level);
        }

        private void InitializeCountLevel()
        {
            _levelView.Initialize(_dataPlayer.Level);
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
            _tutorial = _level.GetComponentInChildren<Tutorial>();
            if(IsEnableTutorial && _tutorial != null && _dataPlayer.Level == 0)
            {
                _tutorial.Initialize(playerHandler: _player, _dataPlayer);
                _fixedUpdater.Add(_tutorial);
            }         
        }

        private void InitializeData()
        {
            if (_saveData == null)
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
            _spotContainer = _level.GetComponentInChildren<SpotContainer>();
            for(int i = 0; i < position.Count; i++)
            {
                var spot = _spotFactory.GetSpot(position[i].Type, position[i].transform.position, _spotContainer);
                _allSpot.Add(spot);
                _fixedUpdater.Add(spot);
            }
        }

        private List<PositionSpot> InitializeSpawnSpotPosition()
        {
            _spawnSpotPosition = _level.GetComponentInChildren<PositionSpawnSpot>();
            _spawnSpotPosition.Initialize();
            return _spawnSpotPosition.Positions;
        }

        private void SpawnResource(List<Position> positions)
        {
            _resourceContainer = _level.GetComponentInChildren<ResourceContainer>();
            _resourceSourceContainer = _level.GetComponentInChildren<ResourceSourceContainer>();

           var resources = _resourceSourceContainer.SpawnResourceSource(positions, _camera, _resourceFactory, _resourceContainer);
            _allResource.AddRange(resources);
            _fixedUpdater.AddRange(resources);
        }

        private List<Position> InitializeSpawnPositionResource()
        {
            _spawnResourcePositions = _level.GetComponentInChildren<ResourceSpawnPosition>();
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
            _playerContainer = _level.GetComponentInChildren<PlayerContainer>();

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

        public void NextLevel()
        {
            RestartGame();
            _dataPlayer.Position = Vector3.zero;
            var level = _dataPlayer.Level;     
            level++;
            _dataPlayer.SetLevel(level);
            SaveGame();

            StartNewGame();
        }
    }
}