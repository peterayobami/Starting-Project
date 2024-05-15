using System.ComponentModel.DataAnnotations;

namespace Starting_Project
{
    public class ProgramCredentials
    {
        [Required(ErrorMessage = "Please provide the program title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please provide the program description")]
        public string Description { get; set; }
        public bool HidePhone { get; set; }
        public bool HideNationality { get; set; }
        public bool HideCurrentResidence { get; set; }
        public bool HideIdNumber { get; set; }
        public bool HideDateOfBirth { get; set; }
        public bool HideGender { get; set; }
        public List<CustomQuestion> PersonalQuestions { get; set; }
        public List<CustomQuestion> CustomQuestions { get; set; }
    }

    public class UpdateProgramCredentials
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public FieldStatus PhoneFieldStatus { get; set; }
        public FieldStatus NationalityFieldStatus { get; set; }
        public FieldStatus CurrentResidenceFieldStatus { get; set; }
        public FieldStatus IdNumberFieldStatus { get; set; }
        public FieldStatus DateOfBirthFieldStatus { get; set; }
        public FieldStatus GenderFieldStatus { get; set; }
        public List<UpdateCustomQuestion> PersonalQuestions { get; set; }
        public List<UpdateCustomQuestion> CustomQuestions { get; set; }
    }
}
