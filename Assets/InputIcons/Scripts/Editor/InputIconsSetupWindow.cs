using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;

namespace InputIcons
{
    public class InputIconsSetupWindow : EditorWindow
    {
        Vector2 scrollPos;

        bool showPart1 = false;
        bool showPart2 = false;
        bool showPart3 = false;

        bool showAdvanced = false;
        GUIStyle textStyleHeader;
        GUIStyle textStyle;
        GUIStyle textStyleYellow;
        GUIStyle textStyleBold;
        GUIStyle buttonStyle;

        private Editor editor;


        private List<InputIconSetBasicSO> iconSetSOs;
        //public List<InputActionAsset> usedInputActionAssets;

        public static SerializedObject serializedManager;
        public static SerializedProperty serializedInputActionAssets;

        public GameObject activationPrefab;

        [MenuItem("Tools/Input Icons Setup", priority = 0)]
        public static void ShowWindow()
        {
            const int width = 800;
            const int height = 500;

            var x = (Screen.currentResolution.width - width) / 2;
            var y = (Screen.currentResolution.height - height) / 2;

            GetWindow<InputIconsSetupWindow>("Input Icons Setup").iconSetSOs = InputIconsSpritePacker.GetInputIconSetScriptableObjects();
            EditorWindow window = GetWindow<InputIconsSetupWindow>("Input Icons Setup");
            window.position = new Rect(x, y, width, height);
        }

        protected void OnEnable()
        {
            // load values
            var data = EditorPrefs.GetString("InputIconsSetupWindow", JsonUtility.ToJson(this, false));
            JsonUtility.FromJsonOverwrite(data, this);

            position.Set(position.x, position.y, 800, 500);

            serializedManager = new SerializedObject(InputIconsManagerSO.Instance);
            serializedInputActionAssets = serializedManager.FindProperty("usedActionAssets");

            activationPrefab = Resources.Load("InputIcons/II_InputIconsActivator") as GameObject;

            //do this so the list does not appear null ... weird bug that otherwise can happen when package just got imported
            serializedInputActionAssets.InsertArrayElementAtIndex(0);
            var elementProperty = serializedInputActionAssets.GetArrayElementAtIndex(0);
            if(elementProperty!=null)
                elementProperty.objectReferenceValue = null;
            serializedInputActionAssets.DeleteArrayElementAtIndex(0);

        }

        protected void OnDisable()
        {
            // save values
            var data = JsonUtility.ToJson(this, false);
            EditorPrefs.SetString("InputIconsSetupWindow", data);
        }

        private bool AllInputActionAssetsNull()
        {
            if (serializedInputActionAssets.arraySize == 0)
                return true;

            for(int i=0; i< serializedInputActionAssets.arraySize; i++)
            {
                if (serializedInputActionAssets.GetArrayElementAtIndex(i).objectReferenceValue as System.Object as InputActionAsset != null)
                    return false;
            }

            return true;

        }
        private void OnGUI()
        {

            textStyleHeader = new GUIStyle(EditorStyles.boldLabel);
            textStyleHeader.wordWrap = true;
            textStyleHeader.fontSize = 14;

            textStyle = new GUIStyle(EditorStyles.label);
            textStyle.wordWrap = true;

            textStyleYellow = new GUIStyle(EditorStyles.label);
            textStyleYellow.wordWrap = true;
            textStyleYellow.normal.textColor = Color.yellow;

            textStyleBold = new GUIStyle(EditorStyles.boldLabel);
            textStyleBold.wordWrap = true;

            buttonStyle = EditorStyles.miniButtonMid;

            scrollPos =
               EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true));

            InputIconsManagerSO.Instance = (InputIconsManagerSO)EditorGUILayout.ObjectField("", InputIconsManagerSO.Instance, typeof(InputIconsManagerSO), true);
            if (InputIconsManagerSO.Instance == null)
            {
                EditorGUILayout.HelpBox("Select the icon manager.", MessageType.Warning);
                EditorGUILayout.EndScrollView();
                return;
            }

            GUILayout.Space(20);


#if !ENABLE_INPUT_SYSTEM
            // New input system backends are enabled.
                        EditorGUILayout.HelpBox("Enable the new Input System in Project Settings for full functionality.\n" +
                "Project Settings -> Player -> Other Settings. Set Active Input Handling to 'Input System Package (new)' or 'Both'.", MessageType.Warning);
#endif

            InputIconsManagerSO.Instance.TEXTMESHPRO_SPRITEASSET_FOLDERPATH = EditorGUILayout.TextField("Default Sprite Asset folder: ", InputIconsManagerSO.Instance.TEXTMESHPRO_SPRITEASSET_FOLDERPATH);

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(serializedInputActionAssets);
            
            if(AllInputActionAssetsNull())
            {
                EditorGUILayout.HelpBox("Select an Input Asset before you continue.", MessageType.Warning);
                EditorGUILayout.EndScrollView();
                serializedManager.ApplyModifiedProperties();
                return;
            }

            GUILayout.Space(10);
            DrawUILine(Color.grey);
            DrawUILine(Color.grey);

            DrawControlSchemePart();

            GUILayout.Space(10);
            DrawUILine(Color.grey);
            DrawUILine(Color.grey);

            EditorGUILayout.BeginHorizontal();
            
            //Quick Setup
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width / 2));

            DrawQuickSetup();

            EditorGUILayout.EndVertical();

            
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUILayout.EndVertical();

            DrawUILineVertical(Color.grey);

            EditorGUILayout.BeginVertical(GUILayout.Width(10));
            EditorGUILayout.EndVertical();


            //manual setup
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Manual Setup", textStyleHeader);
            GUILayout.Label("To setup this tool manually, complete the following steps.\n" +
                "If this is the first time you setup this tool, complete all steps.\n" +
                "If the tool is already setup and you want to make updates to the sprites or the " +
                "TMPro default style sheet, do the parts you need.", textStyle);

            GUILayout.Space(10);

            DrawCustomPartPackSpriteAssets();

            GUILayout.Space(5);
            GUILayout.Space(5);

            DrawCustomPartStyleSheetUpdate();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Finally add the activation prefab to your first scene.", textStyleHeader);
            GUILayout.Label("This will ensure that the InputIconsManager will be active in our builds " +
                "and will update the displayed icons when needed.", textStyle);
            EditorGUI.BeginDisabledGroup(true);
            activationPrefab = (GameObject)EditorGUILayout.ObjectField("", activationPrefab, typeof(GameObject), true);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(20);
            DrawUILine(Color.grey);
            DrawUILine(Color.grey);
            GUILayout.Space(10);

            DrawBottomPart();

            GUILayout.Space(5);
            DrawUILine(Color.grey);
            DrawUILine(Color.grey);
            GUILayout.Space(5);

            DrawAdvanced();
            
            EditorGUILayout.EndScrollView();

            serializedManager.ApplyModifiedProperties();
        }

        private void DrawQuickSetup()
        {
            GUILayout.Label("Quick Setup", textStyleHeader);

            if(AllInputActionAssetsNull())
            {
                EditorGUILayout.HelpBox("Select an Input Asset before you continue.", MessageType.Warning);
            }
            else
            {
                GUILayout.Space(3);

                GUILayout.Label("To setup this tool with the standard functionality, use the buttons below. " +
                    "Wait for Unity to recompile after the first button press.", textStyle);

                if (GUILayout.Button("Step 1: Create Sprite Assets and prepare Style Sheet with empty values" +
                    "\n(then wait for compilation)"))
                {
                    InputIconsUtility.PackIconSets();
                    Debug.Log("Packing button icons completed");

                    Debug.Log("Preparing default TMP style sheet for additional entries ...");

                    InputIconsManagerSO.Instance.CreateInputStyleData();
                    int c = 0;
                    c += InputIconsUtility.PrepareAddingInputStyles(InputIconsManagerSO.Instance.inputStyleKeyboardDataList);
                    c += InputIconsUtility.PrepareAddingInputStyles(InputIconsManagerSO.Instance.inputStyleGamepadDataList);
             
                    Debug.Log("TMP style sheet prepared with "+c+" empty values.");
                    if(c==0)
                    {
                        Debug.LogWarning(c + " empty entries added which is generally not expected. Try the same step again.");
                    }

                    CompilationPipeline.RequestScriptCompilation();
                }

                if (!EditorApplication.isCompiling)
                {
                    if (GUILayout.Button("Step 2: Add Input Action names to Style Sheet"))
                    {
                        Debug.Log("Adding entries to default TMP style sheet ...");
                       
                        InputIconsManagerSO.Instance.CreateInputStyleData();
                        int c = 0;
                        c += InputIconsUtility.AddInputStyles(InputIconsManagerSO.Instance.inputStyleKeyboardDataList);
                        c += InputIconsUtility.AddInputStyles(InputIconsManagerSO.Instance.inputStyleGamepadDataList);

                        Debug.Log("TMP style sheet updated with ("+ c+ ") styles (multiple entries combined to only one)");
                        
                        InputIconsManagerSO.UpdateTMProStyleSheetWithUsedPlayerInputs();
                        TMP_InputStyleHack.RemoveEmptyEntriesInStyleSheet();
                        //TMP_InputStyleHack.RefreshAllTMProUGUIObjects();
                    }
                }
                else
                {

                    GUILayout.Label("... waiting for compilation ...", textStyleYellow);
                }
            }
        }

        private void DrawControlSchemePart()
        {
            GUILayout.Label("First things first: Control scheme names", textStyleHeader);
            GUILayout.Label("To avoid complications later on, let's set this up first.", textStyle);

            GUILayout.Label("Make sure the names of the " +
               "control schemes for keyboard and gamepad are equal to the names of the control schemes you have set in the " +
               "Input Action Asset(s).", textStyle);
            if (InputIconsManagerSO.Instance)
            {
                InputIconsManagerSO.Instance.controlSchemeName_Keyboard = EditorGUILayout.TextField("Keyboard Control Scheme Name", InputIconsManagerSO.Instance.controlSchemeName_Keyboard);
                InputIconsManagerSO.Instance.controlSchemeName_Gamepad = EditorGUILayout.TextField("Gamepad Control Scheme Name", InputIconsManagerSO.Instance.controlSchemeName_Gamepad);
            }
           
        }

        private void DrawCustomPartPackSpriteAssets()
        {
            //GUILayout.Label("Part 1: Packing Input Icon Sets to Sprite Assets", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Create Sprite Assets", textStyleBold);
            showPart1 = EditorGUILayout.Foldout(showPart1, "Choose which sprites to use for displaying input actions.");
            if (showPart1)
            {

                EditorGUILayout.BeginVertical(GUI.skin.window);
                EditorGUILayout.HelpBox("The sprites in the sets below " +
                              "will be used to create sprite assets.\n\n" +
                              "If you want to use different sprites, change them before you pack them.", MessageType.None);
                DrawIconSets();
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Pack sets to Sprite Assets", buttonStyle))
                {
                    Debug.Log("Packing sprites ...");
                    InputIconsUtility.PackIconSets();
                    Debug.Log("Packing sprites completed");
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawCustomPartStyleSheetUpdate()
        {
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Update TMPro Style Sheet", textStyleBold);
            showPart2 = EditorGUILayout.Foldout(showPart2, "Add or update input action asset in default style sheet");
            if (showPart2)
            {

                GUILayout.Label("Check the 'Used Input Action Assets' list above if it contains all desired Input Action Assets.", textStyle);

                if(AllInputActionAssetsNull())
                {
                    { EditorGUILayout.HelpBox("Select an Input Asset before you continue.", MessageType.Warning); }
                }
                else
                {
                    if (serializedInputActionAssets.GetArrayElementAtIndex(0) != null)
                    {
                        GUILayout.Space(10);

                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        GUILayout.Label("First prepare the default style sheet.", textStyle);

                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        EditorGUILayout.BeginVertical();
                        if (GUILayout.Button("Prepare style sheet manually\n(faster, but needs you to update style sheet)"))
                        {
                            Debug.Log("Preparing default TMP style sheet for additional entries ...");
                            InputIconsManagerSO.Instance.CreateInputStyleData();
                            int c = 0;
                            c += InputIconsUtility.PrepareAddingInputStyles(InputIconsManagerSO.Instance.inputStyleKeyboardDataList);
                            c += InputIconsUtility.PrepareAddingInputStyles(InputIconsManagerSO.Instance.inputStyleGamepadDataList);

                            Debug.Log("TMP style sheet prepared with " + c + " empty values.");
                            if (c == 0)
                            {
                                Debug.LogWarning(c + " empty entries added which is generally not expected. Try the same step again.");
                            }

                        }
                        GUILayout.Label("IMPORTANT: UPDATE THE STYLE SHEET.", textStyleBold);
                        GUILayout.Label("The default style sheet should now be open in the inspector. " +
                            "Make a small change in any field of the style sheet and undo it again. " +
                            "Then continue with the next step.", textStyle);

                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        GUILayout.Label("or", textStyle);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical();
                        if (GUILayout.Button("Prepare style sheet automatically\n(requires compilation)"))
                        {
                            Debug.Log("Preparing default TMP style sheet for additional entries ...");
                            InputIconsManagerSO.Instance.CreateInputStyleData();
                            InputIconsUtility.PrepareAddingInputStyles(InputIconsManagerSO.Instance.inputStyleKeyboardDataList);
                            InputIconsUtility.PrepareAddingInputStyles(InputIconsManagerSO.Instance.inputStyleGamepadDataList);
                          
                            Debug.Log("TMP style sheet prepared.");

                            CompilationPipeline.RequestScriptCompilation();
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(5);

                        DrawUILine(Color.grey);
                        GUILayout.Space(5);

                        GUILayout.Label("Now we can add/update the style sheet with our Input Action Assets.", textStyle);

                        if (GUILayout.Button("Add Input Asset styles to default TMP style sheet"))
                        {
                            Debug.Log("Adding entries default TMP style sheet for additional entries ...");
                            InputIconsManagerSO.Instance.CreateInputStyleData();
                            int c = 0;
                            c += InputIconsUtility.AddInputStyles(InputIconsManagerSO.Instance.inputStyleKeyboardDataList);
                            c += InputIconsUtility.AddInputStyles(InputIconsManagerSO.Instance.inputStyleGamepadDataList);

                            Debug.Log("TMP style sheet updated with (" + c + ") styles (multiple entries combined to only one)");

                            TMP_InputStyleHack.RemoveEmptyEntriesInStyleSheet();
                        }

                        GUILayout.Label("Have a look at the style sheet. You should find entries with the name of your input actions. " +
                            "The opening and closing tags might be empty but they will get filled once you start the game.", textStyle);
                        EditorGUILayout.EndVertical();
                    }
                }
               


            }
            EditorGUILayout.EndVertical();
        }

        private void DrawBottomPart()
        {

            GUILayout.Label("How to use Input Icons in TMPro text fields", textStyleHeader);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            showPart3 = EditorGUILayout.Foldout(showPart3, "Using Input Icons");
            if (showPart3)
            {

                GUILayout.Label("Once you have completed the setup you are ready to use Input Icons.", textStyle);

                GUILayout.Space(8);
                GUILayout.Label("Displaying Input Icons", textStyleBold);
                GUILayout.Label("To display Input Icons in TMPro texts, we can use the \'style\' tag.\n" +
                    "We can write <style=NameOfActionMap/NameOfAction> to display the input bindings of the action.\n" +
                    "For example we can write <style=platformer controls/move> to display the bindings of the move action " +
                    "in the platformer controls action map.\n" +
                    "If you have a \'Jump\' action, type <style=platformer controls/jump> respectively.\n" +
                    "To display a single action of a composite binding, type <style=platformer controls/move/down> for example.\n" +
                    "\n" +
                    "All available bindings are saved in the Input Icons Manager for quick access. Open the\n" +
                    "Input Icons Manager and scroll down to the lists. Copy and paste an entry of the\n" +
                    "TMPro Style Tag column into a text field to display the corresponding binding." +
                    "", textStyle);

                GUILayout.Space(8);
                GUILayout.Label("Customization", textStyleBold);
                InputIconsManagerSO.Instance = (InputIconsManagerSO)EditorGUILayout.ObjectField("", InputIconsManagerSO.Instance, typeof(InputIconsManagerSO), true);
                GUILayout.Label("The InputIconsManager provides displaying options for Input Icons and more.", textStyle);



            }
            EditorGUILayout.EndVertical();

        }

        private void DrawAdvanced()
        {
            GUILayout.Label("Advanced", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            showAdvanced = EditorGUILayout.Foldout(showAdvanced, "TMP Style Sheet manipulation");
            if (showAdvanced)
            {
                EditorGUILayout.HelpBox("You can use this button to remove Input Icons style" +
                    " entries from the TMPro style sheet.", MessageType.Warning);

                var style = new GUIStyle(GUI.skin.button);

                if (GUILayout.Button("Remove all Input Icon styles from the TMPro style sheet.", style))
                {
                    InputIconsUtility.RemoveAllStyleSheetEntries();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawIconSets()
        {

            EditorGUI.BeginDisabledGroup(true);
            for (int i = 0; i < iconSetSOs.Count; i++)
            {
                if(iconSetSOs[i]!=null)
                    iconSetSOs[i] = (InputIconSetBasicSO)EditorGUILayout.ObjectField(iconSetSOs[i].deviceDisplayName, iconSetSOs[i], typeof(InputIconSetBasicSO), true);
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 5)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            //r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        public static void DrawUILineVertical(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(padding+thickness), GUILayout.ExpandHeight(true));
            r.width = thickness;
            r.x += padding / 2;
            r.y -= 2;
            r.height += 3;
            EditorGUI.DrawRect(r, color);
        }

    }
}