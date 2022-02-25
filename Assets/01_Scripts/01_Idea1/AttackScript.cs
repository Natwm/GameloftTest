using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [Header("Attack Param")]
    public float radius;

    public bool test;
    RaycastHit[] hit;

    public LayerMask bulletLayer;

    public bool attack;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            attack = true;
        }

        DetectBall();
        LaunchBall();
    }

    /// <summary>
    /// Cast a sphere arround the player to detect if a ball is inside this circle
    /// </summary>
    /// <returns>if there is a ball arround the player</returns>
    private GameObject DetectBall()
    {
        //If the player isn't trying to shoot then do nothing 
        if (!attack )
            return null;

        test = true;
        hit = Physics.SphereCastAll(transform.position, radius, Vector3.forward, bulletLayer);

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

                PlayerController.instance.Ball = targetBall;
                targetBall.GetComponent<Rigidbody>().velocity = targetBall.GetComponent<Rigidbody>().velocity.normalized * 2f;
                return targetBall;
            }
        }

        return null;
    }

    /// <summary>
    /// this methode allow the player to shoot 
    /// </summary>
    private void LaunchBall()
    {
        if (DetectBall() == null)
        {
            LineRendererIndicator.instance.ResetLine();
            TimeController.instance.EndSlowMotion();

            if(PlayerController.instance.Ball != null)
            {
                Vector3 ballSpeed = PlayerController.instance.Ball.GetComponent<Rigidbody>().velocity;
                PlayerController.instance.Ball.GetComponent<Rigidbody>().velocity = ballSpeed.normalized * 25f;
            }
            

            PlayerController.instance.Ball = null;
            return;
        }            

        if (Input.GetMouseButtonUp(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            attack = false;
            if (GetComponent<PlayerController>().Ball != null)
            {
                Vector2 launchDirection = VectorsMethods.GetDirectionFromAtoB((Vector2)PlayerController.instance.Ball.transform.position, PlayerController.instance.CurrentPosition).normalized;

                TimeController.instance.EndSlowMotion();
                PlayerController.instance.Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                PlayerController.instance.Ball.GetComponent<Rigidbody>().AddForce(launchDirection * 5000f);
                PlayerController.instance.Ball = null;

                LineRendererIndicator.instance.ResetLine();
            }
        }
    }

    /// <summary>
    /// |Security| Get the ball around the player ( it arrived that the sphere cast get the player and the ball)
    /// </summary>
    /// <returns>The ball arround the player </returns>
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
        if (test)
            Gizmos.DrawWireSphere(transform.position, radius);
    }
}
