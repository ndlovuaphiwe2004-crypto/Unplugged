using UnityEngine;

public class RiddleManager : MonoBehaviour
{
    public static RiddleManager Instance;
    private int currentRiddleIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public bool CanShowRiddle(int riddleIndex)
    {
      
        return riddleIndex <= currentRiddleIndex;
    }


    public void RiddleSolved()
    {
        currentRiddleIndex++;
        Debug.Log("Riddle solved. Next index unlocked: " + currentRiddleIndex);
    }

}

