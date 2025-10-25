using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameScreenManage : MonoBehaviour
{
    // [수정] 기본 페이드용 이미지만 남김
    public Image fadeImage; // 기존의 검은색 전체 화면 이미지

    private Color transparentColor = new Color(0, 0, 0, 0); // 완전 투명
    private Color grayColor = new Color(0, 0, 0, 0.5f);
    private Color blackColor = Color.black; // 완전 불투명 검정

    void Start()
    {
        // 1. Image 컴포넌트를 가져옵니다.
        fadeImage = GetComponent<Image>();

        // 2. 시작 시 화면이 투명한지 확인합니다.
        
            fadeImage.color = transparentColor;

        // 3. [삭제] 원형 효과 이미지 관련 코드 제거
    }

    // --- 1. Player.cs가 호출할 "단순 페이드" 함수들 ---

    // [신규] 3초간 까매지기
    public void FadeOut3Seconds()
    {
        StopAllCoroutines();
        // 범용 FadeTo 코루틴을 3초, 검은색(blackColor)으로 호출
        StartCoroutine(FadeTo(blackColor, 3.0f));
    }

    // [신규] 3초간 다시 밝아지기
    public void FadeIn3Seconds()
    {
        StopAllCoroutines();
        // 범용 FadeTo 코루틴을 3초, 투명색(transparentColor)으로 호출
        StartCoroutine(FadeTo(transparentColor, 3.0f));
    }

  

    public void GrayFadeIn3Seconds()
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(grayColor, 3.0f));
    }

    // [범용] 목표 색상으로 지정한 시간 동안 페이드
    private IEnumerator FadeTo(Color targetColor, float duration)
    {
        if (fadeImage == null) yield break; // 이미지가 없으면 중단

        float timer = 0f;
        Color startColor = fadeImage.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            fadeImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        fadeImage.color = targetColor;
    }


}