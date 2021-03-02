using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    [SerializeField]
    private Container container;
    [SerializeField]
    private Vector3 heldForward;
    [SerializeField]
    private Vector3 heldUp;
    [SerializeField]
    private float heldDistance = 2.0f;

    private Camera cam;

    private bool isHeld;

    public bool IsHeld { get => isHeld; set => isHeld = value; }

    private void Awake() {
        isHeld = false;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHeld) {
            transform.position = cam.transform.position + (cam.transform.forward * heldDistance);
            transform.rotation = Quaternion.LookRotation(GetHeldForwardVector(), GetHeldUpVector());
        }
    }

    public void OnPickUp() {
        isHeld = true;
        transform.parent = null;
        container.RemovePickUp(this);
        container = null;
    }

    public void OnPlacement(Container _container) {
        isHeld = false;
        container = _container;
        transform.parent = container.transform;
        transform.localRotation = Quaternion.identity;
        container.PlacePickUp(this);
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
}
