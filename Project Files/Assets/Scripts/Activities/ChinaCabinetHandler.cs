using UnityEngine;

public class ChinaCabinetHandler : MonoBehaviour
{
    public GameObject player, china, hands;
    public GameObject sceneCamera;
    private Camera sceneCameraThing;
    private PlayerController playerController;
    

    void Start()
    {
        playerController = (PlayerController)player.GetComponent<MonoBehaviour>();
        sceneCameraThing = sceneCamera.GetComponent<Camera>();
    }

    void OnAwake()
    {
        // reset the game's state here and prepare to start
    }

    void Update()
    {
        Vector3 mousePoint = sceneCameraThing.ScreenToWorldPoint(Input.mousePosition);
        hands.transform.position = new Vector3(mousePoint.x, hands.transform.position.y, hands.transform.position.z);
        // placeholder disengagement. The game should determine when to disengage according to game rules
        if (Input.GetMouseButtonDown(0))
        {
            
            playerController.InitiateDisengagement();
        }
    }
}