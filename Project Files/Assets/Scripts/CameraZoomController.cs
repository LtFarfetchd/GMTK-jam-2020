using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    private enum State {
        ZOOMING,
        RETURNING,
        STATIC
    }
    private State state;
    private Vector3 cameraStart;
    private Vector3 path;
    private Vector3 playerStart;
    private Vector3 playerTarget;
    private float playerXTarget, playerYTarget;
    public GameObject player;

    public void ZoomIn(Vector3 playerStart, Vector3 playerTarget) 
    {
        this.playerStart = playerStart;
        this.playerTarget = playerTarget;
        path = playerTarget - cameraStart;
        state = State.ZOOMING;
    }

    public void ZoomOut() => this.state = State.RETURNING;

    void Update()
    {
        if (state == State.ZOOMING || state == State.RETURNING)
        {
            float movementProportion = 
                Vector3.Distance(player.transform.position, playerTarget)
                / Vector3.Distance(playerStart, playerTarget);
            transform.position = cameraStart + movementProportion * path;
            if (transform.position == (state == State.ZOOMING ? playerTarget : cameraStart))
                state = (state == State.ZOOMING ? State.RETURNING : State.STATIC);
        }
    }
}
