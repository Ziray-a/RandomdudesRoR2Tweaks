using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using System;
using UnityEngine.Networking.NetworkSystem;
using HarmonyLib;

namespace MainGameTweaks
{
    // This is an example plugin that can be put in
    // BepInEx/plugins/ExamplePlugin/ExamplePlugin.dll to test out.
    // It's a small plugin that adds a relatively simple item to the game,
    // and gives you that item whenever you press F2.

    // This attribute specifies that we have a dependency on a given BepInEx Plugin,
    // We need the R2API ItemAPI dependency because we are using for adding our item to the game.
    // You don't need this if you're not using R2API in your plugin,
    // it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(ItemAPI.PluginGUID)]

    // This one is because we use a .language file for language tokens
    // More info in https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Assets/Localization/
    [BepInDependency(LanguageAPI.PluginGUID)]

    // This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(RandomdudesRoR2Tweaks.PluginGUID, RandomdudesRoR2Tweaks.PluginName, RandomdudesRoR2Tweaks.PluginVersion)]

    // This is the main declaration of our plugin class.
    // BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    // BaseUnityPlugin itself inherits from MonoBehaviour,
    // so you can use this as a reference for what you can declare and use in your plugin class
    // More information in the Unity Docs: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class RandomdudesRoR2Tweaks : BaseUnityPlugin
    {
        // The Plugin GUID should be a unique ID for this plugin,
        // which is human readable (as it is used in places like the config).
        // If we see this PluginGUID as it is on thunderstore,
        // we will deprecate this mod.
        // Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Ziray-a";
        public const string PluginName = "RandomdudesRoR2Tweaks";
        public const string PluginVersion = "1.0.0";
        public static Action OnItemCatalogPreInit;

        public static Action OnContagousItemManagerInit;

        public static ManualLogSource log;

        internal static void LogInfoFromClass(string Message)
        {
            log.LogInfo(Message);
        }
        internal static void LogErrorFromClass(string Message)
        {
            log.LogError(Message);
        }


        // The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);

            ConfigFile ExtendedConfigFile = new ConfigFile(Paths.ConfigPath + "\\RandomdudesTweakConfig.cfg", true);
            Items.Mocha.Init(ExtendedConfigFile);
            On.RoR2.ItemCatalog.Init += ItemCatalogInit;
            On.RoR2.Items.ContagiousItemManager.Init += ContaigosCatalogInit;

        }

        // The Update() method is run on every frame of the game.
        private void Update()
        {

        }


        private void ItemCatalogInit(On.RoR2.ItemCatalog.orig_Init orig)
        {
            Action action = OnItemCatalogPreInit;
            if (action != null)
            {
                Logger.LogInfo("Patching Item Catalog");
            }

            orig();
        }

        private void ContaigosCatalogInit(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            Action action = OnContagousItemManagerInit;
            if (action != null)
            {
                Logger.LogInfo("Patching Corruptables");
            }


            orig();


        }

        internal static void addPairToCatalog(ItemDef.Pair transformation)
        {
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem].AddToArray(transformation);
        }



        internal static ItemDef FindItemDefPreCatalogInit(string identifier)
        {
            foreach (ItemDef itemDef in ContentManager.itemDefs)
            {
                if (itemDef.name == identifier)
                {

                    return itemDef;
                }
            }

            return null;
        }




    }

}
