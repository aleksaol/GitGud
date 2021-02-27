using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    class CameraState {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        public void SetFromTransform(Transform t) {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
            x = t.position.x;
            y = t.position.y;
            z = t.position.z;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct) {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);
        }

        public void UpdateTransform(Transform t) {
            t.eulerAngles = new Vector3(pitch, yaw, roll);
            t.position = new Vector3(x, y, z);
        }
    }

    CameraState m_TargetCameraState = new CameraState();
    CameraState m_InterpolatingCameraState = new CameraState();

    [Header("Movement Settings")]
    [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
    public float boost = 3.5f;

    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
    public float positionLerpTime = 0.2f;

    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;


    private const float RAYCAST_TIME = 0.001f;

    [SerializeField]
    private ConsoleHandler consoleHandler;
    [SerializeField]
    private GameObject defaultReticle;
    [SerializeField]
    private GameObject targetReticle;

    private RaycastHit hit;
    private Camera cam;
    private GameObject heldObject;
    private GameObject targetObject;

    private float raycastTimer = 0.0f;
    private int interactableLayerMask;
    private int containerLayerMask;
    private bool holdingObject;
    private bool openMenu;

    public bool OpenMenu { get => openMenu; set => openMenu = value; }

    void OnEnable() {
        m_TargetCameraState.SetFromTransform(transform);
        m_InterpolatingCameraState.SetFromTransform(transform);
    }

    private void Awake() {
        interactableLayerMask = 1 << 8;
        containerLayerMask = 1 << 9;
        holdingObject = false;
        openMenu = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCamera();
        HandleRaycast();
        HandleInput();
    }

    public void ToggleMenu() {
        if (openMenu) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        consoleHandler.ToggleConsole();
        openMenu = !openMenu;
    }

    private void HandleRaycast() {
        if (raycastTimer >= RAYCAST_TIME) {

            // Create a vector at the center of our camera's viewport
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            Debug.DrawRay(transform.position, cam.transform.forward * 999999.0f, Color.red);

            if (holdingObject) {
                if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 999999.0f, containerLayerMask)) {
                    Debug.DrawRay(transform.position, cam.transform.forward * hit.distance, Color.yellow);
                    if (hit.transform.CompareTag("Container")) {
                        Debug.Log("Container");
                    }
                }
            } else {
                // Check if our raycast has hit anything
                if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 999999.0f, interactableLayerMask)) {
                    Debug.DrawRay(transform.position, cam.transform.forward * hit.distance, Color.yellow);
                    if (hit.transform.CompareTag("Interactable")) {
                        Debug.Log("Interactable");
                        targetObject = hit.transform.gameObject;
                        ToggleReticle(false);
                    } else {
                        targetObject = null;
                    }
                } else {
                    ToggleReticle(true);
                    targetObject = null;
                }
            }


            raycastTimer -= RAYCAST_TIME;
        }

        raycastTimer += Time.deltaTime;
    }

    private void HandleInput() {
        

        // Exit Sample  
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        

        if (Input.GetKeyUp(KeyCode.Tab)) {
            ToggleMenu();
        }

    }

    private void HandleCamera() {

        // Rotation
        if (!openMenu) {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
            m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
        }

        // Framerate-independent interpolation
        // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
        var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
        var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
        m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

        m_InterpolatingCameraState.UpdateTransform(transform);
    }

    private void ToggleReticle(bool _default) {
        if (_default) {
            defaultReticle.SetActive(true);
            targetReticle.SetActive(false);
        } else {
            defaultReticle.SetActive(false);
            targetReticle.SetActive(true);
        }
        
    }

    private void HoldObject() {

    }
}
