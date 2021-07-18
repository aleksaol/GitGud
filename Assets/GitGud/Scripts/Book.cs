using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    
    [SerializeField]
    protected BookType type;
    [SerializeField]
    protected int sizeX;
    [SerializeField]
    protected int sizeY;
    [SerializeField]
    protected int basePoints;
    [SerializeField]
    protected int bonusPoints;
    [SerializeField]
    protected string bonusText;
    [SerializeField]
    protected string curseText;


    private Camera cam;
    private GameObject visable;
    private GameObject transparent;
    private Collider boxCollider;

    private InputManager inputManager;
    private PlayerController player;

    private BookState state;

    public BookState State { get => state; set => state = value; }
    public BookType Type { get => type; set => type = value; }

    private void Awake() {
        state = BookState.INVENTORY;
        cam = Camera.main;
        visable = transform.GetChild(0).gameObject;
        transparent = transform.GetChild(1).gameObject;
        boxCollider = GetComponent<Collider>();
        
    }

    private void Start() {
        inputManager = InputManager.Instance;
        player = PlayerController.Instance;
    }

    // Update is called once per frame
    private void Update()
    {
        if (state == BookState.HELD) {
            
            Ray ray = Camera.main.ScreenPointToRay(inputManager.GetMouseScreenPosition());

            transform.position = ray.origin + (ray.direction * 0.5f);
            transform.rotation = Quaternion.LookRotation(-cam.transform.right, cam.transform.up);
        }
    }

    public virtual int BonusPoints() {
        return 0;
    }

    public virtual int CalculatePoint() {
        return basePoints + BonusPoints();
    }

    public void TryPickUp() {
        if (state == BookState.LOCKED || state == BookState.TRYPLACE) {
            return;
        }

        PickUp();
    }

    public void PickUp() {
        if (player.TryPickUpBook(this)) {

            if (state == BookState.PLACED) {
                transform.parent.GetComponent<ShelfPos>().RemoveBook();
                player.AddCalculatedPoints(-basePoints);
            }

            state = BookState.HELD;
            // Set back to inventory before picking up to reset size.
            PlaceBook(player.Inventory.gameObject);
            transform.parent = null;

            visable.SetActive(false);
            transparent.SetActive(true);
            boxCollider.enabled = false;
        }
    }

    public void PlaceBook(GameObject _parent) {
        transform.parent = _parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        if (state != BookState.TRYPLACE) {
            boxCollider.enabled = true;
        }

        if (state == BookState.PLACED) {
            player.AddCalculatedPoints(basePoints);
        }

        if (state == BookState.INVENTORY || state == BookState.LOCKED) {
            visable.SetActive(true);
            transparent.SetActive(false);
        }
    }


    /*
     * Functions called by event triggers
     */

    public void Enter() {
        if (state == BookState.INVENTORY && player.HeldBook == null) {
            transform.localPosition += Vector3.up * 0.1f;
        }
    }

    public void Exit() {
        if (state == BookState.INVENTORY && player.HeldBook == null) {
            transform.localPosition -= Vector3.up * 0.1f;
        }
    }

}
