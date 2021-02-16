using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour
{
    private const float X_START = 0.75f;
    private const float X_STEP = -0.14f;

    [SerializeField]
    private List<GameObject> books;


    public List<GameObject> Books { get => books; set => books = value; }

    private void Awake() {
        books = new List<GameObject>();
        foreach (Transform child in transform) {
            books.Add(child.gameObject);
        }
    }

}
