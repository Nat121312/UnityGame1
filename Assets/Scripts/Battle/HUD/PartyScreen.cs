using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;


public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text textmessage;
    [SerializeField] TeammemberUI memberSlot1;
    [SerializeField] TeammemberUI memberSlot2;
    [SerializeField] TeammemberUI memberSlot3;


    public void SetTeamData(List<Entity> entity) {
            if (entity[0] != null) {
                memberSlot1.SetData(entity[0]);
            }
            else {
                memberSlot1.gameObject.SetActive(false);
            }
            if (entity[1] != null) {
                memberSlot2.SetData(entity[1]);
            }
            else {
                memberSlot2.gameObject.SetActive(false);
            }
            if (entity[2] != null) {
                memberSlot3.SetData(entity[2]);
            }
            else {
                memberSlot3.gameObject.SetActive(false);
            }

        textmessage.text = "Select the Character";
    }
}
