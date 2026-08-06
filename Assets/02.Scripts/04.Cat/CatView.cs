using UnityEngine;

public class CatView : MonoBehaviour
{
    private CatViewModel _catViewModel;

    // TODO(안우재/08.06) : 테스트 용도를 위한 Start부분
    private void Start()
    {
        _catViewModel = new CatViewModel();
        _catViewModel.InitRandomCatStat();
    }

    private void FixedUpdate()
    {
        MoveCat();
    }


    private void MoveCat()
    {
        transform.Translate(Vector3.left * _catViewModel.CatSpeed * Time.deltaTime);
    }
}
