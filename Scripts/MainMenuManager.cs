using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public GameObject levelManager;

    private int currentQuestionIndex = 0;
    private bool isAnimating = false; // Animasiya gedərkən dubl-kliki bloklamaq üçün

    private string[] questions = {
        "1. To what extent is your child able to independently comprehend visual and written documents?",
        "2. How do you evaluate your child's ability to solve logical problems?",
        "3. Is your child able to fully focus on a task in environments where visual objects are present?"
    };

    private int[] parentAnswers = new int[3];

    void Start()
    {
        startPanel.SetActive(true);
        quizPanel.SetActive(false);
        if (levelManager != null) levelManager.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        startPanel.SetActive(false);
        quizPanel.SetActive(true);
        UpdateQuestionUI();

        // İlk sual da səhnəyə animasiya ilə daxil olsun
        StartCoroutine(FadeTextIn());
    }

    public void OnAnswerClicked(int score)
    {
        // Əgər hazırda keçid animasiyası gedirsə, düymə basılmasını qeydə alma
        if (isAnimating) return;

        parentAnswers[currentQuestionIndex] = score;
        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
        {
            // Birbaşa dəyişmək əvəzinə animasiyalı keçidi başladır
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            FinishQuizAndStartGame();
        }
    }

    private void UpdateQuestionUI()
    {
        questionText.text = questions[currentQuestionIndex];
    }

    // --- YENİ ƏLAVƏ EDİLƏN ANİMASİYA KODLARI ---

    IEnumerator TransitionToNextQuestion()
    {
        isAnimating = true;

        // 1. Köhnə mətni yavaşca gizlət (0.3 saniyə)
        yield return StartCoroutine(FadeTextOut());

        // 2. Sualı arxa fonda dəyiş
        UpdateQuestionUI();

        // 3. Yeni mətni yavaşca göstər (0.3 saniyə)
        yield return StartCoroutine(FadeTextIn());

        isAnimating = false;
    }

    IEnumerator FadeTextOut()
    {
        float duration = 0.3f;
        Color c = questionText.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(1, 0, t / duration);
            questionText.color = c;
            yield return null; // Növbəti kadrı gözlə
        }
        c.a = 0;
        questionText.color = c;
    }

    IEnumerator FadeTextIn()
    {
        float duration = 0.3f;
        Color c = questionText.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(0, 1, t / duration);
            questionText.color = c;
            yield return null;
        }
        c.a = 1;
        questionText.color = c;
    }

    // --- QALAN HİSSƏLƏR EYNİDİR ---

    private void FinishQuizAndStartGame()
    {
        quizPanel.SetActive(false);

        string profileData = $@"{{
            ""qavrama"": {parentAnswers[0]},
            ""mentiq"": {parentAnswers[1]},
            ""fokus"": {parentAnswers[2]}
        }}";

        Debug.Log("Profil Node.js serverinə göndərilir: " + profileData);
        StartCoroutine(SendProfileData(profileData));

        if (levelManager != null)
        {
            levelManager.SetActive(true);
        }
    }

    IEnumerator SendProfileData(string jsonData)
    {
        string url = "http://localhost:5000/api/profile";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Profil uğurla göndərildi: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Server xətası: " + request.error);
        }
    }
}