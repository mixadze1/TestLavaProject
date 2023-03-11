using System;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class SavesHandler : MonoBehaviour
    {
         public Game _game;

        public void DeleteSaves()
        {
            IGameSavesHandler savesHandler = _game;
            savesHandler.DeleteAllSaves();
        }
    }
}