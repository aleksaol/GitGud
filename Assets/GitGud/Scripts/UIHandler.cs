using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private CharacterController character;
    [SerializeField]
    private GitHandler gitHandler;

    [SerializeField]
    private GameObject commitPopUp;
    [SerializeField]
    private InputField commitInputField;
    [SerializeField]
    private Button commitConfirmBtn;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckCommitMessage() {
        if (string.IsNullOrWhiteSpace(commitInputField.text)) {
            commitConfirmBtn.interactable = false;
        } else {
            commitConfirmBtn.interactable = true;
        }
    }

    public void Commit() {
        if (!string.IsNullOrWhiteSpace(commitInputField.text)) {
            gitHandler.Commit(commitInputField.text);
            ToggleCommitMessage(false);
        }
    }

    public void ToggleCommitMessage(bool _open) {

        if (_open) {
            if (commitPopUp.activeSelf) {
                return;
            }
            character.ToggleCursorMode(false);
            commitPopUp.SetActive(true);
            commitInputField.text = "";
            commitInputField.ActivateInputField();
            commitInputField.Select();
        } else {
            character.ToggleCursorMode(true);
            commitPopUp.SetActive(false);
            commitInputField.text = "";
            commitInputField.DeactivateInputField();
        }
    }
    
}
