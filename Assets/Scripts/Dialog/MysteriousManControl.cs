using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MysteriousManControl : MonoBehaviour, Interactable
{

    [SerializeField] Dialog dialog;
    public void Interact() {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
}
