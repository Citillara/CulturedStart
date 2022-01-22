using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

namespace zCulturedStart
{
    public class CulturedStartSettings : AttributeGlobalSettings<CulturedStartSettings>
    {
        public override string Id => "zCulturedStart";
        public override string DisplayName => "Cultured Start";
        public override string FolderName => "zCulturedStart";
        public override string FormatType => "json2";
        [SettingPropertyBool("{=CulturedStart53}Skip TW Logo", Order = 0, RequireRestart = false, HintText = "{=CulturedStart54}Skip the TaleWorlds logo. Enabled by default.")]
        [SettingPropertyGroup("{=CulturedStart52}Debug", GroupOrder = 0)]
        public bool ShouldSkipTWLogo { get; set; } = true;
        [SettingPropertyBool("{=CulturedStart55}Skip Campaign Intro", Order = 1, RequireRestart = false, HintText = "{=CulturedStart56}Skip the campaign intro. Enabled by default.")]
        [SettingPropertyGroup("{=CulturedStart52}Debug", GroupOrder = 0)]
        public bool ShouldSkipCampaignIntro { get; set; } = true;
        [SettingPropertyBool("{=CulturedStart57}Skip Character Creation", Order = 2, RequireRestart = false, HintText = "{=CulturedStart58}Skip character creation and start with a random culture and all other options set to default. Disabled by default.")]
        [SettingPropertyGroup("{=CulturedStart52}Debug", GroupOrder = 0)]
        public bool ShouldSkipCharacterCreation { get; set; } = false;
    }
}
