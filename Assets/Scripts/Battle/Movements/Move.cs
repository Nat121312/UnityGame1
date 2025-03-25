using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Move
{
    public MoveBase Base { get; set; }

    public Move(MoveBase pBase) {
        Base = pBase;
    }

}
