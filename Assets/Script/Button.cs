using UnityEngine;
using UnityEngine.Tilemaps;


public class Button : MonoBehaviour
{
    // [추가] 이 버튼의 상태를 알려줄 DoorManager 참조
    public DoorManager dm;
    private SpriteRenderer sr;

    // 현재 Gem에 의해 눌려있는지 여부
    private bool isPressed = false;

    public TilemapRenderer tilemapToHide;

    void Start()
    {
        isPressed = false; // 시작 시 눌리지 않은 상태로 초기화
        sr = GetComponent<SpriteRenderer>();
    }

    // 다른 오브젝트가 Trigger 영역 안으로 "들어오는 순간"
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 들어온 것이 Gem이고, 아직 안 눌려있었다면
        if (!isPressed && other.CompareTag("Gem"))
        {
            isPressed = true;
            sr.enabled = false;
            tilemapToHide.enabled = false;
            dm.NotifyButtonPressed();

        }
    }

    // 다른 오브젝트가 Trigger 영역에서 "나가는 순간"
    private void OnTriggerExit2D(Collider2D other)
    {
        // 나간 것이 Gem이고, 눌려있었다면
        if (isPressed && other.CompareTag("Gem"))
        {
            isPressed = false;
            sr.enabled = true;
            tilemapToHide.enabled = true;
            dm.NotifyButtonReleased();
        }
    }

    
}