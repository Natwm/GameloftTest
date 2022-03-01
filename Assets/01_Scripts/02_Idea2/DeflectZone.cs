using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectZone : MonoBehaviour
{
    public static DeflectZone instance;

    [Header("Attack Param")]
    public float radius;
    public float m_LaunchPower = 500f;
    public int m_AmountOfShoot;

    public bool m_IsLimitedInShooting;
    RaycastHit[] hit;
    RaycastHit pos;

    public LayerMask bulletLayer;

    Vector3 mousePos;
    Vector3 _FirstPosition;
    Vector3 _CurrentPosition;

    GameObject ball;

    Timer GameOverTimer;

    [Header("audio")]
    [SerializeField] private List<AudioClip> launchSounds;
    private AudioSource _Audio;


    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : DeflectZone");
        instance = this;

    }
    private void Start()
    {
        _Audio = GetComponent<AudioSource>();
        m_AmountOfShoot = ScoreManager.instance.parametter.InitialAmountOfShoot;
        GameOverTimer1 = new Timer(5, GameManager.instance.GameOver);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.touchCount == 0)
            return;

        Touch input = Input.GetTouch(0);

        mousePos = input.position;
        mousePos.z = 10;*/

        mousePos = Input.mousePosition;
        mousePos.z = 10;
        CurrentPosition = Camera.main.ScreenToWorldPoint(mousePos);

        Ray ray = Camera.main.ScreenPointToRay(mousePos);


        Physics.Raycast(ray, out pos, Mathf.Infinity);


        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ball = DetectBall(pos.point);
            FirstPosition = Camera.main.ScreenToWorldPoint(mousePos);
        }

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {            
            LaunchBall(pos.point);
        }

    }

    private GameObject DetectBall(Vector3 castPos)
    {
        //print("Nathan = " + castPos);
        hit = Physics.SphereCastAll(castPos, radius, Vector3.forward, bulletLayer);

        if (hit.Length > 1)
        {
            GameObject targetBall = GetBullets();

            if (targetBall == null)
            {
                LineRendererIndicator.instance.ResetLine();

                TimeController.instance.EndSlowMotion();

                return null;
            }

            if (targetBall.GetComponent<BallBehaviours>() != null)
            {
                CameraManager.instance.BeginSlowMotion();

                print(targetBall.GetComponent<BallBehaviours>().FirstShoot);
                TimeController.instance.StartSlowMotion();


                //PlayerController.instance.Ball = targetBall;

                targetBall.GetComponent<Rigidbody>().velocity = targetBall.GetComponent<Rigidbody>().velocity.normalized * 2f;

                targetBall.GetComponent<BallBehaviours>().MoveToThePlayerTimer.Pause();
                return targetBall;
            }
        }

        return null;
    }

    private void LaunchBall(Vector3 castPos)
    {
        /*if (Input.GetMouseButtonUp(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {*/

        if (Ball != null)
        {
            if (m_IsLimitedInShooting)
            {
                m_AmountOfShoot--;

                if (m_AmountOfShoot == 0)
                {
                    GameOverTimer1.Play();
                    return;
                }
            }

            Vector2 launchDirection = VectorsMethods.GetDirectionFromAtoB((Vector2)FirstPosition,(Vector2)CurrentPosition);

            Rigidbody ball_RB = Ball.GetComponent<Rigidbody>();
            BallBehaviours ball_BH = Ball.GetComponent<BallBehaviours>();

            ball_BH.UpdateNBLauch(m_AmountOfShoot);

            TimeController.instance.EndSlowMotion();
            TimeController.instance.slowMotionTimer.Pause();

            ball_RB.velocity = Vector3.zero;
            ball_RB.AddForce(launchDirection * m_LaunchPower);

            ball_BH.MoveToThePlayerTimer.Play();

            Ball = null;

            LineRendererIndicator.instance.ResetLine();

            _Audio.clip = SoundManager.GetRandomSound(launchSounds);
            _Audio.Play();
        }
        //}
    }

    private GameObject GetBullets()
    {
        BallBehaviours ball;
        foreach (var item in hit)
        {
            if (item.collider.gameObject.TryGetComponent<BallBehaviours>(out ball))
            {
                return item.collider.gameObject;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        if (pos.point != Vector3.zero)
            Gizmos.DrawWireSphere(pos.point, radius);
    }

    #region GETTER && SETTER
    public GameObject Ball { get => ball; set => ball = value; }
    public Vector3 CurrentPosition { get => _CurrentPosition; set => _CurrentPosition = value; }
    public Vector3 FirstPosition { get => _FirstPosition; set => _FirstPosition = value; }
    public Timer GameOverTimer1 { get => GameOverTimer; set => GameOverTimer = value; }

    #endregion
}
