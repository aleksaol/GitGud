using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommitNode : MonoBehaviour
{
    private const string OBJ_NAME = "Commit #";
    

    private CommitNode parent1;
    private CommitNode parent2;
    private Commit data;

    public Commit Data { get => data; set => data = value; }


    public void Init(Commit _data, CommitNode _parent1, CommitNode _parent2 = null) {
        data = _data;
        parent1 = _parent1;
        parent2 = _parent2;

        if (data != null) {
            gameObject.name = OBJ_NAME + data.Id.Code;
        }
    }

    private void OrientLine() {
        

        
    }
}
