using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace zCulturedStart
{
    public class CulturedStartManager
    {
        private static readonly CulturedStartManager _culturedStartManager = new CulturedStartManager();

        public static CulturedStartManager Current => _culturedStartManager;

        // 0 = Default, 1 = Skip
        public int QuestOption { get; set; }

        // 0 = Default, 1 = Merchant, 2 = Exiled, 3 = Mercenary, 4 = Looter, 5 = Vassal, 6 = Kingdom, 7 = Holding, 8 = Landed Vassal, 9 = Escaped Prisoner
        public int StartOption { get; set; }

        // 0 = Hometown, 1 = Random, 2 - 7 = Specific Town, 8 = Castle, 9 = Escaping
        public int LocationOption { get; set; }

        public Settlement CastleToAdd { get; set; }

        public Hero CaptorToEscapeFrom { get; set; }

        public Settlement StartingSettlement
        {
            get
            {
                switch (LocationOption)
                {
                    case 0:
                        return Hero.MainHero.HomeSettlement;
                    case 1:
                        return Settlement.FindAll(settlement => settlement.IsTown).GetRandomElementInefficiently();
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
                        return CastleToAdd;
                    default:
                        return Settlement.Find("tutorial_training_field");
                }
            }
        }

        public Vec2 StartingPosition => LocationOption != 9 ? StartingSettlement.GatePosition : CaptorToEscapeFrom.PartyBelongedTo.Position2D;

        public void SetQuestOption(int questOption) => QuestOption = questOption;

        public void SetStartOption(int startOption) => StartOption = startOption;

        public void SetLocationOption(int locationOption) => LocationOption = locationOption;

        public void SetCastleToAdd() => CastleToAdd = (from settlement in Settlement.All
                                                       where settlement.Culture == Hero.MainHero.Culture && settlement.IsCastle
                                                       select settlement).GetRandomElementInefficiently();

        public void SetCaptorToEscapeFrom() => CaptorToEscapeFrom = Hero.FindAll(hero => (hero.Culture == Hero.MainHero.Culture) && hero.IsAlive && hero.MapFaction != null && !hero.MapFaction.IsMinorFaction && hero.IsPartyLeader && hero.PartyBelongedTo.DefaultBehavior != AiBehavior.Hold).GetRandomElementInefficiently();
    }
}
