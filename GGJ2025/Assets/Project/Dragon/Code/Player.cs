using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    [SerializeField] private PlayerConfig config;


    private float movementHorizontal;
    private float movementVertical;
    private bool inAir;
    private bool startJump;
    void Start() {

    }

    void Update() {
        transform.Translate(new Vector3(movementHorizontal * config.MovementSpeed, 0, 0));
    }

    public void OnMove(InputAction.CallbackContext context) => movementHorizontal = context.ReadValue<float>();
    public void OnJump(InputAction.CallbackContext context) => startJump = context.ReadValue<bool>();
}
