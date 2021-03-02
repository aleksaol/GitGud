using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    private List<PickUp> pickUps;
    [SerializeField]
    private int maxPickUps;

    [SerializeField]
    private float xStart;
    [SerializeField]
    private float xStep;
    [SerializeField]
    private float yStart;
    [SerializeField]
    private float yStep;
    [SerializeField]
    private float zStart;
    [SerializeField]
    private float zStep;
    [SerializeField]
    private Vector3 placedRotation;

    // Start is called before the first frame update
    void Start()
    {
        PositionPickUps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemovePickUp(PickUp _pickUp) {
        pickUps.Remove(_pickUp);
        PositionPickUps();
    }

    public void PlacePickUp(PickUp _pickUp) {
        pickUps.Add(_pickUp);
        PositionPickUps();
    }

    private void PositionPickUps() {
        for (int i = 0; i < pickUps.Count; i++) {
            float xPos = xStart + (i * xStep);
            float yPos = yStart + (i * yStep);
            float zPos = zStart + (i * zStep);
            pickUps[i].transform.localPosition = new Vector3(xPos, yPos, zPos);

            pickUps[i].transform.localRotation = Quaternion.Euler(placedRotation);
        }
    }
}
