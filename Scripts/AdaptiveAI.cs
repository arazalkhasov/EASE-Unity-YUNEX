using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AdaptiveAI : MonoBehaviour
{
    public float idleTimeLimit = 5f;
    private float currentIdleTime = 0f;
    private float totalSessionTime = 0f; 
    private int erraticClicks = 0;

    public GameObject targetObject;
    public GameObject handPointer;

    public string taskName = "Meyvə Tapşırığı"; 
    void Start()
    {
        if (handPointer != null)
        {
            handPointer.SetActive(false);
        }
    }

    void Update()
    {
 
        currentIdleTime += Time.deltaTime;
        totalSessionTime += Time.deltaTime; 

        if (currentIdleTime > idleTimeLimit)
        {
            ProvideVisualHint();
        }
    }

    public void RegisterClick()
    {
        currentIdleTime = 0f; 
        erraticClicks++;
        RemoveVisualHint();
    }

    private void ProvideVisualHint()
    {
        if (handPointer != null && !handPointer.activeSelf)
        {
            handPointer.SetActive(true);
        }
    }

    private void RemoveVisualHint()
    {
        if (handPointer != null && handPointer.activeSelf)
        {
            handPointer.SetActive(false);
        }
    }

    private bool isTaskFinished = false;

    public void TaskCompleted()
    {
        if (isTaskFinished) return;
        isTaskFinished = true;

        RemoveVisualHint();

        string sessionData = $@"{{
            ""taskName"": ""{taskName}"",
            ""timeSpent"": {Mathf.Round(totalSessionTime)},
            ""erraticClicks"": {erraticClicks},
            ""success"": true
        }}";

        Debug.Log("Valideyn Dashboard-una gonderilecek Data: " + sessionData);
        StartCoroutine(SendData(sessionData));
    }

    IEnumerator SendData(string jsonData)
    {
        string url = "http://localhost:5000/api/telemetry";

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Mesaj sends succesfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Server error: " + request.error);
        }
    }
}