using System.IO;
using UnityEditor;
using UnityEngine;

namespace GreedyGame.Editor
{
    [InitializeOnLoad]
    [CustomEditor(typeof(SDKXAdsSettings))]
    public class SDKXAdsSettingsEditor : UnityEditor.Editor
    {
        private const string SDKXAdsSettingsDir = "Assets/SDKX";

        private const string SDKXAdsSettingsResDir = "Assets/SDKX/Resources";

        private const string SDKXAdsSettingsFile =
            "Assets/SDKX/Resources/SDKXSettings.asset";

        private const string SDKXAdsSettingsFile2 =
           "Assets/SDKX/SDKXSettings.asset";

        [MenuItem("SDKX/Settings...")]
        public static void OpenInspector()
        {

            if (!AssetDatabase.IsValidFolder(SDKXAdsSettingsResDir))
            {
                AssetDatabase.CreateFolder(SDKXAdsSettingsDir, "Resources");
            }

            if (SDKXAdsSettings.Instance == null)
            {
                SDKXAdsSettings instance = CreateInstance<SDKXAdsSettings>();
                AssetDatabase.CreateAsset(instance, SDKXAdsSettingsFile);
                //SDKXAdsSettings.Instance = instance;
            }

            SDKXAdsSettings.Instance = (SDKXAdsSettings)AssetDatabase.LoadAssetAtPath(
            SDKXAdsSettingsFile, typeof(SDKXAdsSettings));

            Selection.activeObject = SDKXAdsSettings.Instance;

        }

        public override void OnInspectorGUI()
        {
            if (SDKXAdsSettings.Instance ==null)
            {
                SDKXAdsSettings.Instance = (SDKXAdsSettings)AssetDatabase.LoadAssetAtPath(
                SDKXAdsSettingsFile, typeof(SDKXAdsSettings));

            }
            //EditorGUILayout.LabelField("SDKX App ID");

            SDKXAdsSettings.Instance.AppId =
                    EditorGUILayout.TextField("SDKX App ID",
                            SDKXAdsSettings.Instance.AppId);

            EditorGUILayout.LabelField("Google Admob Plugin ", EditorStyles.boldLabel);
            SDKXAdsSettings.Instance.IsAdMobPlugin =
                    EditorGUILayout.Toggle(new GUIContent("Is Imported"),
                            SDKXAdsSettings.Instance.IsAdMobPlugin);

            EditorGUILayout.Separator();

            EditorGUI.BeginDisabledGroup(SDKXAdsSettings.Instance.IsAdMobPlugin);

                    EditorGUILayout.LabelField("Google Ad Manager", EditorStyles.boldLabel);
                    SDKXAdsSettings.Instance.IsAdManagerEnabled =
                            EditorGUILayout.Toggle(new GUIContent("Enabled"),
                                    SDKXAdsSettings.Instance.IsAdManagerEnabled);

            
                    //EditorGUILayout.HelpBox(
                    //    "If Google admob plugin then enable this.",
                    //    MessageType.Info);

                    EditorGUILayout.Separator();

                    EditorGUILayout.LabelField("Google AdMob", EditorStyles.boldLabel);
                    SDKXAdsSettings.Instance.IsAdMobEnabled =
                            EditorGUILayout.Toggle(new GUIContent("Enabled"),
                                    SDKXAdsSettings.Instance.IsAdMobEnabled);

                    EditorGUILayout.Separator();

                    EditorGUI.BeginDisabledGroup(!SDKXAdsSettings.Instance.IsAdMobEnabled);

                    EditorGUILayout.LabelField("AdMob App ID");

                    SDKXAdsSettings.Instance.AdMobAndroidAppId =
                            EditorGUILayout.TextField("Android",
                                    SDKXAdsSettings.Instance.AdMobAndroidAppId);


                    if (SDKXAdsSettings.Instance.IsAdMobEnabled)
                    {
                        EditorGUILayout.HelpBox(
                                "AdMob App ID will look similar to this sample ID: ca-app-pub-3940256099942544~3347511713",
                                MessageType.Info);
                    }

                    EditorGUILayout.Separator();

                    EditorGUI.EndDisabledGroup();

            EditorGUI.EndDisabledGroup();

            if (GUI.changed)
            {
                OnSettingsChanged();
            }
        }

        private void OnSettingsChanged()
        {
            EditorUtility.SetDirty((SDKXAdsSettings)target);
            WriteSettingsToFile();
        }

        internal void WriteSettingsToFile()
        {
            AssetDatabase.SaveAssets();
        }
    }
}
