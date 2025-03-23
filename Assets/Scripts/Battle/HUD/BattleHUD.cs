using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hPBar;
    [SerializeField] MagiculesBar magiculesBar;
    [SerializeField] bool isPlayerUnit;

    Entity _entity;

    public void SetData(Entity entity) {
        _entity = entity;
        nameText.text = entity.Base.Name;
        levelText.text = "Lvl: " + entity.Level; 
        hPBar.SetHP((float) entity.currentHP / entity.MaxHP, _entity.currentHP, _entity.MaxHP); // entity.currentHP / entity.MaxHP
        if (isPlayerUnit == true) {
            magiculesBar.SetMP((float) entity.currentMP / entity.Magicules, _entity.Magicules);
        }
    }

    public IEnumerator UpdateHP() {
        yield return hPBar.SetHPSmooth((float)_entity.currentHP/_entity.MaxHP, _entity.currentHP, _entity.MaxHP); // entity.currentHP / entity.MaxHP
    }

    public IEnumerator UpdateMP() {
        if (isPlayerUnit == true) {
            yield return magiculesBar.SetMPSmooth((float) _entity.currentMP / _entity.Magicules, _entity.currentMP);
        }
    }
}
