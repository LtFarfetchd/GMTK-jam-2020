using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using ActivityVariety = ActivitiesHandlerController.ActivityVariety;
using StatLevel = HUDController.StatLevel;

public class HouseController : MonoBehaviour
{
    public enum Room {
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_RIGHT,
        BOTTOM_LEFT
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
    [DraggablePoint]
    public Vector3 tlPoint, trPoint, brPoint, blPoint;
    public float[] roomStartTimes = new float[4];
    public float gaugeMaximum = 20f;
    public float gaugeIncreaseRate = 2f;
    public float gaugeIncreaseAcceleration = 0.00025f;
    public int gaugeWarningPercentage = 50;

    private Dictionary<Room, Vector2> roomPositions = new Dictionary<Room, Vector2>();
    private Dictionary<Room, List<RoomStat>> roomStats = new Dictionary<Room, List<RoomStat>>();
    private LinkedList<Room> roomPaths = new LinkedList<Room>();
    private ActivitiesHandlerController ahc;
    private int roomValueSecondsBetweenIncreases = 1;

    void Start()
    {
        ahc = (ActivitiesHandlerController)(activitiesHandler.GetComponent<MonoBehaviour>());

        roomPositions.ChainAdd(Room.TOP_LEFT, (Vector2)tlPoint)
            .ChainAdd(Room.TOP_RIGHT, (Vector2)trPoint)
            .ChainAdd(Room.BOTTOM_LEFT, (Vector2)blPoint)
            .ChainAdd(Room.BOTTOM_RIGHT, (Vector2)brPoint);

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

    public float GetRoomGaugeMaximum() => gaugeMaximum;
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

    public StatLevel GetTotalStatLevel(ActivityVariety statVariety)
    {
        float total = 0f;
        int activityCount = 0;
        foreach (Room room in Enum.GetValues(typeof(Room)))
        {
            List<RoomStat> roomActivityStats = roomStats.GetDictValue(room);
            foreach (RoomStat stat in roomActivityStats)
                if (stat.variety == statVariety)
                {
                    activityCount += roomActivityStats.Count;
                    total += stat.value;
                }
        }
        return new StatLevel(statVariety, total, activityCount);
    }

    public void HealStat(ActivityVariety variety, Room room)
    {
        roomStats.GetDictValue(room).Where(roomStat => roomStat.variety == variety).First().value = 0;
    }
}
