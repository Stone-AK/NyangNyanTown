using UnityEngine;

public class CatAnimationControl : MonoBehaviour
{
    [SerializeField] private Animator _catAnimation;

    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int MoveSpeedValue = Animator.StringToHash("MoveSpeed");
    private static readonly int IsAction = Animator.StringToHash("IsAction");
    private static readonly int IsTargetMissing = Animator.StringToHash("IsTargetMissing");

    public void PlayMoveToTarget(float speed)
    {
        SetAllBoolFalse();
        _catAnimation.SetBool(IsMove, true);
        _catAnimation.SetFloat(MoveSpeedValue, speed);
    }

    public void PlayAction()
    {
        SetAllBoolFalse();
        _catAnimation.SetBool(IsAction, true);
    }

    public void PlayTargetMissingAction()
    {
        SetAllBoolFalse();
        _catAnimation.SetBool(IsTargetMissing, true);
    }

    private void SetAllBoolFalse()
    {
        _catAnimation.SetBool(IsMove, false);
        _catAnimation.SetBool(IsAction, false);
        _catAnimation.SetBool(IsTargetMissing, false);
    }
}
