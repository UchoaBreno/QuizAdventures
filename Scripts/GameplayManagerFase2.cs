using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayManagerFase2 : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI perguntaText;
    public Button[] opcoesButtons;           // 4 botões para as opções
    public TextMeshProUGUI feedbackText;

    [Header("Professor")]
    public Slider professorSlider;           // barra de vida
    private int professorVida = 100;         // começa com 100

    [Header("Jogador")]
    public Slider jogadorSlider;             // barra de vida
    private int jogadorVida;                 // inicializada com o valor do GameSession

    private int respostaCorreta;
    private bool jogoAcabou = false;

    void Start()
    {
        // Inicializa vida do jogador a partir do GameSession
        if (GameSession.Instance != null)
        {
            jogadorVida = GameSession.Instance.JogadorVida;
        }
        else
        {
            jogadorVida = 100; // fallback
        }

        NovaPergunta();
        AtualizarProfessor();
        AtualizarJogador();
    }

    void NovaPergunta()
    {
        if (jogoAcabou) return;

        feedbackText.text = "";

        int resultado = 0;
        string pergunta = "";

        int operacao = Random.Range(0, 2); // 0: potenciação, 1: raiz

        if (operacao == 0)
        {
            // Potenciação
            int baseNum = Random.Range(2, 6);  // base entre 2 e 5
            int expoente = Random.Range(2, 4); // expoente 2 ou 3
            resultado = (int)Mathf.Pow(baseNum, expoente);
            pergunta = $"{baseNum}^{expoente}";
        }
        else
        {
            // Raiz quadrada
            int[] quadrados = { 4, 9, 16, 25, 36, 49, 64, 81, 100 };
            int index = Random.Range(0, quadrados.Length);
            int numero = quadrados[index];
            resultado = Mathf.RoundToInt(Mathf.Sqrt(numero));
            pergunta = $"√{numero}";
        }

        perguntaText.text = $"Quanto é {pergunta}?";
        respostaCorreta = resultado;

        int corretaIndex = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            int opcao;
            if (i == corretaIndex)
            {
                opcao = resultado;
            }
            else
            {
                do
                {
                    opcao = Random.Range(Mathf.Max(1, resultado - 5), resultado + 6);
                } while (opcao == resultado);
            }

            opcoesButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = opcao.ToString();
            opcoesButtons[i].onClick.RemoveAllListeners();
            int respostaClicada = opcao;
            opcoesButtons[i].onClick.AddListener(() => VerificarResposta(respostaClicada));
        }
    }

    void VerificarResposta(int respostaJogador)
    {
        if (jogoAcabou) return;

        if (respostaJogador == respostaCorreta)
        {
            feedbackText.text = "✅ Você acertou!";
            professorVida -= 20;
            AtualizarProfessor();

            if (professorVida <= 0)
            {
                feedbackText.text = "🎉 Você venceu a Fase 2!";
                jogoAcabou = true;
                DesativarBotoes();
                Invoke(nameof(CarregarFase3), 2f);
                return;
            }
        }
        else
        {
            feedbackText.text = "❌ Resposta errada!";
            jogadorVida -= 20;

            AtualizarJogador();

            if (GameSession.Instance != null)
            {
                GameSession.Instance.JogadorVida = jogadorVida;
            }

            if (jogadorVida <= 0)
            {
                feedbackText.text = "💀 Você perdeu!";
                jogoAcabou = true;
                DesativarBotoes();
                return;
            }
        }

        Invoke(nameof(NovaPergunta), 1.5f);
    }

    void CarregarFase3()
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.JogadorVida = jogadorVida;
        }

        SceneManager.LoadScene("GameplayScene_Fase3");
    }

    void AtualizarProfessor()
    {
        if (professorSlider != null)
            professorSlider.value = professorVida;
    }

    void AtualizarJogador()
    {
        if (jogadorSlider != null)
            jogadorSlider.value = jogadorVida;
    }

    void DesativarBotoes()
    {
        foreach (Button b in opcoesButtons)
            b.interactable = false;
    }
}
