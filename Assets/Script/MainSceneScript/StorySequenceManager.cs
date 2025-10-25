using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class StorySequenceManager : MonoBehaviour
{
    public CanvasGroup storyboardCanvasGroup; // Storyboard의 CanvasGroup 연결
    public CanvasGroup textCanvasGroup;       // Text의 CanvasGroup 연결
    public CanvasGroup startButtonCanvasGroup; // StartButton의 CanvasGroup 연결

    public float backgroundFadeDuration = 1.0f; // 배경 페이드 시간
    public float textFadeDuration = 1.0f;       // 텍스트 페이드 시간
    public float buttonFadeDuration = 0.5f;     // 버튼 페이드 시간

    // StoryButton의 OnClick 이벤트에서 이 함수를 호출
    public void StartStorySequence()
    {
        // 이미 실행 중이지 않다면 코루틴 시작 (중복 방지)
        if (storyboardCanvasGroup != null && storyboardCanvasGroup.alpha < 1f)
        {
            StartCoroutine(PlaySequence());
        }
    }

    IEnumerator PlaySequence()
    {
        // --- 1. 배경(Storyboard) 페이드 인 ---
        if (storyboardCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(storyboardCanvasGroup, 0f, 1f, backgroundFadeDuration));
            // 페이드 인 후 상호작용 활성화 (배경이 클릭을 막도록)
            storyboardCanvasGroup.interactable = true;
            storyboardCanvasGroup.blocksRaycasts = true;
        }

        // --- 2. 텍스트 페이드 인 ---
        if (textCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(textCanvasGroup, 0f, 1f, textFadeDuration));
        }

        // --- 3. 시작 버튼 페이드 인 ---
        if (startButtonCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(startButtonCanvasGroup, 0f, 1f, buttonFadeDuration));
            // 페이드 인 후 상호작용 활성화
            startButtonCanvasGroup.interactable = true;
            startButtonCanvasGroup.blocksRaycasts = true;
         
        }
    }

    // CanvasGroup의 알파 값을 부드럽게 변경하는 코루
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }
        cg.alpha = endAlpha; // 확정
        yield return new WaitForSeconds(2);
    }

}