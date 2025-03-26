using System;
using System.Collections;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;


    public enum GameState { FreeRoam, Dialog, Battle}
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    public static GameController Instance { get; private set; }

    public GameState state;

    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () => {
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnHideDialog += () => {
            if (state == GameState.Dialog) {
                state = GameState.FreeRoam;
            }
        };

        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    void StartBattle() {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<TeamParty>();
        var wildCharacter = FindFirstObjectByType(typeof(MapArea)).GetComponent<MapArea>().GetWildCharacter();

        battleSystem.StartBattle(playerParty, wildCharacter);
    }

    void EndBattle(bool won) {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
         if (state == GameState.FreeRoam) {
            playerController.HandleUpdate();
         }
         else if (state == GameState.Dialog) {
            DialogManager.Instance.HandleUpdate();
         }
         else if (state == GameState.Battle) {
            battleSystem.HandleUpdate();
         }
    }
}
