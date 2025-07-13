using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public int JogadorVida { get; set; } = 100;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Opcional: para resetar a vida no início do jogo, se quiser
    public void ResetarVida()
    {
        JogadorVida = 100;
    }
}
