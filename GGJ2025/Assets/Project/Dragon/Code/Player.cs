using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    private enum JumpState {
        Grounded = 0,
        Jumping = 1,
        Falling = 2,
    }

    [SerializeField] private PlayerConfig config;
    [SerializeField] private Rigidbody2D myRigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject visualsRoot;
    [SerializeField] private ParticleSystem windParticles;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject windBox;
    [SerializeField] private AudioSource[] soundJump;

    private bool isAttacking;
    private bool holdingBlowButton;
    private float movementHorizontalSpeed;
    private JumpState jumpState;

    private void Start() {
        ChangeJumpState(JumpState.Falling);
        weapon.SetActive(false);
        windBox.SetActive(false);
    }

    void Update() {
        if (myRigidBody.linearVelocityY < 0) {
            ChangeJumpState(JumpState.Falling);
        }

        if (jumpState == JumpState.Jumping) {
            myRigidBody.AddForceY(config.JumpHeldSpeed);
            if (myRigidBody.linearVelocityY >= config.MaxAscendSpeed) {
                ChangeJumpState(JumpState.Falling);
            }
        }

        myRigidBody.linearVelocity = new Vector2(movementHorizontalSpeed * config.MovementSpeed, myRigidBody.linearVelocity.y);
        if (movementHorizontalSpeed != 0) {
            visualsRoot.transform.localScale = new Vector2(movementHorizontalSpeed < 0 ? 1 : -1, 1);
            windParticles.transform.localScale = new Vector2(movementHorizontalSpeed < 0 ? 1 : -1, 1);
        }

        animator.SetBool("Walk", movementHorizontalSpeed != 0 && jumpState == JumpState.Grounded);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Ground")) {
            return;
        }

        if (collision.GetContact(0).normal == Vector2.up) {
            ChangeJumpState(JumpState.Grounded);
        }
    }

    private void ChangeJumpState(JumpState newJumpState) {
        if (newJumpState == jumpState) {
            return;
        }

        Debug.Log($"State Switch {newJumpState}");
        switch (newJumpState) {
            case JumpState.Grounded:
                animator.SetTrigger("Landed");
                animator.SetBool("Jump", false);
                animator.SetBool("Falling", false);
                break;
            case JumpState.Jumping:
                animator.SetBool("Jump", true);
                animator.SetBool("Falling", false);
                animator.ResetTrigger("Landed");
                break;
            case JumpState.Falling:
                animator.SetBool("Falling", true);
                animator.SetBool("Jump", false);
                animator.ResetTrigger("Jump");
                break;
            default:
                throw new System.Exception($"Unhandled jump state {newJumpState}!");
        }

        jumpState = newJumpState;
    }

    public void OnMove(InputAction.CallbackContext context) => movementHorizontalSpeed = context.ReadValue<float>();
    public void OnJump(InputAction.CallbackContext context) {
        if (context.canceled) {
            if (jumpState == JumpState.Jumping) {
                ChangeJumpState(JumpState.Falling);
            }
        }

        if (jumpState != JumpState.Grounded) {
            return;
        }

        if (context.performed) {
            ChangeJumpState(JumpState.Jumping);
            soundJump[Random.Range(0, soundJump.Length)].Play();
            myRigidBody.AddForceY(config.JumpStartSpeed, ForceMode2D.Impulse);
        }
    }


    public void OnAttack(InputAction.CallbackContext context) {
        if (holdingBlowButton && context.canceled) {
            isAttacking = false;
            holdingBlowButton = false;
            animator.SetBool("BlowAir", false);
            return;
        }

        if (isAttacking || !context.performed) {
            return;
        }

        animator.ResetTrigger("Landed");
        animator.SetTrigger("Attack");
        isAttacking = true;
        holdingBlowButton = true;
    }

    public void OnAttackFinished() {
        if (holdingBlowButton) {
            animator.SetBool("BlowAir", true);
        }
        isAttacking = false;
    }

    public void OnBlowAirStart() {
        windBox.SetActive(true);
    }
    public void OnBlowAirEnd() {
        windBox.SetActive(false);
    }
}
