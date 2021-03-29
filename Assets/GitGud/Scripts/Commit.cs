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
    private Dictionary<GameObject, List<PickUp>> status;

    public Commit Parent { get => parent; set => parent = value; }
    public Commit SecondParent { get => secondParent; set => secondParent = value; }
    public string Message { get => message; set => message = value; }
    public string Tag { get => tag; set => tag = value; }
    public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
    public UniqueID Id { get => id; set => id = value; }
    public Dictionary<GameObject, List<PickUp>> Status { get => status; set => status = value; }

    public Commit() { }
    public Commit(string _msg) { message = _msg; }

    public void Init(Commit _parent, Commit _secondParent = null) {
        id = new UniqueID();
        id.GenerateCode();
        Debug.Log(id.Code);
        timeStamp = DateTime.Now;
        status = new Dictionary<GameObject, List<PickUp>>();

        parent = _parent;
        secondParent = _secondParent;

    }

    public void SaveStatus(Dictionary<GameObject, List<PickUp>> _status = null) {
        if (_status == null) {
            status = new Dictionary<GameObject, List<PickUp>>();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Container")) {
                List<PickUp> temp = new List<PickUp>();
                temp.AddRange(obj.GetComponent<Container>().PickUps);
                status.Add(obj, temp);
            }
        } else {
            status = _status;
        }
    }

}
