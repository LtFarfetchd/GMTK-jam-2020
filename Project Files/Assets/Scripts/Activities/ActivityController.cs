using UnityEngine;
using ActivityType = ActivitiesHandlerController.ActivityType;
using ActivityBounds = ActivitiesHandlerController.ActivityBounds;
using ActivityVariety = ActivitiesHandlerController.ActivityVariety;
using Room = HouseController.Room;

public class ActivityController : MonoBehaviour
{
    public GameObject activitiesHandler;
    public float widthInWorldUnits;
    public float heightInWorldUnits;
    public Room room;
    [DraggablePoint]
    public Vector3 engagementPosition;
    public ActivityType type;
    public ActivityVariety variety;
    public GameObject warning;

    private GameObject minigameHandlerObject;
    private ActivitiesHandlerController ahc;
    private bool hasWarning = false;
    
    void Start()
    {
        ahc = (ActivitiesHandlerController)activitiesHandler.GetComponent<MonoBehaviour>();
        ahc.ReportBounds(this, type, new ActivityBounds(
            new Vector2(transform.position.x - widthInWorldUnits/2, transform.position.y + heightInWorldUnits/2)
            , new Vector2(transform.position.x + widthInWorldUnits/2, transform.position.y - heightInWorldUnits/2)
        ));
        
        warning.transform.position = transform.position;
        warning.SetActive(false);

        minigameHandlerObject = ahc.GetActivityHandlerObject(type);
        minigameHandlerObject.transform.position = new Vector3(
            engagementPosition.x, engagementPosition.y, minigameHandlerObject.transform.position.z
        );
        minigameHandlerObject.SetActive(false);
    }

    public Vector2 GetEngagementPosition() => engagementPosition;
    public Room GetRoom() => room;
    public ActivityVariety GetVariety() => variety;

    public void ToggleWarning(bool isWarning)
    {
        hasWarning = isWarning;
        warning.SetActive(isWarning);
    }
}