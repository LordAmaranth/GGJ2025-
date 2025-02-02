using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject {
    [field: SerializeField] public float MovementSpeed { get; private set; } = 1;
    [field: SerializeField] public float JumpStartSpeed { get; private set; } = 20;
    [field: SerializeField] public float MaxAscendSpeed { get; private set; } = 10;
    [field: SerializeField] public float JumpHeldSpeed { get; private set; } = 5;
    [field: SerializeField] public float MaxLandAngle { get; private set; } = 45;
    [field: SerializeField] public float FallSpeedThreshold { get; private set; } = -0.1f;
    [field: SerializeField] public float WindStrength { get; private set; } = -2f;
    [field: SerializeField] public float KillHeight { get; private set; } = -30f;
    [field: SerializeField] public GameObject[] PlayerVisuals { get; private set; }
}
