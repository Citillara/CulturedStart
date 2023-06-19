namespace zCulturedStart
{
    public class CulturedStartManager
    {
        private static readonly CulturedStartManager _culturedStartManager = new CulturedStartManager();

        public static CulturedStartManager Current => _culturedStartManager;

        // 0 = Default, 1 = Skip
        public int QuestOption { get; set; }

        // 0 = Default, 1 = Merchant, 2 = Exiled, 3 = Mercenary, 4 = Looter, 5 = Vassal, 6 = Kingdom, 7 = Holding, 8 = Landed Vassal, 9 = Escaped Prisoner
        public int StoryOption { get; set; }

        // TODO : Convert those things to enums
        // 0 = Hometown, 1 = Random, 2 - 7 = Specific Town, 8 = Castle, 9 = Escaping
        public int LocationOption { get; set; }

        public void SetQuestOption(int questOption) => QuestOption = questOption;

        public void SetStoryOption(int storyOption) => StoryOption = storyOption;

        public void SetLocationOption(int locationOption) => LocationOption = locationOption;
    }
}
