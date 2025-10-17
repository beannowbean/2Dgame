using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 이게 꼭 필요합니다!

public class MainMenu : MonoBehaviour
{
    // 버튼이 클릭될 때 이 함수를 호출할 것입니다. (public이어야 함)
    public void StartGame()
    {
        // "GameScene" 부분에 1단계에서 확인한
        // 실제 게임 씬의 파일 이름을 정확하게 넣으세요. (대소문자 주의)
        SceneManager.LoadScene("SampleScene");
    }
}
