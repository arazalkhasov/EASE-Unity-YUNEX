using UnityEngine;
using System.Collections; 

public class DragAndDrop : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public AdaptiveAI aiManager;


    private Vector3 startPosition; 
    private bool isReturning = false; 
    public float returnSpeed = 3f; 

    void Start()
    {
        
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        
        if (isReturning) return;

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        if (aiManager != null)
        {
            aiManager.RegisterClick();
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        if (isReturning) return;
        transform.position = GetMouseAsWorldPoint() + mOffset;
    }

   
    void OnMouseUp()
    {
        if (isReturning) return;

       
        StartCoroutine(ReturnToStart());
    }


    IEnumerator ReturnToStart()
    {
        isReturning = true;
        Vector3 currentPos = transform.position;
        float t = 0f;

        while (t < 1f)
        {
           
            if (!this.enabled) yield break;

            t += Time.deltaTime * returnSpeed;
            
            transform.position = Vector3.Lerp(currentPos, startPosition, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        transform.position = startPosition;
        isReturning = false;
    }
}