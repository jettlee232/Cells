using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RulePanel_CM : MonoBehaviour
{
    public RawImage rawImage;    
    public Texture2D[] imageTextures;
    public TextMeshProUGUI tmpText;
    public RectTransform targetRectTransform;
    public WordEffect1 wordEffect;
    public float duration = 1.0f; 

    void Start()
    {
        rawImage = gameObject.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();

        wordEffect.enabled = false;
        tmpText = gameObject.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        tmpText.text = "";        

        targetRectTransform = GetComponent<RectTransform>();
        targetRectTransform.sizeDelta = Vector2.zero;

        DOTween.Init();
    }

    public void PanelOpen(string newRule, string imgName)
    {
        StartCoroutine(ChangeRuleTextAndImageAfterFewSec(newRule, imgName));

        rawImage.transform.DOScale(new Vector2(300, 300), duration);
        targetRectTransform.DOSizeDelta(new Vector2(350, 700), duration);

        Debug.Log("Sector1");        
    }

    IEnumerator ChangeRuleTextAndImageAfterFewSec(string newRule, string imgName)
    {
        yield return new WaitForSeconds(1f);
        ChangeText(newRule);
        ChangeImg(imgName);
    }

    public void ChangeText(string newRule)
    {        
        tmpText.text = newRule;
        wordEffect.enabled = true;        
    }

    public void ChangeImg(string imgName)
    {        
        foreach (Texture2D texture in imageTextures)
        {
            if (texture.name == imgName)
            {
                rawImage.texture = texture;
                break;
            }
        }
    }

    public void PanelClose()
    {
        wordEffect.enabled = false;
        tmpText.text = "";
        rawImage.transform.DOScale(Vector2.zero, duration);
        targetRectTransform.DOSizeDelta(Vector2.zero, duration);
        //StartCoroutine(PanelDisabled());
    }

    IEnumerator PanelDisabled()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
