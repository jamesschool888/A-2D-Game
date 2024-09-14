using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using CodeMonkey.Utils;

public class damagePopup : MonoBehaviour
{
    public static damagePopup Create(Vector3 position, float damageAmount, bool isCriticalhit)
    {
        Transform damagePopupTransform = Instantiate(gameAssets.i.damagePopup, position, Quaternion.identity);
        damagePopup damagePopup = damagePopupTransform.GetComponent<damagePopup>();
        damagePopup.Setup(damageAmount, isCriticalhit);

        return damagePopup;
    }
    private static int sortingOder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro tmp;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        tmp = transform.GetComponent<TextMeshPro>();
        
    }

    public void Setup(float damageAmount, bool isCriticalHit)
    {
        string formattedDamage = (damageAmount * 100).ToString("F1");
        tmp.SetText(float.Parse(formattedDamage).ToString());
        if(!isCriticalHit)
        {
            tmp.fontSize = 5;
            textColor = UtilsClass.GetColorFromString("FFC500");
        }
        else
        {
            tmp.fontSize = 6;
            textColor = UtilsClass.GetColorFromString("FF2B00");
        }
        tmp.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOder++;
        tmp.sortingOrder = sortingOder;
        moveVector = new Vector3(.7f, 1) * 30f;
    }

    private void Update()
    {
        transform.position +=  moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            float increaseScaleAmount = 0.3f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = .3f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            tmp.color = textColor;

            if(textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
