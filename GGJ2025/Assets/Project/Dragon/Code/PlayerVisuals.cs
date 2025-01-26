using UnityEngine;

public class PlayerVisuals : MonoBehaviour {
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ParticleSystem WindParticles { get; private set; }
    [field: SerializeField] public Collider2D WindBox { get; private set; }
    [field: SerializeField] public Collider2D AttackHitBox { get; private set; }
    [field: SerializeField] public Collider2D BlowBubbleHitBox { get; private set; }
}
