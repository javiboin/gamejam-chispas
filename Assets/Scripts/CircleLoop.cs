using UnityEngine;
using UnityEngine.InputSystem;

public class CircleLoop : MonoBehaviour
{
    [SerializeField] private GameplayStats gameplayStats;
    private float loopTime;
    private float elapsed;
    private float loopValue;

    private void Start()
    {
        loopTime = gameplayStats.MaxLoopTime;
        transform.localScale = Vector3.one * gameplayStats.MaxScale;
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
        }

        // Mathf.PingPong devuelve un valor que oscila entre 0 y 1 en función del tiempo transcurrido. 
        // Al dividir elapsed por loopTime, obtenemos un valor que representa el progreso de la animación en el rango de 0 a 1. 
        // Luego, Mathf.PingPong hace que este valor oscile entre 0 y 1, creando un efecto de ida y vuelta.
    }
}
