using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandler : MonoBehaviour
{
    private PlayerController player;

    [SerializeField]
    private Image mainFill;
    [SerializeField]
    private Image calculatedFill;
    [SerializeField]
    private Text pointsText;
    [SerializeField]
    private Button commitButton;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = player.Points.ToString() + " / " + player.PointsToNextLevel.ToString();
        mainFill.fillAmount = (float)player.Points / (float)player.PointsToNextLevel;
        calculatedFill.fillAmount = (float)player.TryPoints / (float)player.PointsToNextLevel;

        if (player.CurrentRoom.Library.CheckForChanges() && !player.GitView) {
            commitButton.interactable = true;
        } else {
            commitButton.interactable = false;
        }
    }


    /*
     * Functions called by event triggers.
     */

    public void OnCommit() {
        if (player.CurrentRoom.Library.CalculatePoints() > 0) {
            player.OnCommit();
        }
    }
}
