using UnityEngine;
using UnityEngine.InputSystem;

public class StoneCleaner : MonoBehaviour
{
    public DayNightCycle gameManager;

    [Header("Limpeza")]
    public int sujeira = 100;
    public int limpezaPorEsfregao = 5;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (cam == null)
            UnityEngine.Debug.LogError("Camera.main não encontrada.");
        else
            UnityEngine.Debug.Log("Camera encontrada: " + cam.name);
    }

    void Update()
    {
        if (gameManager == null)
        {
            UnityEngine.Debug.LogWarning("GameManager não foi ligado no Inspector.");
            return;
        }

        if (cam == null)
            return;

        // Mouse no PC
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            UnityEngine.Debug.Log("Mouse pressionado em: " + pos);
            TentarEsfregar(pos);
        }

        // Toque no celular
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 pos = Touchscreen.current.primaryTouch.position.ReadValue();
            UnityEngine.Debug.Log("Toque detectado em: " + pos);
            TentarEsfregar(pos);
        }
    }

    void TentarEsfregar(Vector2 screenPosition)
    {
        if (!gameManager.PodeLimparHoje())
        {
            UnityEngine.Debug.Log("Já limpou hoje.");
            return;
        }

        Ray ray = cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            UnityEngine.Debug.Log("Raycast acertou: " + hit.collider.gameObject.name);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                UnityEngine.Debug.Log("Acertou a pedra!");
                Esfregar();
            }
        }
        else
        {
            UnityEngine.Debug.Log("Raycast não acertou nada.");
        }
    }

    void Esfregar()
    {
        sujeira -= limpezaPorEsfregao;

        if (sujeira < 0)
            sujeira = 0;

        UnityEngine.Debug.Log("Esfregando... sujeira: " + sujeira);

        if (sujeira == 0)
        {
            PedraLimpa();
        }
    }

    void PedraLimpa()
    {
        if (!gameManager.PodeLimparHoje())
            return;

        UnityEngine.Debug.Log("Pedra limpa!");
        gameManager.LimparPedra();
        sujeira = 100;
    }
}