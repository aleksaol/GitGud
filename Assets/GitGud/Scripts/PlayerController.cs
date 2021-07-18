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

    private bool advancedGit = false;

    private int points;
    private int tryPoints;
    private int pointsToNextLevel;
    private int currentLevel;


    public Book HeldBook { get => heldBook; set => heldBook = value; }
    public Inventory Inventory { get => inventory; set => inventory = value; }
    public int Points { get => points; set => points = value; }
    public int TryPoints { get => tryPoints; set => tryPoints = value; }
    public int PointsToNextLevel { get => pointsToNextLevel; set => pointsToNextLevel = value; }
    public bool AdvancedGit { get => advancedGit; set => advancedGit = value; }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        points = 0;
        currentLevel = 0;
        PointsToNextLevel = 20;

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
            points -= pointsToNextLevel;
            pointsToNextLevel += (5 * currentLevel) + 10;
        } while (points >= pointsToNextLevel);
        
    }
}
