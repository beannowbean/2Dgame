using UnityEngine;

public class DoorManager : MonoBehaviour
{
    // 필요한 버튼의 총 개수 (Inspector에서 4로 설정)
    public int requiredButtons = 4;

    // 현재 눌려있는 버튼의 개수
    private int pressedButtonCount = 0;

    // 문 오브젝트 자체 (이 스크립트가 붙어있는 오브젝트)
    private GameObject doorObject;

    void Start()
    {
        doorObject = gameObject; // 이 스크립트가 붙은 오브젝트가 문이라고 가정
        pressedButtonCount = 0; // 시작 시 눌린 버튼 0개
        // 문을 시작 시 비활성화 (이미 Hierarchy에서 꺼놨다면 생략 가능)
        doorObject.SetActive(false);
    }

    // ButtonController가 호출할 함수: 버튼이 눌렸을 때
    public void NotifyButtonPressed()
    {
        pressedButtonCount++;
        CheckDoorStatus();
    }

    // ButtonController가 호출할 함수: 버튼에서 떨어졌을 때
    public void NotifyButtonReleased()
    {
        // 음수가 되지 않도록 방지
        if (pressedButtonCount > 0)
        {
            pressedButtonCount--;
        }
        CheckDoorStatus();
    }

    // 문의 상태를 업데이트하는 함수
    void CheckDoorStatus()
    {
        // 현재 눌린 버튼 수가 필요한 버튼 수와 같으면 문을 켠다
        if (pressedButtonCount == requiredButtons)
        {
            if (!doorObject.activeSelf) // 문이 꺼져있었다면
            {
                doorObject.SetActive(true); // 문 활성화
            }
        }
        // 그렇지 않으면 (하나라도 떨어지면) 문을 끈다
        else
        {
            if (doorObject.activeSelf) // 문이 켜져있었다면
            {
                doorObject.SetActive(false); // 문 비활성화
            }
        }
    }
}