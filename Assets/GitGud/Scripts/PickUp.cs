using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    [SerializeField]
    private Container container;

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
            transform.position = cam.transform.position + (cam.transform.forward * 2.0f);
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
        container.PlacePickUp(this);
    }
}
