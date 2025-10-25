using UnityEngine;
using System.Collections;

public class Bunny : MonoBehaviour
{

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    public float moveDistance = 3f;
    public float jump = 1.0f;
    public Transform groundCheck;
    private float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    public bool facingRight = true;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        sr.flipX = false;
        facingRight = true;

        StartCoroutine(Jump());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    IEnumerator Jump()
    {
        while (true)
        {
            // --- 점프 시작 ---
            // 1. 방향 설정 (스프라이트 뒤집기)
            sr.flipX = !facingRight; // facingRight가 true면 false(오른쪽), false면 true(왼쪽)

            // 2. 점프 애니메이션 시작
            SetAnimationState("isJump"); // "isJump" 파라미터를 true로 설정

            // 3. 점프 힘 적용 (X축 이동 + Y축 점프)
            float moveDirection = facingRight ? 1f : -1f; // 오른쪽이면 1, 왼쪽이면 -1
            rb.linearVelocity = new Vector2(moveDirection * (moveDistance / 1.5f), jump); // 이동 속도 조절 필요
            // 참고: moveDistance/1.5f 부분은 점프 체공 시간(약 1.5초 예상)동안 이동할 속도입니다. 조절 필요.

            // 4. 땅에 착지할 때까지 기다림
            // (점프 후 아주 잠시 기다려서 isGrounded가 false가 되도록 함)
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => isGrounded == true);

            // --- 착지 후 ---
            // 5. 착지했으므로 수평 속도 0으로 (미끄러짐 방지)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            // 6. Idle 애니메이션 상태로 전환 (선택적: Animator에서 자동 전환되게 할 수도 있음)
            SetAnimationState("isIdle"); // "isIdle" 파라미터를 true로 설정

            // 7. 지정된 시간(1초) 동안 대기
            yield return new WaitForSeconds(1);

            // --- 다음 점프 준비 ---
            // 8. 다음 점프 방향 전환
            facingRight = !facingRight;
        }
    }

    void SetAnimationState(string stateParameterName)
    {
        // 다른 상태 파라미터들을 모두 false로 설정 (이름은 실제 파라미터에 맞게 수정)
        anim.SetBool("isJump", false);
        anim.SetBool("isIdle", false);
        // animator.SetBool("isRun", false); // 다른 상태가 있다면 추가

        // 요청된 상태 파라미터만 true로 설정
        if (!string.IsNullOrEmpty(stateParameterName))
        {
            anim.SetBool(stateParameterName, true);
        }
    }
}
