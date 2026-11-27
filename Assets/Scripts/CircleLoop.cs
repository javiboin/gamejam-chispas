using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleLoop : MonoBehaviour
{
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
        transform.localScale = Vector3.one * scale;

        if ((Keyboard.current.spaceKey.wasPressedThisFrame) || (Mouse.current.leftButton.wasPressedThisFrame))
        {
            Debug.Log(loopValue);
            if (loopValue >= 0.8f)
            {
                int points = Mathf.RoundToInt(loopValue * gameplayStats.ExtraScoreMultiplier);
                score += points;
                onScoreChanged?.Invoke(points);


                Debug.Log("Suma Extra! Score: " + score);



                //_extraPointArea.enabled = true;
                _spriteRenderer.color = Color.green;
                PlayResultAnimation(success: true);

                if (_chispasGolpe != null) _chispasGolpe.Play();
            }
            else if (loopValue >= 0.6f && loopValue < 0.8f)
            {
                int points = Mathf.RoundToInt(loopValue * gameplayStats.ScoreMultiplier);
                score += points;
                onScoreChanged?.Invoke(points);


                Debug.Log("Suma puntaje! Score: " + score);



                _spriteRenderer.color = Color.yellow;

                PlayResultAnimation(success: true);

                if (_chispasGolpe != null) _chispasGolpe.Play();
            }
            else
            {
                int points = Mathf.RoundToInt(loopValue * gameplayStats.ScoreMultiplier);
                score += points;
                onScoreChanged?.Invoke(points);

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

    private void PlayResultAnimation(bool success)
    {
        _animator.ResetTrigger(SuccessTrigger);
        _animator.ResetTrigger(FailTrigger);
        _animator.SetTrigger(success ? SuccessTrigger : FailTrigger);
    }
}
