using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControll_CM : MonoBehaviour
{
    public BNG.MyFader_CM scrFader;
    public BNG.PlayerGravity playerGrav; 

    public Vector3 initPos;

    void Start()
    {        
        StartCoroutine(CheckPos());
    }

    IEnumerator CheckPos()
    {
        while (true)
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                Vector3 currentPosition = transform.position;

                // x���� �� �������� 30 �̻� ���������� Ȯ��
                if (Mathf.Abs(currentPosition.x - initPos.x) >= 7f)
                {
                    StartCoroutine(ScreenFadeInAndOut());
                    break;
                }

                // y���� �������� -5 ���Ϸ� ���������� Ȯ��
                if (currentPosition.y <= initPos.y - 2f)
                {
                    StartCoroutine(ScreenFadeInAndOut());
                    break;
                }

                // z���� �� �������� 30 �̻� ���������� Ȯ��
                if (Mathf.Abs(currentPosition.z - initPos.z) >= 7f)
                {
                    StartCoroutine(ScreenFadeInAndOut());
                    break;
                }
            }

            yield return new WaitForSeconds(2f);       
        }       
    }

    IEnumerator ScreenFadeInAndOut()
    {
        playerGrav.enabled = false;
        scrFader.ChangeFadeImageColor(Color.black, 6f, 1f);
        scrFader.DoFadeIn();        

        yield return new WaitForSeconds(1f);
        
        ResetPlayerPosition();

        yield return new WaitForSeconds(0.1f);
        playerGrav.enabled = true;

        scrFader.DoFadeOut();        
    }

    void ResetPlayerPosition()
    {       
        transform.position = initPos;
    }
}
