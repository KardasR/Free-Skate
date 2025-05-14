using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    /// <summary>
    /// When the player scores a goal, the puck gets placed here.
    /// </summary>
    public Transform resetLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Puck"))
        {
            // GOAAAAAAAAAAAAAAAAAAAAAAAAAAALLLLLLLLLLLLLLLAAAAAAASSSSSSSSSSSSSSOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.transform.SetPositionAndRotation(resetLocation.position, resetLocation.rotation);
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }
    }
}
