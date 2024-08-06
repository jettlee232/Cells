using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VRKeyboard_Mito : MonoBehaviour
{
    public InputField AttachedInputField;

    public bool UseShift = false;

    [Header("Sound FX")]
    public AudioClip KeyPressSound;

    List<VRKeyboardKey> KeyboardKeys;

    // �ѱ� ������ ���� ������
    private string cho = "";
    private string jung = "";
    private string jong = "";

    private Dictionary<string, int> choMap = new Dictionary<string, int> {
            {"��", 0}, {"��", 1}, {"��", 2}, {"��", 3}, {"��", 4}, {"��", 5}, {"��", 6}, {"��", 7},
            {"��", 8}, {"��", 9}, {"��", 10}, {"��", 11}, {"��", 12}, {"��", 13}, {"��", 14}, {"��", 15},
            {"��", 16}, {"��", 17}, {"��", 18}
        };

    private Dictionary<string, int> jungMap = new Dictionary<string, int> {
            {"��", 0}, {"��", 1}, {"��", 2}, {"��", 3}, {"��", 4}, {"��", 5}, {"��", 6}, {"��", 7},
            {"��", 8}, {"��", 9}, {"��", 10}, {"��", 11}, {"��", 12}, {"��", 13}, {"��", 14}, {"��", 15},
            {"��", 16}, {"��", 17}, {"��", 18}, {"��", 19}, {"��", 20}
        };

    private Dictionary<string, int> jongMap = new Dictionary<string, int> {
            {"", 0}, {"��", 1}, {"��", 2}, {"��", 3}, {"��", 4}, {"��", 5}, {"��", 6}, {"��", 7}, {"��", 8},
            {"��", 9}, {"��", 10}, {"��", 11}, {"��", 12}, {"��", 13}, {"��", 14}, {"��", 15}, {"��", 16},
            {"��", 17}, {"��", 18}, {"��", 19}, {"��", 20}, {"��", 21}, {"��", 22}, {"��", 23}, {"��", 24},
            {"��", 25}, {"��", 26}, {"��", 27}
        };

    void Awake()
    {
        KeyboardKeys = transform.GetComponentsInChildren<VRKeyboardKey>().ToList();
    }

    public void PressKey(string key)
    {
        if (AttachedInputField != null)
        {
            UpdateInputField(key);
        }
        else
        {
            Debug.Log("Pressed Key : " + key);
        }
    }

    public void UpdateInputField(string key)
    {
        string currentText = AttachedInputField.text;
        int caretPosition = AttachedInputField.caretPosition;
        int textLength = currentText.Length;
        bool caretAtEnd = AttachedInputField.isFocused == false || caretPosition == textLength;

        Debug.Log($"Key pressed: {key}, Current text: {currentText}, Caret position: {caretPosition}");

        string formattedKey = key;
        if (key.ToLower() == "space")
        {
            formattedKey = " ";
        }

        if (formattedKey.ToLower() == "backspace")
        {
            if (caretPosition == 0)
            {
                PlayClickSound();
                return;
            }

            currentText = currentText.Remove(caretPosition - 1, 1);

            if (!caretAtEnd)
            {
                MoveCaretBack();
            }
        }
        else if (formattedKey.ToLower() == "enter")
        {
            // ó���� �ʿ䰡 �ִ� ��� ����
        }
        else if (formattedKey.ToLower() == "shift")
        {
            ToggleShift();
        }
        else
        {
            CombineHangul(formattedKey);
            return;  // �ѱ� ���� ó���� ���⼭ �Ϸ�
        }

        AttachedInputField.text = currentText;

        Debug.Log($"Updated text: {currentText}");

        PlayClickSound();

        if (!AttachedInputField.isFocused)
        {
            AttachedInputField.Select();
        }
    }

    private void CombineHangul(string key)
    {
        bool updated = false;

        if (choMap.ContainsKey(key))
        {
            if (cho == "")
            {
                cho = key;
                updated = true;
            }
            else if (jung == "")
            {
                cho = key;  // ���ο� �ʼ����� ����
                updated = true;
            }
            else
            {
                jong = key;  // �������� ����
                updated = true;
            }
        }
        else if (jungMap.ContainsKey(key))
        {
            if (cho == "")
            {
                cho = key;  // ���ο� �ʼ����� ����
                updated = true;
            }
            else if (jung == "")
            {
                jung = key;
                updated = true;
            }
            else
            {
                // �߼��� �̹� �ִ� ���, ���ο� ���� ����
                cho = key;
                jung = "";
                jong = "";
                updated = true;
            }
        }
        else if (jongMap.ContainsKey(key))
        {
            jong = key;
            updated = true;
        }

        if (updated)
        {
            int choIndex = choMap.ContainsKey(cho) ? choMap[cho] : 0;
            int jungIndex = jungMap.ContainsKey(jung) ? jungMap[jung] : 0;
            int jongIndex = jongMap.ContainsKey(jong) ? jongMap[jong] : 0;

            char hangulChar = (char)(0xAC00 + (choIndex * 21 * 28) + (jungIndex * 28) + jongIndex);
            string currentText = AttachedInputField.text;
            int caretPosition = AttachedInputField.caretPosition;

            if (caretPosition > 0)
            {
                currentText = currentText.Substring(0, caretPosition - 1) + hangulChar + currentText.Substring(caretPosition);
            }
            else
            {
                currentText = hangulChar + currentText.Substring(caretPosition);
            }

            AttachedInputField.text = currentText;

            // ������ �Ϸ�Ǹ� Ŀ�� �̵�
            MoveCaretUp();

            // ������ �Ϸ�Ǹ� �ʱ�ȭ
            if (cho != "" && jung != "")
            {
                cho = "";
                jung = "";
                jong = "";
            }
        }
    }

    public virtual void PlayClickSound()
    {
        if (KeyPressSound != null)
        {
            VRUtils.Instance.PlaySpatialClipAt(KeyPressSound, transform.position, 1f, 0.5f);
        }
    }

    public void MoveCaretUp()
    {
        StartCoroutine(IncreaseInputFieldCaretRoutine());
    }

    IEnumerator IncreaseInputFieldCaretRoutine()
    {
        yield return new WaitForEndOfFrame();
        AttachedInputField.caretPosition = AttachedInputField.caretPosition + 1;
        AttachedInputField.ForceLabelUpdate();
    }

    public void MoveCaretBack()
    {
        StartCoroutine(DecreaseInputFieldCareteRoutine());
    }

    public void ToggleShift()
    {
        UseShift = !UseShift;

        foreach (var key in KeyboardKeys)
        {
            if (key != null)
            {
                key.ToggleShift();
            }
        }
    }

    IEnumerator IncreaseInputFieldCareteRoutine()
    {
        yield return new WaitForEndOfFrame();
        AttachedInputField.caretPosition = AttachedInputField.caretPosition + 1;
        AttachedInputField.ForceLabelUpdate();
    }

    IEnumerator DecreaseInputFieldCareteRoutine()
    {
        yield return new WaitForEndOfFrame();
        AttachedInputField.caretPosition = AttachedInputField.caretPosition - 1;
        AttachedInputField.ForceLabelUpdate();
    }

    public void AttachToInputField(InputField inputField)
    {
        AttachedInputField = inputField;
    }
}