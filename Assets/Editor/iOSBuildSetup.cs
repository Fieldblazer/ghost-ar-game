using UnityEngine;
using UnityEditor;

public class iOSBuildSetup
{
    [MenuItem("Build/Setup iOS for Cloud Build")]
    public static void SetupForiOS()
    {
        // Switch to iOS platform
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        
        // Set Bundle Identifier
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.yourcompany.ghosthideseek");
        
        // Set Version
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.iOS.buildNumber = "1";
        
        // iOS Settings
        PlayerSettings.iOS.targetOSVersionString = "13.0";
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1); // ARM64
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_Standard_2_0);
        
        // AR Camera Permissions
        PlayerSettings.iOS.cameraUsageDescription = "This app uses AR camera to detect and interact with virtual ghosts in your environment.";
        
        // Optimize for Cloud Build
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        
        // Clear signing (Cloud Build handles this)
        PlayerSettings.iOS.appleDeveloperTeamID = "";
        PlayerSettings.iOS.appleEnableAutomaticSigning = false;
        
        Debug.Log("✅ iOS Build Settings Configured for Unity Cloud Build!");
        Debug.Log("Bundle ID: com.yourcompany.ghosthideseek");
        Debug.Log("Version: 1.0.0");
        Debug.Log("Ready for Cloud Build!");
        
        // Save project
        AssetDatabase.SaveAssets();
        
        EditorUtility.DisplayDialog("iOS Setup Complete", 
            "Project configured for Unity Cloud Build!\n\n" +
            "Bundle ID: com.yourcompany.ghosthideseek\n" +
            "Version: 1.0.0\n\n" +
            "Ready to upload to Unity Cloud Build!", "OK");
    }
}