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

    // 한글 조합을 위한 변수들
    private string cho = "";
    private string jung = "";
    private string jong = "";

    private Dictionary<string, int> choMap = new Dictionary<string, int> {
            {"ㄱ", 0}, {"ㄲ", 1}, {"ㄴ", 2}, {"ㄷ", 3}, {"ㄸ", 4}, {"ㄹ", 5}, {"ㅁ", 6}, {"ㅂ", 7},
            {"ㅃ", 8}, {"ㅅ", 9}, {"ㅆ", 10}, {"ㅇ", 11}, {"ㅈ", 12}, {"ㅉ", 13}, {"ㅊ", 14}, {"ㅋ", 15},
            {"ㅌ", 16}, {"ㅍ", 17}, {"ㅎ", 18}
        };

    private Dictionary<string, int> jungMap = new Dictionary<string, int> {
            {"ㅏ", 0}, {"ㅐ", 1}, {"ㅑ", 2}, {"ㅒ", 3}, {"ㅓ", 4}, {"ㅔ", 5}, {"ㅕ", 6}, {"ㅖ", 7},
            {"ㅗ", 8}, {"ㅘ", 9}, {"ㅙ", 10}, {"ㅚ", 11}, {"ㅛ", 12}, {"ㅜ", 13}, {"ㅝ", 14}, {"ㅞ", 15},
            {"ㅟ", 16}, {"ㅠ", 17}, {"ㅡ", 18}, {"ㅢ", 19}, {"ㅣ", 20}
        };

    private Dictionary<string, int> jongMap = new Dictionary<string, int> {
            {"", 0}, {"ㄱ", 1}, {"ㄲ", 2}, {"ㄳ", 3}, {"ㄴ", 4}, {"ㄵ", 5}, {"ㄶ", 6}, {"ㄷ", 7}, {"ㄹ", 8},
            {"ㄺ", 9}, {"ㄻ", 10}, {"ㄼ", 11}, {"ㄽ", 12}, {"ㄾ", 13}, {"ㄿ", 14}, {"ㅀ", 15}, {"ㅁ", 16},
            {"ㅂ", 17}, {"ㅄ", 18}, {"ㅅ", 19}, {"ㅆ", 20}, {"ㅇ", 21}, {"ㅈ", 22}, {"ㅊ", 23}, {"ㅋ", 24},
            {"ㅌ", 25}, {"ㅍ", 26}, {"ㅎ", 27}
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
            // 처리할 필요가 있는 경우 구현
        }
        else if (formattedKey.ToLower() == "shift")
        {
            ToggleShift();
        }
        else
        {
            CombineHangul(formattedKey);
            return;  // 한글 조합 처리는 여기서 완료
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
                cho = key;  // 새로운 초성으로 시작
                updated = true;
            }
            else
            {
                jong = key;  // 종성으로 설정
                updated = true;
            }
        }
        else if (jungMap.ContainsKey(key))
        {
            if (cho == "")
            {
                cho = key;  // 새로운 초성으로 시작
                updated = true;
            }
            else if (jung == "")
            {
                jung = key;
                updated = true;
            }
            else
            {
                // 중성이 이미 있는 경우, 새로운 글자 시작
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

            // 조합이 완료되면 커서 이동
            MoveCaretUp();

            // 조합이 완료되면 초기화
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