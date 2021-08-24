using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : MonoBehaviour
{
    

    public void SpawnBooks(int _amount = 0) {
        int numNewBooks = _amount;
        if (numNewBooks <= 0) {
            numNewBooks = 5;
        }

        for (int i = 0; i < numNewBooks; i++) {
            int bookshelf = Random.Range(0, transform.childCount);
            transform.GetChild(bookshelf).GetComponent<Bookshelf>().SpawnBook();
        }
    }

    public int CalculatePoints() {
        int sum = 0;

        foreach (Transform bookshelfTransform in transform) {
            Bookshelf bookshelf = bookshelfTransform.GetComponent<Bookshelf>();
            if (bookshelf != null) {
                sum += bookshelf.CalculatePoints();
            }
        }

        return sum;
    }

    public bool CheckForChanges() {

        foreach (Transform bookshelfTransform in transform) {
            Bookshelf bookshelf = bookshelfTransform.GetComponent<Bookshelf>();
            if (bookshelf != null) {
                if (bookshelf.CheckForChange()) {
                    return true;
                }
            }
        }

        return false;
    }
}
