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
    private Dictionary<Room, Vector2> roomPositions;
    private float roomOffset;

    void Start()
    {
        roomOffset = houseWidth / 2;
        roomPositions.Add(Room.TOP_LEFT, new Vector2(transform.position.x - roomOffset, transform.position.y - roomOffset));
        roomPositions.Add(Room.TOP_RIGHT, new Vector2(transform.position.x + roomOffset, transform.position.y - roomOffset));
        roomPositions.Add(Room.BOTTOM_LEFT, new Vector2(transform.position.x - roomOffset, transform.position.y + roomOffset));
        roomPositions.Add(Room.BOTTOM_RIGHT, new Vector2(transform.position.x + roomOffset, transform.position.y + roomOffset));
    }

    public Vector2 getRoomPosition(Room roomName)
    {
        Vector2 buffer;
        roomPositions.TryGetValue(roomName, out buffer);
        return buffer;
    }
}
