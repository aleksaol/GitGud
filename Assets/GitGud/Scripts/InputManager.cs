using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance {
        get {
            return _instance;
        }
    }

    private PlayerControls playerControls;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    public Vector2 GetMouseScreenPosition() {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerExit() {
        return playerControls.Player.Exit.triggered;
    }
}
