using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    [SerializeField]
    Ball ballPrefab;

    public static GameManager instance;
    public int lives { get; private set; }
    
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        lives = 3;
    }

    public void LoseLive() {
        lives -= 1;
        print($"Lost life, remanining lives: {lives}");
        if (lives <= 0) {
            print("Game Over");
        } else {
            StartCoroutine(InstantiateBall());
        }
    }

    IEnumerator InstantiateBall() {
        yield return new WaitForSeconds(2);
        Instantiate(ballPrefab);
    }
}
