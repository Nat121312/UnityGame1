using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamParty : MonoBehaviour
{
    [SerializeField]  List<Entity> Team;

    private void Start() {
        foreach (var Entity in Team ) {
            Entity.Init();
        }
    }

    public Entity GetHealthyCharacter() {
        return Team.FirstOrDefault(x => x.currentHP > 0);
    }
    
}
