using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zCulturedStart.Scenarios
{
    public abstract class BaseScenario
    {
        /// <summary>
        /// Used for internal reference
        /// </summary>
        public abstract string Id { get; }
        
        /// <summary>
        /// Menu text
        /// </summary>
        public abstract string Text { get; }

        //public MBList<SkillObject> 
        //public void AddCategoryOption(TextObject text,
        //MBList<SkillObject> effectedSkills,
        //CharacterAttribute effectedAttribute,
        //int focusToAdd,
        //int skillLevelToAdd,
        //int attributeLevelToAdd,
        //CharacterCreationOnCondition optionCondition,
        //CharacterCreationOnSelect onSelect,
        //CharacterCreationApplyFinalEffects onApply,
        //TextObject descriptionText = null,
        //MBList<TraitObject> effectedTraits = null,
        //int traitLevelToAdd = 0,
        //int renownToAdd = 0,
        //int goldToAdd = 0,
        //int unspentFocusPoint = 0,
        //int unspentAttributePoint = 0)

    }
}
