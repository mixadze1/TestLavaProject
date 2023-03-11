using Assets._Scripts.Interfaces;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class Spot : MonoBehaviour, IFixedUpdater
    {
        [SerializeField] private Transform _positionForParticle;
     
        [SerializeField] private Transform _positionForGet;
        [SerializeField] private Transform _positionForCreate;

        [SerializeField] private Resource _prefabItem;

        [SerializeField] private List<MeshRenderer> _meshColorFromResource;

        [SerializeField] private List<MeshRenderer> _meshColorToResource;

        private ParticleSystem _particle;

        private float _rangeNeedResource;

        private float _rangeCreateResource;

        private float _timeCreateResource;
        private float _timeDelayGiveResource;

        private int _amountResourceNeed;
        private int _amountResourceCreate;

        private int _safeAmountResourceNeed;

        private float _fixedTimeForCreate;
        private float _fixedTimeForDelay;

        private SpotView _spotView;

        private ResourceType _resourceNeed;
        private ResourceType _resourceCreate;

        private bool _isDelayGetResource;
        private bool _isCreate;

        public void Initialize(SpotFactory.SpotConfig config, Vector3 position, SpotContainer spotContainer)
        {
            this.transform.position = new Vector3(position.x, 1.5f, position.z);
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
            this.transform.SetParent(spotContainer.transform);
            InitializeParticle(config);
            InitializeTimeDelay(config);
            InitializeAmountCreateAndNeedResources(config);
            InitializeRangeNeedResources(config);
            InitializeRangeCreateResource(config);
            InitializeTypeResources(config);
            InitializeColorSpot(config);
            InitializeSpotView();
            UpdateView();
        }

        public ResourceType TypeResource()
        {
            return _resourceNeed;
        }

        public Resource GetItemPrefab() => _prefabItem;

        public Material GetMaterialItem() => _meshColorFromResource[0].material;

        public bool GetNeedResource(ResourceType type, Vector3 playerPosition, int amount)
        {
            if (_isDelayGetResource || _isCreate)
                return false;

            var offset = 1;
            var resource = Instantiate(_prefabItem, new Vector3(playerPosition.x, playerPosition.y + offset, playerPosition.z) + Vector3.forward, Quaternion.identity);
            resource.Initialize(type);

            if (_amountResourceNeed >= amount)
                _amountResourceNeed -= amount;

            if (_amountResourceNeed == 0)
                StartGenerateResource();

            _isDelayGetResource = true;
            UpdateView();
            PositionToNeedResource(resource);
            return true;
        }

        private void InitializeParticle(SpotFactory.SpotConfig config)
        {
            _particle = Instantiate(config.Particle, _positionForParticle.position, _positionForParticle.rotation);
            _particle.transform.SetParent(this.transform);
        }

        private void InitializeRangeNeedResources(SpotFactory.SpotConfig config)
        {
            _rangeNeedResource = config.RandomRangeNeedResource;
        }

        private void InitializeRangeCreateResource(SpotFactory.SpotConfig config)
        {
            _rangeCreateResource = config.RandomRangeCreateResource;
        }

        private void InitializeTypeResources(SpotFactory.SpotConfig config)
        {
            _resourceNeed = config.TypeResourceNeed;
            _resourceCreate = config.TypeResourceCreate;
        }

        private void InitializeTimeDelay(SpotFactory.SpotConfig config)
        {
            _timeCreateResource = config.TimeCreateResource;
            _timeDelayGiveResource = config.TimeDelayGiveResource;
        }

        private void InitializeAmountCreateAndNeedResources(SpotFactory.SpotConfig config)
        {
            _amountResourceNeed = config.ResourceNeed;
            _amountResourceCreate = config.ResourceCreate;

            _safeAmountResourceNeed = _amountResourceNeed;
        }

        private void InitializeColorSpot(SpotFactory.SpotConfig config)
        {
            foreach (var mesh in _meshColorFromResource)
                mesh.material = config.MaterialFrom;

            foreach (var mesh in _meshColorToResource)
                mesh.material = config.MaterialTo;
        }

        private void PositionToNeedResource(Resource resource)
        {
            var random = Random.Range(-_rangeNeedResource, _rangeNeedResource);
            var timeMove = 1f;
            resource.transform.DOMove(new Vector3(_positionForGet.transform.position.x + random, _positionForGet.transform.position.y + random,
                _positionForGet.transform.position.z + random), timeMove).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Destroy(resource.gameObject);
                });
        }

        private void StartGenerateResource()
        {
            _isCreate = true;
            UpdateView();
        }

        private void InitializeSpotView()
        {
            _spotView = GetComponent<SpotView>();
            _spotView.Initialize(_meshColorFromResource[0].material);
        }

        private bool IsCreate()
        {
            if(_isCreate)
                return true;
            return false;   
        }

        private void UpdateView()
        {
            _spotView.UpdateView(_amountResourceNeed, _amountResourceCreate, _amountResourceNeed);
        }

        private void CreateResource()
        {
            for (int i = 0; i < _amountResourceCreate; i++)
            {
                var item = Instantiate(_prefabItem, this.transform.position, Quaternion.identity);
                item.Initialize(_resourceCreate);
                var position = _positionForCreate.transform.position;
                item.transform.SetParent(this.transform);
                float jumpPower = 3f, timeJump = 1.5f;
                int amountJump = 1;
                item.transform.DOJump(new Vector3(position.x + Random.Range(-2, 2f), position.y, position.z + Random.Range(-_rangeCreateResource, _rangeCreateResource)),
                    jumpPower, amountJump, timeJump).OnComplete(() =>
                    {
                        UpdateView();
                        _spotView.ZeroSlider();
                        item.BoxColliderEnabled(true);
                    });
            }
        }

        public void FixedUpdater()
        {
            if(_isDelayGetResource)
            {
                _fixedTimeForDelay += Time.fixedDeltaTime;

                if (_fixedTimeForDelay > _timeDelayGiveResource)
                {
                    _fixedTimeForDelay = 0;
                    _isDelayGetResource = false;
                }
            }
        
            if (IsCreate())
            {
                _fixedTimeForCreate += Time.fixedDeltaTime;
                _spotView.UpdateViewSlider(_timeCreateResource, _fixedTimeForCreate);
                _meshColorToResource[1].transform.localScale = Vector3.one * 0.5f * (_fixedTimeForCreate / _timeCreateResource);
                if (_fixedTimeForCreate > _timeCreateResource)
                {
                    _fixedTimeForCreate = 0;
                    _isCreate = false;
                    _amountResourceNeed = _safeAmountResourceNeed;
                    _particle.Play();
                    CreateResource();
                }
            }
        }
    }
}