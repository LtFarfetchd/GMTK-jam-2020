using Room = HouseController.Room;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject house;
    public GameObject sceneCamera;
    public float playerSpeed;
    private enum State
    {
        MOVING,
        STATIONARY,
        INTERACTING
    }
    private HouseController houseController;
    private Room currentRoom;
    private List<Room> currentPath;
    private State state;

    void Start()
    {
        houseController = (HouseController)house.GetComponent<MonoBehaviour>();
    }

    private void InitiateMove(Room targetRoom)
    {
        if (targetRoom == currentRoom)
            return;
        currentPath = houseController.GetPathBetweenRooms(currentRoom, targetRoom);
        state = State.MOVING;
    }

    void Update()
    {
        if (state == State.STATIONARY)
        {
            // handle a request for movement
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Input.mousePosition;
                float houseX = house.transform.position.x, houseY = house.transform.position.y;
                Room targetRoom;
                if (mousePos.x < houseX)
                    targetRoom = mousePos.y < houseY ? Room.TOP_LEFT : Room.BOTTOM_LEFT;
                else
                    targetRoom = mousePos.y < houseY ? Room.TOP_RIGHT : Room.BOTTOM_RIGHT;
                if (targetRoom != currentRoom)
                {
                    InitiateMove(targetRoom);
                }
                    
            }
        }

        if (state == State.MOVING)
        {
            int targetIndex = !currentPath.Contains(currentRoom) ? 0 : 1;

            //actual moving is done here

            if ((Vector2)transform.position == houseController.GetRoomPosition(currentPath[targetIndex]))
            {
                currentRoom = currentPath[targetIndex];
                if (targetIndex == currentPath.Count)
                    state = State.STATIONARY;
            }
            
        }
    }
}