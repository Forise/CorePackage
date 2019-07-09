//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public class SwipeDrawer : MonoBehaviour
    {
        #region Fields
        private LineRenderer lineRenderer;
        private Camera cam;

        private float zOffset = 10;
        #endregion

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            cam = FindObjectOfType<Camera>();
            SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
        }

        private void SwipeDetector_OnSwipe(SwipeData data)
        {
            Vector3[] positions = new Vector3[2];
            positions[0] = cam.ScreenToWorldPoint(new Vector3(data.StartPosition.x, data.StartPosition.y, zOffset));
            positions[1] = cam.ScreenToWorldPoint(new Vector3(data.EndPosition.x, data.EndPosition.y, zOffset));
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(positions);
        }
    }
}