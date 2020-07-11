using Room = HouseController.Room;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject house;
    public GameObject sceneCamera;
    public float moveTime;
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
    private Rigidbody2D rb;
    private float moveTimeElapsed;

    void Start()
    {
        currentRoom = Room.BOTTOM_LEFT;
        state = State.STATIONARY;
        rb = GetComponent<Rigidbody2D>();
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
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mp = Input.mousePosition; 
                mp.z = Camera.main.transform.position.z;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(mp);

                float houseX = house.transform.position.x, houseY = house.transform.position.y;
                Room targetRoom;
                if (mousePos.x < houseX)
                    targetRoom = mousePos.y < houseY ? Room.BOTTOM_LEFT : Room.TOP_LEFT;
                else
                    targetRoom = mousePos.y < houseY ? Room.BOTTOM_RIGHT : Room.TOP_RIGHT;
                if (targetRoom != currentRoom)
                {
                    InitiateMove(targetRoom);
                    moveTimeElapsed = 0f;
                }
            }
        }

        if (state == State.MOVING)
        {
            moveTimeElapsed += Time.deltaTime;
            int targetIndex = !currentPath.Contains(currentRoom) ? 0 : 1;
            Vector2 targetPos = houseController.GetRoomPosition(currentPath[targetIndex]);

            rb.MovePosition(Vector2.Lerp(houseController.GetRoomPosition(currentRoom), targetPos, moveTimeElapsed/moveTime));

            if ((Vector2)transform.position == houseController.GetRoomPosition(currentPath[targetIndex]))
            {
                Debug.Log("Boo");
                currentRoom = currentPath[targetIndex];
                if (targetIndex == currentPath.Count - 1)
                    state = State.STATIONARY;
                else
                    moveTimeElapsed = 0f;
            }
            
        }
    }
}