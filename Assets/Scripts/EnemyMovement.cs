using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private CircleLoop _circleLoop; 
    [Header("Movement Settings")]
    [SerializeField] private float _stepDistance = 3f; //distancia en mtss
    //[SerializeField] private float _waitInterval = 2f; //segs movimiento
    [SerializeField] private float _moveSpeed = 6f; //velocidad de mov
    
    [Header ("Game Over Settings")]
    [SerializeField] private int _stepsToReachThePlayer = 9; //pasos para agarrar al jugador
    [SerializeField] GameOverController _gameOverController;

    [Header("Target Settings")]
    [SerializeField] private Transform _playerTarget; //el enemigo, asi el player siempre mira al enemigo

    private Vector3 _targetPosition;
    private Coroutine _moveRoutine;
    private int _currentSteps;

    private void Start()
    {
        if (_playerTarget != null)
        {
            // El enemigo mira al player horizontal
            Vector3 lookPosition = new Vector3(_playerTarget.position.x, transform.position.y, _playerTarget.position.z);
            transform.LookAt(lookPosition);
        }
        //Posicion inicio enemigo
        _targetPosition = transform.position;
        _currentSteps = 0;
    }

    // Llamamos a este metodo desde CIRCLELOOP cuando el jugador ERRA

    public void StepForward()
    {
        _currentSteps++;
        //agrega 1 step forward a la posicion actual
        _targetPosition += transform.forward * _stepDistance;

        if (_moveRoutine == null)
        {
            _moveRoutine = StartCoroutine(MoveToTargetRoutine());
        }
    }

    private IEnumerator MoveToTargetRoutine()
    {
        while (Vector3.Distance(transform.position, _targetPosition)> 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                _moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        // ultima posicion
        transform.position = _targetPosition;
        _moveRoutine = null;

        if (_currentSteps >= _stepsToReachThePlayer)
        {
            TriggerPlayerCaught();
        }
    }
    private void TriggerPlayerCaught()
    {
        Debug.Log("GAME OVER!");
        AudioManager.Instance?.PlayKotaixScream();
        if (_gameOverController != null)
        {
            int finalScore = _circleLoop != null ? _circleLoop.Score : 0;
            _gameOverController.TriggerGameOver(finalScore);
        }
    }
}
