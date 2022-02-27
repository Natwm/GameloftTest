using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;
using DG.Tweening;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController instance;

    private Rigidbody _Rb;
    private Vector2 direction;
    bool isMoving;

    Vector3 mousePos;

    private Vector2 _CurrentPosition;
    private Vector3 initScale;

    [Header("Ball Param")]
    public bool canMove;
    [SerializeField] private float _Speed;
    [SerializeField] private float _MaxBounceAngle;
    public GameObject visuel;

    [Space]
    [Header ("Player Param")]
    [SerializeField] private float _IncreaseTimerValue;

    public GameObject Ball;
    [SerializeField] private bool canBeHit = true;
    
    [Space]
    [Header ("Clamp")]
    [MinMaxSlider(-10, 10)]
    public MinMax clapPosition;

    [Space]
    [Header("Animation")]
    [SerializeField] private Vector3 spreadScale;
    [SerializeField] private float spreadSpeed;

    [Space]
    [Header("Timer")]
    [SerializeField] private float invincibilityTime = 0.3f;
    private Timer invincibilityTimer;

    [Space]
    [Header("Audio")]
    [SerializeField] private List<AudioClip> damages_Sound;
    private AudioSource _Audio;

    private void Awake()
    {
        _Rb = GetComponent<Rigidbody>();
        _Audio = GetComponent<AudioSource>();
        direction = _Rb.position;

        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : PlayerController");
        else
            instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
        invincibilityTimer = new Timer(invincibilityTime, CanGetDamaged);
    }

    // Update is called once per frame
    void Update()
    {

        float posX = Mathf.Clamp(transform.position.x, clapPosition.Min, clapPosition.Max);

        transform.position = new Vector3(posX, transform.position.y, transform.position.z);

        if (DebugManager.instance.isMobil)
        {
            if (Input.touchCount > 0)
            {
                IsMoving = Input.GetTouch(0).phase == TouchPhase.Moved;
                Touch input = Input.GetTouch(0);

                mousePos = input.position;
                mousePos.z = 10;
            }
        }
        else
        {
            IsMoving = Input.GetMouseButton(0);
            mousePos = Input.mousePosition; 
            mousePos.z = 10;//select distance = 10 units from the camera
        }

        if (IsMoving || canMove)
        {
            direction.x = Camera.main.ScreenToWorldPoint(MousePos).x;
        }
    }

    private void FixedUpdate()
    {
        if (canMove && Ball == null && GameManager.instance.state == GameManager.GameState.RUNNING)
        {
            _Rb.MovePosition(Vector2.Lerp(_Rb.position, direction, _Speed * Time.deltaTime));

        }
        else
        {
            _Rb.velocity = Vector3.zero;
        }
            
    }

    private void LateUpdate()
    {
        if (IsMoving)
        {
            CurrentPosition = Camera.main.ScreenToWorldPoint(MousePos);
            //CurrentPosition = new Vector2(CurrentPosition.x*2, CurrentPosition.y);
        }
    }

    private void CanGetDamaged()
    {
        canBeHit = true;
    }
    

    #region Animation

    /// <summary>
    /// Squishy animation when the player get hit
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageScale()
    {
        transform.DOScaleX(transform.localScale.x - 1, 0.1f);
        transform.DOScaleY(transform.localScale.y + 1, 0.1f);
        yield return new WaitForSeconds(0.1f);

        transform.DOScaleX(transform.localScale.x + 1.5f, 0.1f);
        transform.DOScaleY(transform.localScale.y - 1.5f, 0.1f);
        yield return new WaitForSeconds(0.1f);

        transform.DOScaleX(transform.localScale.x - .5f, 0.1f);
        transform.DOScaleY(transform.localScale.y + .5f, 0.1f);

        yield return new WaitForSeconds(0.1f);
        transform.localScale = initScale;
    }

    private void SpreadAnimation(Vector3 newScale)
    {
        visuel.transform.DOScale(newScale, spreadSpeed);
    }

    #endregion

    #region Sound

    public AudioClip GetRandomHitSound() 
    {
        int index = Random.Range(0, damages_Sound.Count);
        return damages_Sound[index];
    }

    #endregion

    #region Interfaces

    public void GetDamage(int _amountOfDamage)
    {
        Vibration.Vibrate();
        StartCoroutine(DamageScale());
        if (canBeHit)
        {
            _Audio.clip = GetRandomHitSound();
            _Audio.Play();

            GameManager.instance.IncreaseTimer(_IncreaseTimerValue);
            canBeHit = false;
            invincibilityTimer.ResetPlay();
        }
    }


    public bool IsDead()
    {
        return _IncreaseTimerValue <= 0;
    }

    public void Dead()
    {
        print("GameOver");
    }

    #endregion

    #region Collision
    private void OnCollisionEnter(Collision collision)
    {
        BallBehaviours ball;

        if(collision.gameObject.TryGetComponent<BallBehaviours>(out ball))
        {
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = transform.position.x - contactPoint.x;

            float playerWidth = GetComponent<Collider>().bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.Rb.velocity);
            float bounceAngle = (offset / playerWidth) * _MaxBounceAngle;

            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -_MaxBounceAngle, _MaxBounceAngle);

            Quaternion rotate = Quaternion.AngleAxis(newAngle, Vector3.forward);

            ball.Rb.velocity = rotate * Vector2.up * ball.Rb.velocity.magnitude;

        }
    }
    #endregion

    #region GETTER && SETTER

    public Vector2 CurrentPosition { get => _CurrentPosition; set => _CurrentPosition = value; }
    public Vector3 MousePos { get => mousePos; set => mousePos = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    #endregion

}
