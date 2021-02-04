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


    public Branch() { commits = new List<Commit>(); }
    public Branch(string _name) { commits = new List<Commit>(); name = _name; }
    public Branch(string _name, Commit _parent) { commits = new List<Commit>(); name = _name; parent = _parent; }
    public Branch(string _name, Commit _parent, List<Commit> _commits) {commits = new List<Commit>(); name = _name; parent = _parent; commits = _commits; }
}
