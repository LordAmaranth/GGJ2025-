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

    private float movementHorizontalSpeed;
    private JumpState jumpState;


    void Update() {
        if (myRigidBody.linearVelocityY < 0) {
            jumpState = JumpState.Falling;
        }

        if (jumpState == JumpState.Jumping) {
            myRigidBody.AddForceY(config.JumpHeldSpeed);
            if (myRigidBody.linearVelocityY >= config.MaxAscendSpeed) {
                jumpState = JumpState.Falling;
            }
        }

        myRigidBody.linearVelocity = new Vector2(movementHorizontalSpeed * config.MovementSpeed, myRigidBody.linearVelocity.y);
        if (movementHorizontalSpeed != 0) {
            visualsRoot.transform.localScale = new Vector2(movementHorizontalSpeed < 0 ? 1 : -1, 1);
        }

        animator.SetBool("Walk", movementHorizontalSpeed != 0 && jumpState == JumpState.Grounded);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Ground")) {
            return;
        }

        if (collision.GetContact(0).normal == Vector2.up) {
            jumpState = JumpState.Grounded;
        }
    }

    public void OnMove(InputAction.CallbackContext context) => movementHorizontalSpeed = context.ReadValue<float>();
    public void OnJump(InputAction.CallbackContext context) {
        if (context.canceled) {
            if (jumpState == JumpState.Jumping) {
                jumpState = JumpState.Falling;
            }
        }

        if (jumpState != JumpState.Grounded) {
            return;
        }

        if (context.performed) {
            jumpState = JumpState.Jumping;
            myRigidBody.AddForceY(config.JumpStartSpeed, ForceMode2D.Impulse);
        }
    }
}
