using Cysharp.Threading.Tasks;

public class SaveLoadManager : BaseManager<SaveLoadManager>
{
    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }



}
