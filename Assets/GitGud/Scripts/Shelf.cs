using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{

    [SerializeField]
    private int shelfNr;
    [SerializeField]
    private Bookshelf bookshelf;

    public int ShelfNr { get => shelfNr; set => shelfNr = value; }
    public Bookshelf Bookshelf { get => bookshelf; set => bookshelf = value; }

}
