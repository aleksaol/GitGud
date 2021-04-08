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

    private void OnApplicationQuit() {
        runtimeDB.ClearDatabase();
        runtimeDB.SaveDatabase();
    }

    public OrderedDictionary<string, DataboxObject.DatabaseEntry> LoadLevel(string _level) {
        return levelsDB.GetEntriesFromTable(_level);
    }

    public void SaveToRuntime(string _level, string _branch, Commit _commit) {
        runtimeDB.AddData(_level, _branch, _commit.Id.Code, _commit);
        runtimeDB.SaveDatabase();
    }

    private void OnDataReady() {
        levelDatabaseLoaded = true;
    }
}
