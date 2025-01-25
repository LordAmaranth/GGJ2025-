using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour {
    [SerializeField] Player player;

    public void OnAttackFinished() {
        player.OnAttackFinished();
    }
}
