using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommitNode : MonoBehaviour
{
    private const string OBJ_NAME = "Commit #";

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject line1;
    [SerializeField]
    private GameObject line2;
    [SerializeField]
    private Text text;

    private CommitNode parent1;
    private CommitNode parent2;
    private Commit data;

    public GameObject Panel { get => panel; set => panel = value; }
    public Commit Data { get => data; set => data = value; }


    public void Init(Commit _data, CommitNode _parent1, CommitNode _parent2 = null) {
        data = _data;
        parent1 = _parent1;
        parent2 = _parent2;

        OrientLine();

        if (data != null) {
            gameObject.name = OBJ_NAME + data.Id.Code;
            text.text = data.Id.GetCode();
        }
    }

    private void OrientLine() {
        

        if (parent1 == null) {
            line1.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        } else {
            float targetCommitX = parent1.transform.parent.localPosition.x;
            float targetCommitY = parent1.transform.parent.localPosition.y + parent1.transform.localPosition.y - GetComponent<RectTransform>().sizeDelta.y / 2;
            float thisCommitX = transform.parent.localPosition.x;
            float thisCommitY = transform.parent.localPosition.y + transform.localPosition.y;
            float angle = Mathf.Atan2(targetCommitY - thisCommitY, targetCommitX - thisCommitX) * Mathf.Rad2Deg;
            line1.transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);

            Vector2 length = new Vector2(targetCommitX - thisCommitX, targetCommitY - thisCommitY);
            line1.GetComponent<RectTransform>().sizeDelta = new Vector2(5.0f, length.magnitude);
        }

        if (parent2 == null) {
            line2.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        } else {

        }
    }
}
