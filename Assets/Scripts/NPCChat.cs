using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

public class NPCChat : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text outputText;

    // 換成你的 loca.lt 網址
    private string url =
    "https://eight-emus-stay.loca.lt/ask";

    public void AskNPC()
    {
        StartCoroutine(SendQuestion());
    }

    IEnumerator SendQuestion()
    {
        outputText.text = "NPC is thinking...";

        string json =
        "{\"question\":\"" +
        inputField.text +
        "\"}";

        UnityWebRequest request =
        new UnityWebRequest(url, "POST");

        request.timeout = 40;

        byte[] bodyRaw =
        Encoding.UTF8.GetBytes(json);

        request.uploadHandler =
        new UploadHandlerRaw(bodyRaw);

        request.downloadHandler =
        new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            outputText.text =
            "Connection error:\n" +
            request.error;
        }
        else
        {
            NPCResponse response =
                JsonUtility.FromJson<NPCResponse>(
                    request.downloadHandler.text
                );

            outputText.text = response.answer;
        }
    }
}

[System.Serializable]
public class NPCResponse
{
    public string answer;
}