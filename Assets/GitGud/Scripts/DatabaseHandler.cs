using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;
using Databox.Dictionary;

public class DatabaseHandler : MonoBehaviour
{

    [SerializeField]
    private DataboxObjectManager manager;

    private DataboxObject runtimeDB;
    private DataboxObject levelsDB;
    private bool levelDatabaseLoaded = false;

    public DataboxObject RuntimeDB { get => runtimeDB; set => runtimeDB = value; }
    public DataboxObject LevelsDB { get => levelsDB; set => levelsDB = value; }
    public bool LevelDatabaseLoaded { get => levelDatabaseLoaded; set => levelDatabaseLoaded = value; }
    

    void OnDisable() {
        if (levelsDB != null) {
            levelsDB.OnDatabaseLoaded -= OnDataReady;
        }
    }

    private void Awake() {
        runtimeDB = manager.GetDataboxObject("Runtime");
        levelsDB = manager.GetDataboxObject("Levels");

        if (levelsDB != null) {
            levelsDB.OnDatabaseLoaded += OnDataReady;
        }

        levelsDB.LoadDatabase();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public OrderedDictionary<string, DataboxObject.DatabaseEntry> LoadLevel(string _level) {
        return levelsDB.GetEntriesFromTable(_level);
    }

    private void OnDataReady() {
        levelDatabaseLoaded = true;
    }
}
