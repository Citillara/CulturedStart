using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace zCulturedStart
{
    public class CulturedStartCharacterCreationContent : SandboxCharacterCreationContent
    {
        public void AddQuestMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=CulturedStart01}Quest Options", null), new TextObject("{=CulturedStart02}How do you want to handle your quests?", null), null, CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory();
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart03}Tutorial Skip (Default Quests)", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(DefaultQuestOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart04}Default start of the game, just without tutorial", null));
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart05}Neretzes' Folly Skip (Skips First Quest)", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(SkipQuestOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart06}Completes the first quest without talking to the quest nobles", null));
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        public void AddStartMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=CulturedStart07}Start Options", null), new TextObject("{=CulturedStart08}Who are you in Calradia...", null), new CharacterCreationOnInit(StartOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            //Default Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart09}A commoner (Default Start)", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(DefaultStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart10}Setting off with your Father, Mother, Brother and your two younger siblings to a new town you'd heard was safer. But you did not make it.", null), null, 0, 0);

            //Merchant Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart11}A budding caravanner", null), new List<SkillObject> { DefaultSkills.Trade }, null, 1, 10, 0, null, new CharacterCreationOnSelect(MerchantStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart12}With what savings you could muster you purchased some mules and mercenaries.", null), null, 0, 0);

            //Exiled Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart13}A noble of {CULTURE} in exile", null), new List<SkillObject> { DefaultSkills.Leadership }, null, 1, 10, 0, null, new CharacterCreationOnSelect(ExiledStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart14}Forced into exile after your parents were executed for suspected treason. With only your family's bodyguard you set off. Should you return you'd be viewed as a criminal.", null), null, 0, 150);

            //Mercenary Start            
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart15}In a failing mercenary company", null), new List<SkillObject> { DefaultSkills.Tactics }, null, 1, 10, 0, null, new CharacterCreationOnSelect(MercenaryStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart16}With men deserting over lack of wages, your company leader was found dead, and you decided to take your chance and lead.", null), null, 0, 50);

            //Looter Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart17}A looter lowlife", null), new List<SkillObject> { DefaultSkills.Roguery }, null, 1, 10, 0, null, new CharacterCreationOnSelect(LooterStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart18}Left impoverished from war, you found a group of like-minded ruffians who were desperate to get by.", null), null, 0, 0);

            //Vassal Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart19}A new vassal of {CULTURE}", null), new List<SkillObject> { DefaultSkills.Steward }, null, 1, 10, 0, null, new CharacterCreationOnSelect(VassalStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart20}A young noble who came into an arrangement with the king for a chance at land.", null), null, 0, 150);

            //Kingdom Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart21}Leading part of {CULTURE}", null), new List<SkillObject> { DefaultSkills.Leadership, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 15, 1, null, new CharacterCreationOnSelect(KingdomStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart22}With the support of companions you have gathered an army. With limited funds and food you decided it's time for action.", null), null, 0, 900);

            //Holding Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart23}You acquired a castle", null), new List<SkillObject> { DefaultSkills.Leadership, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 15, 1, null, new CharacterCreationOnSelect(HoldingStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart24}You acquired a castle through your own means and declared yourself a kingdom for better or worse.", null), null, 0, 900);

            //Landed Vassal Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart25}A landed vassal of {CULTURE}", null), new List<SkillObject> { DefaultSkills.Steward }, null, 1, 10, 0, null, new CharacterCreationOnSelect(LandedVassalStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart26}A young noble who came into an arrangement with the king for land.", null), null, 0, 150);

            //Escaped Prisoner Start
            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart27}An escaped prisoner of a lord of {CULTURE}", null), new List<SkillObject> { DefaultSkills.Roguery }, null, 1, 10, 0, null, new CharacterCreationOnSelect(EscapedStartOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart28}A poor prisoner of petty crimes who managed to break their shackles with a rock and fled.", null), null, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }
        public void AddLocationMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=CulturedStart29}Location Options", null), new TextObject("{=CulturedStart30}Beginning your new adventure...", null), null, CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart31}Near your home in the city where your journey began", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(HometownLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart32}Back to where you started", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart33}In a strange new city (Random)", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(RandomLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart34}Travelling far and wide you arrive at an unknown city", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart35}In a caravan to the Aserai city of Qasira", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(QasariLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart36}You leave the caravan right at the gates", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart37}In a caravan to the Battania city of Dunglanys", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(DunglanysLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart36}You leave the caravan right at the gates", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart38}On a ship to the Empire city of Zeonica", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(ZeonicaLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart39}You leave the ship and arrive right at the gates", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart40}In a caravan to the Sturgia city of Balgard", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(BalgardLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart36}You leave the caravan right at the gates", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart41}In a caravan to the Khuzait city of Ortongard", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(OrtongardLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart36}You leave the caravan right at the gates", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart42}On a river boat to the Vlandia city of Pravend", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(PravendLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart43}You leave the boat and arrive right at the gates", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart44}At your castle", null), new List<SkillObject>(), null, 0, 0, 0, new CharacterCreationOnCondition(CastleLocationOnCondition), new CharacterCreationOnSelect(CastleLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart45}At your newly acquired castle", null), null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CulturedStart46}Escaping from your captor", null), new List<SkillObject>(), null, 0, 0, 0, new CharacterCreationOnCondition(EscapingLocationOnCondition), new CharacterCreationOnSelect(EscapingLocationOnConsequence), new CharacterCreationApplyFinalEffects(DoNothingOnApply), new TextObject("{=CulturedStart47}Having just escaped", null), null);

            characterCreation.AddNewMenu(characterCreationMenu);
        }

        protected void StartOnInit(CharacterCreation characterCreation) => MBTextManager.SetTextVariable("CULTURE", Instance.GetSelectedCulture().Name);

        protected void DefaultQuestOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetQuestOption(0);
        protected void SkipQuestOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetQuestOption(1);

        protected void DefaultStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(0);
        protected void MerchantStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(1);
        protected void ExiledStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(2);
        protected void MercenaryStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(3);
        protected void LooterStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(4);
        protected void VassalStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(5);
        protected void KingdomStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(6);
        protected void HoldingStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(7);
        protected void LandedVassalStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(8);
        protected void EscapedStartOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetStartOption(9);

        protected void HometownLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(0); //Hometown
        protected void RandomLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(1); //Random
        protected void QasariLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(2); //Aserai
        protected void DunglanysLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(3); //Battania
        protected void ZeonicaLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(4); //Empire
        protected void BalgardLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(5); //Sturgia
        protected void OrtongardLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(6); //Khuzait
        protected void PravendLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(7); //Vlandia
        protected void CastleLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(8); //Castle
        protected void EscapingLocationOnConsequence(CharacterCreation characterCreation) => CulturedStartHelper.SetLocationOption(9); //Escaping

        protected void DoNothingOnApply(CharacterCreation characterCreation) { }

        protected bool CastleLocationOnCondition() => CulturedStartHelper.StartOption == 7 || CulturedStartHelper.StartOption == 8;
        protected bool EscapingLocationOnCondition() => CulturedStartHelper.StartOption == 9;
    }
}
