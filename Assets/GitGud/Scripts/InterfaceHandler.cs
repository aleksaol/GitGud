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
    }

    public void OnCommit() {
        Library library = Library.Instance;
        library.CalculatePoints();
    }
}
