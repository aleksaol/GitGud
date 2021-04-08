using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Branch {

    private List<Commit> commits;
    private string name;


    public List<Commit> Commits { get => commits; set => commits = value; }
    public string Name { get => name; set => name = value; }


    public Branch() { commits = new List<Commit>(); }
    public Branch(string _name) { name = _name; commits = new List<Commit>(); }
    public Branch(string _name, List<Commit> _commits) { name = _name; commits = _commits; }


    public Commit FindCommit(string _ID) {
        foreach (Commit commit in commits) {
            if (commit.Id.Code.ToLower().Equals(_ID.ToLower())) {
                return commit;
            }
        }

        return null;
    }
}
