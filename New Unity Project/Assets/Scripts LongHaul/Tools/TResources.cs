using GameSetting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TResources
{
    public class ConstPath
    {
        public const string S_LevelMain = "Level/Main";
        public const string S_LevelData = "Level/Main/Data";
        public const string S_LeveLItem = "Level/Item";
        public const string S_LevelTexture = "Level/Texture";
        public const string S_Entity = "Entity";
        public const string S_StyleCustomization = "Level/Customization";

        public const string S_SFX = "SFX";
    }

    public static TileMapData GetLevelData(string name) => Load<TileMapData>(ConstPath.S_LevelData + "/" + name);
    public static StyleColorData[] GetAllStyleCustomization(enum_Style levelStype) => LoadAll<StyleColorData>(ConstPath.S_StyleCustomization + "/" + levelStype);
    public static LevelItemBase[] GetAllLevelItems(enum_Style _levelStyle, Transform parent)
    {
        LevelItemBase[] levelItemPrefabs = LoadAll<LevelItemBase>(ConstPath.S_LeveLItem + "/" + _levelStyle);
        foreach (LevelItemBase levelItem in levelItemPrefabs)
        {
            if (levelItem.m_ItemType == enum_LevelItemType.Invalid)
                Debug.LogError("Please Edit Level Item(Something invalid): Resources/" + ConstPath.S_LeveLItem + _levelStyle + "/" + levelItem.name);
        }
        LevelItemBase[] instantiatedLevelItem = new LevelItemBase[levelItemPrefabs.Length];
        for (int i = 0; i < levelItemPrefabs.Length; i++)
            instantiatedLevelItem[i] = GameObject.Instantiate(levelItemPrefabs[i], parent);
        return instantiatedLevelItem;
    }
    public static Dictionary<enum_TilePrefabDefinition, List<LevelBase>> GetAllStyledLevels(enum_Style levelStyle)
    {
        Dictionary<enum_TilePrefabDefinition, List<LevelBase>> levelPrefabDic = new Dictionary<enum_TilePrefabDefinition, List<LevelBase>>();
        LevelBase[] levels = LoadAll<LevelBase>(ConstPath.S_LevelMain);
        Renderer matRenderer = levels[0].GetComponentInChildren<Renderer>();
        matRenderer.sharedMaterial.SetTexture("_MainTex", Load<Texture>(ConstPath.S_LevelTexture + "/" + levelStyle));
        levels.Traversal((LevelBase level) => {
            if (level.E_PrefabType == enum_TilePrefabDefinition.Invalid)
                Debug.LogError("Please Edit Level(Something Invalid):Resources/" + ConstPath.S_LevelMain + level.name);

            if (!levelPrefabDic.ContainsKey(level.E_PrefabType))
                levelPrefabDic.Add(level.E_PrefabType, new List<LevelBase>());
            levelPrefabDic[level.E_PrefabType].Add(level);
        });
        return levelPrefabDic;
    }

    public static Dictionary<int, EntityBase> GetAllStyledEntities(enum_Style entityStyle)
    {
        Dictionary<int, EntityBase> entitisDic = new Dictionary<int, EntityBase>();
        entitisDic.Add(0,Instantiate<EntityBase>(ConstPath.S_Entity +"/Player"));
        EntityBase[] entities = LoadAll<EntityBase>(ConstPath.S_Entity + "/" + entityStyle.ToString());
        entities.Traversal((EntityBase entity) => {
            int index = int.Parse(entity.name.Split('_')[0]);
            entitisDic.Add(index, GameObject.Instantiate<EntityBase>(entity));
        });
        return entitisDic;
    }
    public static Dictionary<int, SFXBase> GetAllGameSFXs()
    {
        Dictionary<int, SFXBase> sfxsDic = new Dictionary<int, SFXBase>();
        LoadAll<SFXBase>(ConstPath.S_SFX).Traversal((SFXBase sfx) => {
            sfxsDic.Add(int.Parse(sfx.name.Split('_')[0]),GameObject.Instantiate<SFXBase>(sfx));
        });
        return sfxsDic;
    }
    #region Will Be Replaced By AssetBundle If Needed
    public static T Instantiate<T>(string path, Transform toParent = null) where T : UnityEngine.Object
    {
        GameObject obj = Resources.Load<GameObject>(path);
        if (obj == null)
            throw new Exception("Null Path Of :Resources/" + path.ToString());
        return UnityEngine.Object.Instantiate(obj, toParent).GetComponent<T>();
    }
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        T prefab = Resources.Load<T>(path);
        if (prefab == null)
            Debug.LogWarning("No Prefab At:" + path);
        return prefab;
    }
    public static T[] LoadAll<T>(string path) where T : UnityEngine.Object
    {
        T[] array = Resources.LoadAll<T>(path);

        if (array.Length == 0)
            Debug.LogWarning("No InnerItems At:" + path);
        return array;
    }

    public static IEnumerator LoadAsync<T>(string resourcePath, Action<T> OnLoadFinished) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(resourcePath);
        yield return request;
        if (request.isDone && request.asset != null)
            OnLoadFinished(request.asset as T);
        else
            Debug.LogError("Null Path Of: Resources/" + resourcePath);
    }


    #endregion
    public static T LoadResourceSync<T>(string bundlePath, string name) where T:UnityEngine.Object
    {
        T template = ResourceLoader.ResourcesLoader.LoadFromBundle<T>(bundlePath, name);
        if (template == null)
        {   
            Debug.LogError("Null Path Found Of Bundle Name:" + bundlePath + "/Item Name:" + name);
        }
        return template;
    }
    public static GameObject SpawnGameObjectAt( string bundlePath,string name, Transform parentTrans=null)
    {
#if UNITY_EDITOR            //Android Bundle Item In Editor Won't Show Proper Material, Load By AssetDataBase
        GameObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/BundleAssets/" + bundlePath + "/" + name + ".prefab");
#else
        GameObject obj = ResourceLoader.ResourcesLoader.LoadFromBundle<GameObject>( bundlePath , name );
#endif
        if (obj == null)
        {
            Debug.LogError("Null Path Of:" + bundlePath+"/"+name);
        }
        obj = GameObject.Instantiate(obj, parentTrans);
        return obj;
    }
}

namespace ResourceLoader
{
    public class ResourcesLoader
    {
        static Dictionary<string, AssetBundle> bundleDic = new Dictionary<string, AssetBundle>();
        public static T LoadFromBundle<T>(string subPath,string name) where T:UnityEngine.Object
        {
            AssetBundle bundle;
            if (bundleDic.ContainsKey(subPath))
              {
                bundle = bundleDic[subPath];
            }
            else
            {
                bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + subPath.ToLower());
                bundleDic.Add(subPath, bundle);
            }
            return bundle.LoadAsset<T>(name);
    }
    public static IEnumerator LoadAllResourcesAsync<T>(string subPath,Action<List<T>> templateDel) where T : UnityEngine.Object
        {
            AssetBundle bundle = null;
            AssetBundleRequest request = null;
            List<T> templateList = new List<T>();
            string bundlePath = Application.streamingAssetsPath + "/" + subPath.ToLower();
            WWW bundleInfo = null;
            if (bundleDic.ContainsKey(subPath))
            {
                bundle = bundleDic[subPath];
            }
            else
            {
                bundleInfo= new WWW(bundlePath); ;
            }
            for (; ; )
            {
                while (bundleInfo!=null&&!bundleInfo.isDone)
                    yield return null;
                
                if (bundleInfo != null)
                {
                    bundle = bundleInfo.assetBundle;
                    bundleDic.Add(subPath, bundle);
                }
                if (request == null)
                {
                    request = bundle.LoadAllAssetsAsync<T>();
                }
                while (!request.isDone)
                    yield return null;

                for (int i=0 ; i < request.allAssets.Length; i++)
                {
                    templateList.Add(request.allAssets[i] as T);
                }
                
                templateDel(templateList);
                yield break;
            }
    }
    }

}