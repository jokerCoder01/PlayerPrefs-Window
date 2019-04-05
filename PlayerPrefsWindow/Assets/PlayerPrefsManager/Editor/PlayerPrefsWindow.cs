//// TODOs
////
//// Sort Button Functionality
//// Import Functionality
//// Export Functionality
//// Searchfield Functionality
//// Save Current Functionality

//// BUGFIX
////
//// Fix reading of float PlayerPrefs
//// Fix non-editable types because of that doesn't have references
//// Fix Delete All button that causes errors

using Microsoft.Win32;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsWindow : EditorWindow
{

    Texture refreshIcon;
    Texture plusIcon;
    Texture saveIcon;
    Texture resetIcon;
    Texture deleteIcon;
    enum TYPES { INTEGER, FLOAT, STRING }
    TYPES playerPrefsTypes;
    string searchText;
    string newKey;
    string newValue;

    string[] keyList;
    string[] valueList;
    int[] typeList;
    Vector2 scrollView;
    RegistryKey registryKey;
    string companyName;
    string productName;

    // Create button as a MenuItem to call the ShowWindow method
    [MenuItem("JokerCoder/PlayerPrefs Manager")]
    public static void ShowWindow()
    {
        PlayerPrefsWindow window = (PlayerPrefsWindow)GetWindow(typeof(PlayerPrefsWindow));
        window.titleContent = new GUIContent("PlayerPrefs Manager");
        window.Show();
    }

    // Set variables at the beginning of window
    void OnEnable()
    {
        searchText = "";
        newKey = "";
        newValue = "";
        companyName = PlayerSettings.companyName;
        productName = PlayerSettings.productName;
        registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Unity\UnityEditor\" + companyName + "\\" + productName);
        refreshIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/PlayerPrefsManager/Icons/refresh_Icon.png", typeof(Texture));
        plusIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/PlayerPrefsManager/Icons/plus_Icon.png", typeof(Texture));
        saveIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/PlayerPrefsManager/Icons/save_Icon.png", typeof(Texture));
        resetIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/PlayerPrefsManager/Icons/reset_Icon.png", typeof(Texture));
        deleteIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/PlayerPrefsManager/Icons/delete_Icon.png", typeof(Texture));

        GetAllPlayerPrefs();

    }

    // Called for rendering and handling GUI events
    void OnGUI()
    {
        GUILayout.BeginVertical();

        DrawToolbarGUI();
        DrawAddValueArea();
        DrawPlayerPrefs();

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
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete All"), false, DeleteAll);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Import"), false, Import);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Export"), false, Export);

            menu.ShowAsContext();
        }
    }

    // Draws search field for finding specific PlayerPrefs
    void DrawSearchField()
    {
        searchText = GUILayout.TextField(searchText, 25, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.MinWidth(150)); // It's name written wrong by team in Unity
        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton"))) // It's name written wrong by team in Unity
        {
            // Remove focus if cleared
            searchText = "";
            GUI.FocusControl(null);
        }
    }

    // Draws button to refresh all PlayerPrefs data
    void DrawRefreshButton()
    {
        if (GUILayout.Button(new GUIContent(refreshIcon, "Refresh all PlayerPrefs data"), EditorStyles.toolbarButton, GUILayout.MaxWidth(30)))
        {
            GetAllPlayerPrefs();
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
        GUILayout.Label("Type", GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
        playerPrefsTypes = (TYPES)EditorGUILayout.EnumPopup(playerPrefsTypes);
        if (GUILayout.Button(new GUIContent(plusIcon, "Add new PlayerPrefs data"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(40)))
        {
            AddNewPlayerPrefsData();
            GetAllPlayerPrefs();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    // Add new PlayerPrefs and data comes from key, value and type fields on window
    void AddNewPlayerPrefsData()
    {
        switch (playerPrefsTypes)
        {
            case TYPES.INTEGER:
                PlayerPrefs.SetInt(newKey, int.Parse(newValue));
                break;
            case TYPES.FLOAT:
                if (newValue.Contains("."))
                    newValue = newValue.Replace('.', ',');
                PlayerPrefs.SetFloat(newKey, float.Parse(newValue));
                break;
            case TYPES.STRING:
                PlayerPrefs.SetString(newKey, newValue);
                break;
        }
        newKey = "";
        newValue = "";
    }

    // Gets all PlayerPrefs data that includes keys, values and types and adds them to arrays 
    void GetAllPlayerPrefs()
    {
        keyList = new string[registryKey.ValueCount];
        valueList = new string[registryKey.ValueCount];
        typeList = new int[registryKey.ValueCount];
        int counter = 0;
        foreach (string key in registryKey.GetValueNames())
        {
            string _key = key.Remove(key.LastIndexOf('_'));
            keyList[counter] = _key;

            if (registryKey.GetValueKind(key) == RegistryValueKind.Binary)
                valueList[counter] = System.Text.Encoding.UTF8.GetString((byte[])registryKey.GetValue(key));
            else
                valueList[counter] = registryKey.GetValue(key).ToString();
            
            counter++;
        }
    }

    // Draw Scrollable view for PlayerPrefs list and PlayerPrefs rows that gets data from registryKey
    void DrawPlayerPrefs()
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Key", EditorStyles.boldLabel, GUILayout.MinWidth(100), GUILayout.MaxWidth(200));
        GUILayout.Label("Value", EditorStyles.boldLabel, GUILayout.MinWidth(100), GUILayout.MaxWidth(200));
        GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.MinWidth(100), GUILayout.MaxWidth(200));
        GUILayout.Label("", EditorStyles.boldLabel, GUILayout.MinWidth(75), GUILayout.MaxWidth(75));
        GUILayout.EndHorizontal();
        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        for (int i = 0; i < keyList.Length; i++)
        {
            GUILayout.BeginHorizontal();

            keyList[i] = GUILayout.TextField(keyList[i], GUILayout.MinWidth(100), GUILayout.MaxWidth(200));
            valueList[i] = GUILayout.TextField(valueList[i], GUILayout.MinWidth(100), GUILayout.MaxWidth(200));
            TYPES type = TYPES.INTEGER;
            type = (TYPES)EditorGUILayout.EnumPopup(type, GUILayout.MinWidth(100), GUILayout.MaxWidth(200));
            if (GUILayout.Button(new GUIContent(saveIcon, "Save current data"), EditorStyles.miniButton, GUILayout.MaxWidth(25), GUILayout.MaxHeight(25)))
            {

            }
            if (GUILayout.Button(new GUIContent(resetIcon, "Reset data to default"), EditorStyles.miniButton, GUILayout.MaxWidth(25), GUILayout.MaxHeight(25)))
            {
                GetAllPlayerPrefs();
            }
            if (GUILayout.Button(new GUIContent(deleteIcon, "Delete PlayerPrefs data"), EditorStyles.miniButton, GUILayout.MaxWidth(25), GUILayout.MaxHeight(25)))
            {
                PlayerPrefs.DeleteKey(keyList[i]);
                GetAllPlayerPrefs();
            }

            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        GUILayout.EndVertical();
    }

    // Call this function when Delete All button clicked
    void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    // Call this function when Import button clicked
    void Import()
    {
        Debug.Log("Import PlayerPrefs");
    }

    // Call this function when Export button clicked
    void Export()
    {
        Debug.Log("Export PlayerPrefs");
    }
}
