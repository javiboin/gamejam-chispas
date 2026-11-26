using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{

    [Header("Elementos del Canvas UI")]
    public TextMeshProUGUI textoScore;

    [Tooltip("Opcional: podés poner ' Puntos' o dejarlo vacío")]
    public string sufijo = " Puntos";

    public void ActualizarTextoScore(int nuevoScore)
    {
        if (textoScore != null)
        {
            textoScore.text = "SCORE: " + nuevoScore.ToString() + sufijo;
        }
    }


}
