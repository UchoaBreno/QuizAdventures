using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManagerFaseFinal : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI perguntaText;
    public Button[] opcoesButtons;          // 4 botões para as opções
    public TextMeshProUGUI feedbackText;

    [Header("Professor")]
    public Slider professorSlider;          // barra de vida
    private int professorVida = 100;        // começa com 100

    [Header("Jogador")]
    public Slider jogadorSlider;            // barra de vida
    private int jogadorVida;                // inicializa com vida da sessão

    private int respostaCorreta;
    private bool jogoAcabou = false;

    void Start()
    {
        // Pega a vida atual do jogador da GameSession
        if (GameSession.Instance != null)
        {
            jogadorVida = GameSession.Instance.JogadorVida;
        }
        else
        {
            jogadorVida = 100; // fallback para teste
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

        int operacao = Random.Range(0, 2); // 0: equação linear, 1: fatorial

        if (operacao == 0)
        {
            // Equação linear do tipo ax + b = c
            int x = Random.Range(1, 10);
            int a = Random.Range(1, 5);
            int b = Random.Range(0, 10);
            int c = a * x + b;

            resultado = x;
            pergunta = $"Resolva para x: {a}x + {b} = {c}";
        }
        else
        {
            // Fatorial
            int n = Random.Range(3, 7); // 3! até 6!
            resultado = Fatorial(n);
            pergunta = $"Qual é o valor de {n}!?";
        }

        perguntaText.text = pergunta;
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
                    opcao = resultado + Random.Range(-10, 11);
                } while (opcao == resultado || opcao < 0);
            }

            opcoesButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = opcao.ToString();
            opcoesButtons[i].onClick.RemoveAllListeners();
            int respostaClicada = opcao;
            opcoesButtons[i].onClick.AddListener(() => VerificarResposta(respostaClicada));
        }
    }

    int Fatorial(int n)
    {
        int f = 1;
        for (int i = 2; i <= n; i++) f *= i;
        return f;
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
                feedbackText.text = "🏆 Parabéns! Você venceu o jogo!";
                jogoAcabou = true;
                DesativarBotoes();
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
