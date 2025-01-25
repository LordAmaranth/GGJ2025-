using Project.GGJ2025;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private ZoomCamera2D gameCamera;

    public void OnPlayerJoined(PlayerInput playerInput) {
        gameCamera.targets.Add(playerInput.transform);
    }

    public void OnPlayerLeft(PlayerInput playerInput) {
        gameCamera.targets.Remove(playerInput.transform);
    }
}
