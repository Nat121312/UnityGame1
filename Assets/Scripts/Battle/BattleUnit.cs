using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] EntitiesBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Entity Entity { get; set; }

    public void Setup() {
        Entity = new Entity(_base, level);
        GetComponent<Image>().sprite = Entity.Base.FrontSprite;
    }

}
