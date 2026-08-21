using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
public class BuildingDataEditorWindow : EditorWindow
{
    private const string JSON_PATH = "Assets/04.Data/BuildingData.json";
    private List<BuildingData> _buildingList = new List<BuildingData>();
    private Vector2 _scrollLeft;
    private Vector2 _scrollRight;
    private int _selectedIndex = -1;
    [MenuItem("Tools/Building Data Editor")]
    public static void OpenWindow()
    {
        BuildingDataEditorWindow window = GetWindow<BuildingDataEditorWindow>("건물 Width 편집기");
        window.minSize = new Vector2(550, 350);
        window.LoadJsonData();
        window.Show();
    }
    private void OnEnable()
    {
        LoadJsonData();
    }
    private void LoadJsonData()
    {
        if (!File.Exists(JSON_PATH))
        {
            Debug.LogError($"[{nameof(BuildingDataEditorWindow)}] JSON 파일을 찾을 수 없습니다: {JSON_PATH}");
            return;
        }
        try
        {
            string json = File.ReadAllText(JSON_PATH);
            _buildingList = JsonConvert.DeserializeObject<List<BuildingData>>(json);
            if (_buildingList == null)
            {
                _buildingList = new List<BuildingData>();
                Debug.LogWarning($"[{nameof(BuildingDataEditorWindow)}] JSON 데이터가 비어 있습니다.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(BuildingDataEditorWindow)}] JSON 로드 실패: {ex.Message}");
        }
    }
    private void SaveJsonData()
    {
        if (_buildingList == null || _buildingList.Count == 0)
        {
            Debug.LogWarning($"[{nameof(BuildingDataEditorWindow)}] 저장할 데이터가 없습니다.");
            return;
        }
        try
        {
            string json = JsonConvert.SerializeObject(_buildingList, Formatting.Indented);
            File.WriteAllText(JSON_PATH, json);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("저장 완료", "BuildingData.json 파일에 성공적으로 저장되었습니다.", "확인");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(BuildingDataEditorWindow)}] JSON 저장 실패: {ex.Message}");
        }
    }
    private void OnGUI()
    {
        // 툴바 (새로고침 & 저장)
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("새로고침", EditorStyles.toolbarButton, GUILayout.Width(80)))
        {
            LoadJsonData();
        }
        GUILayout.FlexibleSpace();
        GUI.backgroundColor = new Color(0.4f, 0.9f, 0.4f);
        if (GUILayout.Button("JSON에 저장하기", EditorStyles.toolbarButton, GUILayout.Width(130)))
        {
            SaveJsonData();
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        // 좌측: 건물 목록
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        EditorGUILayout.LabelField("건물 목록", EditorStyles.boldLabel);
        _scrollLeft = EditorGUILayout.BeginScrollView(_scrollLeft, "box");
        for (int i = 0; i < _buildingList.Count; i++)
        {
            BuildingData data = _buildingList[i];
            string displayName = string.IsNullOrEmpty(data.Name) ? data.Id : $"{data.Name} (W:{data.Width})";
            GUI.backgroundColor = (_selectedIndex == i) ? new Color(0.6f, 0.8f, 1f) : Color.white;
            if (GUILayout.Button(displayName, GUILayout.Height(26)))
            {
                _selectedIndex = i;
                GUI.FocusControl(null);
            }
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        // 우측: ID, 이름, Width 슬라이더 편집
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        if (_selectedIndex >= 0 && _selectedIndex < _buildingList.Count)
        {
            BuildingData selected = _buildingList[_selectedIndex];
            _scrollRight = EditorGUILayout.BeginScrollView(_scrollRight);
            EditorGUILayout.LabelField($"[ {selected.Name} ] 편집", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            // 1. ID & 이름 표시
            EditorGUILayout.LabelField("ID", selected.Id);
            EditorGUILayout.LabelField("이름", selected.Name);
            EditorGUILayout.Space(10);
            // 2. Width 조절 슬라이더 (1.0 ~ 100.0, 소수점 첫째 자리 스냅)
            float rawWidth = EditorGUILayout.Slider("Width (좌우 넓이)", selected.Width, 1.0f, 100.0f);
            selected.Width = Mathf.Round(rawWidth * 10.0f) / 10.0f;
            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.HelpBox("왼쪽 목록에서 건물을 선택해주세요.", MessageType.Info);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}