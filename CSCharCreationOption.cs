using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace zCulturedStart
{
    public class CSCharCreationOption
    {
        private static int _CSSelcOption;
        private static int _CSGameOption;
        private static int _CSLocationOption;
        private static int _CSSandboxToggle;
        private static CultureObject _SelectedCulture;

        //1 = Default 2 = Merchant 3 = Exiled 4 = Merc 5 = Looter
        public static int CSSelectOption
        {
            get => _CSSelcOption;
            set => _CSSelcOption = value;
        }

        //0 = FP Default 1 = FP Nezzy 2 = FP Sandbox 3 = Default 4 = Nezzy 5 = Sandbox No Kingdom
        public static int CSGameOption
        {
            get => _CSGameOption;
            set => _CSGameOption = value;
        }

        //0 = Hometown 1 = Random Location 2 - 8 = Specific Town
        public static int CSLocationOption
        {
            get => _CSLocationOption;
            set => _CSLocationOption = value;
        }

        //0 = Story 1 = Sandbox
        public static int CSSandboxToggle
        {
            get => _CSSandboxToggle;
            set => _CSSandboxToggle = value;
        }

        public static CultureObject SelectedCulture
        {
            get => _SelectedCulture;
            set => _SelectedCulture = value;
        }
        public static List<CultureObject> AddtlCulturesList;

        public static Settlement cultureSettlement(Hero hero)
        {
            string sCulture = hero.MapFaction.Culture.StringId;
            switch (sCulture)
            {
                case "sturgia":
                    return Settlement.Find("town_S2");
                case "aserai":
                    return Settlement.Find("town_A8");
                case "vlandia":
                    return Settlement.Find("town_V3");
                case "battania":
                    return Settlement.Find("town_B2");
                case "khuzait":
                    return Settlement.Find("town_K4");
                default:
                    return Settlement.Find("town_ES3");
            }
        }
        public static Settlement cultureSettlement(string sCulture)
        {
            switch (sCulture)
            {
                case "sturgia":
                    return Settlement.Find("town_S2");
                case "aserai":
                    return Settlement.Find("town_A8");
                case "vlandia":
                    return Settlement.Find("town_V3");
                case "battania":
                    return Settlement.Find("town_B2");
                case "khuzait":
                    return Settlement.Find("town_K4");
                default:
                    return Settlement.Find("town_EW2");
            }
        }

        public static Settlement RandcultureSettlement() => Settlement.FindAll((Settlement x) => x.IsTown).GetRandomElementInefficiently<Settlement>();

        public static Settlement CSOptionSettlement()
        {
            int opt = CSCharCreationOption.CSLocationOption;
            switch (opt)
            {
                case 0:
                    return CSCharCreationOption.cultureSettlement(Hero.MainHero);
                case 1:
                    return CSCharCreationOption.RandcultureSettlement();
                case 2:
                    return Settlement.Find("town_A8");
                case 3:
                    return Settlement.Find("town_B2");
                case 4:
                    return Settlement.Find("town_EW2");
                case 5:
                    return Settlement.Find("town_S2");
                case 6:
                    return Settlement.Find("town_K4");
                case 7:
                    return Settlement.Find("town_V3");
                case 8:
                    return CSCharCreationOption.cultureSettlement(Hero.MainHero);
                default:
                    return Settlement.Find("tutorial_training_field");
            }
        }
    }
}
