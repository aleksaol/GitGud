using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;

    public static PlayerController Instance {
        get {
            return _instance;
        }
    }


    private InputManager inputManager;
    private Inventory inventory;

    private Book heldBook;


    public Book HeldBook { get => heldBook; set => heldBook = value; }
    public Inventory Inventory { get => inventory; set => inventory = value; }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        inventory = Inventory.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.PlayerExit()) {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    public bool TryPickUpBook(Book _book) {
        if (heldBook != null && heldBook != _book) {
            return false;
        }

        heldBook = _book;
        inventory.SetCollider(true);
        return true;
    }

    public bool TryPlaceBook() {
        return false;
    }
}
