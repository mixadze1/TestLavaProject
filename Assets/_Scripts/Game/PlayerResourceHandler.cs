using Assets._Scripts.Interfaces;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class PlayerResourceHandler : MonoBehaviour, IFixedUpdater, IHaveSaves, IResourceHandler
    {
        [Header("View")]
        private TextMeshProUGUI _textTrees;
        private TextMeshProUGUI _textCrystal; 
        private TextMeshProUGUI _textMetal;

        [SerializeField, Range(0.5f,5f)] private float _rangeItem = 1f;
        [SerializeField] private LayerMask _layerMask;

        private IPlayerHandler _playerHandler;

        private Tweener _tweener;

        private Dictionary<ResourceType, int> _resources;
        private Dictionary<ResourceType, TextMeshProUGUI> _resourcesView;

        private DataResource _dataResource;

        private float _offset = 0.35f;

        private bool _isTouchResource;

        public void Initialize(IPlayerHandler playerHandler, ResourceView resourceView)
        {
            InitializeView(resourceView);
        
            _playerHandler = playerHandler;   
        }

        private void InitializeView(ResourceView resourceView)
        {
            _textTrees = resourceView.TextTrees;
            _textCrystal = resourceView.TextCrystal;
            _textMetal = resourceView.TextMetal;
            _resourcesView = new Dictionary<ResourceType, TextMeshProUGUI>();
            _resourcesView.Add(ResourceType.Trees, _textTrees);
            _resourcesView.Add(ResourceType.Crystal, _textCrystal);
            _resourcesView.Add(ResourceType.Metal, _textMetal);
        }

        public bool IsTouch() => _isTouchResource;

        public void GetSaves(DataPlayer data, DataResource dataResource)
        {
            _dataResource = dataResource;
            _resources = new Dictionary<ResourceType, int>();         
            GetSafeResource();
            UpdateView(ResourceType.Trees, true);
            UpdateView(ResourceType.Metal, true);
            UpdateView(ResourceType.Crystal, true);
        }

        public void FixedUpdater()
        {
            CheckItemInRange();
        }

        public int AmountResource(ResourceType type)
        {
            return _resources[type];
        }

        public void AddResource(Resource resource)
        {
            resource.BoxColliderEnabled(false);
            _isTouchResource = true;
            resource.GiveItem(_playerHandler);
            int count = _resources[resource.Type];
            count++;
            _resources[resource.Type] = count;
            UpdateView(resource.Type);
            _dataResource.SetResource(resource.Type, count);
        }

        public void RemoveResource(ResourceType type, int amountWant)
        {           
            int count = _resources[type];
            if (count >= amountWant)
            {
                count-= amountWant;
                _resources[type] = count;
                UpdateView(type);
                _dataResource.SetResource(type, count);
            }
            else
            {
                Debug.Log("NotHaveResource");
            }
        }

        private void GetSafeResource()
        {
            _resources.Add(ResourceType.Trees, _dataResource.Trees);
            _resources.Add(ResourceType.Metal, _dataResource.Metal);
            _resources.Add(ResourceType.Crystal, _dataResource.Crystal);
        }

        private void CheckItemInRange()
        {
            var position = transform.position;
            Vector3 newPosition = new Vector3(position.x, position.y + _offset, position.z);
            Collider[] hitColliders = Physics.OverlapSphere(newPosition, _rangeItem, _layerMask);
            foreach (var hitCollider in hitColliders)
            {
                var item = hitCollider.GetComponent<Resource>();
                if (item)
                {
                    AddResource(item);
                }
            }
        }

        private void UpdateView(ResourceType type, bool isStart = false)
        {     
            if (_resources[type] == 0)
            {
                _resourcesView[type].gameObject.SetActive(false);
                return;
            }     

            if (!_resourcesView[type].gameObject.activeSelf)
                _resourcesView[type].gameObject.SetActive(true);

            _resourcesView[type].text = _resources[type].ToString();
            if(!isStart)
            {
                if (_tweener != null)
                    _tweener.Kill();
                   float scaleText = 1.5f, timeScale = 0.15f;
                _tweener = _resourcesView[type].transform.DOScale(scaleText, timeScale).OnComplete(() => _resourcesView[type].transform.DOScale(1, timeScale));
            }
         
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 0, 0.25f); 
            var position = transform.position;
            Vector3 newPosition = new Vector3(position.x, position.y + _offset, position.z);
            Gizmos.DrawSphere(newPosition, _rangeItem);
        }
    }
}
