using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Room = HouseController.Room;

public class ActivitiesHandlerController : MonoBehaviour
{
    public enum ActivityVariety 
    {
        OBLIGATION,
        PROBLEM
    }

    public enum ActivityType 
    {
        BEER_PONG,
        CHINA_CABINET,
        KEG_STAND,
        BATHROOM
    }

    public struct ActivityBounds
    {
        public ActivityBounds(Vector2 topLeft, Vector2 bottomRight)
        {
            this.topLeft = topLeft; this.bottomRight = bottomRight;
        }
        public Vector2 topLeft;
        public Vector2 bottomRight;
    }
    public ActivityType[] typeOrder = new ActivityType[4];
    public GameObject[] orderedActivityHandlerObjects = new GameObject[4];

    private Dictionary<ActivityBounds, ActivityType> activityLocations = new Dictionary<ActivityBounds, ActivityType>();
    private Dictionary<ActivityType, ActivityController> activities = new Dictionary<ActivityType, ActivityController>();
    private Dictionary<ActivityType, GameObject> activityHandlerObjects = new Dictionary<ActivityType, GameObject>();

    void Awake()
    {
        if (!activityHandlerObjects.Any())
            for(int i = 0; i < typeOrder.Length; i++)
                activityHandlerObjects.Add(typeOrder[i], orderedActivityHandlerObjects[i]);
    }

    public void ReportBounds(ActivityController controller, ActivityType type, ActivityBounds bounds)
    {
        activityLocations.Add(bounds, type);
        activities.Add(type, controller);
    }

    public GameObject GetActivityHandlerObject(ActivityType type) => activityHandlerObjects.GetDictValue(type);
    
    public ActivityController SearchByPosition(Room targetRoom, Vector2 position)
    {
        ActivityBounds[] candidateBounds = activityLocations.Keys.Where(
            bounds => 
                bounds.topLeft.x <= position.x
                && bounds.topLeft.y >= position.y
                && bounds.bottomRight.x >= position.x
                && bounds.bottomRight.y <= position.y
                && activities.GetDictValue(activityLocations.GetDictValue(bounds)).GetRoom() == targetRoom
        ).ToArray();
        return candidateBounds.Length == 0 
            ? null 
            : activities.GetDictValue(activityLocations.GetDictValue(candidateBounds[0]));
    } 

    public ActivityController SearchByVariety(Room targetRoom, ActivityVariety targetVariety)
    {
        ActivityType[] candidateTypes = activities.Keys.Where(
            type => 
                activities.GetDictValue(type).GetVariety() == targetVariety
                && activities.GetDictValue(type).GetRoom() == targetRoom
        ).ToArray();
        return candidateTypes.Length == 0
            ? null
            : activities.GetDictValue(candidateTypes[0]);
    }
}