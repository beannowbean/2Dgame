using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    
    public float moveSpeed = 5f;

    public GemItemOnHead gemItemOnHead;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public GameScreenManage sm;
    private float moveInput;
    public float jumpPower;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGround;

    private Animator animator;

    public static bool isIntro = true;
    public bool die = false;

    private float doorDetect = 1.0f;
    public LayerMask doorLayer;

    // 게임 시작 시 한 번만 실행됩니다.
    void Start()
    {
        // Rigidbody2D와 SpriteRenderer 컴포넌트를 미리 가져와 변수에 저장합니다.
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (isIntro == true)
        {
            StartCoroutine(Intro());
        }
        else
        {
            transform.position = new Vector3(-56, -245, 0);
            jumpPower = 21;
            moveSpeed = 25;
        }
  

    }

    // 매 프레임마다 호출됩니다. (입력 감지용)
    void Update()
    {
        PlayerMove();
        PlayerJump();
        CheckDoorInteraction();
    }

    // 고정된 시간 간격으로 호출됩니다. (물리 계산용)
    void FixedUpdate()
    {
        
        // Rigidbody의 속도를 설정하여 캐릭터를 실제로 움직입니다.
        // Y축 속도는 원래 값을 유지하여 점프나 중력에 영향을 주지 않습니다.
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isIntro || die)
        {
            return;
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        // 만약 velocity가 구식(obsolete)이라는 경고가 뜬다면, 아래 코드로 바꿔주세요.
        // rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void PlayerMove()
    {
        if (isIntro || die)
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
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            rb.linearVelocity = new Vector2(0, -moveSpeed);
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
        if (isIntro || die)
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
        // 1. 부딪힌 물체의 태그가 "Gem"인지 확인합니다.
        if (collision.gameObject.CompareTag("Animal"))
        {
            
            sm.FadeOut3Seconds();
            die = true;
            StartCoroutine(Die());
        }
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
        transform.position = new Vector3(-0.85f, 0.28f, 0);

        sr.enabled = false;
        yield return new WaitForSeconds(6);

        sr.enabled = true;
        yield return new WaitForSeconds(3);
        gemItemOnHead.SetGemsOpacity(1);
        gemItemOnHead.SetGemsActive(true);
        yield return new WaitForSeconds(3);
        gemItemOnHead.SetGemsActive(false);
        yield return new WaitForSeconds(1);

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
        gemItemOnHead.SetGemsOpacity(0.5f);
        gemItemOnHead.SetGemsActive(true);
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        gemItemOnHead.SetGemsActive(false);
        yield return new WaitForSeconds(1);

        animator.SetTrigger("isHurt");
        gemItemOnHead.SetGemsActive(true);
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        gemItemOnHead.SetGemsActive(false);
        yield return new WaitForSeconds(1);

        sr.flipX = false;
        AnimatorChange("isRun");
        rb.linearVelocity = new Vector2(moveSpeed/2, 0);
        yield return new WaitForSeconds(2);
        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("isHurt");
        gemItemOnHead.SetGemsActive(true);
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        gemItemOnHead.SetGemsActive(false);
        yield return new WaitForSeconds(1);

        sr.flipX = true;
        AnimatorChange("isRun");
        rb.linearVelocity = new Vector2(-moveSpeed/2, 0);
        yield return new WaitForSeconds(2);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(1);

        animator.SetTrigger("isHurt");
        gemItemOnHead.SetGemsActive(true);
        rb.linearVelocity = new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGround == true);
        gemItemOnHead.SetGemsActive(false);
        AnimatorChange("isIDLE");


        sm.GrayFadeIn3Seconds();
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
        sm.FadeOut3Seconds();

        yield return new WaitForSeconds(3);
        sr.enabled = true;
        transform.position = new Vector3(-56, -245, 0);
        jumpPower = 21;
        moveSpeed = 25;
        yield return new WaitForSeconds(1);
        sm.FadeIn3Seconds();
        isIntro = false;
    }


    IEnumerator Die()
    {
        animator.SetTrigger("isHurt");
        isIntro = false;
        die = false;
        yield return new WaitForSeconds(0.1f);
        sm.FadeOut3Seconds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    

    IEnumerator Ending()
    {
        
        sr.enabled = false;
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(-0.85f, 0.28f, 0);
        yield return new WaitForSeconds(3);
        sm.FadeIn3Seconds();
        sr.enabled = true;


        AnimatorChange("isIDLE");
        yield return new WaitForSeconds(2);

        gemItemOnHead.SetGemsActive(true);
        AnimatorChange("isJumpUp");
        rb.linearVelocity = new Vector2(-moveSpeed, jumpPower);
        yield return new WaitUntil(() => rb.linearVelocity.y < 0.1f);
        AnimatorChange("isJumpDown");
        yield return new WaitUntil(() => isGround == true);
        gemItemOnHead.SetGemsActive(false);

        yield return new WaitForSeconds(2);
        AnimatorChange("isRun");
        rb.linearVelocity = new Vector2(moveSpeed * 2, 0);
        yield return new WaitForSeconds(1.5f);
        rb.linearVelocity = new Vector2(-moveSpeed * 2, 0);
        yield return new WaitForSeconds(1.5f);
        rb.linearVelocity = new Vector2(moveSpeed * 2, 0);
        yield return new WaitForSeconds(1.5f);
        rb.linearVelocity = new Vector2(-moveSpeed * 2, 0);
        yield return new WaitForSeconds(1.5f);
        rb.linearVelocity = Vector2.zero;

        gemItemOnHead.SetGemsOpacity(1);
        gemItemOnHead.SetGemsActive(true);
        AnimatorChange("isJumpUp");
        rb.linearVelocity = new Vector2(-moveSpeed, jumpPower);
        yield return new WaitUntil(() => rb.linearVelocity.y < 0.1f);
        AnimatorChange("isJumpDown");
        yield return new WaitUntil(() => isGround == true);
        yield return new WaitForSeconds(1);
        AnimatorChange("isJumpUp");
        rb.linearVelocity = new Vector2(-moveSpeed, jumpPower);
        yield return new WaitUntil(() => rb.linearVelocity.y < 0.1f);
        AnimatorChange("isJumpDown");
        yield return new WaitUntil(() => isGround == true);
        yield return new WaitForSeconds(1);
        AnimatorChange("isJumpUp");
        rb.linearVelocity = new Vector2(-moveSpeed, jumpPower);
        yield return new WaitUntil(() => rb.linearVelocity.y < 0.1f);
        AnimatorChange("isJumpDown");
        yield return new WaitUntil(() => isGround == true);

        yield return new WaitForSeconds(2);
        sm.FadeIn3Seconds();


        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("StartScene");
    }

    void CheckDoorInteraction()
    {
        // 1. 위쪽 방향키를 "눌렀을 때" (GetKeyDown)
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // 2. 플레이어 바로 위쪽 interactionDistance 반경 안에 "Door" 레이어가 있는지 확인
            Collider2D doorCollider = Physics2D.OverlapCircle(transform.position + Vector3.up * 0.1f, doorDetect, doorLayer);
            // (Vector3.up * 0.1f 는 약간 위쪽을 중심으로 원을 그림)

            // 3. 문 콜라이더가 감지되었다면
            if (doorCollider != null)
            {

                // 4. 플레이어 멈추기
                isIntro = true; // 조작 막기 (플래그 재활용)
                AnimatorChange("isIDLE"); // 멈춤 애니메이션

                sm.FadeOut3Seconds();
                StartCoroutine(Ending());

            }
        }
    }
}