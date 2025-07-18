using UnityEngine;
using UnityEditor;

public class CloudBuildProjectSetup : EditorWindow
{
    [MenuItem("Tools/Setup Project for Cloud Build")]
    public static void ShowWindow()
    {
        GetWindow<CloudBuildProjectSetup>("Cloud Build Setup");
    }

    private string bundleIdentifier = "com.yourcompany.ghosthideseek";
    private string version = "1.0";
    private string cameraUsageDescription = "This app uses AR camera to detect and interact with virtual ghosts in your environment.";

    private void OnGUI()
    {
        GUILayout.Label("Unity Cloud Build Project Setup", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        GUILayout.Label("iOS Settings Configuration:", EditorStyles.boldLabel);
        
        // Bundle Identifier
        EditorGUILayout.LabelField("Current Bundle ID:", PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS));
        bundleIdentifier = EditorGUILayout.TextField("New Bundle ID:", bundleIdentifier);
        
        // Version
        EditorGUILayout.LabelField("Current Version:", PlayerSettings.bundleVersion);
        version = EditorGUILayout.TextField("Version:", version);
        
        // Camera Usage Description
        EditorGUILayout.LabelField("Camera Usage Description:");
        cameraUsageDescription = EditorGUILayout.TextArea(cameraUsageDescription, GUILayout.Height(60));
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Configure iOS Settings for Cloud Build"))
        {
            ConfigureiOSSettings();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Switch to iOS Build Target"))
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        }
        
        if (GUILayout.Button("Open Build Settings"))
        {
            EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }
        
        EditorGUILayout.Space();
        GUILayout.Label("Current Settings Status:", EditorStyles.boldLabel);
        
        // Display current settings
        EditorGUILayout.LabelField("Build Target:", EditorUserBuildSettings.activeBuildTarget.ToString());
        EditorGUILayout.LabelField("Company Name:", PlayerSettings.companyName);
        EditorGUILayout.LabelField("Product Name:", PlayerSettings.productName);
        
        // Check critical settings
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            EditorGUILayout.HelpBox("✅ iOS build target active", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("⚠️ Switch to iOS build target", MessageType.Warning);
        }
        
        if (!string.IsNullOrEmpty(PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS)) && 
            !PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS).Contains("DefaultCompany"))
        {
            EditorGUILayout.HelpBox("✅ Bundle ID configured", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("⚠️ Bundle ID needs configuration", MessageType.Warning);
        }
    }

    private void ConfigureiOSSettings()
    {
        // Set bundle identifier
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleIdentifier);
        
        // Set version
        PlayerSettings.bundleVersion = version;
        
        // Set iOS specific settings
        PlayerSettings.iOS.targetOSVersionString = "13.0";
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1); // ARM64
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_Standard_2_1);
        
        // Set camera usage description
        PlayerSettings.iOS.cameraUsageDescription = cameraUsageDescription;
        
        // Optimize settings for Cloud Build
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        
        // Clear signing settings (Cloud Build handles this)
        PlayerSettings.iOS.appleDeveloperTeamID = "";
        PlayerSettings.iOS.appleEnableAutomaticSigning = false;
        
        Debug.Log("✅ iOS settings configured for Unity Cloud Build!");
        Debug.Log($"Bundle ID: {bundleIdentifier}");
        Debug.Log($"Version: {version}");
        Debug.Log("Ready for Cloud Build upload!");
        
        EditorUtility.DisplayDialog("Success", 
            "iOS settings configured for Unity Cloud Build!\n\n" +
            $"Bundle ID: {bundleIdentifier}\n" +
            $"Version: {version}\n\n" +
            "Your project is ready for Cloud Build.", "OK");
    }
}