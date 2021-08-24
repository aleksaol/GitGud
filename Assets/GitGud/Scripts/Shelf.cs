using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{

    [SerializeField]
    private int shelfNr;
    private Bookshelf bookshelf;

    public int ShelfNr { get => shelfNr; set => shelfNr = value; }
    public Bookshelf Bookshelf { get => bookshelf; set => bookshelf = value; }


    private void Start() {
        bookshelf = GetComponentInParent<Bookshelf>();
    }

}
