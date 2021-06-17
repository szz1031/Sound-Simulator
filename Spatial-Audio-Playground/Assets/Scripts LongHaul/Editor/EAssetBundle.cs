using System.Collections;
using UnityEditor;
using UnityEngine;
public class CreateAssetBundles
{
    [MenuItem("AssetBundle/Build AssetBundles For Android(Must Build Before APK Packing)")]
    static void BuildAllAssetBundlesAndroid()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}