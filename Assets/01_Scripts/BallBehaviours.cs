using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviours : MonoBehaviour
{
    private Rigidbody _Rb;

    [Header("Ball Param")]
    [SerializeField] private float _BallSpeed;
    [Min(1)]
    [SerializeField] private int _BallDamage = 1;

    [SerializeField] private float _speedModifier = 1f;

    private bool firstShoot;
    private bool isLaunchByTheplayer;

    [Space]
    [Header("Timer")]
    Timer moveToThePlayerTimer;
    [SerializeField] float timeBeforeMovingTowardPlayer;

    [Space]
    [Header("vfx")]
    [SerializeField] private GameObject dustSmokeParticule;

    [Space]
    [SerializeField] float forceTowardPlayer = 75f;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioClip impactObstacle_Sound;
    [SerializeField] private AudioClip impactWall_Sound;
    private AudioSource _Audio;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        _Audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector2 force = Vector2.zero;
        force.x = 0;
        force.y = -1f;

        Rb.AddForce(force.normalized * _BallSpeed);

        MoveToThePlayerTimer = new Timer(timeBeforeMovingTowardPlayer, MoveTowardThePlayer);
        MoveToThePlayerTimer.ResetPlay();
    }

    private void Update()
    {
        //If the first Shoot haven't be done yet, do not change the velocity of the ball
        if (!firstShoot)
            return;

        if (Rb.velocity.magnitude >= Rb.velocity.normalized.magnitude * 25)
            Rb.velocity = Rb.velocity.normalized * 25;

    }

    /// <summary>
    /// Add a force every "timeBeforeMovingTowardPlayer" toward The player.
    /// </summary>
    public void MoveTowardThePlayer()
    {
        Vector3 dir = VectorsMethods.GetDirectionFromAtoB(transform.position, GameManager.instance.BallDirectionZone.transform.position).normalized;
        Vector3 leftORight = Random.Range(0, 1) == 0 ? Vector3.left : Vector3.right;

        Rb.AddForce(-dir * forceTowardPlayer);
        Rb.AddForce(leftORight * forceTowardPlayer/1.25f);
        MoveToThePlayerTimer.ResetPlay();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable collisionObject;

        Instantiate(dustSmokeParticule,collision.contacts[0].point,Quaternion.identity);

        firstShoot = true;

        if (collision.gameObject.TryGetComponent<IDamageable>(out collisionObject))
        {
            _Audio.clip = impactObstacle_Sound;
            _Audio.Play();

            if (isLaunchByTheplayer)
            {
                HitStop.instance.FreezeFrame(0.15f);

                isLaunchByTheplayer = false;
            }
                

            CameraManager.instance.ShakeCam();
            collisionObject.GetDamage(_BallDamage);
        }

        _Audio.clip = impactWall_Sound;
        _Audio.Play();

        ///Clamp the velocity at 25.
        Rb.velocity = Rb.velocity.normalized * 25;
    }

    public void OnDestroy()
    {
        MoveToThePlayerTimer.Pause();
    }

    #region GETTER && SETTER

    public Rigidbody Rb { get => _Rb; set => _Rb = value; }
    public bool IsLaunchByTheplayer { get => isLaunchByTheplayer; set => isLaunchByTheplayer = value; }
    public Timer MoveToThePlayerTimer { get => moveToThePlayerTimer; set => moveToThePlayerTimer = value; }

    #endregion
}
