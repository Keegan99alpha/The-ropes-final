using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2d : RayCastController
{
    float maxSlopeAngle = 70f;
    float maxDecentAngle = 65f;

    public CollisionsInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;


    public override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
    }

    public void Move(Vector3 velocity, Vector2 input)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.oldVelocity = velocity;
        playerInput = input;

        if (velocity.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(velocity.x);
        }

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        horizontalCollisions(ref velocity);
        internalCollision(ref velocity);
        if (velocity.y != 0)
        {
            verticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void internalCollision(ref Vector3 velocity)
    {
        Bounds bounds = collider.bounds;
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = -bounds.size.y + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            // Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.blue);

            if (hit)
            {
                if (collisions.fallingThrough)
                {
                    continue;
                }
                if (hit.collider.tag == "Platform")
                {
                    print("An internal collision is occuring");
                    collisions.internalHit = true;
                    collisions.fallingThrough = true;
                    continue;
                }
            }
            collisions.fallingThrough = false;
            collisions.internalHit = false;
        }
    }

    void horizontalCollisions(ref Vector3 velocity)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);


            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.oldVelocity;
                    }
                    float distanceToSlopeBase = 0;
                    if (slopeAngle != collisions.oldSlopeAngle)
                    {
                        distanceToSlopeBase = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeBase * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeBase * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void verticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                if (hit.collider.tag == "Platform")
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collisions.fallingThrough)
                    {
                        continue;
                    }
                    if (playerInput.y == -1)
                    {
                        collisions.fallingThrough = true;
                        Invoke("ResetFallingThrough", 0.5f);
                        continue;
                    }
                }
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
        if (collisions.climbingSlope)
        {
            float dirX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((dirX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, collisionMask);

            if (hit)
            {
                float newSlopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (newSlopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * dirX;
                    collisions.slopeAngle = newSlopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        //y = d*sin(θ)
        //x = d*cos(θ)
        float moveDistance = Mathf.Abs(velocity.x);
        float slopeVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (velocity.y <= slopeVelocityY)
        {
            velocity.y = slopeVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float dirX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (dirX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDecentAngle)
            {
                if (Mathf.Sign(hit.normal.x) == dirX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDist = Mathf.Abs(velocity.x);
                        float descentVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDist;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDist * Mathf.Sign(velocity.x);
                        velocity.y -= descentVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThrough()
    {
        collisions.fallingThrough = false;
    }

    public struct CollisionsInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope, descendingSlope;
        public bool fallingThrough;
        public bool internalHit;
        public float slopeAngle, oldSlopeAngle;

        public int faceDir;

        public Vector3 oldVelocity;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            oldSlopeAngle = slopeAngle;
            slopeAngle = 0f;
        }
    }
}
