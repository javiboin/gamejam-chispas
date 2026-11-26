using UnityEngine;

public class StaticAreaSize : MonoBehaviour
{
    [SerializeField] private GameplayStats gameplayStats;
    private void Start()
    {
        transform.localScale = Vector3.one * gameplayStats.MaxScale;
    }
}
