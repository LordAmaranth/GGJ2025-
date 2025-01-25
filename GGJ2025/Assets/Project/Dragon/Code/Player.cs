using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    [SerializeField] private PlayerConfig config;
    [SerializeField] private Rigidbody2D myRigidBody;

    private float movementHorizontalSpeed;
    private bool isGrounded;
    private bool jumping;
    private bool falling;
    private bool isMovingVertically => myRigidBody.linearVelocityY != 0f;
    void Start() {

    }

    void Update() {
        if (jumping) {
            Debug.Log(myRigidBody.linearVelocityY);

            myRigidBody.AddForceY(config.JumpHeldSpeed);
            if (myRigidBody.linearVelocityY >= config.MaxAscendSpeed) {
                jumping = false;
                falling = true;
            }
            else if (myRigidBody.linearVelocityY < 0) {
                falling = true;
                jumping = false;
            }
        }

        myRigidBody.linearVelocity = new Vector2(movementHorizontalSpeed * config.MovementSpeed, myRigidBody.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Ground")) {
            return;
        }

        if (collision.GetContact(0).normal == Vector2.up) {
            isGrounded = true;
            falling = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context) => movementHorizontalSpeed = context.ReadValue<float>();
    public void OnJump(InputAction.CallbackContext context) {
        if (context.canceled) {
            jumping = false;
        }

        if (!isGrounded || isMovingVertically || falling) {
            return;
        }

        if (context.performed) {
            isGrounded = false;
            jumping = true;
            myRigidBody.AddForceY(config.JumpStartSpeed, ForceMode2D.Impulse);
        }
    }
}
