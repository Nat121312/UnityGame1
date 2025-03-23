using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Text dialogtext;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;
    [SerializeField] Text ppText;
    [SerializeField] Text typeText;
    [SerializeField] Text powertext;
    [SerializeField] Text accuracyText;
    [SerializeField] Color selectColor;

    public void SetDialog(string dialog) {
        dialogtext.text = dialog;
    }

    public IEnumerator TypeDialog(string line) {
        dialogtext.text = "";
        foreach (var letter in line.ToCharArray()) {
            dialogtext.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }

    public void EnableDialogText (bool enabled) {
        dialogtext.enabled = enabled;
    }
    public void EnableActionSelector (bool enabled) {
        actionSelector.SetActive(enabled);
    }
    public void EnableMoveSelector (bool enabled) {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void UpdateActionSelector (int selectedAction) {
        for (int i = 0; i < actionTexts.Count; i++) {
            if (i == selectedAction) {
                actionTexts[i].color = selectColor;
            }
            else {
                actionTexts[i].color = Color.black;
            }
        }
    }

    public void SetMoveNames (List<Move> moves) {
        for (int i = 0; i < moveTexts.Count; ++i) {
            if (i < moves.Count) {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else {
                moveTexts[i].text = "-";
            }
        }
    }

    public void UpdateMoveSelection (int selectedMove, Move move) {
        for (int i = 0; i < moveTexts.Count; i++) {
            if (i == selectedMove) {
                moveTexts[i].color = selectColor;
            }
            else {
                moveTexts[i].color = Color.black;
            }
        }
        ppText.text = $"Cost {move.Base.MagiculeCost}";
        typeText.text = $"{move.Base.Type.ToString()} Type";
        accuracyText.text = $"Accuracy {move.Base.Accuracy}";
        powertext.text = $"Power {move.Base.Power}";
    }
}
