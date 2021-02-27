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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
