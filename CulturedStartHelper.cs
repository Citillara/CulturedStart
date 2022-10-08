using Helpers;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace zCulturedStart
{
    public static class CulturedStartHelper
    {
        public static void ApplyStartOptions()
        {
            // Take away all the stuff to apply to each option
            CulturedStartManager manager = CulturedStartManager.Current;
            GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, Hero.MainHero.Gold, true);
            PartyBase.MainParty.ItemRoster.Clear();
            manager.SetCastleToAdd();
            manager.SetCaptorToEscapeFrom();
            MobileParty.MainParty.Position2D = manager.StartingPosition;
            if (GameStateManager.Current.ActiveState is MapState mapState)
            {
                mapState.Handler.ResetCamera(true, true);
                mapState.Handler.TeleportCameraToMainParty();
            }
            switch (manager.StartOption)
            {
                case 0: // Default
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 1000, true);
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
                    break;
                case 1: // Merchant
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 1600, true);
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
                    PartyBase.MainParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("mule"), 5);
                    AddTroops(1, 5);
                    AddTroops(2, 3);
                    break;
                case 2: // Exiled
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 3000, true);
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
                    AddExiledHero();
                    SetRelationWithRuler();
                    break;
                case 3: // Mercenary
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 250, true);
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 1);
                    AddTroops(1, 10);
                    AddTroops(2, 5);
                    AddTroops(3, 3);
                    AddTroops(4, 1);
                    MobileParty.MainParty.RecentEventsMorale -= 40;
                    Hero.MainHero.BattleEquipment.FillFrom((from character in CharacterObject.All
                                                            where character.Tier == 3 && character.Culture == Hero.MainHero.Culture && !character.IsHero && (character.Occupation == Occupation.Soldier || character.Occupation == Occupation.Mercenary)
                                                            select character).GetRandomElementInefficiently().Equipment);
                    break;
                case 4: // Looter
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 40, true);
                    AddLooters(7);
                    foreach (Kingdom kingdom in Campaign.Current.Kingdoms)
                    {
                        ChangeCrimeRatingAction.Apply(kingdom.MapFaction, 50, false);
                    }
                    break;
                case 5: // Vassal
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 3000, true);
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
                    SetMainHeroAsVassal();
                    SetEquipment(Hero.MainHero, 3);
                    AddTroops(1, 10);
                    AddTroops(2, 4);
                    break;
                case 6: // Kingdom
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 8000, true);
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 15);
                    AddTroops(1, 31);
                    AddTroops(2, 20);
                    AddTroops(3, 14);
                    AddTroops(4, 10);
                    AddTroops(5, 6);
                    CreateKingdom();
                    SetEquipment(Hero.MainHero, 5);
                    Hero.MainHero.Clan.Influence = 100;
                    AddCompanionParties(2);
                    AddCompanions(1);
                    break;
                case 7: // Holding
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 10000, true);
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 15);
                    AddCastle();
                    AddTroops(1, 31);
                    AddTroops(2, 20);
                    AddTroops(3, 14);
                    AddTroops(4, 10);
                    AddTroops(5, 6);
                    CreateKingdom();
                    SetEquipment(Hero.MainHero, 5);
                    AddCompanionParties(1);
                    break;
                case 8: // Landed Vassal
                    GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 10000, true);
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
                    SetMainHeroAsVassal();
                    AddCastle();
                    SetEquipment(Hero.MainHero, 3);
                    AddTroops(1, 10);
                    AddTroops(2, 4);
                    break;
                case 9: // Escaped Prisoner
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 1);
                    EscapeFromCaptor();
                    break;
                default:
                    break;
            }
        }

        private static void SetEquipment(Hero hero, int tier)
        {
            CharacterObject idealTroop = (from character in CharacterObject.All
                                          where character.Tier == tier && character.Culture == hero.Culture && !character.IsHero && !character.Equipment.IsEmpty()
                                          select character).GetRandomElementInefficiently();
            hero.BattleEquipment.FillFrom(idealTroop.Equipment);
        }

        private static void SetRelationWithRuler()
        {
            Hero mainHero = Hero.MainHero;
            Hero ruler = Hero.FindAll(hero => (hero.Culture == mainHero.Culture) && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            CharacterRelationManager.SetHeroRelation(mainHero, ruler, -50);
            foreach (Hero lord in Hero.FindAll(hero => (hero.MapFaction == ruler.MapFaction) && hero.IsAlive))
            {
                CharacterRelationManager.SetHeroRelation(mainHero, lord, -5);
            }
            if (ruler != null)
            {
                CharacterRelationManager.SetHeroRelation(mainHero, ruler, -50);
                ChangeCrimeRatingAction.Apply(ruler.MapFaction, 49, false);
            }
        }

        private static void SetMainHeroAsVassal()
        {
            Hero mainHero = Hero.MainHero;
            // Find a clan that matches culture
            Hero lord = Hero.FindAll(hero => (hero.Culture == mainHero.Culture) && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            if (lord != null)
            {
                // Adding to prevent crash on custom cultures with no kingdom
                CharacterRelationManager.SetHeroRelation(mainHero, lord, 10);
                ChangeKingdomAction.ApplyByJoinToKingdom(mainHero.Clan, lord.Clan.Kingdom, false);
                Hero.MainHero.Clan.Influence = 10;
            }
        }

        private static void AddTroops(int tier, int num)
        {
            CharacterObject troop = (from character in CharacterObject.All
                                     where character.Tier == tier && character.Culture == Hero.MainHero.Culture && !character.IsHero && (character.Occupation == Occupation.Soldier || character.Occupation == Occupation.Mercenary)
                                     select character).GetRandomElementInefficiently();
            PartyBase.MainParty.AddElementToMemberRoster(troop, num, false);
        }

        private static void AddLooters(int num) // Dual purpose cause lazy, adds looters and sets player's gear as looter
        {
            CharacterObject character = MBObjectManager.Instance.GetObject<CharacterObject>("looter");
            PartyBase.MainParty.AddElementToMemberRoster(character, num, false);
            Hero.MainHero.BattleEquipment.FillFrom(character.Equipment);
        }

        private static void AddCompanions(int num) => AddCompanion(num, 2000, false);

        private static void AddCompanionParties(int num) => AddCompanion(num, 200, true);

        private static void AddCompanion(int num, int gold, bool shouldCreateParty)
        {
            Hero mainHero = Hero.MainHero;
            for (int i = 0; i < num; i++)
            {
                CharacterObject wanderer = (from character in CharacterObject.All
                                            where character.Occupation == Occupation.Wanderer && (character.Culture == mainHero.Culture || character.Culture == CulturedStartManager.Current.StartingSettlement.Culture)
                                            select character).GetRandomElementInefficiently();
                Settlement randomSettlement = (from settlement in Settlement.All
                                               where settlement.Culture == wanderer.Culture && settlement.IsTown
                                               select settlement).GetRandomElementInefficiently();
                Hero companion = HeroCreator.CreateSpecialHero(wanderer, randomSettlement, null, null, 33);
                companion.HeroDeveloper.DeriveSkillsFromTraits(false, wanderer);
                SetEquipment(companion, 4);
                companion.HasMet = true;
                companion.Clan = randomSettlement.OwnerClan;
                companion.ChangeState(Hero.CharacterStates.Active);
                AddCompanionAction.Apply(Clan.PlayerClan, companion);
                AddHeroToPartyAction.Apply(companion, MobileParty.MainParty, true);
                GiveGoldAction.ApplyBetweenCharacters(null, companion, gold, true);
                if (shouldCreateParty)
                {
                    MobilePartyHelper.CreateNewClanMobileParty(companion, mainHero.Clan, out bool fromMainclan);
                }
            }
        }

        private static void AddExiledHero()
        {
            Hero mainhero = Hero.MainHero;
            CharacterObject wanderer = (from character in CharacterObject.All
                                        where character.Occupation == Occupation.Wanderer && character.Culture == mainhero.Culture
                                        select character).GetRandomElementInefficiently();
            Equipment exiledHeroEquipment = (from character in CharacterObject.All
                                             where character.Level > 20 && character.Culture == wanderer.Culture && !character.IsHero && character.Tier > 4
                                             select character).GetRandomElementInefficiently().Equipment;
            Equipment mainHeroEquipment = (from character in CharacterObject.All
                                           where character.Tier == 4 && character.Culture == wanderer.Culture && !character.IsHero
                                           select character).GetRandomElementInefficiently().Equipment;
            Settlement randomSettlement = (from settlement in Settlement.All
                                           where settlement.Culture == wanderer.Culture && settlement.IsTown
                                           select settlement).GetRandomElementInefficiently();
            Hero exiledHero = HeroCreator.CreateSpecialHero(wanderer, randomSettlement, null, null, 33);
            exiledHero.HeroDeveloper.DeriveSkillsFromTraits(false, wanderer);
            GiveGoldAction.ApplyBetweenCharacters(null, exiledHero, 4000, true);
            exiledHero.BattleEquipment.FillFrom(exiledHeroEquipment);
            mainhero.BattleEquipment.FillFrom(mainHeroEquipment);
            exiledHero.HasMet = true;
            exiledHero.Clan = randomSettlement.OwnerClan;
            exiledHero.ChangeState(Hero.CharacterStates.Active);
            AddCompanionAction.Apply(Clan.PlayerClan, exiledHero);
            AddHeroToPartyAction.Apply(exiledHero, MobileParty.MainParty, true);
        }

        private static void AddCastle() => ChangeOwnerOfSettlementAction.ApplyByKingDecision(Hero.MainHero, CulturedStartManager.Current.CastleToAdd);

        private static void CreateKingdom() => Campaign.Current.KingdomManager.CreateKingdom(Clan.PlayerClan.Name, Clan.PlayerClan.InformalName, Clan.PlayerClan.Culture, Clan.PlayerClan);

        private static void EscapeFromCaptor() // Escaped Prisoner start 
        {
            Hero captor = CulturedStartManager.Current.CaptorToEscapeFrom;
            if (captor != null)
            {
                CharacterRelationManager.SetHeroRelation(Hero.MainHero, captor, -50);
            }
            // Using Looter gear as baseline
            CharacterObject character = MBObjectManager.Instance.GetObject<CharacterObject>("looter");
            Hero.MainHero.BattleEquipment.FillFrom(character.Equipment);
        }
    }
}
