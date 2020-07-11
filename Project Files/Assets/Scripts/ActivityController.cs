using UnityEngine;
using ActivityType = ActivitiesHandlerController.ActivityType;
using ActivityBounds = ActivitiesHandlerController.ActivityBounds;
using Room = HouseController.Room;

public class ActivityController : MonoBehaviour
{
    public GameObject activitiesHandler;
    public Vector2 boundsTopLeft;
    public Vector2 boundsBottomRight;
    public Room room;
    public Vector2 engagementPosition;
    public ActivityType type;
    
    private ActivitiesHandlerController ahc;
    
    void Start()
    {
        ahc = (ActivitiesHandlerController)activitiesHandler.GetComponent<MonoBehaviour>();
        ahc.ReportBounds(this, type, new ActivityBounds(boundsTopLeft, boundsBottomRight));
    }

    public Vector2 GetEngagementPosition() => engagementPosition;
    public Room GetRoom() => room;
}