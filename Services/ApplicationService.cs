using Microsoft.EntityFrameworkCore;

namespace Starting_Project
{
    /// <summary>
    /// The domain service for application
    /// </summary>
    public class ApplicationService
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<ApplicationService> logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        /// <param name="logger">The injected logger</param>
        public ApplicationService(ApplicationDbContext context, ILogger<ApplicationService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Retrieves pro
        /// </summary>
        /// <param name="credentials">The application credentials</param>
        /// <returns></returns>
        public async Task<OperationResult> SubmitAsync(ApplicationCredentials credentials)
        {
            try
            {
                // If program id was not specified...
                if (string.IsNullOrEmpty(credentials.ProgramId))
                    // Handle the error
                    return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please specify the program id");

                // Retrieve the program
                var program = await context.Programs.FindAsync(credentials.ProgramId, credentials.ProgramId);

                // If program was not found...
                if (program == null)
                    // Handle the error
                    return HandleErrorResult(ErrorTitles.NotFound, StatusCodes.Status404NotFound, "Program with specified id could not be found");

                // Initialize application
                var application = new ApplicationDataModel { };

                // Validate answers

                // If phone is required and was not specified...
                if (!program.HidePhone && credentials.Phone == null)
                    // Handle the error
                    return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please specify phone number");

                // If nationality is required and was not specified...
                if (!program.HideNationality && credentials.Nationality == null)
                    // Handle the error
                    return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please specify nationality");

                // If current residence is required and was not specified...
                if (!program.HideCurrentResidence && credentials.CurrentResidence == null)
                    // Handle the error
                    return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please specify current residence");

                // If id number is required and was not specified...
                if (!program.HideIdNumber && credentials.IdNumber == null)
                    // Handle the error
                    return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please specify id number");

                // If date of birth is required and was not specified...
                if (!program.HideDateOfBirth && credentials.DateOfBirth == null)
                    // Handle the error
                    return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please specify date of birth");

                // If gender is required and was not provided...
                if (!program.HideGender && credentials.Gender == null)
                    // Handle the error
                    return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please specify gender");

                // If personal questions was required...
                if (program.PersonalQuestions.Count > 0)
                {
                    // If some answers were not attempted...
                    if (credentials.PersonalAnswers.Count < program.PersonalQuestions.Count)
                        // Handle the error
                        return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please provide answer to all questions");

                    // For each personal questions...
                    for (int i = 0; i < program.PersonalQuestions.Count; i++)
                    {
                        // Get the question
                        var question = program.PersonalQuestions[i];

                        // Get the corresponding feedback
                        var answer = credentials.PersonalAnswers[i];

                        // If answer was not specified for 'text type' questions...
                        if ((question.Type == QuestionType.Paragraph ||
                            question.Type == QuestionType.DropDown ||
                            question.Type == QuestionType.Date) && answer.Text == null)
                        {
                            if (answer.Text == null)
                            {
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please answer the question: {question.Question}");
                            }

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // If question type was YesOrNo...
                        if (question.Type == QuestionType.YesOrNo)
                        {
                            // If valid answer was not provided...
                            if (!(answer.Text == "YES" || answer.Text == "NO"))
                            {
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please provide a valid answer for the question: {question.Question}");
                            }

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // If question type was drop down...
                        if (question.Type == QuestionType.DropDown)
                        {
                            // If other option was not enabled...
                            // And answer does not correspond to any pre specified option...
                            if (!question.OtherOptionEnabled && question.Choices.Any(x => x == answer.Text))
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please provide a valid answer for the question: {question.Question}");

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // If question type was multiple choice...
                        if (question.Type == QuestionType.MultipleChoice)
                        {
                            // If other option was not enabled...
                            // And answer does not correspond to any pre specified option...
                            if (!question.OtherOptionEnabled && answer.MultipleChioces.Any(x => !question.Choices.Any(y => x == y)))
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please provide a valid answer for the question: ({question.Question})." +
                                    $"One or more of your options are not valid.");

                            // On the other hand, if the number of selected options is greater than the specified maximum...
                            if (answer.MultipleChioces.Length > question.MaxChoiceAllowed)
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Specified options must not be be greater than {question.MaxChoiceAllowed} for the question: ({question.Question}).");

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // If question type was date...
                        if (question.Type == QuestionType.Date)
                        {
                            // If date was format is not valid...
                            if (!DateTime.TryParse(answer.Text, out DateTime date))
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please specify the date in a valid format, such as 'yyyy-MM-dd'");

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // Add the answer to the collection
                        application.PersonalAnswers.Add(answer);
                    }
                }

                // If custom questions was required...
                if (program.CustomQuestions.Count > 0)
                {
                    // If some answers were not attempted...
                    if (credentials.CustomAnswers.Count < program.CustomQuestions.Count)
                        // Handle the error
                        return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please provide answer to all questions");

                    // For each custom questions...
                    for (int i = 0; i < program.CustomQuestions.Count; i++)
                    {
                        // Get the question
                        var question = program.CustomQuestions[i];

                        // Get the corresponding feedback
                        var answer = credentials.CustomAnswers[i];

                        // If answer was not attempted...
                        if (answer == null)
                            // Handle the error
                            return HandleErrorResult(ErrorTitles.BadRequest, StatusCodes.Status400BadRequest, "Please provide answer to all questions");

                        // If answer was not specified for 'text type' questions...
                        if ((question.Type == QuestionType.Paragraph ||
                            question.Type == QuestionType.DropDown ||
                            question.Type == QuestionType.Date) && answer.Text == null)
                        {
                            if (answer.Text == null)
                            {
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please answer the question: {question.Question}");
                            }

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // If question type was YesOrNo...
                        if (question.Type == QuestionType.YesOrNo)
                        {
                            // If valid answer was not provided...
                            if (!(answer.Text == "YES" || answer.Text == "NO"))
                            {
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please provide a valid answer for the question: {question.Question}");
                            }

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // If question type was drop down...
                        if (question.Type == QuestionType.DropDown)
                        {
                            // If other option was not enabled...
                            // And answer does not correspond to any pre specified option...
                            if (!question.OtherOptionEnabled && question.Choices.Any(x => x == answer.Text))
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please provide a valid answer for the question: {question.Question}");

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // If question type was multiple choice...
                        if (question.Type == QuestionType.MultipleChoice)
                        {
                            // If other option was not enabled...
                            // And answer does not correspond to any pre specified option...
                            if (!question.OtherOptionEnabled && answer.MultipleChioces.Any(x => !question.Choices.Any(y => x == y)))
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please provide a valid answer for the question: ({question.Question})." +
                                    $"One or more of your options are not valid.");

                            // On the other hand, if the number of selected options is greater than the specified maximum...
                            if (answer.MultipleChioces.Length > question.MaxChoiceAllowed)
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Specified options must not be be greater than {question.MaxChoiceAllowed} for the question: ({question.Question}).");

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // If question type was date...
                        if (question.Type == QuestionType.Date)
                        {
                            // If date was format is not valid...
                            if (!DateTime.TryParse(answer.Text, out DateTime date))
                                // Handle the error
                                return HandleErrorResult(ErrorTitles.BadRequest,
                                    StatusCodes.Status400BadRequest, $"Please specify the date in a valid format, such as 'yyyy-MM-dd'");

                            // Set the question type
                            answer.Type = question.Type;
                        }

                        // Add the answer to the collection
                        application.AdditionalAnswers.Add(answer);
                    }
                }

                // Set the other application details
                application.ProgramId = credentials.ProgramId;
                application.FirstName = credentials.FirstName;
                application.LastName = credentials.LastName;
                application.IdNumber = credentials.IdNumber;
                application.Email = credentials.Email;
                application.Phone = credentials.Phone;
                application.DateOfBirth = credentials.DateOfBirth;
                application.Gender = credentials.Gender;
                application.CurrentResidence = credentials.CurrentResidence;
                application.Nationality = credentials.Nationality;

                // Create the application
                await context.Applications.AddAsync(application);

                // Save changes
                await context.SaveChangesAsync();

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = ErrorTitles.SystemError,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        private OperationResult HandleErrorResult(string title, int statusCode, string errorMessage)
        {
            // Log the error
            logger.LogError(errorMessage);

            // Return result
            return new OperationResult
            {
                ErrorTitle = title,
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };
        }
    }
}
