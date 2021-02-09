using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Branch {

    [SerializeField]
    private Commit parent;
    [SerializeField]
    private Commit merger;
    [SerializeField]
    private List<Commit> commits;
    [SerializeField]
    private string name;


    public Commit Parent { get => parent; set => parent = value; }
    public Commit Merger { get => merger; set => merger = value; }
    public List<Commit> Commits { get => commits; set => commits = value; }
    public string Name { get => name; set => name = value; }


    public Branch() { Init(); }
    public Branch(string _name) { name = _name; Init(); }
    public Branch(string _name, Commit _parent) { name = _name; parent = _parent; Init(); }
    public Branch(string _name, Commit _parent, List<Commit> _commits) { name = _name; parent = _parent; commits = _commits; }


    public void Init() {
        commits = new List<Commit>();
    }
}
