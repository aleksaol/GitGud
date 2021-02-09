using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Commit {

    [SerializeField]
    private Commit parent;
    [SerializeField]
    private string message;
    [SerializeField]
    private string tag;
    private DateTime timeStamp;

    private List<GameObject> oldPos;
    private List<GameObject> newPos;

    public Commit Parent { get => parent; set => parent = value; }
    public string Message { get => message; set => message = value; }
    public string Tag { get => tag; set => tag = value; }
    public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
    public List<GameObject> OldPos { get => oldPos; set => oldPos = value; }
    public List<GameObject> NewPos { get => newPos; set => newPos = value; }

    public Commit() { Init(); }
    public Commit(string _msg) { message = _msg; Init(); }
    public Commit(string _msg, Commit _cmt) { message = _msg; parent = _cmt; Init(); }

    private void Init() {
        oldPos = new List<GameObject>();
        newPos = new List<GameObject>();
    }

    private void FindChangedObjects() {

    }
}
