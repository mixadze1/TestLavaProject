using Assets._Scripts.Interfaces;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class Tutorial : MonoBehaviour, IFixedUpdater
    {
        [SerializeField] private Transform _positionToResource;
        [SerializeField] private Transform _positionToSpot;

        private DataPlayer _dataPlayer;
        private Transform _arrow;

        private IPlayerHandler _playerHandler;

        private bool _firstState;
        private bool _secondState;

        public void Initialize(IPlayerHandler playerHandler, DataPlayer dataPlayer)
        {
            _dataPlayer = dataPlayer;
            _arrow = playerHandler.GetArrow();
            _playerHandler = playerHandler;
            _firstState = _dataPlayer.IsTutorialPartComplete;
            _secondState = _dataPlayer.IsTutorialComplete;
            _arrow.gameObject.SetActive(true);
        }

        public void FixedUpdater()
        {
            if(_arrow == null) //if we are delete player, arrow == null
                return;            

            if(this == null)
            {
                Debug.Log("Don't delete Tutorial. You can remove tutorial in scene Game, GameObject -> Game ->  in editor disable tick \"IsEnableTutorial\"");
                if (_arrow.gameObject.activeSelf)
                    _arrow.gameObject.SetActive(false);
                return;
            }
            TutorialLogic();
        }

        private void TutorialLogic()
        {

            if (_firstState && _secondState)
            {
                if (_arrow.gameObject.activeSelf)
                    _arrow.gameObject.SetActive(false);
                return;
            }

            if (!_firstState)
                _arrow.transform.LookAt(_positionToResource);

            if (_firstState && !_secondState)
                _arrow.transform.LookAt(_positionToSpot);


            if (_playerHandler.IsTouchResourceSource())
            {
                _firstState = true;
                _dataPlayer.FirstPartTutorialComplete();
            }

            if (_playerHandler.IsTouchSpot() && _firstState)
            {
                _secondState = true;
                _dataPlayer.TutorialComplete();
            }
        }
    }
}