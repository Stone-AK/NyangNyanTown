using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameDataManager : BaseManager<GameDataManager>
{
    private readonly Dictionary<Type, object> _dataTables = new Dictionary<Type, object>();

    private const int LoadingTransitionDelay = 1000;

    public override async UniTask InitializeAsync()
    {
        _dataTables.Clear();
        await PreloadDataAsync();
        await LoadRuntimeDataAsync();

    }

    public async UniTask PreloadDataAsync(IProgress<LoadingProgress> progress = null)
    {
        await UniTask.Delay(1);
    }

    public async UniTask LoadRuntimeDataAsync(IProgress<LoadingProgress> progress = null)
    {
        List<LoadingTask> loadingTasks = CreateLoadingTasks();

        ReportLoadingProgress(progress, 0f, LoadingStep.Initialize);

        await UniTask.Delay(LoadingTransitionDelay);

        float progressStep = 1f / loadingTasks.Count;

        for (int index = 0; index < loadingTasks.Count; index++)
        {
            LoadingTask loadingTask = loadingTasks[index];

            await loadingTask.Execute(loadingTask.Key, destroyCancellationToken);

            float progressValue = progressStep * (index + 1);

            ReportLoadingProgress(progress, progressValue, loadingTask.Step);
        }

        ReportLoadingProgress(progress, 1f, LoadingStep.Complete);

        await UniTask.Delay(LoadingTransitionDelay);
    }

    private List<LoadingTask> CreateLoadingTasks()
    {
        List<LoadingTask> jobs = new List<LoadingTask>
        {
            new LoadingTask(LoadingStep.LoadTestData, AddressableKey.GetDataKey(DataType.TestData), LoadDataAsync<TestData>),
            new LoadingTask(LoadingStep.LoadBuildingData, AddressableKey.GetDataKey(DataType.BuildingData), LoadDataAsync<BuildingData>),
            new LoadingTask(LoadingStep.LoadCatInfoData, AddressableKey.GetDataKey(DataType.CatInfoData), LoadDataAsync<CatInfoData>),
            new LoadingTask(LoadingStep.LoadCatBodySkin, AddressableKey.GetDataKey(DataType.CatBodySkinData), LoadDataAsync<CatBodySkinData>),
            new LoadingTask(LoadingStep.LoadCatEyeSkin, AddressableKey.GetDataKey(DataType.CatEyeSkinData), LoadDataAsync<CatEyeSkinData>),
            new LoadingTask(LoadingStep.LoadCatMouthSkin, AddressableKey.GetDataKey(DataType.CatMouthSkinData), LoadDataAsync<CatMouthSkinData>),

        };

        return jobs;
    }

    private void ReportLoadingProgress(IProgress<LoadingProgress> progress, float value, LoadingStep step)
    {
        if (progress == null)
        {
            return;
        }

        progress.Report(new LoadingProgress(value, step));
    }

    public bool TryGetDataTable<T>(out Dictionary<string, T> dataTable) where T : BaseData
    {
        dataTable = null;

        if (!_dataTables.TryGetValue(typeof(T), out object loadedDataTable))
        {
            Debug.LogError($"[{nameof(GameDataManager)}:{nameof(TryGetDataTable)}] '{typeof(T).Name}' 데이터 테이블이 로드되어 있지 않습니다.");
            return false;
        }

        if (loadedDataTable is not Dictionary<string, T> typeCastDataTable)
        {
            Debug.LogError($"[{nameof(GameDataManager)}:{nameof(TryGetDataTable)}] '{typeof(T).Name}' 로드된 데이터 테이블과 타입이 일치하지 않습니다.");
            return false;
        }

        dataTable = typeCastDataTable;
        return true;
    }

    public bool TryGetData<T>(string dataId, out T data) where T : BaseData
    {
        data = null;

        if (string.IsNullOrWhiteSpace(dataId))
        {
            Debug.LogError($"[{nameof(GameDataManager)}:{nameof(TryGetData)}] 전달된 DataId가 null이거나 빈 문자열 또는 공백 문자열입니다.");
            return false;
        }

        if (!TryGetDataTable(out Dictionary<string, T> dataTable))
        {
            return false;
        }

        if (!dataTable.TryGetValue(dataId, out data))
        {
            Debug.LogError($"[{nameof(GameDataManager)}:{nameof(TryGetData)}] '{typeof(T).Name}' 데이터 테이블에서 DataId '{dataId}'를 찾을 수 없습니다.");
            return false;
        }

        return true;
    }

    private async UniTask LoadDataAsync<T>(string key, CancellationToken cancellationToken) where T : BaseData
    {
        Dictionary<string, T> loadedDataTable = await LoadDataTableAsync<T>(key, cancellationToken);

        if (loadedDataTable == null)
        {
            Debug.LogError($"[{nameof(GameDataManager)}:{nameof(LoadDataAsync)}] '{key}' 데이터 테이블을 로드하지 못했습니다.");
            return;
        }

        _dataTables[typeof(T)] = loadedDataTable;
    }

    private async UniTask<Dictionary<string, T>> LoadDataTableAsync<T>(string key, CancellationToken cancellationToken) where T : BaseData
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            Debug.LogError($"[{nameof(GameDataManager)}:{nameof(LoadDataTableAsync)}] 전달된 key가 null이거나 빈 문자열 또는 공백 문자열입니다.");
            return null;
        }

        try
        {
            TextAsset jsonTextAsset = await GameManager.Instance.ResourceManager.LoadAssetAsync<TextAsset>(key, cancellationToken);

            if (jsonTextAsset == null)
            {
                throw new InvalidOperationException($"'{key}' 데이터 TextAsset 로드에 실패했습니다.");
            }

            List<T> items = JsonConvert.DeserializeObject<List<T>>(jsonTextAsset.text);

            if (items == null)
            {
                throw new InvalidOperationException($"'{key}' JSON 역직렬화 결과가 null입니다.");
            }

            Dictionary<string, T> dataTable = new();

            foreach (T item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Id))
                {
                    throw new FormatException($"'{key}' DataId가 없는 데이터가 존재합니다.");
                }

                if (!dataTable.TryAdd(item.Id, item))
                {
                    throw new FormatException($"'{key}' 중복된 DataId '{item.Id}'가 존재합니다.");
                }
            }

            return dataTable;
        }
        catch (JsonException exception)
        {
            Debug.LogError($"[{nameof(GameDataManager)}:{nameof(LoadDataTableAsync)}] JSON 파싱에 실패했습니다.\n{exception}");
        }
        catch (Exception exception)
        {
            Debug.LogError($"[{nameof(GameDataManager)}:{nameof(LoadDataTableAsync)}] 데이터 테이블 로드 중 오류가 발생했습니다.\n{exception}");
        }

        return null;
    }
}