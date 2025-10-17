using UnityEngine;
using System.Collections;

public class GemItem : MonoBehaviour
{
    public GameObject[] gemsToAppear;
    public float initialDelay = 7.0f;
    public float delayBetweenGems = 0.5f;

    public float fadeInDuration = 2.0f; // 2초 동안 서서히 나타남 (투명 -> 불투명)

    public float hideAllDelay = 3.0f;

    public Transform playerToFollow;

    void Start()
    {
        
        // 1. 모든 젬 오브젝트를 "완전히" 끕니다. (SetActive(false) 사용)
        foreach (GameObject gem in gemsToAppear)
        {
            gem.SetActive(false);
        }

        // 2. 코루틴 시작
        StartCoroutine(ShowGemsSequentially());
    }

    // (Update 함수는 비워둡니다)
    void Update()
    {

    }

    void LateUpdate()
    {
        // 1. 따라다닐 대상(Player)이 지정되었다면
        if (playerToFollow != null)
        {
            // 2. 이 오브젝트(GemManager)의 위치를 "플레이어 위치 + 오프셋"으로 매 프레임 이동
            transform.position = playerToFollow.position;
        }
    }

    // 젬들을 "순차적으로 나타나게" 하는 코루틴
    IEnumerator ShowGemsSequentially()
    {
        // 1. 맨 처음 딜레이 (7초)
        yield return new WaitForSeconds(initialDelay);

        // 2. 리스트에 있는 젬들을 하나씩 순서대로 켠다
        foreach (GameObject gem in gemsToAppear)
        {
            gem.SetActive(true);
            yield return new WaitForSeconds(delayBetweenGems);
        }

        yield return new WaitForSeconds(1);
        // 4. 리스트에 있는 젬들을 다시 "전부" 끈다
        foreach (GameObject gem in gemsToAppear)
        {
            gem.SetActive(false);
        }


        // 1. 맨 처음 딜레이 (11초)
        yield return new WaitForSeconds(9);

        // 2. 리스트에 있는 젬들을 하나씩 순서대로 켠다
        foreach (GameObject gem in gemsToAppear)
        {
            // [수정!] 젬을 그냥 켜는 대신, "흐릿하게 나타나는" 코루틴을 실행
            StartCoroutine(FadeInAndAppear(gem));

        }

        // 3. 모든 젬이 나타난 후
        yield return new WaitForSeconds(3);

        // 4. 모든 젬을 다시 끈다
        foreach (GameObject gem in gemsToAppear)
        {
            gem.SetActive(false); // 오브젝트 자체를 비활성화
        }
    }

    // [새로운 코루틴] 젬 하나를 흐릿하게 나타나게 하는 함수
    IEnumerator FadeInAndAppear(GameObject gem)
    {
        // 1. 젬 오브젝트를 켠다.
        gem.SetActive(true);

        // 2. 젬의 컴포넌트들을 가져온다.
        SpriteRenderer sr = gem.GetComponent<SpriteRenderer>();


        Color halfColor = sr.color;
        halfColor.a = 0.5f;
        sr.color = halfColor;

        yield return new WaitForSeconds(2);
    }
}