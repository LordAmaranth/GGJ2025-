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
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private AudioSource[] soundJump;
    [SerializeField] private AudioSource soundWind;
    [SerializeField] private AudioSource soundAttack;
    [SerializeField] private AudioSource soundKO;
    [SerializeField] private AudioSource soundTaunt1;
    [SerializeField] private AudioSource soundTaunt2;
    [SerializeField] private AudioSource soundTaunt3;
    [SerializeField] private AudioSource soundBlowBubbleStart;
    [SerializeField] private ParticleSystem landBubbleParticles;
    private List<Collider2D> windSources = new();

    private bool isAttacking;
    private bool holdingBlowButton;
    private bool isBlowingBubble;
    private float movementHorizontalSpeed;
    private JumpState jumpState;
    private PlayerVisuals visualsRoot;
    private bool playerIsImmobile = false;

    private void Awake() {
        GameObject a = Instantiate(config.PlayerVisuals[PlayerManager.playerId % config.PlayerVisuals.Length], transform);
        visualsRoot = a.GetComponent<PlayerVisuals>();
        PlayerManager.playerId++;

        visualsRoot.transform.localPosition = Vector3.zero;
        SetPlayerActiveAndResetState();
    }


    private void SetPlayerActiveAndResetState() {
        playerInput.SwitchCurrentActionMap("PlayerImmobile");
        playerInput.SwitchCurrentActionMap("Player");
        visualsRoot.WindBox.enabled = false;
        visualsRoot.AttackHitBox.enabled = false;
        visualsRoot.BlowBubbleHitBox.enabled = false;
        visualsRoot.WindParticles.gameObject.SetActive(false);
        visualsRoot.Animator.SetBool("Walk", false);
        visualsRoot.Animator.SetBool("BlowAir", false);
        visualsRoot.Animator.SetBool("BlowBubble", false);
        visualsRoot.Animator.SetTrigger("Attack");
        ChangeJumpState(JumpState.Falling);
        isAttacking = false;
        holdingBlowButton = false;
        isBlowingBubble = false;
        playerIsImmobile = false;
    }

    void Update() {
        if (playerIsImmobile) {
            return;
        }

        if (transform.position.y < config.KillHeight) {
            myRigidBody.Sleep();
            DisableControls();
            soundKO.Play();
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
            visualsRoot.WindParticles.transform.localScale = new Vector2(movementHorizontalSpeed < 0 ? 1 : -1, 1);
            visualsRoot.WindParticles.transform.GetChild(0).localScale = new Vector2(movementHorizontalSpeed < 0 ? 1 : -1, 1);
        }

        visualsRoot.Animator.SetBool("Walk", movementHorizontalSpeed != 0 && jumpState == JumpState.Grounded);

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

    public void DisableControls() {
        playerInput.SwitchCurrentActionMap("PlayerImmobile");
        playerIsImmobile = true;
        OnBlowAirEnd();
    }

    public void ReenableControls() {
        playerInput.SwitchCurrentActionMap("Player");
        SetPlayerActiveAndResetState();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Player")) {
            return;
        }

        float landAngle = Vector3.Angle(collision.GetContact(0).normal, Vector2.up);
        if (landAngle < config.MaxLandAngle) {
            ChangeJumpState(JumpState.Grounded);
            landBubbleParticles.transform.position = collision.contacts[0].point;
            landBubbleParticles.Play();
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
                visualsRoot.Animator.SetTrigger("Landed");
                visualsRoot.Animator.SetBool("Jump", false);
                visualsRoot.Animator.SetBool("Falling", false);
                break;
            case JumpState.Jumping:
                visualsRoot.Animator.SetBool("Jump", true);
                visualsRoot.Animator.SetBool("Falling", false);
                visualsRoot.Animator.ResetTrigger("Landed");
                break;
            case JumpState.Falling:
                visualsRoot.Animator.SetBool("Falling", true);
                visualsRoot.Animator.SetBool("Jump", false);
                visualsRoot.Animator.ResetTrigger("Landed");
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
            visualsRoot.Animator.SetBool("BlowAir", false);
            return;
        }

        if (isAttacking || isBlowingBubble || !context.performed) {
            return;
        }

        visualsRoot.Animator.ResetTrigger("Landed");
        visualsRoot.Animator.SetTrigger("Attack");
        visualsRoot.AttackHitBox.gameObject.SetActive(true);
        soundAttack.Play();
        isAttacking = true;
        holdingBlowButton = true;
    }
    public void OnBlowBubble(InputAction.CallbackContext context) {
        if (context.canceled) {
            isBlowingBubble = false;
            visualsRoot.BlowBubbleHitBox.enabled = false;
            visualsRoot.Animator.SetBool("BlowBubble", false);
            return;
        }

        if (!context.performed || holdingBlowButton || isAttacking) {
            return;
        }

        isBlowingBubble = true;
        visualsRoot.BlowBubbleHitBox.enabled = true;
        visualsRoot.Animator.SetBool("BlowBubble", true);

    }

    public void OnTaunt1(InputAction.CallbackContext context) {
        if (context.performed) {
            soundTaunt1.Play();
        }
    }
    public void OnTaunt2(InputAction.CallbackContext context) {
        if (context.performed) {
            soundTaunt2.Play();
        }
    }

    public void OnTaunt3(InputAction.CallbackContext context) {
        if (context.performed) {
            soundTaunt3.Play();
        }
    }

    public void OnAttackFinished() {
        if (holdingBlowButton) {
            visualsRoot.Animator.SetBool("BlowAir", true);
        }
        isAttacking = false;
        visualsRoot.AttackHitBox.gameObject.SetActive(false);
    }

    public void OnBlowAirStart() {
        visualsRoot.WindBox.enabled = true;
        visualsRoot.WindParticles.gameObject.SetActive(true);
        soundWind.Play();
    }
    public void OnBlowAirEnd() {
        visualsRoot.WindBox.enabled = false;
        visualsRoot.WindParticles.gameObject.SetActive(false);
        soundWind.Stop();
    }
}
