using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance;

    public static Inventory Instance {
        get {
            return _instance;
        }
    }

    private const float X_STEP = -0.2f;

    private Collider boxCollider;

    private PlayerController player;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        boxCollider = GetComponent<Collider>();
        if (!boxCollider) {
            Debug.LogError("COLLIDER is empty in INVENTORY");
        }
    }

    private void Start() {
        player = PlayerController.Instance;

        OrderBooks();
    }

    private void OrderBooks() {
        int pos = 0;

        foreach (Transform item in transform) {
            item.transform.localPosition = new Vector3(X_STEP * pos, 0.0f, 0.0f);
            pos++;
        }
    }

    public void TryPlaceBook() {
        if (!player.HeldBook) {
            return;
        }

        Book book = player.HeldBook;
        player.HeldBook = null;
        boxCollider.enabled = false;

        book.State = BookState.INVENTORY;
        book.PlaceBook(gameObject);
        OrderBooks();
    }

    public void SetCollider(bool _state) {
        boxCollider.enabled = _state;
    }
}
