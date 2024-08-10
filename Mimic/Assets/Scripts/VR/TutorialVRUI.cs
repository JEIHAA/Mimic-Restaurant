using UnityEngine;
using UnityEngine.UI;

public class TutorialVRUI : MonoBehaviour
{
    public RawImage[] images; // 이미지 배열 설정
    private int currentImageIndex = 0; // 현재 이미지의 인덱스

    void Start()
    {
        // 초기화할 때 첫 번째 이미지를 표시
        ShowImage(currentImageIndex);
    }

    // 다음 이미지 표시
    public void ShowNextImage()
    {
        currentImageIndex = (currentImageIndex + 1) % images.Length; // 다음 이미지 인덱스 계산
        ShowImage(currentImageIndex); // 이미지 표시 함수 호출
    }

    // 이전 이미지 표시
    public void ShowPreviousImage()
    {
        currentImageIndex = (currentImageIndex - 1 + images.Length) % images.Length; // 이전 이미지 인덱스 계산
        ShowImage(currentImageIndex); // 이미지 표시 함수 호출
    }

    // 현재 인덱스에 해당하는 이미지를 표시
    private void ShowImage(int index)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i == index); // 현재 이미지만 활성화
        }
    }
}
