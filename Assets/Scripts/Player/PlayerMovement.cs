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
            rb.angularVelocity = Vector3.zero;
            
            if (rotateInput.sqrMagnitude < 0.1f) return;

            var targetAngle = Mathf.Atan2(rotateInput.x, rotateInput.y) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothAngle, data.rotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        
        public void Cancel()
        {
            rb.velocity = Vector3.zero;
        }
    }
}