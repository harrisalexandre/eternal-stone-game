using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Tempo")]
    public float cycleDuration = 20f;
    private float timer = 0f;

    [Header("Progresso")]
    public int dias = 0;
    public int pontos = 0;
    public int saudePedra = 100;

    [Header("Controle de ações")]
    public bool limpouHoje = false;

    [Header("UI")]
    public TextMeshProUGUI textoUI;

    [Header("Mapas")]
    public GameObject[] mapas;
    private int mapaAtual = -1;

    [Header("Luz do Sol")]
    public Light sol;
    public float sunriseAngle = -40f;
    public float sunsetAngle = 220f;

    [Header("Cores da Luz")]
    public Color corAmanhecer = new Color(1f, 0.7f, 0.45f);
    public Color corDia = new Color(1f, 0.96f, 0.85f);
    public Color corEntardecer = new Color(1f, 0.5f, 0.3f);
    public Color corNoite = new Color(0.25f, 0.35f, 0.6f);

    [Header("Intensidade da Luz")]
    public float intensidadeNoite = 0.15f;
    public float intensidadeAmanhecer = 0.8f;
    public float intensidadeDia = 1.2f;
    public float intensidadeEntardecer = 0.7f;

    void Start()
    {
        AtualizarUI();
        AtualizarMapa();
        AtualizarSol();
    }

    void Update()
    {
        timer += Time.deltaTime;

        AtualizarSol();

        if (timer >= cycleDuration)
        {
            PassarDia();
            timer = 0f;
        }
    }

    void PassarDia()
    {
        dias += 1;
        pontos += 1;
        saudePedra -= 5;
        limpouHoje = false;

        if (saudePedra < 0)
            saudePedra = 0;

        AtualizarUI();
        AtualizarMapa();

        UnityEngine.Debug.Log("Novo dia: " + dias + " | Pontos: " + pontos + " | Saúde: " + saudePedra);
    }

    void AtualizarUI()
    {
        if (textoUI != null)
        {
            textoUI.text =
                "Dias: " + dias +
                "\nPontos: " + pontos +
                "\nSaude:\n" + saudePedra;
        }
    }

    void AtualizarMapa()
    {
        if (mapas == null || mapas.Length == 0)
            return;
        // Altera mapa a cada qtos dias
        int novoMapa = dias / 1;

        if (novoMapa >= mapas.Length)
            novoMapa = mapas.Length - 1;

        if (novoMapa == mapaAtual)
            return;

        for (int i = 0; i < mapas.Length; i++)
        {
            if (mapas[i] != null)
                mapas[i].SetActive(i == novoMapa);
        }

        mapaAtual = novoMapa;
    }

    void AtualizarSol()
    {
        if (sol == null)
            return;

        float t = timer / cycleDuration;

        // rotação do sol
        float angle = Mathf.Lerp(sunriseAngle, sunsetAngle, t);
        sol.transform.rotation = Quaternion.Euler(angle, -30f, 0f);

        // cor e intensidade ao longo do ciclo
        if (t < 0.25f) // amanhecer
        {
            float localT = t / 0.25f;
            sol.color = Color.Lerp(corNoite, corAmanhecer, localT);
            sol.intensity = Mathf.Lerp(intensidadeNoite, intensidadeAmanhecer, localT);
        }
        else if (t < 0.5f) // manhã até meio-dia
        {
            float localT = (t - 0.25f) / 0.25f;
            sol.color = Color.Lerp(corAmanhecer, corDia, localT);
            sol.intensity = Mathf.Lerp(intensidadeAmanhecer, intensidadeDia, localT);
        }
        else if (t < 0.75f) // tarde / entardecer
        {
            float localT = (t - 0.5f) / 0.25f;
            sol.color = Color.Lerp(corDia, corEntardecer, localT);
            sol.intensity = Mathf.Lerp(intensidadeDia, intensidadeEntardecer, localT);
        }
        else // noite
        {
            float localT = (t - 0.75f) / 0.25f;
            sol.color = Color.Lerp(corEntardecer, corNoite, localT);
            sol.intensity = Mathf.Lerp(intensidadeEntardecer, intensidadeNoite, localT);
        }
    }

    public bool PodeLimparHoje()
    {
        return !limpouHoje;
    }

    public void LimparPedra()
    {
        if (limpouHoje)
        {
            UnityEngine.Debug.Log("Já limpou hoje.");
            return;
        }

        saudePedra += 10;

        if (saudePedra > 100)
            saudePedra = 100;

        limpouHoje = true;

        AtualizarUI();

        UnityEngine.Debug.Log("Pedra limpa com sucesso. Saúde: " + saudePedra);
    }
}