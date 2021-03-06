﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GitHandler : MonoBehaviour {

    public const string NOT_GIT = "Not a Git command. ";
    public const string UNKNOWN_GIT = "Unknown Git command. ";
    public const string HELP = "Type 'Help' for help.";

    private List<Branch> branches;
    private Branch currentBranch;
    private Commit currentCommit;

    public List<Branch> Branches { get => branches; set => branches = value; }
    public Branch CurrentBranch { get => currentBranch; set => currentBranch = value; }
    public Commit CurrentCommit { get => currentCommit; set => currentCommit = value; }


    private void Awake() {
        branches = new List<Branch>();
        string main = "Main";

        if (CheckBranchName(main)) {
            Branch temp = new Branch(main);
            temp.Init(null);
            branches.Add(temp);
            currentBranch = FindBranch(main);
        }

        
        if (currentBranch == null) {
            Debug.LogError("NO MAIN BRANCH CREATED");
        } else {
            currentCommit = new Commit("Initial Commit");
            currentCommit.Init(null);
            currentBranch.Commits.Add(currentCommit);
        }

    }

    public string GitCommand(string command) {
        string cmd = command.ToLower().Trim(' ');

        if (string.IsNullOrEmpty(cmd)) {
            return null;
        }
        

        if (cmd.Length < 4) {
            // No command
            Debug.Log("NOT A GIT COMMAND");
            return NOT_GIT + HELP;
        }

        if (cmd.Substring(0, 4) == "help") {
            // Help command
            Debug.Log("HELP");
            return "";
        }

        string[] cmds = cmd.Split(' ');

        if (cmds[0] != "git" || cmds.Length < 2) {
            // Not a git command
            Debug.Log("NOT A GIT COMMAND");
            return UNKNOWN_GIT + HELP;
        } else {
            string feedback = "";

            switch (cmds[1]) {
                case "commit":
                    if (cmds.Length < 3) {
                        // No commit message recieved
                        // Open window to enter message
                        feedback = "No commit message.";
                    } else {
                        string msg = cmd.Substring(11).TrimStart(' ');
                        if (string.IsNullOrWhiteSpace(msg)) {
                            feedback = "No commit message.";
                            break;
                        } else {
                            Commit(msg);
                        }
                    }
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
                    feedback = UNKNOWN_GIT + HELP;
                    break;
            }

            return feedback;
        }
    }

    public void Commit(string _msg) {

        if (_msg.Length <= 0) {
            // Open window to enter message
            Debug.Log("NO COMMIT MESSAGE");
        } else {
            Commit temp = new Commit(_msg);
            temp.Init(currentCommit);
            currentCommit = temp;
            currentBranch.Commits.Add(currentCommit);
        }
    }

    public void Fetch() { }
    public void Push() { }
    public void Pull() { }
    public void Branch() { }
    public void Checkout(string _ID, bool _isBranch) {
        Commit commitToCheckout;
        bool found = false;
        
        if (_isBranch) {
            foreach (Branch branch in branches) {
                if (branch.Name.ToLower().Equals(_ID.ToLower())) {
                    currentBranch = branch;
                    currentCommit = branch.Commits[branch.Commits.Count - 1];
                    found = true;
                    break;
                }
            }

            if (!found) {
                Debug.Log("NO BRANCH WITH THAT NAME FOUND");
            }
        } else {
            foreach (Branch branch in branches) {
                commitToCheckout = branch.FindCommit(_ID);

                if (commitToCheckout != null) {
                    currentBranch = branch;
                    currentCommit = commitToCheckout;
                    found = true;
                    break;
                }
            }

            if (!found) {
                Debug.Log("NO COMMIT WITH THAT CODE FOUND");
            }
        }

        Debug.Log("Current branch and commit ID: " + currentBranch.Name + " - " + currentCommit.Id.Code);
        
    }
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
