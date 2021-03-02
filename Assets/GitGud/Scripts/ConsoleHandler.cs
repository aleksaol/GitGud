using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleHandler : MonoBehaviour
{

    [SerializeField]
    private GameObject commandTextPrefab;
    [SerializeField]
    private GameObject feedbackTextPrefab;
    
    [SerializeField]
    private RectTransform scrollContainer;
    [SerializeField]
    private GitHandler gitHandler;
    [SerializeField]
    private InputField commandLine;

    private Animator animator;

    private bool consoleIsOpen;

    private void Awake() {
        consoleIsOpen = false;

        animator = GetComponent<Animator>();
        if(!animator) {
            Debug.LogError("Animator is empty");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void cmdInput(InputField _cmdLine) {

        if (!consoleIsOpen) {
            return;
        }

        string cmd = _cmdLine.text.Trim(' ');
        string feedback = gitHandler.GitCommand(cmd);
        

        _cmdLine.text = "";
        commandLine.Select();
        commandLine.ActivateInputField();

        if (string.IsNullOrEmpty(cmd)) {
            return;
        }

        GameObject tmp = Instantiate(commandTextPrefab, scrollContainer);
        tmp.GetComponent<Text>().text = cmd;

        if (!string.IsNullOrEmpty(feedback)) {
            tmp = Instantiate(feedbackTextPrefab, scrollContainer);
            tmp.GetComponent<Text>().text = feedback;
        }
    }

    public void ToggleConsole() {
        if (consoleIsOpen) {
            // Close console
            consoleIsOpen = false;
            commandLine.text = "";
            commandLine.DeactivateInputField();
            animator.SetBool("ConsoleState", false);
        } else {
            // Open console
            consoleIsOpen = true;
            animator.SetBool("ConsoleState", true);
            commandLine.ActivateInputField();
            commandLine.Select();
        }
    }
}
