using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;


public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text textdialog;
    [SerializeField] TeammemberUI memberSlot1;
    [SerializeField] TeammemberUI memberSlot2;
    [SerializeField] TeammemberUI memberSlot3;
    List<Entity> entity;


    public void SetTeamData(List<Entity> entity) {
            this.entity = entity;
            if (entity.Count >= 1) {
                if (entity[0] != null) {
                    memberSlot1.SetData(entity[0]);
                }
            }
            else {
                memberSlot1.gameObject.SetActive(false);
            }

            if (entity.Count >= 2) {
                if (entity[1] != null) {
                    memberSlot2.SetData(entity[1]);
                }
            }
            else {
                memberSlot2.gameObject.SetActive(false);
            }
            if (entity.Count >= 3) {
                if (entity[2] != null) {
                    memberSlot3.SetData(entity[2]);
                }
            }
            else {
                memberSlot3.gameObject.SetActive(false);
            }

        textdialog.text = "Select the Character";
    }

    public void UpdateMemberSelected(int selectedChar) {
        memberSlot1.SetSelected(false); 
        memberSlot2.SetSelected(false);       
        memberSlot3.SetSelected(false);
        if (selectedChar == 0) {
            memberSlot1.SetSelected(true);
        }
        else if (selectedChar == 1) {
            memberSlot2.SetSelected(true);
        }
        else if (selectedChar == 2) {
            memberSlot3.SetSelected(true);
        }
    }

    public void SetMessagetext (string textMessage) {
        textdialog.text = textMessage;
    }
}
