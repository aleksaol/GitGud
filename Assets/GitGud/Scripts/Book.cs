using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField]
    private Vector3 heldForward;
    [SerializeField]
    private Vector3 heldUp;

    private Camera cam;
    private GameObject visable;
    private GameObject transparent;
    private Collider boxCollider;

    private InputManager inputManager;
    private PlayerController player;

    private BookState state;

    public BookState State { get => state; set => state = value; }


    private void Awake() {
        state = BookState.INVENTORY;
        cam = Camera.main;
        visable = transform.GetChild(0).gameObject;
        transparent = transform.GetChild(1).gameObject;
        boxCollider = GetComponent<Collider>();
        
    }

    private void Start() {
        inputManager = InputManager.Instance;
        player = PlayerController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BookState.HELD) {
            
            Ray ray = Camera.main.ScreenPointToRay(inputManager.GetMouseScreenPosition());

            transform.position = ray.origin + (ray.direction * 0.5f);
            transform.rotation = Quaternion.LookRotation(GetHeldForwardVector(), GetHeldUpVector());
        }
    }

    public void TryPickUp() {
        if (state == BookState.LOCKED || state == BookState.TRYPLACE) {
            return;
        }

        PickUp();
    }

    public void PickUp() {
        if (player.TryPickUpBook(this)) {

            if (state == BookState.PLACED) {
                transform.parent.GetComponent<ShelfPos>().RemoveBook();
            }

            state = BookState.HELD;
            // Set back to inventory before picking up to reset size.
            PlaceBook(player.Inventory.gameObject);
            transform.parent = null;

            visable.SetActive(false);
            transparent.SetActive(true);
            boxCollider.enabled = false;
        }
    }

    public void PlaceBook(GameObject _parent) {
        transform.parent = _parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        if (state != BookState.TRYPLACE) {
            boxCollider.enabled = true;
        }

        if (state == BookState.INVENTORY || state == BookState.LOCKED) {
            visable.SetActive(true);
            transparent.SetActive(false);
        }
    }

    private Vector3 GetHeldForwardVector() {
        if (heldForward.x != 0.0f) {
            return cam.transform.right * heldForward.x;
        }

        if (heldForward.y != 0.0f) {
            return cam.transform.up * heldForward.y;
        }

        if (heldForward.z != 0.0f) {
            return cam.transform.forward * heldForward.z;
        }

        return cam.transform.up;
    }

    private Vector3 GetHeldUpVector() {
        if (heldUp.x != 0.0f) {
            return cam.transform.right * heldUp.x;
        }

        if (heldUp.y != 0.0f) {
            return cam.transform.up * heldUp.y;
        }

        if (heldUp.z != 0.0f) {
            return cam.transform.forward * heldUp.z;
        }

        return cam.transform.up;
    }

    public void Enter() {
        if (state == BookState.INVENTORY && player.HeldBook == null) {
            transform.localPosition += Vector3.up * 0.1f;
        }
    }

    public void Exit() {
        if (state == BookState.INVENTORY && player.HeldBook == null) {
            transform.localPosition -= Vector3.up * 0.1f;
        }
    }
}
