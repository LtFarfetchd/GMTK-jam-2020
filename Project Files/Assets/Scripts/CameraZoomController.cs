using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    private enum State {
        ZOOMING,
        RETURNING,
        STATIC
    }
    private State state;
    private Vector2 cameraStart;
    private Vector2 path;
    private Vector2 playerStart;
    private Vector2 playerTarget;
    private float playerXTarget, playerYTarget;
    public GameObject player;
    public GameObject house;

    public void ZoomIn(Vector2 playerStart, Vector2 playerTarget) 
    {
        this.playerStart = playerStart;
        this.playerTarget = playerTarget;
        path = playerTarget - cameraStart;
        state = State.ZOOMING;
    }

    public void ZoomOut() => this.state = State.RETURNING;

    void Start()
    {
        cameraStart = house.transform.position;
    }

    void Update()
    {
        if (state == State.ZOOMING || state == State.RETURNING)
        {
            float movementProportion = 
                Vector3.Distance(player.transform.position, playerTarget)
                / Vector3.Distance(playerStart, playerTarget);
            transform.position = cameraStart + movementProportion * path;
            if ((Vector2)transform.position == (state == State.ZOOMING ? playerTarget : cameraStart))
                state = (state == State.ZOOMING ? State.RETURNING : State.STATIC);
        }
    }
}
