﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfPos : MonoBehaviour
{
    [SerializeField]
    private int posNr;
    private Shelf shelf;

    private PlayerController player;

    public int PosNr { get => posNr; set => posNr = value; }
    public Shelf Shelf { get => shelf; set => shelf = value; }

    private void Start() {
        player = PlayerController.Instance;
        shelf = GetComponentInParent<Shelf>();
    }

    public void RemoveBook() {
        shelf.Bookshelf.RemoveBook(shelf.ShelfNr, posNr);
    }


    /*
     * Functions called by event triggers
     * */

    public void Enter() {
        if (player.HeldBook) {
            shelf.Bookshelf.TryPlaceBook(shelf.ShelfNr, posNr);
        }
    }
}
