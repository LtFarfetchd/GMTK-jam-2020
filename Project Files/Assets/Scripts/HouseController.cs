using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public enum Room {
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT
    }

    public enum RoomVariety {
        OBLIGATION,
        PROBLEM
    }

    public struct RoomStat {
        public RoomVariety variety;
        public float value;
        public RoomStat(RoomVariety variety)
        {
            this.variety = variety;
            value = 0f;
        }
        public void IncreaseValue(float amount) => value += amount;
    }

    public float houseWidth;
    public float[] roomStartTimes = new float[4];
    public float gaugeIncreaseRate = 2f;
    public float gaugeIncreaseAcceleration = 0.00025f;

    private Dictionary<Room, Vector2> roomPositions = new Dictionary<Room, Vector2>();
    private Dictionary<Room, RoomStat> roomStats = new Dictionary<Room, RoomStat>();
    private LinkedList<Room> roomPaths = new LinkedList<Room>();
    private float roomOffset;
    private int roomValueSecondsBetweenIncreases = 5;

    void Start()
    {
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
        
        roomStats.ChainAdd(Room.TOP_LEFT, new RoomStat(RoomVariety.OBLIGATION))
            .ChainAdd(Room.TOP_RIGHT, new RoomStat(RoomVariety.OBLIGATION))
            .ChainAdd(Room.BOTTOM_LEFT, new RoomStat(RoomVariety.PROBLEM))
            .ChainAdd(Room.BOTTOM_RIGHT, new RoomStat(RoomVariety.PROBLEM));
    }

    void Update()
    {
        gaugeIncreaseRate += gaugeIncreaseAcceleration;
        if (Time.time > 0 && Time.time.IsApproximatelyDivisibleBy(roomValueSecondsBetweenIncreases, Time.deltaTime))
        {
            Debug.Log(Time.time);
            for (int i = 0; i < roomStartTimes.Length; i++)
            {
                if (Time.time > roomStartTimes[i])
                    roomStats[(Room)i].IncreaseValue(gaugeIncreaseRate); 
            }
        }   
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
