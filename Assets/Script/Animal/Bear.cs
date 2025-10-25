using UnityEngine;

public class BearController : MonoBehaviour
{
    // 이동 속도 (Inspector에서 조절 가능)
    public float moveSpeed = 2.0f;
    // 방향을 바꾸기까지 이동할 거리 (Inspector에서 조절 가능)
    public float moveDistance = 5.0f;

    // 컴포넌트 참조
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // 상태 변수
    private bool movingRight = true; // 현재 오른쪽으로 움직이는 중인가?
    private Vector3 startPosition; // 한 방향으로 이동 시작한 위치

    void Start()
    {
        // 1. 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // 2. 초기 상태 설정
        startPosition = transform.position; // 현재 위치를 시작 위치로 기록
        movingRight = true; // 시작은 오른쪽
        sr.flipX = false; // 오른쪽 보기

        // (선택 사항) 물리 설정
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // 1. 시작 위치로부터 얼마나 이동했는지 거리 계산
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        // 2. 이동 거리가 목표 거리(moveDistance)를 넘었는지 확인
        if (distanceTraveled >= moveDistance)
        {
            // 3. 방향 전환
            movingRight = !movingRight;
            sr.flipX = !movingRight;

            // 4. 현재 위치를 새로운 시작 위치로 기록
            startPosition = transform.position;
        }
    }

    void FixedUpdate()
    {
        // 1. 현재 방향에 따라 이동 속도 설정
        float currentSpeed = movingRight ? moveSpeed : -moveSpeed;

        // 2. Rigidbody의 속도를 설정하여 곰을 움직임
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }
}