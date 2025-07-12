using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI perguntaText;

    public string[] perguntas = {
        "Qual a capital do Brasil?",
        "Quanto é 2 + 2?",
        "Qual é a cor do céu?"
    };

    private int perguntaAtual = 0;

    void Start()
    {
        // Só mostra perguntas se estiver na cena de quiz
        if (perguntaText != null)
            MostrarPergunta();
    }

    void MostrarPergunta()
    {
        perguntaText.text = perguntas[perguntaAtual];
    }

    public void Responder()
    {
        perguntaAtual++;
        if (perguntaAtual >= perguntas.Length)
        {
            perguntaText.text = "Fim!";
        }
        else
        {
            MostrarPergunta();
        }
    }

    // Método para ser chamado no botão Jogar
    public void Jogar()
    {
        // Altere "GameplayScene" para o nome da cena de gameplay
        SceneManager.LoadScene("GameplayScene");
    }

    // Método opcional para sair do jogo (botão sair)
    public void Sair()
    {
        Application.Quit();
        Debug.Log("Jogo encerrado.");
    }
}
