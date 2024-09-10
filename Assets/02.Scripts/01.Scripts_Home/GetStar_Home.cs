using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetStar_Home : MonoBehaviour
{
    public GameObject starWatch;

    public Button closeBtn;
    public Transform starsParent;

    public TextMeshProUGUI starsText;

    public GameObject starEffect;

    void Start()
    {
        if (starsText == null)
        {
            starWatch = GameObject.FindGameObjectWithTag("Watch");

            starsText = starWatch.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (closeBtn == null)
        {
            GameObject go = transform.Find("List").Find("Button_Close").gameObject;
            if (!go.GetComponent<Button>())
                go.AddComponent<Button>();
            closeBtn = go.GetComponent<Button>();
        }

        closeBtn.onClick.AddListener(GetStarScore);

        starEffect = Resources.Load<GameObject>("MyGetStarEffect");

        UpdateScoreDisplay();
    }

    public void GetStarScore()
    {
        starsParent = transform.Find("Stars");

        if (starsParent != null)
        {
            int starCount = starsParent.childCount;

            if (starCount > 0)
            {
                // PlayerScore�� �� ó�� ���� �����Ҷ� �ʱ�ȭ? ���� ����? �װ� ���߿� ����
                // Star ������Ʈ ������ Ȯ���Ͽ� PlayerScore ����
                int currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
                currentScore += starCount;
                PlayerPrefs.SetInt("PlayerScore", currentScore);
                UpdateScoreDisplay();

                // Star ������Ʈ�� �Ծ����� ȿ��
                StartCoroutine(PlayStarEffects(starWatch.transform));
            }
        }
    }

    public void UpdateScoreDisplay()
    {
        int currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
        starsText.text = currentScore.ToString();
        Debug.Log("���� ���� : " + currentScore);
    }

    public IEnumerator PlayStarEffects(Transform tr)
    {
        //List<Vector3> starPositions = new List<Vector3>();
        //foreach (Transform star in starsParent)
        //{
        //    starPositions.Add(star.localPosition);
        //}

        foreach (Transform starPos in starsParent)
        {
            GameObject effect = Instantiate(starEffect, starPos.position, starEffect.transform.rotation);

            effect.transform.SetParent(tr);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
