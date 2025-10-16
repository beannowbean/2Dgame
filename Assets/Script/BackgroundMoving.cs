using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    // 카메라 (플레이어의 시점)
    private Transform cameraTransform;

    // 배경이 카메라를 따라가는 속도 (이 값이 클수록 빨리 움직임)
    // back은 0.1, middle은 0.5처럼 값을 다르게 설정할 것입니다.
    [SerializeField] private float parallaxEffectMultiplier;

    private Vector3 lastCameraPosition;

    void Start()
    {
        // Main Camera를 찾아서 Transform 정보를 가져옵니다.
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    // LateUpdate는 모든 Update 함수가 호출된 후에 실행됩니다.
    // 카메라 이동이 끝난 후에 배경을 움직여야 떨림(jitter) 현상이 없습니다.
    void LateUpdate()
    {
        // 카메라가 얼마나 움직였는지 계산합니다.
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // 배경의 위치를 카메라의 움직임에 parallaxEffectMultiplier를 곱한 만큼 이동시킵니다.
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier, 0);

        // 현재 카메라 위치를 저장해 다음 프레임에 사용합니다.
        lastCameraPosition = cameraTransform.position;
    }
}