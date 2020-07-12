using System.Collections.Generic;
using UnityEngine;
using ActivityVariety = ActivitiesHandlerController.ActivityVariety;

public class HouseController : MonoBehaviour
{
    public enum Room {
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT
    }

    public class RoomStat {
        public ActivityVariety variety;
        public float value;
        public RoomStat(ActivityVariety variety)
        {
            this.variety = variety;
            value = 0f;
        }
        public void IncreaseValue(float max, float amount)
        {
            if (value + amount < max)
                value += amount;
            else
                value = max;
        }
    }

    public GameObject activitiesHandler;
    public float houseWidth;
    public float[] roomStartTimes = new float[4];
    public float gaugeMaximum = 20f;
    public float gaugeIncreaseRate = 2f;
    public float gaugeIncreaseAcceleration = 0.00025f;
    public int gaugeWarningPercentage = 50;

    private Dictionary<Room, Vector2> roomPositions = new Dictionary<Room, Vector2>();
    private Dictionary<Room, List<RoomStat>> roomStats = new Dictionary<Room, List<RoomStat>>();
    private LinkedList<Room> roomPaths = new LinkedList<Room>();
    private ActivitiesHandlerController ahc;
    private float roomOffset;
    private int roomValueSecondsBetweenIncreases = 5;

    void Start()
    {
        ahc = (ActivitiesHandlerController)(activitiesHandler.GetComponent<MonoBehaviour>());
        roomOffset = houseWidth / 4;

        roomPositions.ChainAdd(Room.TOP_LEFT, new Vector2(transform.position.x - roomOffset, transform.position.y + roomOffset))
            .ChainAdd(Room.TOP_RIGHT, new Vector2(transform.position.x + roomOffset, transform.position.y + roomOffset))
            .ChainAdd(Room.BOTTOM_LEFT, new Vector2(transform.position.x - roomOffset, transform.position.y - roomOffset))
            .ChainAdd(Room.BOTTOM_RIGHT, new Vector2(transform.position.x + roomOffset, transform.position.y - roomOffset));

        roomPaths
            .ChainAddLast(Room.TOP_LEFT)
            .ChainAddLast(Room.TOP_RIGHT)
            .ChainAddLast(Room.BOTTOM_RIGHT)
            .ChainAddLast(Room.BOTTOM_LEFT);

        roomStats.ChainAdd(Room.TOP_LEFT, new List<RoomStat>(){new RoomStat(ActivityVariety.OBLIGATION)})
            .ChainAdd(Room.TOP_RIGHT, new List<RoomStat>(){new RoomStat(ActivityVariety.OBLIGATION)})
            .ChainAdd(Room.BOTTOM_LEFT, new List<RoomStat>(){new RoomStat(ActivityVariety.PROBLEM)})
            .ChainAdd(Room.BOTTOM_RIGHT, new List<RoomStat>(){new RoomStat(ActivityVariety.PROBLEM)});
    }

    void Update()
    {
        gaugeIncreaseRate += gaugeIncreaseAcceleration;
        if (Time.time > 0 && Time.time.IsApproximatelyDivisibleBy(roomValueSecondsBetweenIncreases, Time.deltaTime))
        {
            for (int i = 0; i < roomStartTimes.Length; i++)
            {
                List<RoomStat> roomStatsList = roomStats.GetDictValue((Room)i);
                foreach (RoomStat roomStat in roomStatsList)
                {
                    if (Time.time > roomStartTimes[i])
                        roomStat.IncreaseValue(gaugeMaximum, gaugeIncreaseRate); 

                    if (roomStat.value >= gaugeMaximum * ((float)gaugeWarningPercentage/100))
                        InitiateActivityWarning((Room)i, roomStat.variety);
                }
            }
        }
    }

    private void InitiateActivityWarning(Room room, ActivityVariety variety)
    {
        ActivityController activity = ahc.SearchByVariety(room, variety);
        if (activity == null)
            return;
        activity.ToggleWarning(true);
    }

    public Vector2 GetRoomPosition(Room roomName) => roomPositions.GetDictValue(roomName);

    public List<Room> GetPathBetweenRooms(Room origin, Room target)
    {
        List<Room> path = new List<Room>();
        LinkedListNode<Room> roomNode = roomPaths.Find(origin);
        if (roomNode.GetNextNode(true).Value == target)
            path.Add(target);
        else if (roomNode.GetNextNode(false).Value == target)
            path.Add(target);
        else
        {
            path.Add(roomNode.GetNextNode(true).Value);
            path.Add(target);
        } 
        return path;
    }
}
