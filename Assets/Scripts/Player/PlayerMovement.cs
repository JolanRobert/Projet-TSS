using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerController playerController;

        private PlayerData data => playerController.Data;
        
        private float turnSmoothAngle;
        private float accelerationProgress;
        private float deccelerationProgress;
        private float rotationProgress;

        private Quaternion targetRotation;

        private void FixedUpdate()
        {
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, data.rotationSpeed * Time.fixedDeltaTime));
        }

        public void Move(Vector2 moveInput)
        {
            if (moveInput is { x: < 0.1f, y: < 0.1f } and { x: > -0.1f, y: > -0.1f })
            {
                accelerationProgress = 0;
                deccelerationProgress += Time.deltaTime;
                rb.velocity = rb.velocity.normalized * (Mathf.Clamp(data.moveDeceleration.Evaluate(deccelerationProgress), 0, 1) * data.moveSpeed);
            }
            else
            {
                accelerationProgress += Time.deltaTime;
                deccelerationProgress = 0;
                rb.velocity = new Vector3(moveInput.x, 0, moveInput.y) * (data.moveAcceleration.Evaluate(accelerationProgress) * data.moveSpeed);
            }
        }

        public void Rotate(Vector2 rotateInput)
        {
            var direction = new Vector3(rotateInput.x, 0f, rotateInput.y);
            if (direction.magnitude < 0.1f) return;
            targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        
        public void Cancel()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}