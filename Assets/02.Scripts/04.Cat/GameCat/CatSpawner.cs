using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    private Vector3 _spawnPosition = new();

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

            if (_spawnPosition.x != this.gameObject.GetComponent<Building>().GetEntrancePoint().position.x)
                SettingSpawnTransform();

            await GameManager.Instance.CatManager.SpawnCat(_spawnPosition);
        }
    }

    private void SettingSpawnTransform()
    {
        _spawnPosition = this.gameObject.GetComponent<Building>().GetEntrancePoint().position;
        _spawnPosition.y = -1.5f;
        _spawnPosition.z = 0f;
    }
}
