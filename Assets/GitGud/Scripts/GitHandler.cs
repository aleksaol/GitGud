using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitHandler : MonoBehaviour {
    [SerializeField]
    private List<Branch> branches;
    [SerializeField]
    private Branch currentBranch;
    [SerializeField]
    private Commit currentCommit;

    public List<Branch> Branches { get => branches; set => branches = value; }
    public Branch CurrentBranch { get => currentBranch; set => currentBranch = value; }
    public Commit CurrentCommit { get => currentCommit; set => currentCommit = value; }


    private void Awake() {
        branches = new List<Branch> {
            new Branch("Main")
        };

        currentBranch = FindBranch("Main");
        if(currentBranch == null) {
            Debug.LogError("NO MAIN BRANCH CREATED");
        } else {
            currentCommit = new Commit("Initial Commit");
            currentBranch.Commits.Add(currentCommit);
        }

    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void GitCommand(string command) {
        string cmd = command.ToLower();

        if (cmd.Length < 4) {
            // No command
        }

        if (cmd.Substring(0, 4) == "help") {
            // Help command
        }

        string[] cmds = cmd.Split(' ');

        if (cmds[0] != "git" || cmds.Length < 2) {
            // Not a git command
        } else {
            switch (cmds[1]) {
                case "commit":
                    break;
                case "fetch":
                    break;
                case "push":
                    break;
                case "pull":
                    break;
                case "branch":
                    break;
                case "checkout":
                    break;
                case "merge":
                    break;
                case "rebase":
                    break;
                case "reset":
                    break;
                case "revert":
                    break;
                case "tag":
                    break;
                default:
                    // UNKOWN COMMAND
                    break;
            }
        }
    }

    public void Commit(string _msg) {
        string msg;

        if (_msg.Length <= 0) {
            // Open window to enter message
        } else {
            msg = _msg;
        }

    }

    public void Fetch() { }
    public void Push() { }
    public void Pull() { }
    public void Branch() { }
    public void Checkout() { }
    public void Merge() { }
    public void Rebase() { }
    public void ResetCmd() { }
    public void Revert() { }
    public void Tag() { }



    /*
     *  HELPER FUNCTIONS
     */
    private bool CheckBranchName(string _name) {
        foreach (Branch branch in branches) {
            if (_name.ToLower() == branch.Name.ToLower()) {
                return false;
            }
        }

        return true;
    }

    private Branch FindBranch(string _name) {
        foreach (Branch branch in branches) {
            if (_name.ToLower() == branch.Name.ToLower()) {
                return branch;
            }
        }

        return null;
    }
}
