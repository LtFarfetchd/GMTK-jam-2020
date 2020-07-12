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
    }

    public int GetNoisiness() => (int)(noisiness * 100);
}
