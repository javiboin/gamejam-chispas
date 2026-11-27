using System.Collections;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private const float ScorePulseScale = 2f;
    private const float ScorePulseDuration = 0.2f;

    private Coroutine _scorePulseRoutine;
    private Vector3 _scoreBaseScale;
    private Color _scoreBaseColor;
    private bool _scoreScaleCaptured;
    private bool _hasDisplayedScore;
    private int _lastDisplayedScore;

    [Header("Elementos del Canvas UI")]
    public TextMeshProUGUI textoScore;

    [Tooltip("Opcional: podés poner ' Puntos' o dejarlo vacío")]
    public string sufijo = " Puntos";

    public void ActualizarTextoScore(int nuevoScore)
    {
        if (textoScore != null)
        {
            bool scoreAumento = _hasDisplayedScore && nuevoScore > _lastDisplayedScore;
            textoScore.text = "SCORE: " + nuevoScore.ToString() + sufijo;
            _lastDisplayedScore = nuevoScore;
            _hasDisplayedScore = true;

            if (scoreAumento)
            {
                IniciarPulsoPuntaje();
            }

            Debug.Log("Score actualizado: " + nuevoScore);
        }
    }

    private void IniciarPulsoPuntaje()
    {
        if (!_scoreScaleCaptured)
        {
            _scoreBaseScale = textoScore.transform.localScale;
            _scoreBaseColor = textoScore.color;
            _scoreScaleCaptured = true;
        }

        if (_scorePulseRoutine != null)
        {
            StopCoroutine(_scorePulseRoutine);
            textoScore.transform.localScale = _scoreBaseScale;
            textoScore.color = _scoreBaseColor;
        }

        textoScore.color = Color.yellow;
        _scorePulseRoutine = StartCoroutine(PulsoPuntaje());
    }

    private IEnumerator PulsoPuntaje()
    {
        float halfDuration = ScorePulseDuration * 0.5f;
        float elapsed = 0f;
        Vector3 expandedScale = _scoreBaseScale * ScorePulseScale;

        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / halfDuration);
            textoScore.transform.localScale = Vector3.Lerp(
                _scoreBaseScale,
                expandedScale,
                Mathf.SmoothStep(0f, 1f, progress));
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / halfDuration);
            textoScore.transform.localScale = Vector3.Lerp(
                expandedScale,
                _scoreBaseScale,
                Mathf.SmoothStep(0f, 1f, progress));
            yield return null;
        }

        textoScore.transform.localScale = _scoreBaseScale;
        textoScore.color = _scoreBaseColor;
        _scorePulseRoutine = null;
    }

    private void OnDisable()
    {
        if (_scorePulseRoutine != null)
        {
            StopCoroutine(_scorePulseRoutine);
            _scorePulseRoutine = null;
        }

        if (_scoreScaleCaptured && textoScore != null)
        {
            textoScore.transform.localScale = _scoreBaseScale;
            textoScore.color = _scoreBaseColor;
        }
    }

}
