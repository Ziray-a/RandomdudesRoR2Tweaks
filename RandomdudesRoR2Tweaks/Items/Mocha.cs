using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;



using static MainGameTweaks.RandomdudesRoR2Tweaks;
namespace MainGameTweaks.Items
{

    public static class Mocha
    {

        public enum ItemRarities
        {
            White,
            Green,
            Red,
        }
        public static ConfigEntry<bool> Enable { get; set; }
        public static ConfigEntry<ItemRarities> MochaRarity { get; set; }
        public static ConfigEntry<string> MochaCorruptionList { get; set; }
        public static ConfigEntry<bool> MochaCorruption { get; set; }


        internal static void Init(ConfigFile conf)
        {
            LoadConfig(conf);

            if (Enable.Value == true)
            {
                /*
                LogInfoFromClass("Loading Mocha");
                LogInfoFromClass("The following config will be applied: "
                + "\n Corrupted?: " + MochaCorruption.Value
                + "\n Rarity: " + MochaRarity.Value
                + "\n CorruptionList: " + MochaCorruptionList.Value);
                */
                OnItemCatalogPreInit += Modify;

                if (MochaCorruption.Value == true)
                {
                    OnContagousItemManagerInit += Corrupt;
                }
            }
            return;
        }

        public static void LoadConfig(ConfigFile conf)
        {
            Enable = conf.Bind("MochaTweaks", "Enable Changes", false, "easily disable tampering");
            MochaRarity = conf.Bind("MochaTweaks", "Mocha Item Rarity", ItemRarities.White, "Change this to the desired Item Rarity");
            MochaCorruption = conf.Bind("MochaTweaks", "Mocha as Void Item", false, "Enable to make Mocha a Void Item");
            MochaCorruptionList = conf.Bind("MochaTweaks", "Mocha corruption list", "Srynge, Hoof", "Add the Item Values of the desired Items to corrupt with Mocha spereated by a Comma");
        }


        public static void Corrupt()
        {
            foreach (string Item in MochaCorruptionList.Value.Split(new char[] { ',', ' ' }))
            {
                ItemDef.Pair transformation = new ItemDef.Pair()
                {
                    itemDef1 = FindItemDefPreCatalogInit(Item),
                    itemDef2 = DLC1Content.Items.AttackSpeedAndMoveSpeed

                };
                addPairToCatalog(transformation);

            }

        }

        public static void Modify()
        {
            ItemDef Mocha = DLC1Content.Items.AttackSpeedAndMoveSpeed;

            if (MochaCorruption.Value == true)
            {
                switch (MochaRarity.Value)
                {
                    case ItemRarities.Red:
                        Mocha.tier = ItemTier.VoidTier3;
                        break;

                    case ItemRarities.White:
                        Mocha.tier = ItemTier.VoidTier1;
                        break;

                    case ItemRarities.Green:
                        Mocha.tier = ItemTier.VoidTier2;
                        break;
                }

            }
            else
            {
                switch (MochaRarity.Value)
                {
                    case ItemRarities.Red:
                        Mocha.tier = ItemTier.Tier3;
                        break;

                    case ItemRarities.White:
                        Mocha.tier = ItemTier.Tier1;
                        break;

                    case ItemRarities.Green:
                        Mocha.tier = ItemTier.Tier2;
                        break;
                }
            }
        }

    }

}