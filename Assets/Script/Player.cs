using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // 이동 속도 (Inspector 창에서 조절 가능)
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float moveInput;
    public float jumpPower;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGround;

    private Animator animator;

    public bool isIntro { get; private set; }

    // 게임 시작 시 한 번만 실행됩니다.
    void Start()
    {
        isIntro = true;
        // Rigidbody2D와 SpriteRenderer 컴포넌트를 미리 가져와 변수에 저장합니다.
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        transform.position = new Vector3(-0.85f, 0.28f, 0);
        sr.enabled = false;
        StartCoroutine(Intro());

  

    }

    // 매 프레임마다 호출됩니다. (입력 감지용)
    void Update()
    {
        PlayerMove();
        PlayerJump();
    }

    // 고정된 시간 간격으로 호출됩니다. (물리 계산용)
    void FixedUpdate()
    {
        
        // Rigidbody의 속도를 설정하여 캐릭터를 실제로 움직입니다.
        // Y축 속도는 원래 값을 유지하여 점프나 중력에 영향을 주지 않습니다.
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isIntro)
        {
            return;
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        // 만약 velocity가 구식(obsolete)이라는 경고가 뜬다면, 아래 코드로 바꿔주세요.
        // rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void PlayerMove()
    {
        if (isIntro)
        {
            return;
        }
        // 좌우 방향키 또는 A, D 키 입력을 받습니다. (-1, 0, 1)
        moveInput = Input.GetAxisRaw("Horizontal");

        // 캐릭터 방향 전환
        if (moveInput > 0) // 오른쪽을 누를 때
        {
            sr.flipX = false;
            if (isGround == true & rb.linearVelocity.y < 0.1f)
                AnimatorChange("isRun");
        }
        else if (moveInput < 0) // 왼쪽을 누를 때
        {
            sr.flipX = true;
            if(isGround == true & rb.linearVelocity.y < 0.1f)
                AnimatorChange("isRun");
        }
        else
        {
            if(isGround == true & rb.linearVelocity.y < 0.1f)
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                AnimatorChange("isIDLE");
            }
        }
    }

    void PlayerJump()
    {
        if (isIntro)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            // Y축 속도(velocity)에 점프 힘을 싣습니다.
            // X축 속도는 기존 속도를 유지합니다.
            AnimatorChange("isJumpUp");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
        else if (isGround == false & rb.linearVelocity.y < 0.1f)
        {
            AnimatorChange("isJumpDown");
        }

        // === 방향 전환 ===
        if (moveInput > 0)
        {
            sr.flipX = false;
        }
        else if (moveInput < 0)
        {
            sr.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void AnimatorChange(string temp)
    {
        animator.SetBool("isIDLE", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isJumpUp", false);
        animator.SetBool("isJumpDown", false);

        animator.SetBool(temp, true);
    }


    IEnumerator Intro()
    {
        yield return new WaitForSeconds(6);

        sr.enabled = true;
        yield return new WaitForSeconds(5);

        AnimatorChange("isRun");
        rb.linearVelocity = new Vector2(moveSpeed, 0);
        yield return new WaitForSeconds(2);

        AnimatorChange("isJumpUp");
        rb.linearVelocity = new Vector2(moveSpeed, jumpPower);
        yield return new WaitUntil(() => rb.linearVelocity.y < 0.1f);
        AnimatorChange("isJumpDown");
        yield return new WaitUntil(() => isGround == true);
        rb.linearVelocity = Vector2.zero;
        AnimatorChange("isIDLE");
        yield return new WaitForSeconds(2);

        sr.flipX = true;
        AnimatorChange("isJumpUp");
        rb.linearVelocity = new Vector2(-moveSpeed, jumpPower);
        yield return new WaitUntil(() => rb.linearVelocity.y < 0.1f);
        AnimatorChange("isJumpDown");
        yield return new WaitUntil(() => isGround == true);
        rb.linearVelocity = Vector2.zero;
        AnimatorChange("isIDLE");
        yield return new WaitForSeconds(2);

        animator.SetTrigger("isHurt");
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        yield return new WaitForSeconds(1);

        animator.SetTrigger("isHurt");
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        yield return new WaitForSeconds(1);

        sr.flipX = false;
        AnimatorChange("isRun");
        rb.linearVelocity = new Vector2(moveSpeed/2, 0);
        yield return new WaitForSeconds(2);
        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("isHurt");
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        yield return new WaitForSeconds(1);

        sr.flipX = true;
        AnimatorChange("isRun");
        rb.linearVelocity = new Vector2(-moveSpeed/2, 0);
        yield return new WaitForSeconds(2);
        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("isHurt");
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        yield return new WaitForSeconds(4);

        animator.SetTrigger("isHurt");
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        yield return new WaitForSeconds(1);

        AnimatorChange("isRun");
        rb.linearVelocity = new Vector2(-moveSpeed, 0);
        yield return new WaitForSeconds(2);
        rb.linearVelocity = Vector2.zero;

        sr.flipX = false;
        yield return new WaitForSeconds(0.5f);
        sr.enabled = false;


        yield return new WaitForSeconds(5);
        sr.enabled = true;
        transform.position = new Vector3(-56, -245, 0);
        jumpPower = 15;
        moveSpeed = 25;

        isIntro = false;
    }
}