using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeammemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HPBar hPBar;
    [SerializeField] MagiculesBar magiculesBar;
    [SerializeField] Image image;
    [SerializeField] Color highlightedColor;

    Entity _entity;

    public void SetData(Entity entity) {
        _entity = entity;
        nameText.text = entity.Base.Name;
        hPBar.SetHP((float) entity.currentHP / entity.MaxHP, _entity.currentHP, _entity.MaxHP); // entity.currentHP / entity.MaxHP
        magiculesBar.SetMP( entity.currentMP / entity.MagiculeCount, _entity.MagiculeCount);
        image.sprite = entity.Base.FrontSprite;
    }

    public void SetSelected(bool selected) {
        if (selected == true) {
            nameText.color = highlightedColor;
        }
        else if (selected == false) {
            nameText.color = Color.black;
        }
    }
}
