using UnityEngine;


// 이름을 GemItem에서 GemManager로 바꾸는 것을 추천합니다. 
// 여러 젬을 관리하기 때문입니다.
public class GemItemOnHead : MonoBehaviour
{
    public GameObject[] gemsToAppear;
    public Transform playerToFollow;

    // 모든 젬의 SpriteRenderer를 미리 저장해 둡니다.
    private SpriteRenderer[] gemRenderers;

    void Start()
    {
        // 1. 모든 젬의 SpriteRenderer 컴포넌트를 미리 찾아 배열에 저장
        gemRenderers = new SpriteRenderer[gemsToAppear.Length];
        for (int i = 0; i < gemsToAppear.Length; i++)
        {
            gemRenderers[i] = gemsToAppear[i].GetComponent<SpriteRenderer>();
        }

        // 2. 시작 시 모든 젬을 비활성화
        SetGemsActive(false);
    }

    void LateUpdate()
    {
        // 3. 플레이어 위치 추적 (이 기능은 유지)
        if (playerToFollow != null)
        {
            transform.position = playerToFollow.position;
        }
    }

    // [제어 함수 1] 젬 보이게/안 보이게 (Player.cs에서 호출)
    public void SetGemsActive(bool isActive)
    {
        foreach (GameObject gem in gemsToAppear)
        {
            gem.SetActive(isActive);
        }
    }

    // [제어 함수 2] 젬 투명도 즉시 설정 (Player.cs에서 호출)
    // alpha 값: 0.0 (완전 투명) ~ 1.0 (완전 불투명)
    public void SetGemsOpacity(float alpha)
    {
        // 0.0 ~ 1.0 사이 값으로 고정
        float clampedAlpha = Mathf.Clamp(alpha, 0f, 1f);

        foreach (SpriteRenderer sr in gemRenderers)
        {
            if (sr != null)
            {
                Color newColor = sr.color;
                newColor.a = clampedAlpha;
                sr.color = newColor;
            }
        }
    }

    
}