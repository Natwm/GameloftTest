using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectZone : MonoBehaviour
{
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

    public GameObject a;

    // Start is called before the first frame update
    void Start()
    {

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
            _FirstPosition = inputPos;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        Physics.Raycast(ray, out pos, Mathf.Infinity);

        print(pos.point);

        Debug.DrawRay(ray.origin, ray.direction * 100);

        Instantiate(a, pos.point, Quaternion.identity);

        DetectBall(pos.point);
        LaunchBall(pos.point);
    }

    private void LateUpdate()
    {
        _CurrentPosition = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private GameObject DetectBall(Vector3 castPos)
    {
        test = true;
        print("Nathan = " + castPos);
        hit = Physics.SphereCastAll(castPos, radius, Vector3.forward, bulletLayer);

        if (hit.Length > 1)
        {
            GameObject targetBall = GetBullets();

            if (targetBall == null)
                return null;

            if (targetBall.GetComponent<BallBehaviours>() != null)
            {
                if (!TimeController.instance.SlowMotionTimer.IsStarted())
                {
                    TimeController.instance.StartSlowMotion();
                }

                //PlayerController.instance.Ball = targetBall;
                targetBall.GetComponent<Rigidbody>().velocity = targetBall.GetComponent<Rigidbody>().velocity.normalized * 2f;
                return targetBall;
            }
        }

        return null;
    }

    private void LaunchBall(Vector3 castPos)
    {
        if (DetectBall(castPos) == null)
        {
            LineRendererIndicator.instance.ResetLine();
            TimeController.instance.EndSlowMotion();
            ball = null;
            return;
        }

        if (Input.GetMouseButtonUp(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (ball != null)
            {
                Vector2 launchDirection = VectorsMethods.GetDirectionFromAtoB((Vector2)_FirstPosition, (Vector2)_CurrentPosition).normalized;

                TimeController.instance.EndSlowMotion();
                ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ball.GetComponent<Rigidbody>().AddForce(launchDirection * 5000f);
                ball = null;

                LineRendererIndicator.instance.ResetLine();
            }
        }
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
