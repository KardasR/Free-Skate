using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    #region Public Fields

    /// <summary>
    /// Parent node of the player. This is an empty node
    /// </summary>
    public Transform Player;

    /// <summary>
    /// Seperate empty node child node of the Player node
    /// </summary>
    public Transform Orientation;

    /// <summary>
    /// The node with the player model, rigid body, etc. Child of the player node.
    /// </summary>
    public Transform PlayerObj;

    /// <summary>
    /// Rigid body of the playerobj
    /// </summary>
    public Rigidbody Rigidbody;

    /// <summary>
    /// How fast the player rotates.
    /// </summary>
    public float RotationSpeed;

    #endregion Public Fields

    #region Core Unity Methods

    // Update is called once per frame
    void Update()
    {
        // Get the camera's forward direction on the XZ plane (ignore Y)
        Vector3 camForward = transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Set orientation based on camera
        Orientation.forward = camForward;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDir = camForward * verticalInput + camRight * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir.normalized, Time.deltaTime * RotationSpeed);
        }
    }

    #endregion Core Unity Methods
}
