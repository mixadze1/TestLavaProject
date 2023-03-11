using Assets._Scripts.Interfaces;
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

        private Dictionary<ResourceType, int> Resources;

        private DataResource _dataResource;

        private float _offset = 0.5f;

        private bool _isTouchResource;

        public void Initialize(IPlayerHandler playerHandler, ResourceView resourceView)
        {
            _textTrees = resourceView.TextTrees;
            _textCrystal = resourceView.TextCrystal;
            _textMetal = resourceView.TextMetal;   
            _playerHandler = playerHandler;   
        }

        public bool IsTouch() => _isTouchResource;

        public void GetSaves(DataPlayer data, DataResource dataResource)
        {
            _dataResource = dataResource;
            Resources = new Dictionary<ResourceType, int>();
            Debug.Log(Resources);
           
            GetSafeResource();
            UpdateView();
        }

        public void FixedUpdater()
        {
            CheckItemInRange();
        }

        public int AmountResource(ResourceType type)
        {
            return Resources[type];
        }

        public void AddResource(Resource item)
        {
            _isTouchResource = true;
            Debug.Log(_playerHandler);
            item.GiveItem(_playerHandler);
            int count = Resources[item.Type];
            count++;
            Resources[item.Type] = count;
            UpdateView();
            _dataResource.SetResource(item.Type, count);
        }

        public void RemoveResource(ResourceType type, int amountWant)
        {           
            int count = Resources[type];
            if (count >= amountWant)
            {
                count-= amountWant;
                Resources[type] = count;
                UpdateView();
                _dataResource.SetResource(type, count);
            }
            else
            {
                Debug.Log("NotHaveResource");
            }
        }

        private void GetSafeResource()
        {
            Resources.Add(ResourceType.Trees, _dataResource.Trees);
            Resources.Add(ResourceType.Metal, _dataResource.Metal);
            Resources.Add(ResourceType.Crystal, _dataResource.Crystal);
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

        private void UpdateView()
        {
            _textCrystal.text =Resources[ResourceType.Crystal] + ":CRYSTAL";
            _textTrees.text =  Resources[ResourceType.Trees] + ":TREES";
            _textMetal.text = Resources[ResourceType.Metal] + ":METAL";
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 1, 0.5f); 
            var position = transform.position;
            Vector3 newPosition = new Vector3(position.x, position.y + _offset, position.z);
            Gizmos.DrawSphere(newPosition, _rangeItem);
        }
    }
}
