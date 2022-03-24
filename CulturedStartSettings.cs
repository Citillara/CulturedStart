using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Settings.Base.Global;

namespace zCulturedStart
{
    public class CulturedStartSettings : AttributeGlobalSettings<CulturedStartSettings>
    {
        public override string Id => "zCulturedStart";

        public override string DisplayName => "Cultured Start";

        public override string FolderName => "zCulturedStart";

        public override string FormatType => "json2";

        [SettingPropertyBool("{=CulturedStart52}Skip TW Logo", Order = 0, RequireRestart = false, HintText = "{=CulturedStart53}Skip the TaleWorlds logo. Enabled by default.")]
        [SettingPropertyGroup("{=CulturedStart51}Debug", GroupOrder = 0)]
        public bool ShouldSkipTWLogo { get; set; } = true;

        [SettingPropertyBool("{=CulturedStart54}Skip Campaign Intro", Order = 1, RequireRestart = false, HintText = "{=CulturedStart55}Skip the campaign intro. Enabled by default.")]
        [SettingPropertyGroup("{=CulturedStart51}Debug", GroupOrder = 0)]
        public bool ShouldSkipCampaignIntro { get; set; } = true;

        [SettingPropertyDropdown("{=CulturedStart56}Skip Character Creation Menus", Order = 2, RequireRestart = false, HintText = "{=CulturedStart57}Skip character creation menus and start with a random culture and skipped options set to default. Default is None.")]
        [SettingPropertyGroup("{=CulturedStart51}Debug", GroupOrder = 0)]
        public DropdownDefault<string> MenusToSkip { get; set; } = new DropdownDefault<string>(new string[] { "{=CulturedStart58}None", "{=CulturedStart59}Base", "{=CulturedStart60}All" }, 0);
    }
}
