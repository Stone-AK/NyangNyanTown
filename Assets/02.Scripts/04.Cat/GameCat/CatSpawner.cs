using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        CreateCatSpawner();
    }

    private void OnDisable()
    {
        RemoveCatSpawner();
    }

    private void Start()
    {
        AutoSpawnCatInWhile().Forget();
    }

    private void CreateCatSpawner()
    {
        if(GameManager.Instance.CatManager == null)
            return;

        GameManager.Instance.CatManager.CatSpanweList.Add(this);
    }

    private void RemoveCatSpawner()
    {
        if (GameManager.Instance.CatManager == null)
            return;

        GameManager.Instance.CatManager.CatSpanweList.Remove(this);
    }

    private async UniTask AutoSpawnCatInWhile()
    {
        while(true)
        {
            double delaySeconds = GameUtil.Random.NextDouble() + 1.0;

            await UniTask.Delay(
                TimeSpan.FromSeconds(delaySeconds),
                cancellationToken: this.GetCancellationTokenOnDestroy()
            );

            if (GameManager.Instance.CatManager == null)
                return;

            Transform spawnTransform = this.gameObject.GetComponent<Building>().GetEntrancePoint();
            await GameManager.Instance.CatManager.SpawnCat(spawnTransform);
        }
    }
}
