using UnityEngine;

public class CatView : MonoBehaviour
{
    private CatViewModel _catViewModel;

    private Transform _targetPosition;

    private void FixedUpdate()
    {
        MoveCat();
    }

    private void MoveCat()
    {
        if(_catViewModel == null)
            return;

        // TODO(안우재/08.06) : 목표 방향으로 갈 수 있도록 rotation 부분 추가 필요, Right 이동이 앞으로 개념
        transform.Translate(Vector3.right * _catViewModel.CatSpeed * Time.deltaTime);
    }

    public void InitCatView(CatViewModel catViewModel)
    {
        _catViewModel = catViewModel;
    }
}
