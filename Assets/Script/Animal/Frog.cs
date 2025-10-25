using UnityEngine;
using System.Collections; // 코루틴 사용 시 필수!

public class Frog : MonoBehaviour
{
 
    public float frogSpeed = 3.0f; // 한 번 점프 시 이동할 거리
    public float frogJump = 3.0f;
    public float jumpuptime = 1;
    public float jumpdowntime = 1;
 

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private bool facingRight = false; // 현재 오른쪽을 보고 있는지 여부 (시작은 왼쪽)

    void Start()
    {
        // 1. 필요한 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 2. 물리 효과 설정 (선택 사항)
        // 중력 영향을 받지 않고 옆으로만 뛰게 하려면

        // 3. 시작 시 왼쪽 보기 설정
        sr.flipX = true; // true = 왼쪽, false = 오른쪽
        facingRight = false;

        // 4. 개구리 행동 사이클 코루틴 시작
        StartCoroutine(FrogCycle());
    }

    // 개구리 행동 (점프 -> 대기 -> 점프 -> 대기 ...) 무한 반복
    IEnumerator FrogCycle()
    {
        // 게임이 실행되는 동안 계속 반복
        while (true)
        {
           
            SetAnimationState("isJumpup"); // 점프 애니메이션 시작
            if(facingRight == true)
            {
                rb.linearVelocity = new Vector2(-frogSpeed, frogJump);
            }
            else
            {
                rb.linearVelocity = new Vector2(frogSpeed, frogJump);
            }
            yield return new WaitForSeconds(jumpuptime);
            SetAnimationState("isJumpdown");
            yield return new WaitForSeconds(jumpdowntime);


            // --- Idle 단계 ---
            SetAnimationState("isIDLE"); // Idle 애니메이션 시작
            yield return new WaitForSeconds(2); // 2초 대기


            // --- 다음 점프 준비 ---
            facingRight = !facingRight; // 방향 전환
            sr.flipX = !facingRight; // 스프라이트 뒤집기 (true = 왼쪽, false = 오른쪽)
        }
    }

    // 애니메이션 상태를 안전하게 변경하는 함수
    void SetAnimationState(string stateName)
    {
        // 모든 상태를 먼저 false로 초기화
        animator.SetBool("isJumpup", false);
        animator.SetBool("isJumpdown", false);
        animator.SetBool("isIDLE", false);

        
        animator.SetBool(stateName, true);
        
    }
}