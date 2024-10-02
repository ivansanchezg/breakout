using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesTMP;
    [SerializeField] TextMeshProUGUI gameOverTMP;
    [SerializeField] TextMeshProUGUI youWinTMP;

    public static UI instance;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void UpdateLivesText(int lives) {
        livesTMP.text = $"Lives: {lives}";
    }

    public void DisplayGameOver() {
        gameOverTMP.gameObject.SetActive(true);
    }

    public void DisplayYouWin() {
        youWinTMP.gameObject.SetActive(true);
    }
}
