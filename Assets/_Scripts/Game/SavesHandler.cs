using Assets._Scripts.Interfaces;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class SavesHandler : MonoBehaviour, IFixedUpdater
    {
        private DataPlayer _dataPlayer;
        private DataResource _dataResource;
        private SaveData _saveData;

        private float _minute = 60;
        private float _fixedTime;

        public void Initialize(DataPlayer dataPlayer, DataResource dataResource, SaveData saveData)
        {
            _dataResource = dataResource;
            _dataPlayer = dataPlayer;
            _saveData = saveData;
        }

        public void DeleteSaves()
        {
            if (_saveData == null)
                _saveData = new SaveData();

            _saveData.DeleteFile(_saveData.FilePlayer);
            _saveData.DeleteFile(_saveData.FileResource);
        }

        public void SaveGame()
        {
            Debug.Log("SaveGame");
            _saveData.SaveGame(_dataPlayer, _saveData.FilePlayer);
            _saveData.SaveGame(_dataResource, _saveData.FileResource);
        }

        public void FixedUpdater()
        {
            _fixedTime += Time.fixedDeltaTime;
            if (_fixedTime > _minute)
            {           
                _fixedTime = 0;
                SaveGame();
            }
        }
    }
}