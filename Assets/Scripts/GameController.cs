using System;
using System.Collections;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;


    public enum GameState { FreeRoam, Dialog, Battle}
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    public static GameController Instance { get; private set; }

    public GameState state;

    public GameObject battleHUD;

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
            // Nothing
         }
    }
}
