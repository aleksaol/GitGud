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
    [SerializeField]
    private UIHandler uiHandler;

    private RaycastHit hit;
    private Camera cam;
    private GameObject heldObject;
    private GameObject targetObject;
    private GameObject targetContainer;

    private float raycastTimer = 0.0f;
    private int NotinteractableLayerMask;
    private int NotcontainerLayerMask;
    private bool lockCamera;

    public bool OpenMenu { get => lockCamera; set => lockCamera = value; }

    void OnEnable() {
        m_TargetCameraState.SetFromTransform(transform);
        m_InterpolatingCameraState.SetFromTransform(transform);
    }

    private void Awake() {
        NotinteractableLayerMask = ~(1 << 8);
        NotcontainerLayerMask = ~(1 << 9);
        lockCamera = false;
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

    private void HandleRaycast() {
        if (raycastTimer >= RAYCAST_TIME) {

            // Create a vector at the center of our camera's viewport
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            Debug.DrawRay(transform.position, cam.transform.forward * 999999.0f, Color.red);

            if (heldObject != null) {
                targetObject = null;

                if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 999999.0f, NotinteractableLayerMask)) {
                    Debug.DrawRay(transform.position, cam.transform.forward * hit.distance, Color.yellow);
                    if (hit.transform.CompareTag("Container")) {
                        targetContainer = hit.transform.gameObject;
                        ToggleReticle(false);
                    } else {
                        targetContainer = null;
                        ToggleReticle(true);
                    }
                } else {
                    targetContainer = null;
                    ToggleReticle(true);
                }
            } else {
                targetContainer = null;

                // Check if our raycast has hit anything
                if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 999999.0f, NotcontainerLayerMask)) {
                    Debug.DrawRay(transform.position, cam.transform.forward * hit.distance, Color.yellow);
                    if (hit.transform.CompareTag("Interactable")) {
                        targetObject = hit.transform.gameObject;
                        ToggleReticle(false);
                    } else {
                        targetObject = null;
                        ToggleReticle(true);
                    }
                } else {
                    targetObject = null;
                    ToggleReticle(true);
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

        if (Input.GetMouseButtonUp(0) && !lockCamera) {
            PickUpObject();
            PlaceObject();
        }
        

        if (Input.GetKeyUp(KeyCode.Tab)) {
            if (consoleHandler.ConsoleIsOpen) {
                ToggleConsole(false);
            } else {
                ToggleConsole(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Q)) {
            uiHandler.ToggleCommitMessage(true);
        }

    }

    private void HandleCamera() {

        // Rotation
        if (!lockCamera) {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
            m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;

            m_TargetCameraState.pitch = Mathf.Clamp(m_TargetCameraState.pitch, -50.0f, 50.0f);
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

    public void ToggleCursorMode(bool _lock) {
        if (_lock) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        lockCamera = !_lock;
    }

    public void ToggleConsole(bool _open) {
        if (_open) {
            ToggleCursorMode(false);
        } else {
            ToggleCursorMode(true);
        }

        consoleHandler.ToggleConsole();
        
    }

    private void PickUpObject() {
        if (heldObject == null && targetObject != null) {
            targetObject.GetComponent<PickUp>().OnPickUp();
            heldObject = targetObject;
        }
        
    }

    private void PlaceObject() {
        if (heldObject != null && targetContainer != null) {
            heldObject.GetComponent<PickUp>().OnPlacement(targetContainer.GetComponent<Container>());
            heldObject = null;
        }
    }
}
