using UnityEngine;


public class IngameGem : MonoBehaviour
{
    // 젬이 마우스를 따라오는 속도 (Inspector에서 조절 가능)
    public float dragSpeed = 10f;

    private Rigidbody2D rb;
    private float originalGravity;
    private Vector3 offset;
    private Transform mainCameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 1. 젬의 원래 중력 값을 저장해둡니다.
        originalGravity = rb.gravityScale;
        mainCameraTransform = Camera.main.transform; // 카메라를 매번 찾는 것 방지
    }

    // 마우스 버튼을 "누른" 순간
    void OnMouseDown()
    {
        // 1. 드래그하는 동안 중력을 0으로 만듭니다.
        rb.gravityScale = 0;

        // 2. 물리적으로 움직여야 하므로 Dynamic 타입을 유지합니다.
        rb.bodyType = RigidbodyType2D.Dynamic;

        // 3. 마우스와 젬 사이의 거리(offset) 계산
        Vector3 mouseWorldPos = GetMouseWorldPos();
        offset = transform.position - mouseWorldPos;
    }

    // 마우스 버튼을 "누른 채로 드래그하는" 동안
    void OnMouseDrag()
    {
        // 1. 마우스가 가리키는 목표 위치 계산
        Vector3 targetPosition = GetMouseWorldPos() + offset;

        // 2. 젬의 현재 위치에서 목표 위치까지의 "방향" 계산
        Vector2 directionToTarget = (targetPosition - transform.position);

        // 3. [핵심] 젬의 "속도"를 목표 방향으로 설정합니다.
        // 물리 엔진이 이 속도로 움직이려다가 타일맵 콜라이더와 충돌합니다.
        rb.linearVelocity = directionToTarget * dragSpeed;
    }

    // 마우스 버튼을 "뗀" 순간
    void OnMouseUp()
    {
        // 1. 젬을 그 자리에 멈춥니다.
        rb.linearVelocity = Vector2.zero;

        // 2. 젬의 중력을 원래대로 복구합니다.
        rb.gravityScale = originalGravity;
    }

    // 마우스 위치를 2D 월드 좌표로 변환하는 헬퍼 함수
    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCameraTransform.position.z - Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}