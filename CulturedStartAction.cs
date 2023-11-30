using HarmonyLib;
using Helpers;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace zCulturedStart
{
    public static class CulturedStartAction
    {
        public static readonly Color RED = new Color(1f, 0.2f, 0.2f);

        public static void Apply(int storyOption, int locationOption)
        {
            Hero mainHero = null;
            try
            {
                // This thing throws an exception if it's null instead of returning null
                mainHero = Hero.MainHero;
            }
            catch (NullReferenceException)
            {
                // We can't really reconver in this case
                ErrorMessage("No hero", storyOption, locationOption);
                return;
            }
            if (mainHero.PartyBelongedTo == null)
            {
                // We can't really reconver in this case. Maybe something to do later
                ErrorMessage("No party", storyOption, locationOption);
                return;
            }
            if (mainHero.PartyBelongedTo.ItemRoster == null)
            {
                // We can't really reconver in this case. Maybe something to do later
                ErrorMessage("No inventory", storyOption, locationOption);
                return;
            }
            Hero ruler = Hero.FindAll(hero => hero.Culture == mainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            Hero captor = Hero.FindAll(hero => hero.Culture == mainHero.Culture && hero.IsAlive && hero.MapFaction != null && !hero.MapFaction.IsMinorFaction && hero.IsPartyLeader && hero.PartyBelongedTo.DefaultBehavior != AiBehavior.Hold).GetRandomElementInefficiently();
            Settlement startingSettlement = null;
            // Take away all the stuff to apply to each option
            mainHero.PartyBelongedTo.ItemRoster.Clear();
            GiveGoldAction.ApplyBetweenCharacters(mainHero, null, mainHero.Gold, true);
            switch (locationOption)
            {
                case 0:
                    // HomeSettelement is not always the Hero's correct culture hometown
                    startingSettlement = mainHero.HomeSettlement;

                    if (mainHero.Culture.Id != startingSettlement.Culture.Id)
                    {
                        // Not the correct one, let's find one that would be fitting
                        Settlement settlement1 = Settlement.FindAll(settlement => settlement.IsTown && settlement.Culture.Id == mainHero.Culture.Id).GetRandomElementInefficiently();
                        if (settlement1 != null)
                        {
                            startingSettlement = settlement1;
                        }
                    }
                    break;
                case 1:
                    startingSettlement = Settlement.FindAll(settlement => settlement.IsTown).GetRandomElementInefficiently();
                    break;
                case 2:
                    startingSettlement = Settlement.Find("town_A8");
                    break;
                case 3:
                    startingSettlement = Settlement.Find("town_B2");
                    break;
                case 4:
                    startingSettlement = Settlement.Find("town_EW2");
                    break;
                case 5:
                    startingSettlement = Settlement.Find("town_S2");
                    break;
                case 6:
                    startingSettlement = Settlement.Find("town_K4");
                    break;
                case 7:
                    startingSettlement = Settlement.Find("town_V3");
                    break;
                case 8:
                    startingSettlement = (from settlement in Settlement.All
                                          where settlement.Culture == mainHero.Culture && settlement.IsCastle
                                          select settlement).GetRandomElementInefficiently();
                    break;
                default:
                    break;
            }
            if (locationOption != 9)
            {
                if (startingSettlement != null)
                {
                    mainHero.PartyBelongedTo.Position2D = GetSettelementSafePosition(startingSettlement);
                }
                else
                {
                    Settlement tutorialSettelement = Settlement.Find("tutorial_training_field");
                    if (tutorialSettelement != null)
                    {
                        mainHero.PartyBelongedTo.Position2D = tutorialSettelement.Position2D;
                    }
                }
            }
            else // Escaping captor scenario
            {
                if (captor != null)
                {
                    if (captor.PartyBelongedTo != null)
                    {
                        // Original code. There should be some distance, but as the AI doesn't react immediately should be fine. 
                        mainHero.PartyBelongedTo.Position2D = captor.PartyBelongedTo.Position2D;
                    }
                    else
                    {
                        // Graceful start even if it's a problem
                        mainHero.PartyBelongedTo.Position2D = GetSettelementSafePosition(startingSettlement);
                        ErrorMessage("Issue getting captor party", storyOption, locationOption);
                    }
                }
                else
                {
                    // No captor ?!
                    mainHero.PartyBelongedTo.Position2D = GetSettelementSafePosition(startingSettlement);
                    ErrorMessage("No captor", storyOption, locationOption);
                }
            }

            if (GameStateManager.Current.ActiveState is MapState mapState)
            {
                mapState.Handler.ResetCamera(true, true);
                mapState.Handler.TeleportCameraToMainParty();
            }
            switch (storyOption)
            {
                case 0: // Default
                    ApplyInternal(mainHero, gold: 1000, grain: 2);
                    break;
                case 1: // Merchant
                    ApplyInternal(mainHero, gold: 1600, grain: 2, mules: 5, troops: new int[] { 5, 3 });
                    break;
                case 2: // Exiled
                    ApplyInternal(mainHero, gold: 3000, grain: 2, tier: 4, companions: 1);
                    if (ruler != null)
                    {
                        ChangeCrimeRatingAction.Apply(ruler.MapFaction, 50, false);
                        CharacterRelationManager.SetHeroRelation(mainHero, ruler, -50);
                        foreach (Hero lord in Hero.FindAll(hero => hero.MapFaction == ruler.MapFaction && !hero.IsFactionLeader && hero.IsAlive))
                        {
                            CharacterRelationManager.SetHeroRelation(mainHero, lord, -5);
                        }
                    }
                    break;
                case 3: // Mercenary
                    ApplyInternal(mainHero, gold: 250, grain: 1, tier: 3, troops: new int[] { 10, 5, 3, 1 }, isMercenary: true);
                    mainHero.PartyBelongedTo.RecentEventsMorale -= 40;
                    break;
                case 4: // Looter
                    ApplyInternal(mainHero, gold: 40, grain: 0, troops: new int[] { 7 }, isLooter: true);
                    foreach (Kingdom kingdom in Campaign.Current.Kingdoms)
                    {
                        ChangeCrimeRatingAction.Apply(kingdom.MapFaction, 50, false);
                    }
                    break;
                case 5: // Vassal
                    ApplyInternal(mainHero, gold: 3000, grain: 2, tier: 3, troops: new int[] { 10, 4 }, ruler: ruler);
                    break;
                case 6: // Kingdom
                    ApplyInternal(mainHero, gold: 8000, grain: 15, tier: 5, troops: new int[] { 31, 20, 14, 10, 6 }, companions: 3, companionParties: 2, hasKingdom: true);
                    break;
                case 7: // Holding
                    ApplyInternal(mainHero, gold: 10000, grain: 15, tier: 5, troops: new int[] { 31, 20, 14, 10, 6 }, companions: 1, companionParties: 1, castle: startingSettlement, hasKingdom: true);
                    break;
                case 8: // Landed Vassal
                    ApplyInternal(mainHero, gold: 10000, grain: 2, tier: 3, troops: new int[] { 10, 4 }, ruler: ruler, castle: startingSettlement);
                    break;
                case 9: // Escaped Prisoner
                    ApplyInternal(mainHero, gold: 0, grain: 1, isLooter: true);
                    if (captor != null)
                    {
                        CharacterRelationManager.SetHeroRelation(mainHero, captor, -50);
                    }
                    break;
                default:
                    break;
            }
        }

        private static void ApplyInternal(Hero mainHero, int gold, int grain, int mules = 0, int tier = -1, int[] troops = null, int companions = 0, int companionParties = 0, Hero ruler = null, Settlement castle = null, bool isMercenary = false, bool isLooter = false, bool hasKingdom = false)
        {
            CharacterObject idealTroop = (from character in CharacterObject.All
                                          where character.Tier == tier && character.Culture == mainHero.Culture && !character.IsHero && !character.Equipment.IsEmpty()
                                          select character).GetRandomElementInefficiently();
            mainHero.PartyBelongedTo.ItemRoster.AddToCounts(DefaultItems.Grain, grain);
            mainHero.PartyBelongedTo.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("mule"), mules);
            GiveGoldAction.ApplyBetweenCharacters(null, mainHero, gold, true);
            if (isMercenary)
            {
                idealTroop = (from character in CharacterObject.All
                              where character.Tier == tier && character.Culture == mainHero.Culture && !character.IsHero && character.Occupation == Occupation.Mercenary && !character.Equipment.IsEmpty()
                              select character).GetRandomElementInefficiently();
            }
            else if (isLooter)
            {
                idealTroop = MBObjectManager.Instance.GetObject<CharacterObject>("looter");
                tier = idealTroop.Tier;
            }
            if (idealTroop != null)
            {
                mainHero.BattleEquipment.FillFrom(idealTroop.Equipment);
            }
            for (int i = 0; i < troops?.Length; i++)
            {
                int troopTier = i + 1;
                int num = troops[i];
                CharacterObject troop = (from character in CharacterObject.All
                                         where character.Tier == troopTier && character.Culture == mainHero.Culture && !character.IsHero && character.Occupation == Occupation.Soldier
                                         select character).GetRandomElementInefficiently();
                if (idealTroop?.Occupation == Occupation.Bandit)
                {
                    troop = idealTroop;
                }
                mainHero.PartyBelongedTo.AddElementToMemberRoster(troop, num, false);
            }
            for (int i = 0; i < companions; i++)
            {
                CharacterObject wanderer = (from character in CharacterObject.All
                                            where character.Occupation == Occupation.Wanderer && character.Culture == mainHero.Culture
                                            select character).GetRandomElementInefficiently();
                Settlement randomSettlement = (from settlement in Settlement.All
                                               where settlement.Culture == wanderer.Culture && settlement.IsTown
                                               select settlement).GetRandomElementInefficiently();
                Hero companion = HeroCreator.CreateSpecialHero(wanderer, randomSettlement, null, null, 33);
                companion.HeroDeveloper.InitializeHeroDeveloper(false, wanderer);
                companion.SetHasMet();
                companion.ChangeState(Hero.CharacterStates.Active);
                if (idealTroop != null)
                {
                    companion.BattleEquipment.FillFrom(idealTroop.Equipment);
                }
                AddCompanionAction.Apply(Clan.PlayerClan, companion);
                AddHeroToPartyAction.Apply(companion, mainHero.PartyBelongedTo, false);
                GiveGoldAction.ApplyBetweenCharacters(null, companion, 2000, true);
                if (i < companionParties)
                {
                    MobilePartyHelper.CreateNewClanMobileParty(companion, mainHero.Clan, out bool fromMainclan);
                }
            }
            if (ruler != null)
            {
                // Adding to prevent crash on custom cultures with no kingdom
                CharacterRelationManager.SetHeroRelation(mainHero, ruler, 10);
                ChangeKingdomAction.ApplyByJoinToKingdom(mainHero.Clan, ruler.Clan.Kingdom, false);
                mainHero.Clan.Influence = 10;
            }
            if (castle != null)
            {
                ChangeOwnerOfSettlementAction.ApplyByKingDecision(mainHero, castle);
            }
            if (hasKingdom)
            {
                Campaign.Current.KingdomManager.CreateKingdom(mainHero.Clan.Name, mainHero.Clan.InformalName, mainHero.Clan.Culture, mainHero.Clan);
                mainHero.Clan.Influence = 100;
            }
        }

        static Vec2 GetSettelementSafePosition(Settlement settlement)
        {
            if (settlement.IsTown)
            {
                return settlement.GatePosition;
            }
            else if (settlement.IsCastle)
            {
                return settlement.GatePosition;
            }
            return settlement.Position2D;
        }

        static void ErrorMessage(string message, int storyOption, int locationOption)
        {
            InformationManager.DisplayMessage(
                new InformationMessage($"CulturedStart Error : {message}. ({storyOption};{locationOption}). Please report this bug and/or try to change mod load order", new Color(1f, 0.5f, 0f)));
        }

        static void WarningMessage(string message, int storyOption, int locationOption)
        {
            InformationManager.DisplayMessage(
                new InformationMessage($"CulturedStart Warning : {message}. ({storyOption};{locationOption}). Please report this bug and/or try to change mod load order", new Color(1f, 1f, 0f)));
        }

    }
}
