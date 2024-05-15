namespace Starting_Project
{
    public class ApplicationDataModel : BaseDataModel
    {
        /// <summary>
        /// The first name of the applicant
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the applicant
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email address of the applicant
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the applicant
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The nationality of the applicant
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// The current residence of the applicant
        /// </summary>
        public string CurrentResidence { get; set; }

        /// <summary>
        /// The Id number of the applicant
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// The date of birth of the applicant
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// The gender of the applicant
        /// </summary>
        public string Gender { get; set; }

        public List<OtherAnswer> PersonalAnswers { get; set; } = [];

        public List<OtherAnswer> AdditionalAnswers { get; set; } = [];
    }
}
