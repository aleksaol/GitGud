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


    [SerializeField]
    private GameObject cam1;
    [SerializeField]
    private GameObject cam2;
    [SerializeField]
    private Room currentRoom;
    [SerializeField]
    private GameObject workingDirectory;
    [SerializeField]
    private List<GameObject> bookPrefabs;

    private InputManager inputManager;
    private Book heldBook;
    private Camera cam;

    private bool advancedGit = false;
    private bool gitView;

    private int points;
    private int tryPoints;
    private int pointsToNextLevel;
    private int currentLevel;


    public Book HeldBook { get => heldBook; set => heldBook = value; }
    public Room CurrentRoom { get => currentRoom; set => currentRoom = value; }
    public bool AdvancedGit { get => advancedGit; set => advancedGit = value; }
    public bool GitView { get => gitView; set => gitView = value; }
    public int Points { get => points; set => points = value; }
    public int TryPoints { get => tryPoints; set => tryPoints = value; }
    public int PointsToNextLevel { get => pointsToNextLevel; set => pointsToNextLevel = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public List<GameObject> BookPrefabs { get => bookPrefabs; set => bookPrefabs = value; }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        points = 0;
        currentLevel = 0;
        PointsToNextLevel = 5;
        gitView = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        transform.parent = currentRoom.PlayerPos;
        cam = Camera.main;
        currentRoom.Library.SpawnBooks();
        OnCommit();
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

        if (heldBook != null) {
            if (heldBook.State == BookState.HELD) {

                Ray ray = Camera.main.ScreenPointToRay(inputManager.GetMouseScreenPosition());

                heldBook.transform.position = ray.origin + (ray.direction * 0.5f);
                heldBook.transform.rotation = Quaternion.LookRotation(-cam.transform.right, cam.transform.up);
            }
        }
    }

    public bool TryPickUpBook(Book _book) {
        if (heldBook != null && heldBook != _book) {
            return false;
        }

        heldBook = _book;
        return true;
    }

    public void AddPoints(int _amount) {
        points += _amount;
        if (points >= pointsToNextLevel) {
            NextLevel();
        }
        tryPoints = points;
    }

    public void AddCalculatedPoints(int _amount) {
        tryPoints += _amount;
    }

    public void NextLevel() {
        
        do {
            currentLevel++;
            pointsToNextLevel = (5 * currentLevel) + 5;
        } while (points >= pointsToNextLevel);
        
    }

    public void ChangeCommit(Transform _room, Library _lib) {
        currentRoom.Library = _lib;
        transform.parent = _room;
        transform.localPosition = Vector3.zero;
    }

    public void OnCommit() {
        GameObject temp = Instantiate(transform.parent.parent.gameObject, transform.parent.parent.position, Quaternion.identity);
        temp.transform.parent = transform.parent.parent.parent;

        temp.GetComponent<Room>().Init(currentRoom.Commit);

        transform.parent.parent.position += (Vector3.left * 30.0f);
    }

    /*
     * Functions called by event triggers.
     */
    public void SwitchCamera() {
        if (gitView) {
            gitView = false;
            cam2.SetActive(false);
            cam1.SetActive(true);

        } else {
            gitView = true;
            cam1.SetActive(false);
            cam2.SetActive(true);
            
        }
    }
}
