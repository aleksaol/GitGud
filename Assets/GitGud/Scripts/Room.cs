using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private const string OBJ_NAME = "Commit #";

    [SerializeField]
    private Transform playerPos;

    private PlayerController player;

    private Library library;
    private Commit commit;

    private Collider boxCollider;

    public Library Library { get => library; set => library = value; }
    public Commit Commit { get => commit; set => commit = value; }
    public Transform PlayerPos { get => playerPos; set => playerPos = value; }

    private void Awake() {
        boxCollider = GetComponent<Collider>();
        if (!boxCollider) {
            Debug.LogError("COLLIDER is empty in INVENTORY");
        } else {
            boxCollider.enabled = false;
        }

        
        library = GetComponentInChildren<Library>();
        
    }

    private void Start() {
        player = PlayerController.Instance;
    }

    public void Init(Commit _parent) {
        commit = new Commit();
        commit.Init(_parent);
        gameObject.name = OBJ_NAME + commit.Id.Code;
    }

    /*
     * Functions called by event triggers
     */

    public void Enter() {
        
    }

    public void Exit() {
        
    }

    public void OnClick() {
        player.ChangeCommit(playerPos, library);
    }
}
