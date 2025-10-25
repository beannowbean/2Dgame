using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera_Action : MonoBehaviour
{
    public GameObject Target;               // 카메라가 따라다닐 타겟

    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 0.28f;           // 카메라의 y좌표
    public float offsetZ = -10.0f;          // 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치

    public float timer = 0.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;

    public Player playerScript; // 1. Inspector에서 Player를 연결할 슬롯
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (Player.isIntro == true)
        {
            StartCoroutine(IntroCamera());
        }
        
    }


    private void Update()
    {
        if (Player.isIntro == false)
        {
            cam.orthographicSize = 30;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Target != null)
        {
            // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
            TargetPos = new Vector3(
                Target.transform.position.x + offsetX,
                Target.transform.position.y + offsetY,
                Target.transform.position.z + offsetZ
                );

            // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
        }
    }


    IEnumerator IntroCamera()
    {
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(19.15f, 0.28f, 0);

        while (timer < 5.0f)
        {
            timer += Time.deltaTime;
            float t = timer / 5;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        transform.position = endPosition;
    }
}
