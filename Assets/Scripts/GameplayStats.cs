using UnityEngine;

[CreateAssetMenu(fileName = "GameplayStats", menuName = "Scriptable Objects/GameplayStats")]
public class GameplayStats : ScriptableObject
{
    [Header("Loop Time")]
    [SerializeField, Min(0)] private float LoopTime = 1f;
    [SerializeField, Min(0)] private float maxScale = 2.8f;
    [SerializeField, Min(0)] private float minScale = 0.2f;

    [Header("Score")]
    [SerializeField, Min(0)] private float scoreMultiplier = 100f;
    [SerializeField, Min(0)] private float extraScoreMultiplier = 200f;

    public float MaxLoopTime => LoopTime;
    public float MaxScale => maxScale;
    public float MinScale => minScale;
    public float ScoreMultiplier => scoreMultiplier;
    public float ExtraScoreMultiplier => extraScoreMultiplier;

}
