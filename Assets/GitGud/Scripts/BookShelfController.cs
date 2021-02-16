using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelfController : MonoBehaviour
{
    
    private const float SHELF_1_Y = 1.97f;
    private const float SHELF_2_Y = 1.47f;
    private const float SHELF_3_Y = 0.97f;
    private const float SHELF_4_Y = 0.47f;

    [SerializeField]
    private BookShelf shelf1;
    [SerializeField]
    private BookShelf shelf2;
    [SerializeField]
    private BookShelf shelf3;
    [SerializeField]
    private BookShelf shelf4;

    public BookShelf Shelf1 { get => shelf1; set => shelf1 = value; }
    public BookShelf Shelf2 { get => shelf2; set => shelf2 = value; }
    public BookShelf Shelf3 { get => shelf3; set => shelf3 = value; }
    public BookShelf Shelf4 { get => shelf4; set => shelf4 = value; }

    private void Awake() {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
