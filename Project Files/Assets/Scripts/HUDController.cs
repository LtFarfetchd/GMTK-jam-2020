using UnityEngine;
using ActivityVariety = ActivitiesHandlerController.ActivityVariety;

public class HUDController : MonoBehaviour
{
    public struct StatLevel
    {
        public StatLevel(ActivityVariety statVariety, float level, int activityCount)
        {
            this.statVariety = statVariety; this.level = level; this.activityCount = activityCount;
        }
        public ActivityVariety statVariety;
        public float level;
        public int activityCount;
    }
    public GameObject house;
    public GameObject noiseGaugeFill;
    public GameObject partyGaugeFill;
    public int maxGaugeFillScale = 242;

    private HouseController houseController;
    public float noisiness = 0f;
    
    void Start()
    {
        houseController = (HouseController)house.GetComponent<MonoBehaviour>();
    }

    void Update()
    {
        StatLevel problemLevel = houseController.GetTotalStatLevel(ActivityVariety.PROBLEM);
        StatLevel obligationLevel = houseController.GetTotalStatLevel(ActivityVariety.OBLIGATION);
        float maximumLevel = 
            houseController.GetRoomGaugeMaximum() * (problemLevel.activityCount + obligationLevel.activityCount);
        float currentLevel = problemLevel.level + obligationLevel.level;
        noisiness = (currentLevel/maximumLevel);

        noiseGaugeFill.transform.localScale = new Vector3(
            1,
            problemLevel.level / (houseController.GetRoomGaugeMaximum() * problemLevel.activityCount) * maxGaugeFillScale ,
            1
        );

        partyGaugeFill.transform.localScale = new Vector3(
            1,
            obligationLevel.level / (houseController.GetRoomGaugeMaximum() * obligationLevel.activityCount) * maxGaugeFillScale ,
            1
        );

        if ((int)noisiness == 1)
            EndGame();
    }

    public int GetNoisiness() => (int)(noisiness * 100);

    private void EndGame()
    {
        Debug.Log("Game Over");
    }
}
