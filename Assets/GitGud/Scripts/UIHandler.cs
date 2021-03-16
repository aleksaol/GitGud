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
    private GameObject checkoutPopUp;


    private InputField activeInputField;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveInputField(InputField _inputField) {
        activeInputField = _inputField;
    }

    public void CheckForEmptyMessage(Button _confirmBtn) {
        if (string.IsNullOrWhiteSpace(activeInputField.text)) {
            _confirmBtn.interactable = false;
        } else {
            _confirmBtn.interactable = true;
        }
    }

    public void Commit() {
        if (!string.IsNullOrWhiteSpace(activeInputField.text)) {
            gitHandler.Commit(activeInputField.text);
            ToggleCommitMessage(false);
        }
    }

    public void Checkout() {
        bool branch = checkoutPopUp.GetComponentInChildren<Toggle>().isOn;

        if (!string.IsNullOrWhiteSpace(activeInputField.text)) {
            gitHandler.Checkout(activeInputField.text, branch);
            ToggleCheckoutMessage(false);
        }
    }

    public void ToggleCommitMessage(bool _open) {
        TogglePopUp(commitPopUp, _open);
    }

    public void ToggleCheckoutMessage(bool _open) {
        TogglePopUp(checkoutPopUp, _open);
    }

    private void TogglePopUp(GameObject _popUp, bool _open) {
        if (_open) {
            if (_popUp.activeSelf) {
                return;
            }

            character.ToggleCursorMode(false);
            _popUp.SetActive(true);
            activeInputField = _popUp.GetComponentInChildren<InputField>();
            ToggleInputField(true);
        } else {
            character.ToggleCursorMode(true);
            _popUp.SetActive(false);
            ToggleInputField(false);
            activeInputField = null;
        }
    }
    

    private void ToggleInputField(bool _activate) {
        if (_activate) {
            activeInputField.text = "";
            activeInputField.ActivateInputField();
            activeInputField.Select();
        } else {
            activeInputField.text = "";
            activeInputField.DeactivateInputField();
        }
    }
}
