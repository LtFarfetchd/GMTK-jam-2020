using Room = HouseController.Room;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject house;
    public GameObject sceneCamera;
    public GameObject activitiesHandler;
    public float moveTime;
    public float engageTime;
    public GameObject miniGameScreen;
    private enum State
    {
        MOVING,
        STATIONARY,
        ENGAGING,
        DISENGAGING,
        ENGAGED
    }
    private HouseController houseController;
    private CameraZoomController cameraZoomController;
    private ActivitiesHandlerController ahc;
    private Room currentRoom;
    private List<Room> currentPath;
    private ActivityController targetActivity;
    private State state;
    private Rigidbody2D rb;
    private float timeElapsed;
    private Animator animator;
    private string movementDirection;
    private bool isInitialised = false;

    void Start()
    {
        currentRoom = Room.BOTTOM_LEFT;
        state = State.STATIONARY;
        rb = GetComponent<Rigidbody2D>();
        houseController = (HouseController)house.GetComponent<MonoBehaviour>();
        cameraZoomController = (CameraZoomController)sceneCamera.GetComponent<MonoBehaviour>();
        ahc = (ActivitiesHandlerController)activitiesHandler.GetComponent<MonoBehaviour>();
        animator = GetComponent<Animator>();
    }

    private void InitiateMove(Room targetRoom)
    {
        if (targetRoom == currentRoom)
            return;
        timeElapsed = 0f;
        currentPath = houseController.GetPathBetweenRooms(currentRoom, targetRoom);
        movementDirection = GetMovementDirection(
            houseController.GetRoomPosition(currentRoom), houseController.GetRoomPosition(currentPath[0])
        );
        animator.SetBool(movementDirection, true);
        state = State.MOVING;
    }

    private void InitiateEngagement()
    {
        timeElapsed = 0f;
        cameraZoomController.StartZoom(transform.position, targetActivity.GetEngagementPosition());
        movementDirection = GetMovementDirection(
            houseController.GetRoomPosition(currentRoom), targetActivity.engagementPosition
        );
        animator.SetBool(movementDirection, true);
        state = State.ENGAGING;
    }

    public void InitiateDisengagement()
    {
        timeElapsed = 0f;
        ahc.GetActivityHandlerObject(targetActivity.type).SetActive(false);
        miniGameScreen.transform.position = new Vector3(0, 6, miniGameScreen.transform.position.z);
        movementDirection = GetMovementDirection(
            targetActivity.engagementPosition, houseController.GetRoomPosition(currentRoom)
        );
        animator.SetBool(movementDirection, true);
        state = State.DISENGAGING;
    }

    void Update()
    {
        if (!isInitialised)
        {
            transform.position = houseController.GetRoomPosition(currentRoom);
            isInitialised = true;
        }

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
                }
                else 
                {
                    targetActivity = ahc.SearchByPosition(currentRoom, mousePos);
                    if (targetActivity != null)
                    {
                        InitiateEngagement();
                    }
                }
            }
        }

        if (state == State.MOVING)
        {
            timeElapsed += Time.deltaTime;
            int targetIndex = !currentPath.Contains(currentRoom) ? 0 : 1;
            Vector2 targetPos = houseController.GetRoomPosition(currentPath[targetIndex]);
            Debug.Log(Vector2.Lerp(houseController.GetRoomPosition(currentRoom), targetPos, timeElapsed/moveTime));

            rb.MovePosition(Vector2.Lerp(houseController.GetRoomPosition(currentRoom), targetPos, timeElapsed/moveTime));

            if ((Vector2)transform.position == targetPos)
            {
                currentRoom = currentPath[targetIndex];
                if (targetIndex == currentPath.Count - 1)
                {
                    state = State.STATIONARY;
                    animator.SetBool(movementDirection, false);
                }
                else
                {
                    timeElapsed = 0f;
                    animator.SetBool(movementDirection, false);
                    movementDirection = GetMovementDirection(
                        houseController.GetRoomPosition(currentRoom), houseController.GetRoomPosition(currentPath[1])
                    );
                    animator.SetBool(movementDirection, true);
                }
                    
            }
        }

        if (state == State.ENGAGING || state == State.DISENGAGING)
        {
            bool engaging = state == State.ENGAGING;
            timeElapsed += Time.deltaTime;
            Vector2 targetPos = targetActivity.GetEngagementPosition();
            Vector2 roomPos = houseController.GetRoomPosition(currentRoom);

            Vector2 newPos = Vector2.Lerp(roomPos, targetPos, 
                engaging ? timeElapsed/engageTime : 1 - timeElapsed/engageTime);
            rb.MovePosition(newPos);

            if ((Vector2)transform.position == (engaging ? targetPos : roomPos))
            {
                animator.SetBool(movementDirection, false);
                state = engaging ? State.ENGAGED : State.STATIONARY;
                if (state == State.DISENGAGING)
                    cameraZoomController.StopZoom();
            }
        }

        if (state == State.ENGAGED)
        {
            GameObject minigameHandler = ahc.GetActivityHandlerObject(targetActivity.type);
            if (!minigameHandler.activeSelf)
            {
                minigameHandler.SetActive(true);            
                miniGameScreen.transform.position = new Vector3(
                    sceneCamera.transform.position.x, sceneCamera.transform.position.y, miniGameScreen.transform.position.z
                );
            }
        }
    }

    private string GetMovementDirection(Vector2 currentPos, Vector2 targetPos)
    {   
        if (currentPos.x < targetPos.x)
            return "MovingRight";
        if (currentPos.x > targetPos.x)
            return "MovingLeft";
        if (currentPos.y < targetPos.y)
            return "MovingUp";
        if (currentPos.y > targetPos.y)
            return "MovingDown";
        return null;
    }
}