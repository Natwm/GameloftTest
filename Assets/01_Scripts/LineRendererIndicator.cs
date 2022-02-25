using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererIndicator : MonoBehaviour
{
    public static LineRendererIndicator instance;

    [SerializeField] private int _Reflections;
    [SerializeField] private int _Maxlength;

    private LineRenderer _Line;
    private Ray _Ray;
    private RaycastHit _Hit;

    private Vector2 _firstPosition;
    private Vector2 _CurrentPosition;

    public LayerMask mask;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : LineRendererIndicator");
        instance = this;

        _Line = GetComponent<LineRenderer>();
    }

    private void Update()
    {

        if (PlayerController.instance.IsMoving && PlayerController.instance.Ball !=null)
        {
            Vector3 ballPosition = PlayerController.instance.Ball.transform.position;
            Vector3 dir = VectorsMethods.GetDirectionFromAtoB((Vector2)ballPosition, _CurrentPosition);
            
            _Ray = new Ray(ballPosition, dir);

            _Line.positionCount = 1;
            _Line.SetPosition(0, ballPosition);

            float reamainingLength = _Maxlength;

            for (int i = 0; i < _Reflections; i++)
            {
                if (Physics.Raycast(_Ray.origin, _Ray.direction, out _Hit, reamainingLength))
                {
                    _Line.positionCount += 1;
                    _Line.SetPosition(_Line.positionCount - 1, _Hit.point);

                    reamainingLength -= Vector3.Distance(_Ray.origin, _Hit.point);

                    _Ray = new Ray(_Hit.point, Vector3.Reflect(_Ray.direction, _Hit.normal));

                    if (!_Hit.collider.CompareTag("Wall") && !_Hit.collider.CompareTag("Obstacle"))
                        break;
                }
                else
                {
                    _Line.positionCount += 1;
                    _Line.SetPosition(_Line.positionCount - 1, _Ray.origin + _Ray.direction * reamainingLength);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (PlayerController.instance.IsMoving)
        {
            _CurrentPosition = Camera.main.ScreenToWorldPoint(PlayerController.instance.MousePos);
        }
    }

    public void ResetLine()
    {
        _Line.positionCount = 0;
    }
}
