using Project.GGJ2025;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private ZoomCamera2D gameCamera;
    public static int playerId = 0;

    public void OnPlayerJoined(PlayerInput playerInput) {
        if (gameCamera == null) {
            return;
        }
        gameCamera.targets.Add(playerInput.transform);
    }

    public void OnPlayerLeft(PlayerInput playerInput) {
        if (gameCamera == null) {
            return;
        }
        gameCamera.targets.Remove(playerInput.transform);
    }
}
