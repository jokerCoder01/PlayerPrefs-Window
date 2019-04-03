//// TODOs
////
//// Sort Button Functionality
//// More Button Functionality
//// Searchfield Functionality
//// Refresh Button Functionality
//// AddNewPlayerPrefsData Functionality

using UnityEditor;
using UnityEngine;

public class PlayerPrefsWindow : EditorWindow
{

    Texture refreshIcon;
    Texture plusIcon;
    enum TYPES { INTEGER, FLOAT, STRING }
    TYPES playerPrefsTypes;
    string searchText;
    string newKey;
    string newValue;

    // Create button as a MenuItem to call the ShowWindow method
    [MenuItem("JokerCoder/PlayerPrefs Manager")]
    public static void ShowWindow()
    {
        PlayerPrefsWindow window = (PlayerPrefsWindow)GetWindow(typeof(PlayerPrefsWindow));
        window.titleContent = new GUIContent("PlayerPrefs Manager");
        window.Show();
    }

    void OnEnable()
    {
        searchText = "";
        newKey = "";
        newValue = "";
        refreshIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/PlayerPrefsManager/Icons/refresh_Icon.png", typeof(Texture));
        plusIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/PlayerPrefsManager/Icons/plus_Icon.png", typeof(Texture));
    }

    // Called for rendering and handling GUI events
    void OnGUI()
    {
        GUILayout.BeginVertical();

        DrawToolbarGUI();
        DrawAddValueArea();

        GUILayout.EndVertical();
    }

    // Draw all toolbar items
    void DrawToolbarGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        DrawSortButton();
        DrawMoreButton();
        DrawSearchField();
        DrawRefreshButton();

        GUILayout.EndHorizontal();
    }

    // Sort all PlayerPrefs alphabetically
    void DrawSortButton()
    {
        if (GUILayout.Button("Sort", EditorStyles.toolbarPopup, GUILayout.MaxWidth(50)))
        {
            
        }
    }

    // Shows popup that includes more options to use
    void DrawMoreButton()
    {
        if (GUILayout.Button("More", EditorStyles.toolbarDropDown, GUILayout.MaxWidth(50)))
        {
            
        }
    }

    // Draws search field for finding specific PlayerPrefs
    void DrawSearchField()
    {
        searchText = GUILayout.TextField(searchText, 25, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.MinWidth(150)); // It's name written wrong by team in Unity
        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
        {
            // Remove focus if cleared
            searchText = "";
            GUI.FocusControl(null);
        }
    }

    // Draws button to refresh all PlayerPrefs data
    void DrawRefreshButton()
    {
        if(GUILayout.Button(new GUIContent(refreshIcon, "Refresh all PlayerPrefs data"), EditorStyles.toolbarButton, GUILayout.MaxWidth(30)))
        {

        }
    }

    // Draws area to add new PlayerPrefs' keys and values
    void DrawAddValueArea()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Add new PlayerPrefs", EditorStyles.boldLabel);

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Key", GUILayout.MaxWidth(100));
        newKey = GUILayout.TextField(newKey);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Value", GUILayout.MaxWidth(100));
        newValue = GUILayout.TextField(newValue);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Type", GUILayout.MaxWidth(100));
        playerPrefsTypes = (TYPES)EditorGUILayout.EnumPopup(playerPrefsTypes);
        if (GUILayout.Button(new GUIContent(plusIcon, "Add new PlayerPrefs data"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(40)))
        {
            AddNewPlayerPrefsData();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    // Add new PlayerPrefs and data comes from key, value and type fields on window
    void AddNewPlayerPrefsData()
    {

    }
}
