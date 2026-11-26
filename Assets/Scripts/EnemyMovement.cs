using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _stepDistance = 3f; //distancia en mtss

    [SerializeField] private float _waitInterval = 2f; //segs movimiento

    [SerializeField] private float _moveSpeed = 6f; //velocidad de mov

    private Coroutine _moveRoutine;

    private void Start()
    {
        StartMoving();
    }

    public void StartMoving()
    {
        if (_moveRoutine == null)
        {
            _moveRoutine = StartCoroutine(MoveForwardRoutine());
        }
    }

    public void StopMoving()
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }
    }

    private IEnumerator MoveForwardRoutine()
    {
        while (true)
        {
            // Tiempo de espera
            yield return new WaitForSeconds(_waitInterval);
        
            // Calcula la posición destino en línea recta hacia adelante (eje Z local / flecha azul)
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + (transform.forward * _stepDistance);

            // Se mueve a la posicion 
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    _moveSpeed * Time.deltaTime
                );

                yield return null;
            }
            // Ajusta a la posición final exacta para evitar desviaciones por decimales (punto flotante)
            transform.position = targetPosition;
        }
    }    
}
