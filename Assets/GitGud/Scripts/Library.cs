using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : MonoBehaviour
{
    private static Library _instance;

    public static Library Instance {
        get {
            return _instance;
        }
    }


    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }


    public void CalculatePoints() {
        foreach (Transform bookshelfTransform in transform) {
            Bookshelf bookshelf = bookshelfTransform.GetComponent<Bookshelf>();
            if (bookshelf != null) {
                bookshelf.LockPositions();
            }
        }
    }
}
