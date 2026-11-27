using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleLoop : MonoBehaviour
{
    [SerializeField] private GameplayStats gameplayStats;
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private Animator _animator;
    private float loopTime;
    private float elapsed;
    private float loopValue;
    private int score;
    public int Score => score; //publica para mostrarla

    public Action onScoreChanged;

    private void Start()
    {
        loopTime = gameplayStats.MaxLoopTime;
        transform.localScale = Vector3.one * gameplayStats.MaxScale;

        score = 0;
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
                score += Mathf.RoundToInt(loopValue * gameplayStats.ExtraScoreMultiplier);
                onScoreChanged?.Invoke();
                Debug.Log("Suma Extra! Score: " + score);
                _animator.SetBool("Success", true);
                _animator.SetBool("Fail", false);
            }
            else if (loopValue >= 0.6f && loopValue < 0.8f)
            {
                score += Mathf.RoundToInt(loopValue * gameplayStats.ScoreMultiplier);
                onScoreChanged?.Invoke();
                Debug.Log("Suma puntaje! Score: " + score);
                _animator.SetBool("Fail", false);
                _animator.SetBool("Success", true);
            }
            else
            {
                score += Mathf.RoundToInt(loopValue * gameplayStats.ScoreMultiplier);
                onScoreChanged?.Invoke(); 
                Debug.Log("Se acerca Kotaix! Score: " + score);
                _animator.SetBool("Fail", true);
                _animator.SetBool("Success", false);

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
}
