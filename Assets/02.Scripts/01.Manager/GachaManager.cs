using Cysharp.Threading.Tasks;
using UnityEngine;

public class GachaManager : BaseManager<GachaManager>
{
    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }


}
