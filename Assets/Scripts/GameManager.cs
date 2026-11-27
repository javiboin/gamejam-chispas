using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Puntaje")]
    public int scoreTotal = 0;

    [Header("Referencias UI")]
    public HUDController hudController;
    public GameObject panelGameOver;
    public TextMeshProUGUI textoScoreFinal;
    [SerializeField] private CircleLoop circleLoop;
    public Action<int> onScoreChanged;

    [Header("Modo Prueba (Tarea 2 - Punto 3)")]
    public bool modoPruebaClick = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (circleLoop != null)
        {
            circleLoop.onScoreChanged += SumarPuntos;
        }
    }

    private void OnDisable()
    {
        if (circleLoop != null)
        {
            circleLoop.onScoreChanged -= SumarPuntos;
        }
    }

    private void Start()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
        ActualizarUI();
    }
    public void SumarPuntos(int puntajeDelMomento)
    {
        scoreTotal += puntajeDelMomento;
        ActualizarUI();
        onScoreChanged?.Invoke(scoreTotal);
    }

    public void ActivarGameOver()
    {
        if (textoScoreFinal != null)
        {
            textoScoreFinal.text = "SCORE: " + scoreTotal.ToString();
        }

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }
    }


    public void ReiniciarPartida()
    {
        scoreTotal = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ActualizarUI()
    {
        if (hudController != null)
        {
            hudController.ActualizarTextoScore(scoreTotal);
        }
    }
}