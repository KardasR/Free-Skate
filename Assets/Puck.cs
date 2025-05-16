using UnityEngine;

public class Puck : MonoBehaviour
{
    #region Private Fields

    /// <summary>
    /// Rigidbody of puck used to move the puck around.
    /// </summary>
    private Rigidbody _body;

    /// <summary>
    /// This needs to be 0 to allow the user to pickup the puck.
    /// </summary>
    private float _pickupCooldownTimeLeft = 0f;

    #endregion

    #region Public Fields

    /// <summary>
    /// How long until a puck can be picked up after being released.
    /// </summary>
    public float PickupCooldownDuration;

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Is the puck being held?
    /// </summary>
    public bool IsHeld
    {
        get; private set;
    }

    /// <summary>
    /// Can the puck be picked up?
    /// </summary>
    public bool Pickupable
    {
        get
        {
            return !IsHeld && _pickupCooldownTimeLeft <= 0f;
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Mark the puck is being held and turn the physics on.
    /// </summary>
    public void Hold()
    {
        IsHeld = true;
        _body.isKinematic = true;
    }

    /// <summary>
    /// Mark the puck is being released. Turn off the physics and puck it with the force given.
    /// </summary>
    public void Release(Vector3 force)
    {
        _pickupCooldownTimeLeft = PickupCooldownDuration;

        IsHeld = false;
        _body.isKinematic = false;
        _body.AddForce(force, ForceMode.Impulse);
    }

    #endregion

    #region Core Unity Methods

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _body = GetComponent<Rigidbody>();  // should I switch to start()?
    }

    // Update is called once per frame
    void Update()
    {
        if (_pickupCooldownTimeLeft > 0f)
            _pickupCooldownTimeLeft = Mathf.Max(0, _pickupCooldownTimeLeft - Time.deltaTime);
    }

    #endregion Core Unity Methods
}
