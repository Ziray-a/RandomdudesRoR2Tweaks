using BepInEx;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Helpers.ConfigHelper;


namespace Helpers.ConfigHelper
{

    class ConfigHelper
    {


        enum ItemRarities
        {
            White,
            Green,
            Red,
        }
        private ConfigEntry<ItemRarities> MochaRarity;
        private ConfigEntry<string> MochaCorruptionList;
        private ConfigEntry<bool> MochaCorruption;

        public void LoadConfigandConfigure(ConfigFile conf)
        {
            MochaRarity = conf.Bind("Tweaks", "Mocha Item Rarity", ItemRarities.White, "Change this to the desired Item Rarity");
            MochaCorruption = conf.Bind("Tweaks", "Mocha as Void Item", false, "Enable to make Mocha a Void Item");
            MochaCorruptionList = conf.Bind("Tweaks", "Mocha corruption list", "Srynge, Hoof", "Add the Item Values of the desired Items to corrupt with Mocha spereated by a Comma");

            object MochaKey = "RoR2/DLC1/AttackSpeedAndMoveSpeed/AttackSpeedAndMoveSpeed.asset";
            var loadedAsset = Addressables.LoadAssetAsync<ItemDef>(MochaKey).WaitForCompletion();

            if (MochaCorruption.Value)
            {
                switch (MochaRarity.Value)
                {
                    case ItemRarities.Red:
                        loadedAsset.tier = ItemTier.VoidTier3;
                        break;

                    case ItemRarities.White:
                        loadedAsset.tier = ItemTier.VoidTier1;
                        break;

                    case ItemRarities.Green:
                        loadedAsset.tier = ItemTier.VoidTier2;
                        break;
                }

            }
            else
            {
                switch (MochaRarity.Value)
                {
                    case ItemRarities.Red:
                        loadedAsset.tier = ItemTier.Tier3;
                        break;

                    case ItemRarities.White:
                        loadedAsset.tier = ItemTier.Tier1;
                        break;

                    case ItemRarities.Green:
                        loadedAsset.tier = ItemTier.Tier2;
                        break;
                }
            }

        }

    }

}