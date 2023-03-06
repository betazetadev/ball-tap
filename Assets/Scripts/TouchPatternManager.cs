using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class TouchPatternManager : MonoBehaviour
{
    // The minimum distance between points to be considered for the circle gesture
    public float circleMinDistance = 10f;

    // The maximum distance error allowed for the circle gesture
    public float circleMaxError = 10f;

    // The minimum number of points required for the circle gesture
    public int circleMinPoints = 10;

    // The event to be called when a circle gesture is detected
    public UnityEvent onCircleGestureDetected;

    public Camera mainCamera;
    
    // The list of points collected during the long press
    private List<Vector2> longPressPoints = new List<Vector2>();

    // The center point of the circle gesture
    private Vector2 circleCenter;

    // The radius of the circle gesture
    private float circleRadius;

    // The error of the circle gesture
    private float circleError;

    // Flag to check if the long press is still active
    private bool isLongPressActive = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isLongPressActive = true;

            // Clear the list of points when starting a new long press
            longPressPoints.Clear();

            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            longPressPoints.Add(mousePos);
        }
        else if (Input.GetMouseButton(0))
        {
            if (isLongPressActive)
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                // Add the new point only if it's far enough from the last one
                if (Vector2.Distance(mousePos, longPressPoints[longPressPoints.Count - 1]) >= circleMinDistance)
                {
                    longPressPoints.Add(mousePos);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isLongPressActive = false;

            // Check if there are enough points to be considered for the circle gesture
            if (longPressPoints.Count < circleMinPoints)
            {
                return;
            }

            // Calculate the center and radius of the circle gesture
            if (CalculateCircleGesture())
            {

                var pointsTouched = longPressPoints.Count;
                PlayerPrefs.SetInt("TouchBonusCounter", pointsTouched);
                // Invoke the event if a circle gesture was detected
                onCircleGestureDetected.Invoke();
            }
        }
    }

    private bool CalculateCircleGesture()
    {
        // Calculate the average position of the points to find the center of the circle gesture
        float avgX = 0f;
        float avgY = 0f;

        foreach (Vector2 point in longPressPoints)
        {
            avgX += point.x;
            avgY += point.y;
        }

        circleCenter = new Vector2(avgX / longPressPoints.Count, avgY / longPressPoints.Count);

        // Calculate the radius and error of the circle gesture
        float sumDistance = 0f;
        float sumDistanceSquared = 0f;

        foreach (Vector2 point in longPressPoints)
        {
            float distance = Vector2.Distance(point, circleCenter);
            sumDistance += distance;
            sumDistanceSquared += distance * distance;
        }

        circleRadius = sumDistance / longPressPoints.Count;

        float error = 0f;

        if (longPressPoints.Count > 1)
        {
            float variance = sumDistanceSquared / longPressPoints.Count - (sumDistance / longPressPoints.Count) * (sumDistance / longPressPoints.Count);
            error = Mathf.Sqrt(variance);
        }

        circleError = error;

        // Check if the gesture is a circle or ellipse based on the error
        return error <= circleMaxError;
    }
}