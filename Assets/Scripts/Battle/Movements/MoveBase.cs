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

    public string Name {
        get { return Movename; }
    }

    public string Description {
        get { return description; }
    }
    public Types Type {
        get { return type; }
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
