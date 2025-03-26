using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class MapArea : MonoBehaviour
{
    [SerializeField] List<Entity> wildCharacters;

    public Entity GetWildCharacter() {
        var wildCharacter = wildCharacters[Random.Range(0, wildCharacters.Count)];
        wildCharacter.Init();
        return wildCharacter;
    }
}