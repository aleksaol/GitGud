using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commit {

    private Commit parent;
    private Commit secondParent;
    private string message;
    private string tag;

    private DateTime timeStamp;
    private UniqueID id;
    private List<Container> containers;

    public Commit Parent { get => parent; set => parent = value; }
    public Commit SecondParent { get => secondParent; set => secondParent = value; }
    public string Message { get => message; set => message = value; }
    public string Tag { get => tag; set => tag = value; }
    public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
    public UniqueID Id { get => id; set => id = value; }
    public List<Container> Containers { get => containers; set => containers = value; }

    public Commit() { }
    public Commit(string _msg) { message = _msg; }

    public void Init(Commit _parent, Commit _secondParent = null) {
        id = new UniqueID();
        id.GenerateCode();
        Debug.Log(id.Code);
        timeStamp = DateTime.Now;
        containers = new List<Container>();

        parent = _parent;
        secondParent = _secondParent;
    }

    private void StoreChanges(Transform _interactable) {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Container")) {
            Container temp = new Container();
            temp = obj.GetComponent<Container>();
            containers.Add(temp);
        }
    }
}
