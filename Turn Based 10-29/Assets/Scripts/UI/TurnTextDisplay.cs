using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnTextDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text turnTextDisplay;

    private void Awake()
    {
        TurnManager.instance.OnTurnBegin += UpdateTurnDisplay;
    }

    private void UpdateTurnDisplay()
    {
        turnTextDisplay.text = "Turns: " + TurnManager.instance.TurnsRemaining.ToString();
    }
}
