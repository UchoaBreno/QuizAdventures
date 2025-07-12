using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI perguntaText;
    public Button[] opcoesButtons; // 4 botões para as opções
    public TextMeshProUGUI feedbackText;

    [Header("Professor")]
    public Slider professorSlider;    // barra de vida
    private int professorVida = 100;  // começa com 100

    [Header("Jogador")]
    public Slider jogadorSlider;     // barra de vida
    private int jogadorVida = 100;   // começa com 100

    private int respostaCorreta;
    private bool jogoAcabou = false;

    void Start()
    {
        NovaPergunta();
        AtualizarProfessor();
        AtualizarJogador();
    }

    void NovaPergunta()
    {
        if (jogoAcabou) return;

        feedbackText.text = "";

        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);
        int operacao = Random.Range(0, 4); // 0: +, 1: -, 2: *, 3: /
        int resultado = 0;
        string simbolo = "";

        switch (operacao)
        {
            case 0: resultado = num1 + num2; simbolo = "+"; break;
            case 1: resultado = num1 - num2; simbolo = "-"; break;
            case 2: resultado = num1 * num2; simbolo = "×"; break;
            case 3: resultado = num1 / num2; simbolo = "/"; num1 *= num2; resultado = num1 / num2; break; // evita fração
        }

        perguntaText.text = $"Quanto é {num1} {simbolo} {num2}?";
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
                    opcao = Random.Range(resultado - 10, resultado + 10);
                } while (opcao == resultado);
            }

            opcoesButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = opcao.ToString();
            opcoesButtons[i].onClick.RemoveAllListeners();
            opcoesButtons[i].onClick.AddListener(() => VerificarResposta(opcao));
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
                feedbackText.text = "🎉 Você venceu!";
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
