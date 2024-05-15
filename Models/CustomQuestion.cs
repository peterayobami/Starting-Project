using System.ComponentModel.DataAnnotations;

namespace Starting_Project
{
    public class CustomQuestion
    {
        [Required(ErrorMessage = "Please specify the question")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Please specify the type of question")]
        public string Type { get; set; }
        public string[] Choices { get; set; }
        public int MaxChoiceAllowed { get; set; }
        public bool OtherOptionEnabled { get; set; }
    }

    public class UpdateCustomQuestion
    {
        public string Question { get; set; }
        public string Type { get; set; }
        public string[] Choices { get; set; }
        public int MaxChoiceAllowed { get; set; }
        public FieldStatus OtherOptionStatus { get; set; }
    }
}
