using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Branch {

    private Commit parent;
    private Commit merger;
    private List<Commit> commits;
    private string name;


    public Commit Parent { get => parent; set => parent = value; }
    public Commit Merger { get => merger; set => merger = value; }
    public List<Commit> Commits { get => commits; set => commits = value; }
    public string Name { get => name; set => name = value; }


    public Branch() { }
    public Branch(string _name) { name = _name; }
    public Branch(string _name, Commit _parent) { name = _name; parent = _parent; }
    public Branch(string _name, Commit _parent, List<Commit> _commits) { name = _name; parent = _parent; commits = _commits; }


    public void Init(Commit _parent) {
        commits = new List<Commit>();
        parent = _parent;
    }

    public Commit FindCommit(string _ID) {
        foreach (Commit commit in commits) {
            if (commit.Id.Code.ToLower().Equals(_ID.ToLower())) {
                return commit;
            }
        }

        return null;
    }
}
