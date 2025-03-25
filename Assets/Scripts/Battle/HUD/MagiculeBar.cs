using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MagiculesBar : MonoBehaviour
{
    [SerializeField] GameObject Magicules;
    [SerializeField] Text MPTextAmount;
    public void SetMP(float MagiculesNormalized, float MaxMP) {
        Magicules.transform.transform.localScale = new Vector3(MagiculesNormalized, 1f, 1f);
         MPTextAmount.text = $"{MaxMP}";
    }

    public IEnumerator SetMPSmooth(float newMP, float currentMP) {
        float curMP = Magicules.transform.localScale.x;
        float changeAmt = curMP - newMP;
        MPTextAmount.text = $"{currentMP}";

        while (curMP - newMP > Mathf.Epsilon) {
            curMP -= changeAmt * Time.deltaTime;
            Magicules.transform.transform.localScale = new Vector3(curMP, 1f, 1f);
            yield return null;
        }
        Magicules.transform.transform.localScale = new Vector3(newMP, 1f, 1f);
    }
}
