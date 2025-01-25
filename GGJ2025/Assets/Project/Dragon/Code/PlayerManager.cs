using Project.GGJ2025;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour {
    public static int playerId = 0;

    public void OnPlayerJoined(PlayerInput playerInput) {
        DataStore.Instance.AddPlayer(playerInput.GetComponent<Player>());
    }

    public void OnPlayerLeft(PlayerInput playerInput) {
        DataStore.Instance.RemovePlayer(playerInput.GetComponent<Player>());
    }
}
