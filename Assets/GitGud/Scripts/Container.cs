using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

public class Container : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> pickUps;
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


    public List<GameObject> PickUps { get => pickUps; set => pickUps = value; }


    public void RemovePickUp(GameObject _pickUp) {
        pickUps.Remove(_pickUp);
        PositionPickUps();
    }

    public void PlacePickUp(GameObject _pickUp) {
        pickUps.Add(_pickUp);
        PositionPickUps();
    }

    public void PositionPickUps() {
        for (int i = 0; i < pickUps.Count; i++) {
            pickUps[i].transform.parent = transform;

            float xPos = xStart + (i * xStep);
            float yPos = yStart + (i * yStep);
            float zPos = zStart + (i * zStep);
            pickUps[i].transform.localPosition = new Vector3(xPos, yPos, zPos);

            pickUps[i].transform.localRotation = Quaternion.Euler(placedRotation);
        }
    }
}
