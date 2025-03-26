using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;

    public Entity Entity { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(Entity entity) {
        Entity = entity;
        image.sprite = Entity.Base.FrontSprite;

        image.color = originalColor;
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

    public void PlayAttackAnimation() {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit == true) {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50, 0.4f));
        }
        else {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50, 0.4f));
        }
        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.4f));
    }

    public void PlayHitAnimation() {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.2f));
        sequence.Append(image.DOColor(originalColor, 0.2f));
    }

    public void PlayFaintAnimation() {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

}
