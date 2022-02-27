using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectZone : MonoBehaviour
{
    public static DeflectZone instance;

    [Header("Attack Param")]
    public float radius;

    public bool test;
    RaycastHit[] hit;
    RaycastHit pos;

    public LayerMask bulletLayer;

    Vector3 mousePos;

    Vector3 _FirstPosition;
    Vector3 _CurrentPosition;

    GameObject ball;

    public GameObject Ball { get => ball; set => ball = value; }
    public Vector3 CurrentPosition { get => _CurrentPosition; set => _CurrentPosition = value; }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : DeflectZone");
        instance = this;

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

        Vector3 inputPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            Ball = DetectBall(pos.point);
            _FirstPosition = pos.point;
        }

        if (Input.GetMouseButtonUp(0))
        {
            LaunchBall(pos.point);
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        Physics.Raycast(ray, out pos, Mathf.Infinity);

        Debug.DrawRay(ray.origin, ray.direction * 100);

    }

    private void LateUpdate()
    {
        CurrentPosition = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private GameObject DetectBall(Vector3 castPos)
    {
        test = true;
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

            Vector2 launchDirection = VectorsMethods.GetDirectionFromAtoB((Vector2)_FirstPosition, (Vector2)CurrentPosition).normalized;

            Rigidbody ball_RB = Ball.GetComponent<Rigidbody>();
            BallBehaviours ball_BH = Ball.GetComponent<BallBehaviours>();

            TimeController.instance.EndSlowMotion();

            ball_RB.velocity = Vector3.zero;
            ball_RB.AddForce(launchDirection * 5000f);

            ball_BH.MoveToThePlayerTimer.Play();

            Ball = null;

            LineRendererIndicator.instance.ResetLine();
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
}
