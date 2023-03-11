using Assets._Scripts.Game;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets._Scripts.Initialize
{
    public class BootstraperServices
    {
        public async Task AllServices() // remaining initialize all services
        {
            List<Task> servicesInitializationTasks = new List<Task>(); 
            servicesInitializationTasks.Add(Task.Delay(100));
            servicesInitializationTasks.Add(Task.Delay(250));

            await Task.WhenAll(servicesInitializationTasks);
        }
    }
}