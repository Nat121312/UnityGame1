using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] EntitiesBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Entity Entity { get; set; }

    Image image;
    Vector3 originalPos;
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
    }

    public void Setup() {
        Entity = new Entity(_base, level);
        image.sprite = Entity.Base.FrontSprite;
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation() {
        if (isPlayerUnit == true) {
            image.transform.transform.localPosition = new Vector3(-800f, originalPos.y);
        }
        else {
            image.transform.transform.localPosition = new Vector3(800f, originalPos.y);
        }
        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

}
