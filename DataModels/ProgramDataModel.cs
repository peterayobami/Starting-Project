namespace Starting_Project
{
    /// <summary>
    /// The inspections database container representational model
    /// </summary>
    public class ProgramDataModel : BaseDataModel
    {
        /// <summary>
        /// The title of the program
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the program
        /// </summary>
        public string Description { get; set; }
        public bool HidePhone { get; set; }
        public bool HideNationality { get; set; }
        public bool HideCurrentResidence { get; set; }
        public bool HideIdNumber { get; set; }
        public bool HideDateOfBirth { get; set; }
        public bool HideGender { get; set; }
        public List<CustomQuestion> PersonalQuestions { get; set; } = [];
        public List<CustomQuestion> CustomQuestions { get; set; } = [];
    }
}