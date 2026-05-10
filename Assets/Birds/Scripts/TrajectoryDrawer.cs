using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField]
    private int trajectoryPositions = 20;

    [SerializeField]
    private float trajectoryTimeStep = 0.1f;

    [SerializeField]
    private LineRenderer trajectoryLine;

    public void SetLineEnabled(bool enabled)
    {
        trajectoryLine.enabled = enabled;
    }

    public void DrawTrajectory(Vector3 startPosition, Vector3 velocity)
    {
        trajectoryLine.positionCount = trajectoryPositions;
        for (int i = 0; i < trajectoryPositions; i++)
        {
            //pos(t) = startPos + v*t + gravity * t * t/2
            float t = trajectoryTimeStep * i;
            Vector3 position = startPosition + velocity * t + Physics.gravity * t * t / 2;
            trajectoryLine.SetPosition(i, position);
        }
    }
}
