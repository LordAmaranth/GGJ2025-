using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject {
    [field: SerializeField] public float MovementSpeed { get; private set; } = 1;
    [field: SerializeField] public float MaxFallSpeed { get; private set; } = 5;
    [field: SerializeField] public float JumpSpeed { get; private set; } = 7;
    [field: SerializeField] public float Gravity { get; private set; } = 9.8f;
}
