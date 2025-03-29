using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyAction, EnemyMove, Busy }
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen teamScreen;
    BattleState state;
    public event Action<bool> OnBattleOver;
    int currentAction;
    int currentMove;

    TeamParty playerTeam;
    Entity wildCharacter;
    public void StartBattle(TeamParty playerParty, Entity wildCharacter)
    {
        this.playerTeam = playerParty;
        this.wildCharacter = wildCharacter;

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle() {
        playerUnit.Setup(playerTeam.GetHealthyCharacter());
        playerHUD.SetData(playerUnit.Entity);
        enemyUnit.Setup(wildCharacter);
        enemyHUD.SetData(enemyUnit.Entity);

        dialogBox.SetMoveNames(playerUnit.Entity.Moves);

        yield return StartCoroutine(dialogBox.TypeDialog($"{enemyUnit.Entity.Base.Name} has Appeared."));

        PlayerAction();
    }

    void PlayerAction() {
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("Choose an Action");
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

        bool isFainted = false;

        if (Random.value*100 <= move.Base.Accuracy) {
            enemyUnit.PlayHitAnimation();

            var damageDetails = enemyUnit.Entity.TakeDamage(move, playerUnit.Entity);

            if (damageDetails.Fainted == true) {
                isFainted = true;
            }

            StartCoroutine(enemyHUD.UpdateHP());
            StartCoroutine(playerHUD.UpdateMP());
        }
        else {
           isFainted = false;
           yield return dialogBox.TypeDialog($"The move has failed! ");
        }

        if (isFainted == true) {
            yield return dialogBox.TypeDialog($"{enemyUnit.Entity.Base.Name} Fainted! ");
            enemyUnit.PlayFaintAnimation();

            yield return dialogBox.TypeDialog($"You have gained {((float)enemyUnit.Entity.MagiculeCount / 1000)} Magicule for your {playerUnit.Entity.Base.Name}! ");

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
        bool doMove = false;
        while (doMove == false) {
            doMove = enemyUnit.Entity.UpdateMagiculeCount(move);
            if (doMove == false) {
                move = enemyUnit.Entity.GetRandomMove();
            }
        }
        
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
            var nextCharacter = playerTeam.GetHealthyCharacter();
            if (nextCharacter != null) {
                playerUnit.Setup(nextCharacter);
                playerHUD.SetData(nextCharacter);

                dialogBox.SetMoveNames(nextCharacter.Moves);

                yield return StartCoroutine(dialogBox.TypeDialog($"{nextCharacter.Base.Name} has entered the battle."));

                PlayerAction();
            }
            else {
                OnBattleOver(false);
            }

            
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
            currentAction -= 2;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            currentAction += 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            currentAction -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            currentAction += 2;
        }

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelector(currentAction);

        if  (Input.GetKeyDown(KeyCode.Z) | Input.GetKeyDown(KeyCode.KeypadEnter)) { 
            if (currentAction == 0) {
                // Fight
                PlayerMove();
            }
            else if (currentAction == 0) {
                // Storage
            }
            else if (currentAction == 2) {
                // Change Character
                OpenTeamScreen();
            }
            else if (currentAction == 3) {
                // Run
            }
        }
    }

    void HandleMoveSelection() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currentMove -= 2;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            currentMove += 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            currentMove -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            currentMove += 2;
        }

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Entity.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Entity.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z)) {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X)) {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }

    void OpenTeamScreen() {
        teamScreen.SetTeamData(playerTeam.Team);
        teamScreen.gameObject.SetActive(true);
    }



    

}
