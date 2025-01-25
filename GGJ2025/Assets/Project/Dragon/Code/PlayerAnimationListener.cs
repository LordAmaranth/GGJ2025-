using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour {
    [SerializeField] Player player;

    public void OnAttackFinished() {
        player.OnAttackFinished();
    }
    public void OnBlowAirStart() {
        player.OnBlowAirStart();
    }
    public void OnBlowAirEnd() {
        player.OnBlowAirEnd();
    }
}
