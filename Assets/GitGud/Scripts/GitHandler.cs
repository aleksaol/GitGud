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
    [SerializeField]
    private GitTreeHandler gitTree;

    private List<Branch> branches;
    private Branch currentBranch;
    private Commit currentCommit;

    private string levelTable;

    public List<Branch> Branches { get => branches; set => branches = value; }
    public Branch CurrentBranch { get => currentBranch; set => currentBranch = value; }
    public Commit CurrentCommit { get => currentCommit; set => currentCommit = value; }


    private void Awake() {

        branches = new List<Branch>();
        levelTable = SceneManager.GetActiveScene().name;

        StartCoroutine(LoadDatabaseRoutine());

    }

    private IEnumerator LoadDatabaseRoutine() {
        while (!databaseHandler.LevelDatabaseLoaded) {
            yield return null;
        }

        string main = "Main";

        OrderedDictionary<string, DataboxObject.DatabaseEntry> data = databaseHandler.LoadLevel(levelTable);

        foreach (string entryBranch in data.Keys) {
            databaseHandler.LevelsDB.RegisterToDatabase(databaseHandler.RuntimeDB, levelTable, entryBranch, entryBranch);

            if (CheckBranchName(entryBranch)) {
                Branch tempBranch = new Branch(entryBranch);
                branches.Add(tempBranch);

                Dictionary<string, Dictionary<Type, DataboxType>> valueCommits = databaseHandler.RuntimeDB.GetValuesFromEntry(levelTable, entryBranch);

                foreach (string valueCommit in valueCommits.Keys) {
                    Commit tempCommit = databaseHandler.RuntimeDB.GetData<Commit>(levelTable, entryBranch, valueCommit);
                    Commit testCommit = FindCommit(valueCommit);

                    if (testCommit == null) {
                        Commit newCommit = new Commit(tempCommit.Message);
                        newCommit.Init(FindCommit(tempCommit.ParentOneID), FindCommit(tempCommit.ParentTwoID), valueCommit, tempCommit.State);
                        newCommit.Tag = tempCommit.Tag;
                        tempBranch.Commits.Add(newCommit);
                    } else {
                        tempBranch.Commits.Add(testCommit);
                    }
                }
            } else {
                Debug.LogError("ERROR! Branch alreday exists!");
            }
        }

        Checkout(main, true);
        gitTree.InitTree();

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
            string feedback = UNKNOWN_GIT + HELP;
            string arg;

            if (cmds.Length < 3) {
                return "No name or argument given";
            }

            switch (cmds[1]) {
                case "commit":
                    if (!cmds[2].Equals("-m")) {
                        return "Unkown argument provided. Try -M for message";
                    } else {
                        string msg = cmd.Substring(14);

                        msg = msg.TrimEnd(' ');

                        if (string.IsNullOrWhiteSpace(msg)) {
                            return "No commit message.";
                        } else {
                            return Commit(msg);
                        }
                    }
                case "fetch":
                    break;
                case "push":
                    break;
                case "pull":
                    break;
                case "branch":
                    arg = cmd.Substring(11);

                    if (CheckBranchName(arg)) {
                        NewBranch(arg);
                        return Checkout(arg, true);
                    } else {
                        return "Branch " + arg + " already exists.";
                    }
                case "checkout":
                    if (cmds[2].Equals("-b")) {
                        arg = cmd.Substring(16);

                        if (CheckBranchName(arg)) {
                            NewBranch(arg);
                            return Checkout(arg, true);
                        } else {
                            return "Branch " + arg + " already exists.";
                        }
                    } else {
                        string checkout = cmd.Substring(13);

                        if (FindBranch(checkout) != null) {
                            return Checkout(checkout, true);
                        } else if (FindCommit(checkout) != null) {
                            return Checkout(checkout, false);
                        } else {
                            return "No branch or commit found";
                        }
                    }
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
                    return UNKNOWN_GIT + HELP;
            }

            return feedback;
        }
    }

    public string Commit(string _msg) {

        if (_msg.Length <= 0) {
            // Open window to enter message
            Debug.Log("NO COMMIT MESSAGE");
            return "No commit message provided";
        } else if (currentBranch == null) {
            return "Not on any branches";
        } else {
            Commit temp = new Commit(_msg);
            temp.Init(currentCommit);
            currentCommit = temp;
            currentBranch.Commits.Add(currentCommit);
            databaseHandler.SaveToRuntime(levelTable, currentBranch.Name, currentCommit);
            gitTree.CreateCommitNode(currentBranch.Name, currentCommit);
            return "Commit created";
        }
    }

    public void Fetch() { }
    public void Push() { }
    public void Pull() { }
    public string NewBranch(string _branch) {
        string feedback = "Branch " + _branch;

        if (CheckBranchName(_branch)) {
            Branch temp = new Branch(_branch);
            temp.Commits.Add(currentCommit);
            branches.Add(temp);
            databaseHandler.SaveToRuntime(levelTable, temp.Name, currentCommit);
            gitTree.CreateBranchNode(_branch, currentCommit);
            return feedback + " created.";
        } else {
            return feedback + " already exists.";
        }
    }
    public string Checkout(string _ID, bool _isBranch) {
        
        
        if (_isBranch) {
            Branch branch = FindBranch(_ID);
            if (branch != null) {
                currentBranch = branch;
                currentCommit = currentBranch.Commits[currentBranch.Commits.Count - 1];
                LoadCurrentCommit();
                return "Checked out branch: " + currentBranch.Name;
            }
        } else {
            Commit commit = FindCommit(_ID);

            if (commit != null) {
                currentBranch = null;
                currentCommit = commit;
                LoadCurrentCommit();
                return "Checked out commit: " + currentCommit.Id.Code + ". Warning! Detached HEAD mode.";
            }
        }

        if (_isBranch) {
            return "No branch with name " + _ID + " was found.";
        } else {
            return "No commit with ID " + _ID + " was found.";
        }
        
    }
    public void Merge() { }
    public void Rebase() { }
    public void ResetCmd() { }
    public void Revert() { }
    public void Tag() { }



    /*
     *  HELPER FUNCTIONS
     */
    public int GetIndexOfBranch(string _branch) {
        return branches.IndexOf(FindBranch(_branch));
    }

    public int GetIndexOfCommit(Commit _commit, string _branch) {
        return FindBranch(_branch).Commits.IndexOf(_commit);
    }

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
                    temp.GetComponent<PickUp>().ThisContainer = _obj.GetComponent<Container>();
                    _obj.GetComponent<Container>().PickUps.Add(temp);
                }

                _obj.GetComponent<Container>().PositionPickUps();
            } else {
                Debug.LogError("Object not found in dictionary");
            }
        }
    }
}
