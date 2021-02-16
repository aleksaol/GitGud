using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject commandTextPrefab;
    [SerializeField]
    private GameObject feedbackTextPrefab;

    [SerializeField]
    private RectTransform scrollContainer;
    [SerializeField]
    private GitHandler gitHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cmdInput(InputField _cmdLine) {
        string cmd = _cmdLine.text;
        string feedback = gitHandler.GitCommand(cmd);
        _cmdLine.text = "";

        GameObject tmp = Instantiate(commandTextPrefab, scrollContainer);
        tmp.GetComponent<Text>().text = cmd;

        if (feedback.Length > 0) {
            tmp = Instantiate(feedbackTextPrefab, scrollContainer);
            tmp.GetComponent<Text>().text = feedback;
        }
    }
}
