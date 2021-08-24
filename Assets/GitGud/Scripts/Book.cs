using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{

    /*
     * Perfect mate = 5
     * Good mate = 2
     * Bad mate = -2
     * 
     * Perfect position = 10
     * Bad position = cursed
     */
    
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

    private GameObject visable;
    private GameObject transparent;
    private Collider boxCollider;

    private PlayerController player;

    private BookState state;

    public BookState State { get => state; set => state = value; }
    public BookType Type { get => type; set => type = value; }

    private void Awake() {
        state = BookState.PLACED;
        visable = transform.GetChild(0).gameObject;
        transparent = transform.GetChild(1).gameObject;
        boxCollider = GetComponent<Collider>();
        
    }

    private void Start() {
        player = PlayerController.Instance;
    }


    public virtual int BonusPoints() {
        return 0;
    }

    public virtual int CalculatePoint() {
        int neighbours;



        return basePoints + BonusPoints();
    }

    public void PickUp() {
        if (player.TryPickUpBook(this)) {

            if (state == BookState.PLACED) {
                transform.parent.GetComponent<ShelfPos>().RemoveBook();
            }

            state = BookState.HELD;
            // Set back to inventory before picking up to reset size.
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

        if (state != BookState.TRY_PLACE) {
            boxCollider.enabled = true;
        }

        if (state == BookState.PLACED) {
            visable.SetActive(true);
            transparent.SetActive(false);
        }
    }

    private int GetNeighbours() {
        return 0;
    }


    /*
     * Functions called by event triggers
     */

    public void Enter() {
        if (state == BookState.PLACED && player.HeldBook == null) {
            transform.localPosition += Vector3.forward * 0.1f;
        }
    }

    public void Exit() {
        if (state == BookState.PLACED && player.HeldBook == null) {
            transform.localPosition -= Vector3.forward * 0.1f;
        }
    }

}
