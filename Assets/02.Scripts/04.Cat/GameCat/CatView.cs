using Cysharp.Threading.Tasks;
using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public class CatView : MonoBehaviour
{
    private CatViewModel _catViewModel;
    private Building _targetBuilding;
    private Vector3 _targetTransform;
    private Transform _pointInBuilding;

    [SerializeField] private SkinnedMeshRenderer _bodyRenderer;
    [SerializeField] private SkinnedMeshRenderer _eyeRenderer;
    [SerializeField] private SkinnedMeshRenderer _mouthRenderer;
    [SerializeField] private CatAnimationControl _catAnimationControl;
    [SerializeField] private GameObject _newCatParticle;

    public CatViewModel CatViewModelProp { get => _catViewModel; }

    private void FixedUpdate()
    {
        if (_catViewModel == null)
            return;

        if (_targetBuilding == null)
        {
            if (_catViewModel.CatState == CatState.MoveToTarget)
            {
                SearchDespawnPoint();
            }

            return;
        }

        if (_catViewModel.CatState == CatState.MoveToTarget)
        {
            MoveCatOnFixedUpdate();
            CheckCatArriveTarget();
        }
    }

    public void BindViewMdoel<T>(T bindVM) where T : ViewModelBase
    {
        if (bindVM == null)
            return;
        bindVM.PropertyChanged += OnPropChagned_View;

        InvokeCurrentValue(bindVM);
    }

    private void InvokeCurrentValue(ViewModelBase viewModel)
    {
        switch (viewModel)
        {
            case CatViewModel catViewModel:
                OnPropChagned_View(catViewModel, new PropertyChangedEventArgs(nameof(CatViewModel.CatState)));
                break;

            case CatEncyclopediaViewModel encyclopediaViewModel:
                OnPropChagned_View(encyclopediaViewModel, new PropertyChangedEventArgs(nameof(CatEncyclopediaViewModel.IsCollected)));
                break;
        }
    }

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        if (sender is CatViewModel)
        {
            switch (e.PropertyName)
            {
                case nameof(CatViewModelProp.CatState):
                    {
                        ActionFromStatus();
                    }
                    break;
            }
        }
        else if (sender is CatEncyclopediaViewModel encyclopediaVM)
        {
            switch(e.PropertyName)
            {
                case nameof(CatEncyclopediaViewModel.IsCollected):
                    {
                        ActiveNotCollectedCatParticle(encyclopediaVM.IsCollected);
                    }
                    break;
            }
        }
    }

    private void ActiveNotCollectedCatParticle(bool isCollected)
    {
        if(isCollected == false)
            _newCatParticle.SetActive(true);
        else if(isCollected == true)
            _newCatParticle.SetActive(false);
    }

    private void MoveCatOnFixedUpdate()
    {
        if(_catViewModel == null)
            return;

        _catAnimationControl.PlayMoveToTarget(_catViewModel.CatSpeed);

        transform.Translate(Vector3.forward * _catViewModel.CatSpeed * Time.fixedDeltaTime);
    }

    public void InitCatView(CatViewModel catViewModel, Building targetBuilding, CatEncyclopediaViewModel enclopediaVM)
    {
        if (catViewModel == null || targetBuilding == null)
            return;

        _catViewModel = catViewModel;
        _targetBuilding = targetBuilding;

        BindViewMdoel(_catViewModel);
        BindViewMdoel(enclopediaVM);
        SettingMaterial(catViewModel);
        SettingTargetPosition();
    }

    private async void SettingMaterial(CatViewModel catViewModel)
    {
        // ID가 특수 고양이일 경우 ViewModel에서 BodyAddressableNum을 1000으로 설정
        if(catViewModel.CatBodyAddressableNum == 1000)
        {
            SettingSpecialMaterial(catViewModel);
        }
        else
        {
            try
            {
                Material bodyMaterial =
                await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                    GameManager.Instance.CatManager.CatBodySkinDatas[catViewModel.CatBodyAddressableNum].AddressableString,
                    destroyCancellationToken);

                if (bodyMaterial != null)
                {
                    _bodyRenderer.sharedMaterial = bodyMaterial;
                }

                Material eyeMaterial =
                    await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                        GameManager.Instance.CatManager.CatEyeSkinDatas[catViewModel.CatEyeAddressableNum].AddressableString,
                        destroyCancellationToken);

                if (eyeMaterial != null)
                {
                    _eyeRenderer.sharedMaterial = eyeMaterial;
                }

                Material mouthMaterial =
                    await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                        GameManager.Instance.CatManager.CatMouthSkinDatas[catViewModel.CatMouthAddressableNum].AddressableString,
                        destroyCancellationToken);

                if (mouthMaterial != null)
                {
                    _mouthRenderer.sharedMaterial = mouthMaterial;
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    private async void SettingSpecialMaterial(CatViewModel catViewModel)
    {
        string specialCatId = catViewModel.CatId;
        GameManager.Instance.DataManager.TryGetData<CatInfoData>(specialCatId, out var catData);

        Material bodyMaterial;

        if (catData.SpecialCatBody == "None")
        {
            bodyMaterial =
            await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                "Cat/Material/Body_01", destroyCancellationToken);

            if (bodyMaterial != null)
            {
                _bodyRenderer.sharedMaterial = bodyMaterial;
            }
        }
        else
        {
            bodyMaterial = await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                    catData.SpecialCatBody, destroyCancellationToken);

            if (bodyMaterial != null)
            {
                _bodyRenderer.sharedMaterial = bodyMaterial;
            }
        }

        Material eyeMaterial;

        if (catData.SpecialCatEye == "None")
        {
            eyeMaterial =
            await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                "Cat/Material/Eye_01", destroyCancellationToken);

            if (eyeMaterial != null)
            {
                _bodyRenderer.sharedMaterial = eyeMaterial;
            }
        }
        else
        {
            eyeMaterial = await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                    catData.SpecialCatBody, destroyCancellationToken);

            if (eyeMaterial != null)
            {
                _bodyRenderer.sharedMaterial = eyeMaterial;
            }
        }

        Material mouthMaterial;

        if (catData.SpecialCatMouth == "None")
        {
            mouthMaterial =
            await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                "Cat/Material/Mouth_01", destroyCancellationToken);

            if (mouthMaterial != null)
            {
                _bodyRenderer.sharedMaterial = mouthMaterial;
            }
        }
        else
        {
            mouthMaterial = await GameManager.Instance.ResourceManager.LoadAssetAsync<Material>(
                    catData.SpecialCatBody, destroyCancellationToken);

            if (mouthMaterial != null)
            {
                _bodyRenderer.sharedMaterial = mouthMaterial;
            }
        }
    }

    private void SettingTargetPosition()
    {
        if (_targetBuilding == null)
        {
            Debug.Log("목표 오브젝트를 찾을 수 없습니다.");
            return;
        }

        if (_targetBuilding.TryGetComponent<Building>(out Building buildingObject))
        {
            _targetTransform = buildingObject.GetEntrancePoint().transform.position;
            _targetTransform.y = 0f;
            _targetTransform.z = 0f;
        }

        Vector3 direction = (_targetBuilding.transform.position - transform.position).normalized;
        direction.y = 0f;
        direction.z = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void CheckCatArriveTarget()
    {
        if (_targetBuilding == null)
            return;

        float remainingX = _targetTransform.x - transform.position.x;

        if (remainingX * transform.forward.x <= 0f)
        {
            if(CheckTargetExistenceORChanged())
            {
                // 목표지점이 옮겨지거나 철거된 상태
                _catViewModel.CatState = CatState.TargetMissing;
                return;
            }

            if (_targetBuilding.GetComponent<CatSpawner>())
            {
                ArriveSpanwPositionAndEscape();
            }
            else if (_targetBuilding.TryGetComponent<Building>(out Building buildingObject))
            {
                ArriveBuildingEntrance(buildingObject);
            }

            return;
        }
    }

    private bool CheckTargetExistenceORChanged()
    {
        if(_targetBuilding == null || !_targetBuilding.gameObject.activeInHierarchy)
            return true;

        if (!Mathf.Approximately(_targetBuilding.transform.position.x, _targetTransform.x))
        {
            return true;
        }

        return false;
    }

    private void ArriveBuildingEntrance(Building building)
    {
        if(building == null)
            return;

        if(building.GetAvailableCatPointCount() == 0)
        {
            Debug.Log("해당 건물은 빈 자리가 없습니다.");
            _catViewModel.CatState = CatState.TargetMissing;
            // 대기 모션 출력 관련 메서드
            return;
        }

        _pointInBuilding = building.GetAvailableCatPoint(this);
        this.gameObject.transform.position = _pointInBuilding.position;
        _catViewModel.CatState = CatState.InBuildingAction;
    }

    private void ArriveSpanwPositionAndEscape()
    {
        UnBindViewMdoel(_catViewModel);
        UnBindViewMdoel(GameManager.Instance.EconomyService_DH.CatEncyclopediaList[_catViewModel.CatId]);
        // TODO(안우재/08.09) : 가까운 Spawn지역에 도착 시 탈출하는 모션 출력 필요.
        GameManager.Instance.CatManager.DespawnCat(this.gameObject);
    }

    private void UnBindViewMdoel<T>(T unBindVM) where T : ViewModelBase
    {
        if (unBindVM == null)
            return;
        unBindVM.PropertyChanged -= OnPropChagned_View;
        ActionFromStatus();
    }

    private async void ActionFromStatus()
    {
        try
        {
            if (_catViewModel == null)
                return;

            if (_catViewModel.CatState == CatState.InBuildingAction)
            {
                ChangeLayer(0);
                _catAnimationControl.PlayAction();
                await UniTask.Delay(TimeSpan.FromSeconds(4f), cancellationToken: this.GetCancellationTokenOnDestroy());
                _catViewModel.CatState = CatState.SearchTarget;
            }
            else if (_catViewModel.CatState == CatState.TargetMissing)
            {
                _catAnimationControl.PlayTargetMissingAction();
                await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: this.GetCancellationTokenOnDestroy());
                _catViewModel.CatState = CatState.SearchTarget;
            }
            else if (_catViewModel.CatState == CatState.SearchTarget)
            {
                SearchDespawnPoint();
            }
            else if (_catViewModel.CatState == CatState.MoveToTarget)
            {
                ChangeLayer(6);
            }
        }
        catch(OperationCanceledException) 
        { 

        }
    }

    private void ChangeLayer(int layer)
    {
        _bodyRenderer.gameObject.layer = layer;
        _eyeRenderer.gameObject.layer = layer;
        _mouthRenderer.gameObject.layer = layer;
        _newCatParticle.layer = layer;
    }

    private void MoveCatBuildingEntrance()
    {
        if (_pointInBuilding != null && _targetBuilding != null)
        {
            if(_targetBuilding.GetEntrancePoint() != null)
            {
                Vector3 entrancePoint = _targetBuilding.GetEntrancePoint().position;
                entrancePoint.z = 0;

                _targetBuilding.ReturnCatPoint(_pointInBuilding, this);
                _pointInBuilding = null;

                transform.position = entrancePoint;
            }
        }
    }

    public void SearchDespawnPoint()
    {
        MoveCatBuildingEntrance();

        if (GameManager.Instance.CatManager.CatSpanweList.Count == 0)
        {
            // 고양이가 필드에 있지만 Despawn될 Spawn 지역이 없는 상태 현재는 그냥 바로 Despawn
            GameManager.Instance.CatManager.DespawnCat(this.gameObject);
            return;
        }

        CatSpawner targetSpawner = null;
        float nearDistance = float.MaxValue;
        for(int i = 0; i < GameManager.Instance.CatManager.CatSpanweList.Count; i++)
        {
            CatSpawner spawner = GameManager.Instance.CatManager.CatSpanweList[i];

            if (spawner == null || !spawner.gameObject.activeInHierarchy)
                continue;

            float distance = Mathf.Abs(
                spawner.transform.position.x - transform.position.x
            );

            if (distance < nearDistance)
            {
                nearDistance = distance;
                targetSpawner = spawner;
            }
        }

        if (targetSpawner == null)
        {
            GameManager.Instance.CatManager.DespawnCat(gameObject);
            return;
        }

        _targetBuilding = targetSpawner.gameObject.GetComponent<Building>();
        SettingTargetPosition();
        _catViewModel.CatState = CatState.MoveToTarget;
    }

    public void EscapeDestroyBuilding()
    {
        _pointInBuilding = null;
        _targetBuilding = null;

        _catViewModel.CatState = CatState.TargetMissing;
    }
}
