using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Controller2d target;
    public Vector2 focusAreaSize;
    public float verticalOffset;

    public float projectedDstX;
    public float projectedSmoothTimeX;
    public float projectedSmoothTimeY;

    FocusArea focusArea;

    float currentProjectedX;
    float targetProjectedX;
    float projectedDirX;
    float smoothTimeVelX;
    float smoothVelY;

    bool lookAheadStop;

    void Start()
    {
        focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
    }

    private void LateUpdate()
    {
        focusArea.update(target.collider.bounds);
        Vector2 focusPoint = focusArea.centre + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0)
        {
            projectedDirX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0)
            {
                lookAheadStop = false;
                targetProjectedX = projectedDirX * projectedDstX;
            }
            else
            {
                if (!lookAheadStop)
                {
                    lookAheadStop = true;
                    targetProjectedX = currentProjectedX + (projectedDirX * projectedDstX - currentProjectedX) / 4f;
                }
            }
        }

        currentProjectedX = Mathf.SmoothDamp(currentProjectedX, targetProjectedX, ref smoothTimeVelX, projectedSmoothTimeX);

        focusPoint.y = Mathf.SmoothDamp(transform.position.y, focusPoint.y, ref smoothVelY, smoothVelY);
        focusPoint += Vector2.right * currentProjectedX;

        transform.position = (Vector3)focusPoint + Vector3.forward * -10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;

        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - (size.x / 2);
            right = targetBounds.center.x + (size.x / 2);
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void update(Bounds targetBounds)
        {
            float shiftX = 0.0f;

            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0.0f;

            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
