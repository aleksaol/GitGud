﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : MonoBehaviour
{
    private const int NUM_SHELFS = 4;
    private const int NUM_POS = 6;

    private GameObject[,] positions;

    private PlayerController player;

    private void Start() {
        positions = new GameObject[NUM_SHELFS, NUM_POS];

        player = PlayerController.Instance;
    }

    public bool TryPlaceBook(int _shelf, int _pos) {
        if (IsPositionAvailable(_shelf, _pos)) {
            Book book = player.HeldBook;

            book.State = BookState.TRYPLACE;
            book.PlaceBook(transform.GetChild(_shelf).GetChild(_pos - 1).gameObject);

            return true;
        }

        return false;
    }

    public void Exit() {
        if (player.HeldBook) {
            Book book = player.HeldBook;

            book.PickUp();
        }
    }

    public void ConfirmPlacement() {
        if (!player.HeldBook) {
            return;
        }

        if (player.HeldBook.transform.parent == null) {
            return;
        }

        Book book = player.HeldBook;
        player.HeldBook = null;
        ShelfPos pos = book.transform.parent.GetComponent<ShelfPos>();
        positions[pos.Shelf.ShelfNr - 1, pos.PosNr - 1] = book.gameObject;

        book.State = BookState.PLACED;
        book.PlaceBook(book.transform.parent.gameObject);
    }

    public void RemoveBook (int _shelf, int _pos) {
        positions[_shelf - 1, _pos - 1] = null;
    }

    /*
     * NOTE: this assumes a square book.
     */
    private bool IsPositionAvailable(int _shelf, int _pos, int _xSize = 0, int _ySize = 0) {
        int xPos = _pos - 1;
        int yPos = _shelf - 1;

        if (positions[yPos, xPos] == null) {
            // If size is only one pos, return true.
            if (_xSize == 0 && _ySize == 0) {
                return true;
            }

            // If size is outside boundary, return false.
            if (xPos + _xSize >= NUM_POS || yPos + _ySize >= NUM_SHELFS) {
                return false;
            }

            for (int i = 0; i < _xSize; i++) {
                for (int j = 0; j < _ySize; j++) {
                    int newXPos = i + xPos;
                    int newYPos = j + yPos;

                    if (positions[newYPos, newXPos] != null) {
                        return false;
                    }
                }

            }

            return true;
        }
        
        return false;
    }


    /*
    private void InitShelfPos() {
        for (int i = 1; i < transform.childCount; i++) {
            for (int j = 0; j < transform.GetChild(i).childCount; j++) {
                positions[i, j] = transform.GetChild(i).GetChild(j).gameObject;
            }
        }
    }*/
}
