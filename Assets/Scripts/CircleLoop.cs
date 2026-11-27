using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleLoop : MonoBehaviour
{
    private const float ScorePulseDuration = 0.2f;
    private const float ScorePulseStrength = 0.2f;

    private static readonly int SuccessTrigger = Animator.StringToHash("Success");
    private static readonly int FailTrigger = Animator.StringToHash("Fail");
    [SerializeField] private GameplayStats gameplayStats;
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("VFX Partículas")]
    [SerializeField] private ParticleSystem _chispasGolpe;

    private float loopTime;
    private float elapsed;
    private float loopValue;
    private float scorePulseElapsed = ScorePulseDuration;
    private int score;
    public int Score => score; 

    public Action<int> onScoreChanged;

    private void Start()
    {
        loopTime = gameplayStats.MaxLoopTime;
        transform.localScale = Vector3.one * gameplayStats.MaxScale;

        score = 0;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        //_spriteRenderer.enabled = false;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        loopValue = Mathf.PingPong(elapsed / loopTime, 1f);
        float scale = Mathf.Lerp(gameplayStats.MaxScale, gameplayStats.MinScale, loopValue);
        float pulseMultiplier = 1f;
        if (scorePulseElapsed < ScorePulseDuration)
        {
            scorePulseElapsed = Mathf.Min(scorePulseElapsed + Time.unscaledDeltaTime, ScorePulseDuration);
            float pulseProgress = scorePulseElapsed / ScorePulseDuration;
            pulseMultiplier += Mathf.Sin(pulseProgress * Mathf.PI) * ScorePulseStrength;
        }
        transform.localScale = Vector3.one * scale * pulseMultiplier;

        changeColor();

        if ((Keyboard.current.spaceKey.wasPressedThisFrame) || (Mouse.current.leftButton.wasPressedThisFrame))
        {
            Debug.Log(loopValue);
            if (loopValue >= 0.7f)
            {
                int points = Mathf.RoundToInt(loopValue * gameplayStats.ExtraScoreMultiplier);
                RegistrarPuntaje(points);


                Debug.Log("Suma Extra! Score: " + score);



                _spriteRenderer.color = Color.green;
                PlayResultAnimation(success: true);
                AudioManager.Instance?.PlayPiedrasChispas();

                if (_chispasGolpe != null) _chispasGolpe.Play();
            }
            else if (loopValue >= 0.5f && loopValue < 0.7f)
            {
                int points = Mathf.RoundToInt(loopValue * gameplayStats.ScoreMultiplier);
                RegistrarPuntaje(points);


                Debug.Log("Suma puntaje! Score: " + score);



                _spriteRenderer.color = Color.yellow;

                PlayResultAnimation(success: true);
                AudioManager.Instance?.PlayPiedrasChispas();

                if (_chispasGolpe != null) _chispasGolpe.Play();
            }
            else
            {
                int points = Mathf.RoundToInt(loopValue * gameplayStats.ScoreMultiplier);
                RegistrarPuntaje(points);

                Debug.Log("Se acerca Kotaix! Score: " + score);

                _spriteRenderer.color = Color.red;

                
                PlayResultAnimation(success: false);

                if (_enemyMovement != null)
                {
                    _enemyMovement.StepForward();
                }

            }   
        }
        // Mathf.PingPong devuelve un valor que oscila entre 0 y 1 en función del tiempo transcurrido. 
        // Al dividir elapsed por loopTime, obtenemos un valor que representa el progreso de la animación en el rango de 0 a 1. 
        // Luego, Mathf.PingPong hace que este valor oscile entre 0 y 1, creando un efecto de ida y vuelta.
    }

    private void RegistrarPuntaje(int puntos)
    {
        score += puntos;
        onScoreChanged?.Invoke(puntos);

        if (puntos != 0)
        {
            scorePulseElapsed = 0f;
        }
    }

    private void PlayResultAnimation(bool success)
    {
        _animator.ResetTrigger(SuccessTrigger);
        _animator.ResetTrigger(FailTrigger);
        _animator.SetTrigger(success ? SuccessTrigger : FailTrigger);
    }

    private void changeColor()
    {
            if (loopValue >= 0.7f)
            {
                _spriteRenderer.color = Color.green;
            }
            else if (loopValue >= 0.5f && loopValue < 0.7f)
            {
                _spriteRenderer.color = Color.yellow;
            }
            else
            {
                _spriteRenderer.color = Color.red;
            } 
    }
}
