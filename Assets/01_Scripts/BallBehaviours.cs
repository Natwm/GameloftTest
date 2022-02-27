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
    [SerializeField] private List<AudioClip> impactObstacle_Wood_Sound;
    [SerializeField] private List<AudioClip> impactObstacle_Steel_Sound;
    [SerializeField] private List<AudioClip> impactObstacle_Immortal_Sound;
    [SerializeField] private List<AudioClip> impactObstacle_Target_Sound;
    [SerializeField] private List<AudioClip> impactWall_Sound;
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

    /*private void Update()
    {
        //If the first Shoot haven't be done yet, do not change the velocity of the ball
        if (!firstShoot)
            return;

        /*if (Rb.velocity.magnitude >= Rb.velocity.normalized.magnitude * 25)
            Rb.velocity = Rb.velocity.normalized * 25;

    }*/

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
        Basic_ObstacleBehaviours basicObj;
        Target_ObstacleBehaviours targetObj;
        Coffre_ObstacleBehaviours coffreObj;
        Immortal_ObstacleBehaviours immortalObj;

        Instantiate(dustSmokeParticule,collision.contacts[0].point,Quaternion.identity);

        firstShoot = true;

        if (collision.gameObject.TryGetComponent<IDamageable>(out collisionObject))
        {
            if(collision.gameObject.TryGetComponent<Target_ObstacleBehaviours>(out targetObj))
            {
                _Audio.clip = SoundManager.GetRandomSound(impactObstacle_Target_Sound);
                _Audio.Play();
            }else if(collision.gameObject.TryGetComponent<Coffre_ObstacleBehaviours>(out coffreObj))
            {
                _Audio.clip = SoundManager.GetRandomSound(impactObstacle_Steel_Sound);
                _Audio.Play();
            }
            else if(collision.gameObject.TryGetComponent<Immortal_ObstacleBehaviours>(out immortalObj))
            {
                _Audio.clip = SoundManager.GetRandomSound(impactObstacle_Immortal_Sound);
                _Audio.Play();
            }
            else if(collision.gameObject.TryGetComponent<Basic_ObstacleBehaviours>(out basicObj))
            {
                _Audio.clip = SoundManager.GetRandomSound(impactObstacle_Wood_Sound);
                _Audio.Play();
            }
            

            if (isLaunchByTheplayer)
            {
                HitStop.instance.FreezeFrame(0.15f);

                isLaunchByTheplayer = false;
            }

            CameraManager.instance.IncreaseShakeCam();
            CameraManager.instance.ResetShakeParametterTimer.ResetPlay();
            StartCoroutine(CameraManager.instance.ShakeCam());

            collisionObject.GetDamage(_BallDamage);

            Rb.AddForce(-collision.contacts[0].point * 50f);
        }
        else
        {
            _Audio.clip = SoundManager.GetRandomSound(impactWall_Sound);
            _Audio.Play();
        }

        ///Clamp the velocity at 25.
        Rb.velocity *= _speedModifier;//Rb.velocity.normalized * 25;
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
