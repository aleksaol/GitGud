using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private const float RAYCAST_TIME = 0.001f;

    private float raycastTimer = 0.0f;
    private RaycastHit hit;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (raycastTimer >= RAYCAST_TIME) {

            // Create a vector at the center of our camera's viewport
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            Debug.DrawRay(transform.position, cam.transform.forward * 999999.0f, Color.red);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 999999.0f)) {
                Debug.DrawRay(transform.position, cam.transform.forward * hit.distance, Color.yellow);
                if (hit.transform.CompareTag("Interactable")) {
                    Debug.Log("Interactable");
                }
            }

            raycastTimer -= RAYCAST_TIME;
        }

        raycastTimer += Time.deltaTime;
        
    }
}
