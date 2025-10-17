using UnityEngine;
using UnityEngine.UI; // UI (Image)를 다루기 위해 필수!
using System.Collections; // 코루틴을 위해 필수!

public class ScreenFader : MonoBehaviour
{
    // Inspector 창에서 조절할 값들
    public float delay = 20.0f;      // 20초 뒤에 시작
    public float delay2 = 10.0f;
    public float fadeDuration = 3.0f;  // 3초에 걸쳐 어두워짐
    public float targetAlpha = 0.5f;   // 50%의 반투명 검은색 (0~1)

    private Image fadeImage;

    

    void Start()
    {
        

        // 1. Image 컴포넌트를 가져옵니다.
        fadeImage = GetComponent<Image>();

        // 2. 코루틴을 시작합니다.
        StartCoroutine(FadeToBlack());
    }

    private void Update()
    {
        
    }

    IEnumerator FadeToBlack()
    {
        // 1. 맨 처음 20초를 기다립니다.
        yield return new WaitForSeconds(delay);

        // 2. 3초에 걸쳐 서서히 어둡게 만듭니다.
        float timer = 0f;
        Color startColor = fadeImage.color; // 현재 색상 (알파 0)
        Color endColor = new Color(0, 0, 0, targetAlpha); // 목표 색상 (알파 0.5)
        Color endColor2 = new Color(0, 0, 0, 1);
;
        while (timer < fadeDuration)
        {
            // 타이머 시간 증가
            timer += Time.deltaTime;

            // 현재 진행도 (0~1)
            float t = timer / fadeDuration;

            // Lerp를 사용해 부드럽게 색상(알파값)을 변경
            fadeImage.color = Color.Lerp(startColor, endColor, t);

            // 다음 프레임까지 대기
            yield return null;
        }

        // 3. 오차 방지를 위해 정확한 목표 색상으로 설정
        fadeImage.color = endColor;
        timer = 0f;

        yield return new WaitForSeconds(delay2);

        while (timer < fadeDuration)
        {
            // 타이머 시간 증가
            timer += Time.deltaTime;

            // 현재 진행도 (0~1)
            float t = timer / fadeDuration;

            // Lerp를 사용해 부드럽게 색상(알파값)을 변경
            fadeImage.color = Color.Lerp(endColor, endColor2, t);

            // 다음 프레임까지 대기
            yield return null;
        }

        fadeImage.color = endColor2;
        timer = 0f;

        yield return new WaitForSeconds(2);
        while (timer < fadeDuration)
        {
            // 타이머 시간 증가
            timer += Time.deltaTime;

            // 현재 진행도 (0~1)
            float t = timer / fadeDuration;

            // Lerp를 사용해 부드럽게 색상(알파값)을 변경
            fadeImage.color = Color.Lerp(endColor2, startColor, t);

            // 다음 프레임까지 대기
            yield return null;
        }
    }
}