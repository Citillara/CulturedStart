using HarmonyLib;
using StoryMode.Behaviors.Quests.FirstPhase;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox;

namespace zCulturedStart
{
    class CSBehaviour : CampaignBehaviorBase
    {
        public override void RegisterEvents() => CampaignEvents.OnQuestStartedEvent.AddNonSerializedListener(this, new Action<QuestBase>(OnQuestStarted));
        public override void SyncData(IDataStore dataStore) { }

        private void OnQuestStarted(QuestBase quest)
        {
            if (quest.StringId == "investigate_neretzes_banner_quest" && CSCharCreationOption.CSGameOption == 1)
            {
                AccessTools.Method(typeof(QuestBase), "CompleteQuestWithSuccess").Invoke(quest, null);
            }
            if (quest.StringId == "main_storyline_create_kingdom_quest_1" || quest.StringId == "main_storyline_create_kingdom_quest_0")
            {
                if (Clan.PlayerClan.Kingdom != null)
                {
                    if (Clan.PlayerClan.Kingdom.RulingClan == Clan.PlayerClan)
                    {
                        Type type = typeof(CreateKingdomQuest);
                        JournalLog log = (JournalLog)AccessTools.Field(type, "_clanIndependenceRequirementLog").GetValue(quest);
                        AccessTools.Field(type, "_hasPlayerCreatedKingdom").SetValue(quest, true);
                        Object[] parameters = new object[] { log, 1 };
                        AccessTools.Method(typeof(QuestBase), "UpdateQuestTaskStage").Invoke(quest, parameters);
                    }
                }
            }
        }
    }
}
