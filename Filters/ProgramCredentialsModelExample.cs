using Swashbuckle.AspNetCore.Filters;

namespace Starting_Project
{
        /// <summary>
        /// Program credential examples
        /// </summary>
    public class ProgramCredentialsModelExample : IExamplesProvider<ProgramCredentials>
    {
        /// <summary>
        /// Gets the sample value for object properties
        /// </summary>
        /// <returns></returns>
        public ProgramCredentials GetExamples()
        {
            return new ProgramCredentials
            {
                Title = "Summer Internship Program",
                Description = "You can provide all information about the program here. Please make sure to set the expectation and keep it clear",
                HidePhone = false,
                HideNationality = false,
                HideCurrentResidence = false,
                HideIdNumber = false,
                HideDateOfBirth = false,
                HideGender = false,
                PersonalQuestions =
                [
                    new CustomQuestion
                    {
                        Question = "What is your height?",
                        Type = QuestionType.Paragraph
                    }
                ],
                CustomQuestions =
                [
                    new CustomQuestion
                    {
                        Question = "Have you ever been rejected by the UK embassy?",
                        Type = QuestionType.YesOrNo
                    },
                    new CustomQuestion
                    {
                        Question = "Please select at least 3 answers from the dropdowns below.",
                        Type = QuestionType.MultipleChoice,
                        Choices = ["creative", "outgoing", "college needed att", "incoming"],
                        MaxChoiceAllowed = 3
                    },
                    new CustomQuestion
                    {
                        Question = "How many years of experience do you have, please enter the number below.",
                        Type = QuestionType.Number
                    },
                    new CustomQuestion
                    {
                        Question = "Please provide the date that you have moved to the UK.",
                        Type = QuestionType.Date
                    }
                ]
            };
        }
    }
}
