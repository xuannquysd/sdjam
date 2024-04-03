using UnityEditor;

namespace QuickScript
{
    public static class QuickScriptEditor
    {
        #region Unity Script
        [MenuItem("Assets/Create Script/Unity/Component", priority = 0)]
        public static void CreateComponent()
        {
            string templatePath = "Assets/Quick Scripts/Templates/Unity/Component Template.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewComponent.cs");
        }

        [MenuItem("Assets/Create Script/Unity/Scriptable Object", priority = 1)]
        public static void CreateScriptableObject()
        {
            string templatePath = "Assets/Quick Scripts/Templates/Unity/Scriptable Object Template.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScriptableObject.cs");
        }
        
        [MenuItem("Assets/Create Script/Unity/Editor", priority = 2)]
        public static void CreateEditor()
        {
            string templatePath = "Assets/Quick Scripts/Templates/Unity/Editor Template.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEditor.cs");
        }
        
        [MenuItem("Assets/Create Script/Unity/Editor Window", priority = 3)]
        public static void CreateEditorWindow()
        {
            string templatePath = "Assets/Quick Scripts/Templates/Unity/Editor Window Template.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEditorWindow.cs");
        }
        #endregion

        #region C# Core
        [MenuItem("Assets/Create Script/C# Core/Class", priority = 4)]
        public static void CreateClass()
        {
            string templatePath = "Assets/Quick Scripts/Templates/C# Core/Class Template.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewClass.cs");
        }
        
        [MenuItem("Assets/Create Script/C# Core/Abstract Class", priority = 5)]
        public static void CreateAbstractClass()
        {
            string templatePath = "Assets/Quick Scripts/Templates/C# Core/Abstract Class Template.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewAbstractClass.cs");
        }
        
        [MenuItem("Assets/Create Script/C# Core/Interface", priority = 6)]
        public static void CreateInterface()
        {
            string templatePath = "Assets/Quick Scripts/Templates/C# Core/Interface Template.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewInterface.cs");
        }
        #endregion
    }
}