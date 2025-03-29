using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeammemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HPBar hPBar;
    [SerializeField] MagiculesBar magiculesBar;
    [SerializeField] Image image;

    Entity _entity;

    public void SetData(Entity entity) {
        _entity = entity;
        nameText.text = entity.Base.Name;
        hPBar.SetHP((float) entity.currentHP / entity.MaxHP, _entity.currentHP, _entity.MaxHP); // entity.currentHP / entity.MaxHP
        magiculesBar.SetMP( entity.currentMP / entity.MagiculeCount, _entity.MagiculeCount);
        image.sprite = entity.Base.FrontSprite;
    }
}
