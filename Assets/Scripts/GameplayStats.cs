using UnityEngine;

[CreateAssetMenu(fileName = "GameplayStats", menuName = "Scriptable Objects/GameplayStats")]
public class GameplayStats : ScriptableObject
{
    [Header("Loop Time")]
    [SerializeField, Min(1)] private float maxLoopTime = 1f;

    public float MaxLoopTime => maxLoopTime;

}
