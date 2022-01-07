using HarmonyLib;
using Helpers;
using StoryMode;
using StoryMode.Behaviors;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace zCulturedStart
{
    public class CulturedStartLocPatch
    {
        [HarmonyPatch(typeof(TrainingFieldCampaignBehavior), "OnCharacterCreationIsOver")]
        public class CulturedStartLocTrainingFieldPatch
        {
            private static bool Prefix(TrainingFieldCampaignBehavior __instance)
            {
                //Setting Various extra values to try and match usual complete tutorial phase to make sure events fire.
                AccessTools.Field(typeof(TrainingFieldCampaignBehavior), "_talkedWithBrotherForTheFirstTime").SetValue(__instance, true);
                TutorialPhase.Instance.PlayerTalkedWithBrotherForTheFirstTime();

                Hero brother = (Hero)AccessTools.Property(typeof(StoryModeHeroes), "ElderBrother").GetValue(null);
                brother.ChangeState(Hero.CharacterStates.Disabled);
                //Believe this is line missed that is causing all the brother issues
                brother.Clan = CampaignData.NeutralFaction;

                Vec2 StartPos = GetSettlementLoc(CSCharCreationOption.CSOptionSettlement());
                MobileParty.MainParty.Position2D = StartPos;
                MapState mapstate = GameStateManager.Current.ActiveState as MapState;
                mapstate.Handler.TeleportCameraToMainParty();
                SelectClanName();
                return false;
            }
        }
        [HarmonyPatch(typeof(TutorialPhaseCampaignBehavior), "OnCharacterCreationIsOver")]
        public class CulturedStartLocTutorialPhasePatch
        {
            private static void Postfix()
            {
                StoryModeManager.Current.MainStoryLine.CompleteTutorialPhase(true);
                CSApplyChoices.ApplyStoryOptions();
            }
        }
        public static Vec2 GetSettlementLoc(Settlement settlement) => settlement.GatePosition; //This is to get the vector of a HC settlement...Possible Todo Home town

        public static void SelectClanName() => InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=JJiKk4ow}Select your family name: ", null).ToString(), string.Empty, true, false, GameTexts.FindText("str_done", null).ToString(), null, new Action<string>(OnChangeClanNameDone), null, false, new Func<string, Tuple<bool, string>>(FactionHelper.IsClanNameApplicable), "", ""), false);
        private static void OnChangeClanNameDone(string newClanName)
        {
            TextObject textObject = new TextObject(newClanName ?? "", null);
            Clan.PlayerClan.InitializeClan(textObject, textObject, Clan.PlayerClan.Culture, Clan.PlayerClan.Banner);
            OpenBannerSelectionScreen();
        }

        private static void OpenBannerSelectionScreen() => Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<BannerEditorState>(), 0);
    }
}
