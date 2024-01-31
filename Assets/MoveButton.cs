using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveButton : MonoBehaviour
{
    public Move move;
    public int moveIndex;
    public BattleManager battleManager;
    TextMeshProUGUI buttonText;
    public TextMeshProUGUI moveNameBox, moveDescBox, moveSideEffectBox, movePowerText;
    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        move = battleManager.playerMovePool[Mathf.Clamp(moveIndex, 0, battleManager.playerMovePool.Count - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        if (move != null)
        {
            buttonText.text = move.name;
        }
    }

    public void UpdateMoveInfo()
    {
        if (move == null) return;
        moveNameBox.text = move.name;
        moveDescBox.text = move.description;
        moveSideEffectBox.text = move.sideEffectDescription;
        movePowerText.text = "Power: " + move.damageMin + "-" + move.damageMax;
    }

    public void AssignMove()
    {
        battleManager.ChooseMove(move);
    }
}
