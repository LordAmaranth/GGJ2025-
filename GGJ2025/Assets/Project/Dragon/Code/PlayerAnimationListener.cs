using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour {
    Player player;
    private void Awake() {
        player = transform.parent.GetComponent<Player>();
    }

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
