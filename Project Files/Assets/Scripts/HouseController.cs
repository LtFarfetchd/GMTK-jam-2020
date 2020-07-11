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
    public float houseWidth;
    private Dictionary<Room, Vector2> roomPositions = new Dictionary<Room, Vector2>();
    private LinkedList<Room> roomPaths = new LinkedList<Room>();
    private float roomOffset;

    void Start()
    {
        roomOffset = houseWidth / 2;

        roomPositions.ChainAdd(Room.TOP_LEFT, new Vector2(transform.position.x - roomOffset, transform.position.y - roomOffset))
            .ChainAdd(Room.TOP_RIGHT, new Vector2(transform.position.x + roomOffset, transform.position.y - roomOffset))
            .ChainAdd(Room.BOTTOM_LEFT, new Vector2(transform.position.x - roomOffset, transform.position.y + roomOffset))
            .ChainAdd(Room.BOTTOM_RIGHT, new Vector2(transform.position.x + roomOffset, transform.position.y + roomOffset));

        roomPaths
            .ChainAddLast(Room.TOP_LEFT)
            .ChainAddLast(Room.TOP_RIGHT)
            .ChainAddLast(Room.BOTTOM_RIGHT)
            .ChainAddLast(Room.BOTTOM_LEFT);
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
            path.Add(roomNode.Next.Value);
            path.Add(target);
        } 
        return path;
    }
}
