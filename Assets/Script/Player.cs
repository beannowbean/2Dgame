using UnityEngine;

public class Player : MonoBehaviour
{
    // 이동 속도 (Inspector 창에서 조절 가능)
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float moveInput;

    // 게임 시작 시 한 번만 실행됩니다.
    void Start()
    {
        // Rigidbody2D와 SpriteRenderer 컴포넌트를 미리 가져와 변수에 저장합니다.
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 매 프레임마다 호출됩니다. (입력 감지용)
    void Update()
    {
        // 좌우 방향키 또는 A, D 키 입력을 받습니다. (-1, 0, 1)
        moveInput = Input.GetAxisRaw("Horizontal");

        // 캐릭터 방향 전환
        if (moveInput > 0) // 오른쪽을 누를 때
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0) // 왼쪽을 누를 때
        {
            spriteRenderer.flipX = true;
        }
    }

    // 고정된 시간 간격으로 호출됩니다. (물리 계산용)
    void FixedUpdate()
    {
        // Rigidbody의 속도를 설정하여 캐릭터를 실제로 움직입니다.
        // Y축 속도는 원래 값을 유지하여 점프나 중력에 영향을 주지 않습니다.
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 만약 velocity가 구식(obsolete)이라는 경고가 뜬다면, 아래 코드로 바꿔주세요.
        // rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}