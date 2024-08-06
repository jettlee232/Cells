using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class STTManager : MonoBehaviour
{
    [Serializable]
    public class VoiceRecognize
    {
        public string text;
    }

    public string microphoneID = null;
    public AudioClip recording = null;
    //public AudioSource source = null;

    private int recordingLengthSec = 15;
    float floatRecordingLengthSec = 15.0f;
    private int recordingHZ = 22050;

    public string lang = "Kor";    // 언어 코드 ( Kor, Jpn, Eng, Chn )
    public string url = "https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=";
    string fullUrl;

    //public TextMeshProUGUI resultText;
    public TMP_InputField inputField;
    public TextMeshProUGUI recordText;

    #region API 정보 취급주의
    // API 정보
    private string clientId = "qhtlcg8b9s";
    private string clientSecret = "VkXw2QeXWqte8ztAkUameyIGFK3CW7sNnC21e1I5";
    #endregion

    public GameObject startButton;
    public GameObject stopButton;

    public Image timerImage;
    private Coroutine timerCoroutine;

    private void Start()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Microphone device: " + device);
        }

        //string desiredMicrophoneName = "Headset Microphone(Oculus Virtual Audio Device)";
        //if (Array.Exists(Microphone.devices, device => device == desiredMicrophoneName))
        //{
        //    microphoneID = desiredMicrophoneName;
        //}
        //else
        //{
        //    microphoneID = Microphone.devices[0];
        //}
        //microphoneID = Microphone.devices[1];
        //source = GetComponent<AudioSource>();
        fullUrl = url + lang;

        startButton.SetActive(true);
        stopButton.SetActive(false);

        if (recordText != null)
            recordText.gameObject.SetActive(false);

        timerImage.fillAmount = 0;
    }

    private void Update()
    {
        if (recordText != null)
        {
            if (Microphone.IsRecording(microphoneID))
                recordText.gameObject.SetActive(true);
            else
                recordText.gameObject.SetActive(false);
        }
    }

    public void StartRecording()
    {
        if (Microphone.IsRecording(microphoneID))
        {
            return;
        }

        Debug.Log("start recording");
        recording = Microphone.Start(microphoneID, false, recordingLengthSec, recordingHZ);

        startButton.SetActive(false);
        stopButton.SetActive(true);

        StartCoroutine(StopRecordingAfterDelay(floatRecordingLengthSec));
        timerCoroutine = StartCoroutine(UpdateTimer(floatRecordingLengthSec));
    }

    private IEnumerator StopRecordingAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        StopRecording();
    }

    private IEnumerator UpdateTimer(float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            timerImage.fillAmount = timeElapsed / duration;
            yield return null;
        }

        // 타이머가 끝나면 fillAmount를 1로 설정 (만약 중간에 StopRecording이 호출되지 않았다면)
        timerImage.fillAmount = 1f;
    }

    public void StopRecording()
    {
        Debug.Log("스탑 레코딩 버튼은 눌림");
        if (!Microphone.IsRecording(microphoneID))
        {
            Debug.Log("리턴");
            return;
        }
        else
        {
            Microphone.End(microphoneID);

            Debug.Log("stop recording");
            if (recording == null)
            {
                Debug.LogError("nothing recorded");
                return;
            }

            // AudioClip을 byte array로 변환
            byte[] byteData = WavUtility.FromAudioClip(recording);
            //byte[] byteData = ConvertAudioClipToByteArray(recording);

            // 녹음된 AudioClip을 API 서버로 전송
            if (byteData != null)
                StartCoroutine(PostVoice(fullUrl, byteData));

            // AudioSource에 녹음된 Clip 재생
            //source.clip = recording;

            startButton.SetActive(true);
            stopButton.SetActive(false);

            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            timerImage.fillAmount = 0;
        }
    }

    /*
    private byte[] ConvertAudioClipToByteArray(AudioClip audioClip)
    {
        MemoryStream stream = new MemoryStream();
        const int headerSize = 44;
        ushort bitDepth = 16;

        int fileSize = audioClip.samples * BlockSize_16Bit + headerSize;

        // audio clip의 정보들을 file stream에 추가(링크 참고 함수 선언)
        WriteFileHeader(ref stream, fileSize);
        WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
        WriteFileData(ref stream, audioClip, bitDepth);

        // stream을 array형태로 바꿈
        byte[] bytes = stream.ToArray();

        return bytes;
    }
    */

    private IEnumerator PostVoice(string url, byte[] voiceData)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", clientId);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", clientSecret);
        request.SetRequestHeader("Content-Type", "application/octet-stream");

        request.uploadHandler = new UploadHandlerRaw(voiceData);

        yield return request.SendWebRequest();

        if (request == null)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            VoiceRecognize voiceRecognize = JsonUtility.FromJson<VoiceRecognize>(responseText);

            Debug.Log(voiceRecognize.text);

            /*
            if (resultText != null)
            {
                resultText.text = voiceRecognize.text;
            }
            */

            if (inputField != null)
            {
                inputField.text = voiceRecognize.text;
            }
        }
    }
}
