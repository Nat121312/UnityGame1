using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Entities", menuName = "Entities/Create a new Entity")]
public class EntitiesBase : ScriptableObject
{
    // Characteristics
    [SerializeField] string Entityname;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Types type;

    // Base Stats
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int magicAttack;
    [SerializeField] int defense;
    [SerializeField] int magicules;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMoves> learnableMoves;
    public string Name {
        get { return Entityname; }
    }

    public string Description {
        get { return description; }
    }

    public Sprite FrontSprite {
        get { return frontSprite; }
    }

    public Types Type {
        get { return type; }
    }

    public int MaxHP {
        get { return maxHP; }
    }

    public int Attack {
        get { return attack; }
    }
    public int MagicAttack {
        get { return magicAttack; }
    }
    public int Defense {
        get { return defense; }
    }
    public int Magicules {
        get { return magicules; }
    }
    public int Speed {
        get { return speed; }
    }

    public List<LearnableMoves> LearnableMoves {
        get { return learnableMoves; }
    }
   
}

// Moves to Learn
[System.Serializable]
public class LearnableMoves {
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base {
        get { return moveBase; }
    }

    public int Level {
        get { return level; }
    }
}

// Types for the game logic
public enum Types {
    Fire, Water, Earth, Wind, Space, Light, Dark
}

public class TypeChart {
    static float [][] chart = {
        // Attacker             Defender
                            // Fir     Wat    Ear   Win    Spa    Lig  Dar
        /* Fir */ new float[] {1f   , 0.8f , 1.5f , 1f   , 1f   , 1f, 1f},
        /* Wat */ new float[] {1.5f , 1f   , 0.8f , 1f   , 1f   , 1f, 1f},
        /* Ear */ new float[] {1f   , 1f   , 1f   , 0.8f , 1.5f , 1f, 1f},
        /* Win */ new float[] {1f   , 1f   , 1.5f , 1f   , 0.8f , 1f, 1f},
        /* Spa */ new float[] {1f   , 1f   , 0.8f ,  1.5f,    1f, 1f, 1f},
        /* Lig */ new float[] {1.25f, 1.25f, 1.25f, 1.25f, 1.25f, 1f, 2f},
        /* Dar */ new float[] {1.25f, 1.25f, 1.25f, 1.25f, 1.25f, 2f, 1f}
    };

    public static float GetEffectivenes(Types attackertype, Types defenderType) {
        int fila = (int)attackertype;
        int colum = (int)defenderType;

        return chart[fila][colum];
    }
}

