using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject Health;
    [SerializeField] Text HPTextAmount;
    public void SetHP(float hpNormalized, int currentHP, int MaxHP) {
        Health.transform.transform.localScale = new Vector3(hpNormalized, 1f, 1f);
        HPTextAmount.text = $"{currentHP} / {MaxHP}";
    }

    public IEnumerator SetHPSmooth(float newHP, int currentHP, int MaxHP) {
        float curHP = Health.transform.transform.localScale.x;
        float changeAmt = curHP - newHP;
        HPTextAmount.text = $"{currentHP} / {MaxHP}";

        while (curHP - newHP > Mathf.Epsilon) {
            curHP -= changeAmt * Time.deltaTime;
            Health.transform.transform.localScale = new Vector3(curHP, 1f, 1f);
            yield return null;
        }
        Health.transform.transform.localScale = new Vector3(newHP, 1f, 1f);
    }
}
