using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    [SerializeField] Ball ballPrefab;
    [SerializeField] Brick brickPrefab;
    [SerializeField] GameObject bricksParent;

    public static GameManager instance;
    public int lives { get; private set; }

    PlayerInput playerInput;
    bool isGameOver;

    Ball ball;
    Brick[,] bricks;
    int rows = 5;
    int cols = 10;

    Color[] colors = new Color[] {
        Color.blue,
        Color.green,
        Color.magenta,
        Color.red,
        Color.grey,
    };
    
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        playerInput = new();
    }

    void Start() {
        lives = 3;
        isGameOver = false;

        var xOffset = 1.5f;
        var xStart = -6.75f;
        var yOffset = 0.5f;
        var yStart = 4f;

        bricks = new Brick[rows, cols];

        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                bricks[row, col] = Instantiate(
                    brickPrefab,
                    new Vector3((col * xOffset) + xStart , yStart - (row * yOffset), 0),
                    Quaternion.identity,
                    bricksParent.transform
                );

                bricks[row, col].SetColor(colors[row]);
            }
        }

        ball = Instantiate(ballPrefab);
    }

    void OnEnable() {
        playerInput.Player.Enter.performed += RestartGame;
        playerInput.Enable();
    }

    void OnDisable() {
        playerInput.Player.Enter.performed -= RestartGame;
        playerInput.Disable();
    }

    public void DestroyBrick(Brick brick) {
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                if (bricks[row, col] == brick) {
                    bricks[row, col] = null;
                    brick.Destroy();
                    CheckWin();
                    return;
                }
            }
        }
    }

    public void LoseLive() {
        lives -= 1;
        UI.instance.UpdateLivesText(lives);
        if (lives <= 0) {
            isGameOver = true;
            UI.instance.DisplayGameOver();
        } else {
            StartCoroutine(InstantiateBall());
        }
    }

    IEnumerator InstantiateBall() {
        yield return new WaitForSeconds(2);
        ball = Instantiate(ballPrefab);
    }

    void RestartGame(InputAction.CallbackContext context) {
        if (isGameOver) {
            SceneManager.LoadScene("Breakout");
        }
    }

    void CheckWin() {
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                if (bricks[row, col] != null) {
                    return;
                }
            }
        }

        isGameOver = true;
        UI.instance.DisplayYouWin();
        Destroy(ball.gameObject);
    }
}
