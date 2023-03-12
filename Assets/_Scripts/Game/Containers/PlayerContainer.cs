using UnityEngine;

namespace Assets._Scripts.Game.Containers
{
    public class PlayerContainer : MonoBehaviour
    {

        public Player SpawnPlayer(Player playerPrefab,Joystick joystick, DataPlayer dataPlayer, DataResource dataResource,ResourceView resourceView)
        {
            var player = Instantiate(playerPrefab);
            player.transform.SetParent(this.transform);
            player.transform.localPosition = Vector3.zero;
            player.Initialize(joystickHandler: joystick, dataPlayer, dataResource, resourceView);
            return player;
        }
    }
}