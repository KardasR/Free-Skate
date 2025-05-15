using UnityEngine;

public class Player : MonoBehaviour
{

    #region Private Fields

    /// <summary>
    /// Read input to see if the user is pressing A or D. 
    /// <para/>
    /// Value will be normalized, negative for A and positive for D
    /// </summary>
    private float _inputH;

    /// <summary>
    /// Read input to see if the user is pressing W or S. 
    /// <para/>
    /// Value will be normalized, negative for S and positive for W
    /// </summary>
    private float _inputV;

    /// <summary>
    /// Read input to see if the user is pressing shift.
    /// </summary>
    private bool _inputSprinting;

    /// <summary>
    /// Physics-based body for the player.
    /// <para/>
    /// Used to move the player around with physics.
    /// </summary>
    private Rigidbody _body;

    /// <summary>
    /// The puck being held by the player.
    /// <para/>
    /// Only one puck can be held at a time.
    /// </summary>
    private Puck _heldPuck;

    #endregion

    #region Public Fields

    /// <summary>
    /// Amount of friction to stop the player.
    /// </summary>
    public float IceFriction;

    /// <summary>
    /// How fast the player moves.
    /// </summary>
    public float SkatingSpeed;

    /// <summary>
    /// How hard the puck is pushed off the stick.
    /// </summary>
    public float PassingSpeed;

    /// <summary>
    /// How much to multiply the skating speed by when the player is sprinting.
    /// </summary>
    public float SprintModifier;

    /// <summary>
    /// Max speed allowed, avoids issues with moving the player/puck too fast.
    /// </summary>
    public float MaxSpeed;

    /// <summary>
    /// Where the puck will stay when the player is holding onto it.
    /// </summary>
    public Transform PuckHoldPoint;

    #endregion

    #region Private Methods

    /// <summary>
    /// This will manually move the puck with the puck hold point.
    /// </summary>
    private void MovePuck()
    {
        if (_heldPuck != null)
        {
            _heldPuck.transform.SetPositionAndRotation(PuckHoldPoint.position, PuckHoldPoint.rotation);
        }
    }

    /// <summary>
    /// This will be what moves the player.
    /// </summary>
    private void MovePlayer()
    {
        // push the player in the direction they want to go. Ideally the direction comes from the camera.
        Vector3 moveInput = new Vector3(_inputH, 0, _inputV).normalized;

        // only apply friction when velocity is non-zero
        if (_body.linearVelocity.sqrMagnitude > 0.01f)
        {
            Vector3 friction = -_body.linearVelocity.normalized * IceFriction;
            _body.AddForce(friction, ForceMode.Acceleration);
        }

        Vector3 moveDir = transform.TransformDirection(moveInput);
        Vector3 forward = transform.forward;

        //Angle between input direction and object forward
        float alignment = Vector3.Dot(moveDir.normalized, forward);

        //Allow full force if mostly forward, reduce otherwise
        float directionPenalty = Mathf.Clamp01((alignment + 1f) / 2f);

        //Optionally exaggerate penalty for sharp sideways / backwards input
        directionPenalty = Mathf.Pow(directionPenalty, 2f);

        float speed = SkatingSpeed * (_inputSprinting ? SprintModifier : 1f) * directionPenalty;// * directionPenalty;
        
        float currentSpeedInMoveDir = Vector3.Dot(_body.linearVelocity, moveDir.normalized);
        if (currentSpeedInMoveDir < MaxSpeed)
            _body.AddForce(moveDir * speed, ForceMode.Acceleration);
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Show the cursor if in the editor, quit if outside
    /// </summary>
    private void Quit()
    {
        #if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        #else
        Application.Quit();
        #endif
    }

    #endregion Private Methods

    #region Public Methods

    /// <summary>
    /// Only let the user pickup the puck if they aren't holding one and the puck can be picked up.
    /// </summary>
    /// <param name="puck"></param>
    public void TryPickupPuck(Puck puck)
    {
        if (_heldPuck == null && puck.Pickupable)
        {
            _heldPuck = puck;
            puck.Hold();
        }
    }

    #endregion Public Methods

    #region Core Unity Methods

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        _inputH            = Input.GetAxis("Horizontal");   // a/d
        _inputV            = Input.GetAxis("Vertical");     // w/s
        _inputSprinting     = Input.GetAxis("Fire3") == 1f; // shift

        if (_heldPuck != null && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 shotDir = PuckHoldPoint.forward;
            _heldPuck.Release(shotDir * PassingSpeed);
            _heldPuck = null;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();

        if (Input.GetKeyDown(KeyCode.F))
            HideCursor();
    }

    /// <summary>
    /// Do all of the movement here as this is nice for rigid body movement
    /// </summary>
    void FixedUpdate()
    {
        MovePlayer();
        MovePuck();
    }

    #endregion Core Unity Methods
}
