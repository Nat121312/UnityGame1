using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Entities/Create a new Movement")]

public class MoveBase : ScriptableObject
{
    [SerializeField] string Movename;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Types type;

    // Base Stats
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int magiculeCost;
    [SerializeField] MoveOrigin origin;

    public string Name {
        get { return Movename; }
    }

    public string Description {
        get { return description; }
    }
    public Types Type {
        get { return type; }
    }

    public MoveOrigin Origin {
        get { return origin; }
    }

    public int Power {
        get { return power; }
    }

    public int Accuracy {
        get { return accuracy; }
    }
    public int MagiculeCost {
        get { return magiculeCost; }
    }
}

public enum MoveOrigin { Magic, Physic }
