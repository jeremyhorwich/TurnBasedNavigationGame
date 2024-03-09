using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueDisplay;

    private void Awake()
    {
        a_DisplayDialogue.OnUpdateDialogueDisplay += UpdateTurnDisplay;
    }

    private void UpdateTurnDisplay(string dialogue)
    {
        dialogueDisplay.text = dialogue;
    }
}
