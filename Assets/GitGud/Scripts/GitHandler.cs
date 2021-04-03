using Databox;
using Databox.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GitHandler : MonoBehaviour {

    public const string NOT_GIT = "Not a Git command. ";
    public const string UNKNOWN_GIT = "Unknown Git command. ";
    public const string HELP = "Type 'Help' for help.";

    [SerializeField]
    private DatabaseHandler databaseHandler;

    private List<Branch> branches;
    private Branch currentBranch;
    private Commit currentCommit;

    public List<Branch> Branches { get => branches; set => branches = value; }
    public Branch CurrentBranch { get => currentBranch; set => currentBranch = value; }
    public Commit CurrentCommit { get => currentCommit; set => currentCommit = value; }


    private void Awake() {

        branches = new List<Branch>();

        StartCoroutine(LoadDatabaseRoutine());

        /*if (CheckBranchName(main)) {
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
            currentCommit.SaveStatus();
            currentBranch.Commits.Add(currentCommit);
        }

        LoadCurrentCommit();*/

    }

    private IEnumerator LoadDatabaseRoutine() {
        while (!databaseHandler.LevelDatabaseLoaded) {
            yield return null;
        }

        string main = "Main";
        string levelTable = SceneManager.GetActiveScene().name;

        OrderedDictionary<string, DataboxObject.DatabaseEntry> data = databaseHandler.LoadLevel(levelTable);

        foreach (string entryBranch in data.Keys) {
            databaseHandler.LevelsDB.RegisterToDatabase(databaseHandler.RuntimeDB, levelTable, entryBranch, entryBranch);

            if (CheckBranchName(entryBranch)) {
                Branch tempBranch = new Branch(entryBranch);
                tempBranch.Init(null);
                branches.Add(tempBranch);

                Dictionary<string, Dictionary<Type, DataboxType>> valueCommits = databaseHandler.RuntimeDB.GetValuesFromEntry(levelTable, entryBranch);

                foreach (string valueCommit in valueCommits.Keys) {
                    Commit tempCommit = databaseHandler.RuntimeDB.GetData<Commit>(levelTable, entryBranch, valueCommit);

                    Commit newCommit = new Commit(tempCommit.Message);
                    newCommit.Init(FindCommit(tempCommit.ParentOneID), FindCommit(tempCommit.ParentTwoID), valueCommit);
                    newCommit.Tag = tempCommit.Tag;
                    newCommit.SaveStatus(tempCommit.State);
                    tempBranch.Commits.Add(newCommit);
                }
            } else {
                Debug.LogError("ERROR! Branch alreday exists!");
            }
        }

        Checkout(main, true);

        Debug.Log("Current branch: " + currentBranch.Name);
        Debug.Log("Current commit: " + currentCommit.Id.Code);

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
        } else if (currentBranch == null) {
            Commit temp = new Commit(_msg);
            temp.Init(currentCommit);
            temp.SaveStatus();
            currentCommit = temp;
            currentBranch.Commits.Add(currentCommit);
        } else {
            Commit temp = new Commit(_msg);
            temp.Init(currentCommit);
            temp.SaveStatus();
            currentCommit = temp;
            currentBranch.Commits.Add(currentCommit);
        }
    }

    public void Fetch() { }
    public void Push() { }
    public void Pull() { }
    public void Branch() { }
    public void Checkout(string _ID, bool _isBranch) {
        Commit commitToCheckout = null;
        Branch branchToCheckout = null;
        bool found = false;
        
        if (_isBranch) {
            foreach (Branch branch in branches) {
                if (branch.Name.ToLower().Equals(_ID.ToLower())) {
                    branchToCheckout = branch;
                    found = true;
                    break;
                }
            }

            if (!found) {
                Debug.Log("NO BRANCH WITH THAT NAME FOUND");
                return;
            }
        } else {
            foreach (Branch branch in branches) {
                commitToCheckout = branch.FindCommit(_ID);

                if (commitToCheckout != null) {
                    found = true;
                    break;
                }
            }

            if (!found) {
                Debug.Log("NO COMMIT WITH THAT CODE FOUND");
                return;
            }
        }

        if (branchToCheckout != null) {
            currentBranch = branchToCheckout;
            currentCommit = currentBranch.Commits[currentBranch.Commits.Count - 1];
        } else if (commitToCheckout != null) {
            currentBranch = null;
            currentCommit = commitToCheckout;
        }

        LoadCurrentCommit();
        
        Debug.Log("Current commit ID: " + currentCommit.Id.Code);
        
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

    private Commit FindCommit(string _id) {
        Commit commit;

        foreach (Branch _branch in branches) {
            commit = _branch.FindCommit(_id);
            if (commit != null) {
                return commit;
            }
        }

        return null;
    }

    private void LoadCurrentCommit() {
        foreach (GameObject _obj in GameObject.FindGameObjectsWithTag("Container")) {
            List<string> tempList;
            
            if (currentCommit.State.TryGetValue(_obj.name.ToString(), out tempList)) {
                _obj.GetComponent<Container>().PickUps.Clear();
                
                foreach (string _objName in tempList) {
                    GameObject temp = GameObject.Find(_objName);
                    _obj.GetComponent<Container>().PickUps.Add(temp);
                }

                _obj.GetComponent<Container>().PositionPickUps();
            } else {
                Debug.LogError("Object not found in dictionary");
            }
        }
    }
}
