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
            // 1. Wait the configured time before moving
            yield return new WaitForSeconds(_waitInterval);

            // 2. Calculate target position straight ahead (local Z axis / blue arrow)
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + (transform.forward * _stepDistance);

            // 3. Move smoothly until reaching the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    _moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            // Snap to exact final position to avoid floating point drift
            transform.position = targetPosition;
        }
    }    
}
