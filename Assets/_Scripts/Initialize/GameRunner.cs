using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._Scripts.Initialize
{
    public class GameRunner : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private async void Start()
        {
            await BootstraperServices();
            LoadGame();
        }


        private async Task BootstraperServices()
        {
            BootstraperServices bootstrap = new BootstraperServices();
            await bootstrap.AllServices();
        }

        private void LoadGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}