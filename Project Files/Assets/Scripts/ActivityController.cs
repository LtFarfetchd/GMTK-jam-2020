using UnityEngine;
using ActivityType = ActivitiesHandlerController.ActivityType;
using ActivityBounds = ActivitiesHandlerController.ActivityBounds;
using ActivityVariety = ActivitiesHandlerController.ActivityVariety;
using Room = HouseController.Room;

public class ActivityController : MonoBehaviour
{
    public GameObject activitiesHandler;
    public Vector2 boundsTopLeft;
    public Vector2 boundsBottomRight;
    public Room room;
    public Vector2 engagementPosition;
    public ActivityType type;
    public ActivityVariety variety;

    private ActivitiesHandlerController ahc;
    private bool hasWarning = false;
    
    void Start()
    {
        ahc = (ActivitiesHandlerController)activitiesHandler.GetComponent<MonoBehaviour>();
        ahc.ReportBounds(this, type, new ActivityBounds(boundsTopLeft, boundsBottomRight));
    }

    public Vector2 GetEngagementPosition() => engagementPosition;
    public Room GetRoom() => room;
    public ActivityVariety GetVariety() => variety;

    public void InitiateWarning()
    {
        hasWarning = true;
    }

    public void StopWarning()
    {
        hasWarning = false;
    }
}