using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviours : MonoBehaviour
{
    private Rigidbody _Rb;

    [Header ("Ball Param")]
    [SerializeField] private float _BallSpeed;
    [Min(1)]
    [SerializeField] private int _BallDamage = 1;

    [SerializeField] private float _speedModifier = 1f;
    private bool firstShoot;

    [Space]
    [Header("Timer")]
    Timer moveToThePlayerTimer;
    [SerializeField] float timeBeforeMovingTowardPlayer;

    [Space]
    [SerializeField] float forceTowardPlayer = 75f;

    public Rigidbody Rb { get => _Rb; set => _Rb = value; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector2 force = Vector2.zero;
        force.x = 0;
        force.y = -1f;

        Rb.AddForce(force.normalized * _BallSpeed);

        moveToThePlayerTimer = new Timer(timeBeforeMovingTowardPlayer, MoveTowardThePlayer);
        moveToThePlayerTimer.ResetPlay();
    }

    private void Update()
    {
        //If the first Shoot haven't be done yet, do not change the velocity of the ball
        if (!firstShoot)
            return;

        if(Rb.velocity.magnitude >= Rb.velocity.normalized.magnitude * 25)
            Rb.velocity = Rb.velocity.normalized * 25;

    }

    /// <summary>
    /// Add a force every "timeBeforeMovingTowardPlayer" toward The player.
    /// </summary>
    public void MoveTowardThePlayer()
    {
        Vector3 dir = VectorsMethods.GetDirectionFromAtoB(transform.position, PlayerController.instance.transform.position).normalized;
        Rb.AddForce(-dir * forceTowardPlayer);

        moveToThePlayerTimer.ResetPlay();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable collisionObject;

        firstShoot = true;

        if (collision.gameObject.TryGetComponent<IDamageable>(out collisionObject))
        {
            CameraManager.instance.ShakeCam();
            collisionObject.GetDamage(_BallDamage);
        }
        
        ///Clamp the velocity at 25.
        Rb.velocity = Rb.velocity.normalized * 25;
    }

    public void OnDestroy()
    {
        moveToThePlayerTimer.Pause();
    }
}
