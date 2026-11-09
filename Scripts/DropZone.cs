using UnityEngine;
using System.Collections;

public class DropZone : MonoBehaviour
{
    public GameObject correctObject;
    public AdaptiveAI aiManager;
    
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == correctObject)
        {
            Debug.Log("Ugurlu!");

            collision.transform.position = transform.position;

            DragAndDrop drag = collision.GetComponent<DragAndDrop>();

            if (drag != null)
            {
                drag.enabled = false;
            }

            StartCoroutine(ScaleDownAndDisappear(collision.gameObject));

            if (aiManager != null)
            {
                aiManager.TaskCompleted();
                GameObject.Find("LevelManager").SendMessage("NextLevel");
            }
        }
    }

    IEnumerator ScaleDownAndDisappear(GameObject obj)
    {
        float duration = 0.25f;
        float elapsed = 0f;

        Vector3 originalScale = obj.transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            obj.transform.localScale = Vector3.Lerp(
                originalScale,
                Vector3.zero,
                t
            );

            yield return null;
        }

        obj.transform.localScale = Vector3.zero;

        obj.SetActive(false);
    }
}