using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GitTreeHandler : MonoBehaviour
{
    private const float X_START = 5.0f;
    private const float X_STEP = 100.0f;
    private const float Y_START = -5.0f;
    private const float Y_STEP = -75.0f;

    [SerializeField]
    private GitHandler gitHandler;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private GameObject commitNode;
    [SerializeField]
    private GameObject branchNode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitTree() {

        for (int i = 0; i < gitHandler.Branches.Count; i++) {
            Branch branch = gitHandler.Branches[i];
            GameObject nodeBranch = CreateBranchNode(branch.Name, null);
            nodeBranch.transform.localPosition += new Vector3(X_STEP * i, 0.0f, 0.0f);

            for (int j = 0; j < branch.Commits.Count; j++) {
                Commit commit = branch.Commits[j];

                CreateCommitNode(branch.Name, commit);
            }
        }
    }

    public GameObject CreateBranchNode(string _bName, Commit _commit) {
        if (FindBranchNode(_bName) != null) {
            Debug.LogError("Branch node already exists");
            return null;
        } else {
            GameObject nodeBranch = Instantiate(branchNode, content.transform);
            nodeBranch.name = _bName;

            if (_commit == null) {
                // This should only apply when first initializing tree.
                nodeBranch.transform.localPosition = new Vector2(X_START, Y_START);
            } else {
                CommitNode nodeCommit = FindCommitNode(_commit.Id.Code);
                int xPos = gitHandler.GetIndexOfBranch(_bName);
                nodeBranch.transform.localPosition = new Vector2((xPos * X_STEP) + X_START, nodeCommit.transform.localPosition.y + Y_START);
            }

            return nodeBranch;
        }
    }

    public CommitNode CreateCommitNode(string _bName, Commit _commit) {
        GameObject nodeBranch = FindBranchNode(_bName);

        if (nodeBranch == null) {
            nodeBranch = CreateBranchNode(_bName, _commit);
        }

        CommitNode checkCommit = FindCommitNode(_commit.Id.Code);

        if (checkCommit != null) {
            return checkCommit;
        } else {
            GameObject nodeCommit = Instantiate(commitNode, nodeBranch.transform);
            int height = gitHandler.GetIndexOfCommit(_commit, _bName);
            nodeCommit.transform.localPosition = new Vector2(0.0f, Y_STEP * height);

            CommitNode parent1 = null;
            CommitNode parent2 = null;
            if (_commit.Parent != null) {
                parent1 = FindCommitNode(_commit.Parent.Id.Code);
            }
            if (_commit.SecondParent != null) {
                parent2 = FindCommitNode(_commit.SecondParent.Id.Code);
            }

            nodeCommit.GetComponent<CommitNode>().Init(_commit, parent1, parent2);
            return nodeCommit.GetComponent<CommitNode>();
        }
    }

    private GameObject FindBranchNode(string _name) {
        for (int i = 0; i < content.transform.childCount; i++) {
            if (content.transform.GetChild(i).name.ToLower() == _name.ToLower()) {
                return content.transform.GetChild(i).gameObject;
            }
        }

        return null;
    }

    private CommitNode FindCommitNode(string _id) {
        for (int i = 0; i < content.transform.childCount; i++) {
            Transform branch = content.transform.GetChild(i);
            for (int j = 0; j < branch.childCount; j++) {
                CommitNode commitNode = branch.GetChild(j).GetComponent<CommitNode>();
                if (commitNode.Data.Id.Code == _id) {
                    return commitNode;
                }
            }
        }

        return null;
    }
}
