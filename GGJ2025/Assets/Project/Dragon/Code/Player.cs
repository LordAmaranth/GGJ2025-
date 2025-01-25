using System.Collections.Generic;
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
    [SerializeField] private GameObject straw;
    [SerializeField] private Collider2D windBox;
    [SerializeField] private AudioSource[] soundJump;
    [SerializeField] private List<Collider2D> windSources = new();

    private bool isAttacking;
    private bool holdingBlowButton;
    private bool isBlowingBubble;
    private float movementHorizontalSpeed;
    private JumpState jumpState;

    private void Start() {
        ChangeJumpState(JumpState.Falling);
        weapon.SetActive(false);
        straw.SetActive(false);
        windBox.enabled = false;
        windParticles.gameObject.SetActive(false);
    }

    void Update() {
        if (transform.position.y < -30) {
            transform.position = new(transform.position.x, 20);
        }

        if (myRigidBody.linearVelocityY < config.FallSpeedThreshold) {
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

        for (int i = windSources.Count - 1; i >= 0; i--) {
            Collider2D windSource = windSources[i];
            if (windSource.enabled) {

                int directionMultiplier = 0;
                if (windSource.transform.position.x < transform.position.x) {
                    directionMultiplier = 1;
                }
                else if (windSource.transform.position.x > transform.position.x) {
                    directionMultiplier = -1;
                }
                myRigidBody.AddForceX(config.WindStrength * directionMultiplier);
            }
            else {
                windSources.Remove(windSource);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Player")) {
            return;
        }

        float landAngle = Vector3.Angle(collision.GetContact(0).normal, Vector2.up);
        if (landAngle < config.MaxLandAngle) {
            ChangeJumpState(JumpState.Grounded);
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger) {
        if (!trigger.CompareTag("Air")) {
            return;
        }

        if (!windSources.Contains(trigger)) {
            windSources.Add(trigger);
        }
    }
    private void OnTriggerExit2D(Collider2D trigger) {
        if (!trigger.CompareTag("Air")) {
            return;
        }

        if (windSources.Contains(trigger)) {
            windSources.Remove(trigger);
        }
    }

    private void ChangeJumpState(JumpState newJumpState) {
        if (newJumpState == jumpState) {
            return;
        }

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

        if (isAttacking || isBlowingBubble || !context.performed) {
            return;
        }

        animator.ResetTrigger("Landed");
        animator.SetTrigger("Attack");
        isAttacking = true;
        holdingBlowButton = true;
    }
    public void OnBlowBubble(InputAction.CallbackContext context) {
        if (context.canceled) {
            isBlowingBubble = false;
            straw.SetActive(false);
            return;
        }

        if (!context.performed || holdingBlowButton || isAttacking) {
            return;
        }

        isBlowingBubble = true;
        straw.SetActive(true);

    }

    public void OnAttackFinished() {
        if (holdingBlowButton) {
            animator.SetBool("BlowAir", true);
        }
        isAttacking = false;
    }

    public void OnBlowAirStart() {
        windBox.enabled = true;
        windParticles.gameObject.SetActive(true);
    }
    public void OnBlowAirEnd() {
        windBox.enabled = false;
        windParticles.gameObject.SetActive(false);
    }
}
