using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamParty : MonoBehaviour
{
    [SerializeField]  List<Entity> team;

    public List<Entity> Team {
        get {
            return team;
        }
    }

    private void Start() {
        foreach (var Entity in team ) {
            Entity.Init();
        }
    }

    public Entity GetHealthyCharacter() {
        return team.FirstOrDefault(x => x.currentHP > 0);
    }
    
}
