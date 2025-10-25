using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro를 사용하는 경우 추가

public class GameScreenManage : MonoBehaviour
{
    public Image fadeImage; // 검은색 전체 화면 이미지

    // [추가] Clear! 텍스트의 CanvasGroup 참조
    public CanvasGroup clearTextCanvasGroup;
    // [추가] Clear! 텍스트 페이드인 시간
    public float textFadeInDuration = 1.5f;

    private Color transparentColor = new Color(0, 0, 0, 0); // 완전 투명
    private Color grayColor = new Color(0, 0, 0, 0.5f);     // 50% 투명 (기존 변수)
    private Color blackColor = Color.black;                 // 완전 불투명 검정

    void Start()
    {
        // 1. 페이드 이미지 설정
        if (fadeImage != null)
        {
            fadeImage.color = transparentColor; // 시작 시 투명
        }
        else
        {
            Debug.LogError("Fade Image 컴포넌트가 이 오브젝트에 없습니다!", gameObject);
        }

        // 2. [추가] 시작 시 ClearText 숨기기 및 비활성화
        if (clearTextCanvasGroup != null)
        {
            clearTextCanvasGroup.alpha = 0f;
            clearTextCanvasGroup.interactable = false;
            clearTextCanvasGroup.blocksRaycasts = false;
            // 게임 시작 시에는 오브젝트 자체를 꺼둘 수 있습니다 (선택 사항).
            // clearTextCanvasGroup.gameObject.SetActive(false); 
        }
        else
        {
            Debug.LogWarning("'Clear Text Canvas Group'이 Inspector에 연결되지 않았습니다.");
        }
    }

    // --- 1. Player.cs가 호출할 "단순 페이드" 함수들 ---
    // (FadeOut3Seconds, FadeIn3Seconds, GrayFadeIn3Seconds 함수는 그대로 둡니다)
    public void FadeOut3Seconds() { StopAllCoroutines(); StartCoroutine(FadeTo(blackColor, 3.0f)); }
    public void FadeIn3Seconds() { StopAllCoroutines(); StartCoroutine(FadeTo(transparentColor, 3.0f)); }
    public void GrayFadeIn3Seconds() { StopAllCoroutines(); StartCoroutine(FadeTo(grayColor, 3.0f)); }

    // --- 2. [신규] Player.cs가 호출할 "엔딩 시작" 함수 ---
    public void StartEndingSequence()
    {
        // 이미 진행 중인 다른 페이드/연출을 중지
        StopAllCoroutines();
        // 엔딩 시퀀스 코루틴 시작
        StartCoroutine(EndingSequenceRoutine());
    }

    // --- 3. [신규] 엔딩 시퀀스 (화면 검게 -> Clear! 텍스트) ---
    private IEnumerator EndingSequenceRoutine()
    {
        // 1단계: 화면을 검게 만듦 (예: 1초 동안)
        yield return StartCoroutine(FadeTo(blackColor, 1.0f));

        // 2단계: 잠시 대기 (예: 0.5초)
        yield return new WaitForSeconds(0.5f);

        // 3단계: Clear! 텍스트를 서서히 나타나게 함
        yield return StartCoroutine(ShowClearText());
        yield return new WaitForSeconds(5f);

        yield return StartCoroutine(ClosedClearText());
    }

    // --- 4. [신규] Clear! 텍스트를 서서히 나타나게 하는 코루틴 ---
    private IEnumerator ShowClearText()
    {
        if (clearTextCanvasGroup == null) yield break; // 연결 안 됐으면 중단

        // 필요하다면 여기서 텍스트 오브젝트를 활성화
        clearTextCanvasGroup.gameObject.SetActive(true); 

        float timer = 0f;
        while (timer < textFadeInDuration)
        {
            timer += Time.deltaTime;
            // 알파 값을 0에서 1로 변경
            clearTextCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / textFadeInDuration);
            yield return null;
        }
        clearTextCanvasGroup.alpha = 1f; // 확정

        // (선택 사항) 텍스트가 나타난 후 상호작용 가능하게
        // clearTextCanvasGroup.interactable = true;
        // clearTextCanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator ClosedClearText()
    {
        float timer = 0f;
        while (timer < textFadeInDuration)
        {
            timer += Time.deltaTime;
            // 알파 값을 0에서 1로 변경
            clearTextCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / textFadeInDuration);
            yield return null;
        }
        clearTextCanvasGroup.alpha = 0f; // 확정
        clearTextCanvasGroup.gameObject.SetActive(false);
    }

    // [범용] 목표 색상으로 지정한 시간 동안 페이드 (기존 함수)
    private IEnumerator FadeTo(Color targetColor, float duration)
    {
        if (fadeImage == null) yield break;

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