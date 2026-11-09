using UnityEngine;
using System.Collections;

public class ScaleBalance : MonoBehaviour
{
    public Transform scaleArm; 
    public GameObject correctWeight; 
    public AdaptiveAI aiManager; 

    public float tiltedAngle = 20f; 

    void Start()
    {
      
        if (scaleArm != null)
        {
            scaleArm.localRotation = Quaternion.Euler(-98, 90, -90);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == correctWeight)
        {
            Debug.Log("Terezi tapşırığı uğurludur!");

          
            other.GetComponent<DragAndDrop>().enabled = false;

           
            other.transform.position = transform.position;

           
            other.transform.SetParent(scaleArm);


            StartCoroutine(BalanceTheScale());

            
            if (aiManager != null)
            {
                aiManager.TaskCompleted();
            }
        }
    }


    IEnumerator BalanceTheScale()
    {
        Quaternion startRot = scaleArm.localRotation;
        Quaternion targetRot = Quaternion.Euler(-90, 90, -90); 
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f; 
          
            scaleArm.localRotation = Quaternion.Lerp(startRot, targetRot, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        scaleArm.localRotation = targetRot; 
    }
}