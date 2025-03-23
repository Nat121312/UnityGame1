using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyAction, EnemyMove, Busy }
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] BattleDialogBox dialogBox;
    BattleState state;
    public event Action<bool> OnBattleOver;
    int currentAction;
    int currentMove;
    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle() {
        playerUnit.Setup();
        playerHUD.SetData(playerUnit.Entity);
        enemyUnit.Setup();
        enemyHUD.SetData(enemyUnit.Entity);

        dialogBox.SetMoveNames(playerUnit.Entity.Moves);

        yield return StartCoroutine(dialogBox.TypeDialog($"{enemyUnit.Entity.Base.Name} has Appeared."));

        PlayerAction();
    }

    void PlayerAction() {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an Action"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove() {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);

    }

    IEnumerator PerformPlayerMove() {
        state = BattleState.Busy;

        var move = playerUnit.Entity.Moves[currentMove];
        bool doMove = playerUnit.Entity.UpdateMagiculeCount(move);

        if (doMove == false) {
            yield return dialogBox.TypeDialog($"{playerUnit.Entity.Base.Name} doesn't have enough Magicules to attack...");
            PlayerAction();
        }

        yield return dialogBox.TypeDialog($"{playerUnit.Entity.Base.Name} used {move.Base.Name}");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        enemyUnit.PlayHitAnimation();

        var damageDetails = enemyUnit.Entity.TakeDamage(move, playerUnit.Entity);

        StartCoroutine(enemyHUD.UpdateHP());
        StartCoroutine(playerHUD.UpdateMP());

        if (damageDetails.Fainted == true) {
            yield return dialogBox.TypeDialog($"{enemyUnit.Entity.Base.Name} Fainted! ");
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove() {
        state = BattleState.EnemyMove;
        var move = enemyUnit.Entity.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.Entity.Base.Name} used {move.Base.Name}");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        playerUnit.PlayHitAnimation();
        var damageDetails = playerUnit.Entity.TakeDamage(move, enemyUnit.Entity);

        yield return playerHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted == true) {
            yield return dialogBox.TypeDialog($"{playerUnit.Entity.Base.Name} Fainted! ");
            playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails) {
        if (damageDetails.Critic > 1f) {
            yield return dialogBox.TypeDialog($"A critical hit!");
        }
        
        if (damageDetails.TypeEffectiveness > 1) {
            yield return dialogBox.TypeDialog($"It's Supereffective!");
        }
        else if (damageDetails.TypeEffectiveness < 1) {
            yield return dialogBox.TypeDialog($"It's not very effective!");
        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction) {
            HandleActionSelector();   
        }
        else if (state == BattleState.PlayerMove) {
            HandleMoveSelection();   
        }
       
    }

    void HandleActionSelector() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (currentAction == 0) {
                currentAction = 2;
            }
            else if (currentAction == 1) {
                currentAction = 3;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentAction == 0) {
                currentAction = 1;
            }
            else if (currentAction == 2) {
                currentAction = 3;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (currentAction == 1) {
                currentAction = 0;
            }
            else if (currentAction == 3) {
                currentAction = 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentAction == 3) {
                currentAction = 1;
            }
            else if (currentAction == 2) {
                currentAction = 0;
            }
        }

        dialogBox.UpdateActionSelector(currentAction);

        if  (Input.GetKeyDown(KeyCode.Z) | Input.GetKeyDown(KeyCode.KeypadEnter)) { 
            if (currentAction == 0) {
                // Fight
                PlayerMove();
            }
            else if (currentAction == 0) {
                // Storage
            }
            else if (currentAction == 0) {
                // Change Character
            }
            else if (currentAction == 0) {
                // Run
            }
        }
    }

    void HandleMoveSelection() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentMove < playerUnit.Entity.Moves.Count - 1) {
                currentMove += 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (currentMove > 0) {
                currentMove -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (currentMove < playerUnit.Entity.Moves.Count - 2) {
                currentMove += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentMove > 1) {
                currentMove -= 2;
            }
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Entity.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z)) {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }



    

}
