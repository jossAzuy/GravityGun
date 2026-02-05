
using MoreMountains.Feedbacks;
using UnityEngine;

public class TestingFeel : MonoBehaviour
{
    [Header("Testing Feel")] 
    public MMF_Player textfeedback; // Drag your MMF_Feedback here from the inspector
    public MMF_Player textfeedback2; // Drag your MMF_Feedback here from the inspector

    // Start is called before the first frame update
    void Start()
    {
        textfeedback.PlayFeedbacks(); // Play the feedback when the game starts
    }

    // Update is called once per frame
    void Update()
    {
        textfeedback2.PlayFeedbacks();
    }
}
