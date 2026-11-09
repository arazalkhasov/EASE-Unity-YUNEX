using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels; 
    private int currentLevelIndex = 0;

    void Start()
    {
  
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(i == 0);
        }
    }

    public void NextLevel()
    {
   
        if (currentLevelIndex < levels.Length)
        {
            levels[currentLevelIndex].SetActive(false);
        }

        currentLevelIndex++;

      
        if (currentLevelIndex < levels.Length)
        {
            levels[currentLevelIndex].SetActive(true);
            Debug.Log("Səviyyə dəyişdi: " + currentLevelIndex);
        }
        else
        {
            Debug.Log("Bütün səviyyələr bitdi!");
        }
    }
}