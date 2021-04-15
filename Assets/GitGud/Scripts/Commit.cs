using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;
using UnityEditor;

[Serializable]
[DataboxTypeAttribute(Name = "Commit Class")]
public class Commit : DataboxType {

    private Commit parent;
    private Commit secondParent;
    [SerializeField]
    private string message;
    [SerializeField]
    private string tag;

    [SerializeField]
    private DateTime timeStamp;
    [SerializeField]
    private UniqueID id;
    [SerializeField]
    private Dictionary<string, List<string>> state = new Dictionary<string, List<string>>();

    [SerializeField]
    private string parentOneID = "";
    [SerializeField]
    private string parentTwoID = "";

    GameObject container = null;
    List<GameObject> pickUps = new List<GameObject>();

    public Commit Parent { get => parent; set => parent = value; }
    public Commit SecondParent { get => secondParent; set => secondParent = value; }
    public string Message { get => message; set => message = value; }
    public string Tag { get => tag; set => tag = value; }
    public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
    public UniqueID Id { get => id; set => id = value; }
    public Dictionary<string, List<string>> State { get => state; set => state = value; }
    public string ParentOneID { get => parentOneID; set => parentOneID = value; }
    public string ParentTwoID { get => parentTwoID; set => parentTwoID = value; }

    public Commit() { }
    public Commit(string _msg) { message = _msg; }

    public void Init(Commit _parent, Commit _secondParent = null, string _id = null, Dictionary<string, List<string>> _status = null) {
        id = new UniqueID();
        if (string.IsNullOrEmpty(_id)) {
            id.GenerateCode();
        } else {
            id.GenerateCode(_id);
        }
        
        timeStamp = DateTime.Now;

        parent = _parent;
        if (parent != null) {
            parentOneID = parent.id.Code;
        }
        
        secondParent = _secondParent;
        if (secondParent != null) {
            parentTwoID = secondParent.id.Code;
        }

        SaveStatus(_status);

    }

    public void SaveStatus(Dictionary<string, List<string>> _status = null) {
        if (_status == null) {
            state = new Dictionary<string, List<string>>();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Container")) {
                List<string> temp = new List<string>();
                foreach (GameObject _pickUp in obj.GetComponent<Container>().PickUps) {
                    temp.Add(_pickUp.name.ToString());
                }
                state.Add(obj.name.ToString(), temp);
            }
        } else {
            Dictionary<string, List<string>> temp = new Dictionary<string, List<string>>(_status);
            state = temp;
        }
    }
    
    public override void DrawEditor() {
        using (new GUILayout.VerticalScope("Box")) {
            GUILayout.Label("Parent 1 ID");
            parentOneID = GUILayout.TextField(parentOneID);

            GUILayout.Label("Parent 2 ID");
            parentTwoID = GUILayout.TextField(parentTwoID);

            GUILayout.Label("Message");
            message = GUILayout.TextField(message);

            GUILayout.Label("Tag");
            tag = GUILayout.TextField(tag);

            container = (GameObject) EditorGUILayout.ObjectField("Container: ", container, typeof(GameObject), true);

            for (int i = 0; i < pickUps.Count; i++) {
                pickUps[i] = (GameObject) EditorGUILayout.ObjectField("PickUp: ", pickUps[i], typeof(GameObject), true);
            }

            using (new GUILayout.HorizontalScope()) {
                if (GUILayout.Button("add pickup")) {
                    pickUps.Add(null);
                }

                if (pickUps.Count > 0) {
                    if (GUILayout.Button("Remove pickup")) {
                        pickUps.RemoveAt(pickUps.Count - 1);
                    }
                }
            }
            

            if (GUILayout.Button("add entry")) {
                List<string> newList = new List<string>();
                foreach (GameObject _pickUp in pickUps) {
                    newList.Add(_pickUp.name.ToString());
                }
                state.Add(container.name.ToString(), newList);
                container = null;
                pickUps = new List<GameObject>();
            }

            foreach (var key in state.Keys) {
                using (new GUILayout.HorizontalScope()) {
                    GUILayout.Label("Key:");
                    GUILayout.Label(key);
                    GUILayout.Label("Values:");
                    foreach (string _pickUp in state[key]) {
                        GUILayout.Label(_pickUp);
                    }
                    

                    if (GUILayout.Button("-", GUILayout.Width(20))) {
                        state.Remove(key);
                    }
                }
            }
        }
    }
}
