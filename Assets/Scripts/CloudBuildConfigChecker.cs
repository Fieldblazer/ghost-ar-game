using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CloudBuildConfigChecker : EditorWindow
{
    [MenuItem("Tools/Check Unity Cloud Build Config")]
    public static void ShowWindow()
    {
        GetWindow<CloudBuildConfigChecker>("Cloud Build Checker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Unity Cloud Build Configuration Checker", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // iOS Player Settings Check
        GUILayout.Label("iOS Player Settings:", EditorStyles.boldLabel);
        
        string companyName = PlayerSettings.companyName;
        string productName = PlayerSettings.productName;
        string bundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
        string version = PlayerSettings.bundleVersion;
        
        EditorGUILayout.LabelField("Company Name:", companyName);
        if (string.IsNullOrEmpty(companyName))
        {
            EditorGUILayout.HelpBox("⚠️ Company Name is required!", MessageType.Warning);
        }
        
        EditorGUILayout.LabelField("Product Name:", productName);
        EditorGUILayout.LabelField("Bundle Identifier:", bundleId);
        if (string.IsNullOrEmpty(bundleId) || bundleId.Contains("DefaultCompany"))
        {
            EditorGUILayout.HelpBox("⚠️ Bundle Identifier needs to be set to your Apple Developer App ID!", MessageType.Error);
        }
        
        EditorGUILayout.LabelField("Version:", version);
        
        EditorGUILayout.Space();
        
        // Build Settings Check  
        GUILayout.Label("Build Settings:", EditorStyles.boldLabel);
        
        bool iOSSupported = false;
        BuildTarget currentTarget = EditorUserBuildSettings.activeBuildTarget;
        EditorGUILayout.LabelField("Current Build Target:", currentTarget.ToString());
        
        if (currentTarget == BuildTarget.iOS)
        {
            EditorGUILayout.HelpBox("✅ iOS build target is active", MessageType.Info);
            iOSSupported = true;
        }
        else
        {
            EditorGUILayout.HelpBox("⚠️ Switch to iOS build target for accurate settings", MessageType.Warning);
        }
        
        EditorGUILayout.Space();
        
        // XR Settings Check
        GUILayout.Label("AR/XR Configuration:", EditorStyles.boldLabel);
        
        var xrSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.iOS);
        if (xrSettings != null && xrSettings.Manager != null)
        {
            var loaders = xrSettings.Manager.activeLoaders;
            if (loaders.Count > 0)
            {
                EditorGUILayout.HelpBox("✅ XR providers configured", MessageType.Info);
                foreach (var loader in loaders)
                {
                    EditorGUILayout.LabelField("- " + loader.GetType().Name);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("⚠️ No XR providers active. AR may not work!", MessageType.Warning);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("⚠️ XR Management not configured", MessageType.Warning);
        }
        
        EditorGUILayout.Space();
        
        // Scene Settings Check
        GUILayout.Label("Scene Configuration:", EditorStyles.boldLabel);
        
        var scenes = EditorBuildSettings.scenes;
        if (scenes.Length > 0)
        {
            EditorGUILayout.HelpBox($"✅ {scenes.Length} scene(s) in build", MessageType.Info);
            foreach (var scene in scenes)
            {
                if (scene.enabled)
                {
                    EditorGUILayout.LabelField("✅ " + scene.path);
                }
                else
                {
                    EditorGUILayout.LabelField("❌ " + scene.path + " (disabled)");
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("⚠️ No scenes added to build settings!", MessageType.Error);
        }
        
        EditorGUILayout.Space();
        
        // Action Buttons
        if (GUILayout.Button("Open Player Settings"))
        {
            SettingsService.OpenProjectSettings("Project/Player");
        }
        
        if (GUILayout.Button("Open Build Settings"))
        {
            EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }
        
        if (GUILayout.Button("Switch to iOS Build Target"))
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        }
    }
}