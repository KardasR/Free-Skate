using UnityEngine;

public class PuckPickupTrigger : MonoBehaviour
{
    public Player skater;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Puck") && collider.TryGetComponent(out Puck puck))
        {
            skater.TryPickupPuck(puck);
        }
    }
}
