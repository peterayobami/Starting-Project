namespace Starting_Project
{
    /// <summary>
    /// Manages program transactions
    /// </summary>
    public class ProgramService
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger"/>
        /// </summary>
        private readonly ILogger<ProgramService> logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProgramService(ApplicationDbContext context, ILogger<ProgramService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Retrieves program for review/application
        /// </summary>
        /// <param name="id">The id of programm to retrieve</param>
        /// <returns></returns>
        public async Task<OperationResult<ProgramApiModel>> FetchAsync(string id)
        {
            try
            {
                // If id was not specified...
                if (string.IsNullOrEmpty(id))
                {
                    // Log error
                    logger.LogError("Please specify the program id");

                    // Return error result
                    return new OperationResult<ProgramApiModel>
                    {
                        ErrorTitle = ErrorTitles.BadRequest,
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Please specify the program id"
                    };
                }

                // Fetch the program
                var program = await context.Programs.FindAsync(id, id);

                // If program could not be found...
                if (program == null)
                {
                    // Log error
                    logger.LogError("Program with specified id could not be found");

                    // Return error result
                    return new OperationResult<ProgramApiModel>
                    {
                        ErrorTitle = ErrorTitles.NotFound,
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Program with specified id could not be specified"
                    };
                }

                // Return response
                return new OperationResult<ProgramApiModel>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Result = new ProgramApiModel
                    {
                        Title = program.Title,
                        Description = program.Description,
                        HidePhone = program.HidePhone,
                        HideNationality = program.HideNationality,
                        HideCurrentResidence = program.HideCurrentResidence,
                        HideIdNumber = program.HideIdNumber,
                        HideDateOfBirth = program.HideDateOfBirth,
                        HideGender = program.HideGender,
                        PersonalQuestions = program.PersonalQuestions,
                        CustomQuestions = program.CustomQuestions
                    }
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error result
                return new OperationResult<ProgramApiModel>
                {
                    ErrorTitle = ErrorTitles.SystemError,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Creates a program with specified credentials
        /// </summary>
        /// <param name="credentials">The program credentials</param>
        /// <returns></returns>
        public async Task<OperationResult> CreateAsync(ProgramCredentials credentials)
        {
            try
            {
                // Set the program id
                string programId = Guid.NewGuid().ToString();

                // If personal questions were specified...
                if (credentials.PersonalQuestions.Count > 0)
                {
                    // For each of them...
                    for (int i = 0; i < credentials.PersonalQuestions.Count; i++)
                    {
                        // Get the question
                        var question = credentials.PersonalQuestions[i];

                        // If type is not valid...
                        if (!(question.Type == QuestionType.Paragraph || 
                            question.Type == QuestionType.YesOrNo ||
                            question.Type == QuestionType.DropDown || 
                            question.Type == QuestionType.MultipleChoice ||
                            question.Type == QuestionType.Number ||
                            question.Type == QuestionType.Date))
                        {
                            // Log error
                            logger.LogError($"Specified question type for question: ({question.Question}), is not valid");

                            // Return error result
                            return new OperationResult
                            {
                                ErrorTitle = ErrorTitles.BadRequest,
                                StatusCode = StatusCodes.Status400BadRequest,
                                ErrorMessage = $"Specified question type for question: ({question.Question}), is not valid"
                            };
                        }

                        if (question.Type == QuestionType.MultipleChoice && question.MaxChoiceAllowed < 2)
                        {
                            // Log error
                            logger.LogError($"Please specify allowed maximum choices for the question: ({question.Question})");

                            // Return error result
                            return new OperationResult
                            {
                                ErrorTitle = ErrorTitles.BadRequest,
                                StatusCode = StatusCodes.Status400BadRequest,
                                ErrorMessage = $"Please specify allowed maximum choices for the question: ({question.Question})"
                            };
                        }
                    }
                }

                // If additional questions were specified...
                if (credentials.CustomQuestions.Count > 0)
                {
                    // For each of them...
                    for (int i = 0; i < credentials.CustomQuestions.Count; i++)
                    {
                        // Get the question
                        var question = credentials.CustomQuestions[i];

                        // If type is not valid...
                        if (!(question.Type == QuestionType.Paragraph ||
                            question.Type == QuestionType.YesOrNo ||
                            question.Type == QuestionType.DropDown ||
                            question.Type == QuestionType.MultipleChoice ||
                            question.Type == QuestionType.Number ||
                            question.Type == QuestionType.Date))
                        {
                            // Log error
                            logger.LogError($"Specified question type for question: ({question.Question}), is not valid");

                            // Return result
                            return new OperationResult
                            {
                                ErrorTitle = ErrorTitles.BadRequest,
                                StatusCode = StatusCodes.Status400BadRequest,
                                ErrorMessage = $"Specified question type for question: ({question.Question}), is not valid"
                            };
                        }
                    }
                }

                // Set the program credentials
                var program = new ProgramDataModel
                {
                    Id = programId,
                    ProgramId = programId,
                    Title = credentials.Title,
                    Description = credentials.Description,
                    HidePhone = credentials.HidePhone,
                    HideNationality = credentials.HideNationality,
                    HideCurrentResidence = credentials.HideCurrentResidence,
                    HideIdNumber = credentials.HideIdNumber,
                    HideDateOfBirth = credentials.HideDateOfBirth,
                    HideGender = credentials.HideGender,
                    PersonalQuestions = credentials.PersonalQuestions,
                    CustomQuestions = credentials.CustomQuestions
                };

                // Create the program
                await context.Programs.AddAsync(program);

                // Save changes
                await context.SaveChangesAsync();

                // Return the result
                return new OperationResult { StatusCode = StatusCodes.Status201Created };
            }
            catch (Exception ex)
            {
                // Log the error
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

        /// <summary>
        /// Updates program with specified id
        /// </summary>
        /// <param name="credentials">The new program's credentials</param>
        /// <param name="programId">The id of program to update</param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateAsync(UpdateProgramCredentials credentials, string programId)
        {
            try
            {
                // Retrieve the program
                var program = await context.Programs.FindAsync(programId, programId);

                // If program was not found...
                if (program == null)
                {
                    // Log error
                    logger.LogError("Specified program does not exist");

                    // Return result
                    return new OperationResult
                    {
                        ErrorTitle = ErrorTitles.NotFound,
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Program with specified id could not be found"
                    };
                }

                // NB: Retain existing value, if client specifies no value
                program.Title = credentials.Title ?? program.Title;
                program.Description = credentials.Description ?? program.Description;

                // NB: If client had not to modify these boolean flags, the default value is zero. i.e. 'Unspecified'
                // Therefore, the they retain their existing value, whether true or false.
                program.HidePhone = credentials.PhoneFieldStatus == FieldStatus.Unspecified ? program.HidePhone : credentials.PhoneFieldStatus == FieldStatus.Enabled;
                program.HideNationality = credentials.NationalityFieldStatus == FieldStatus.Unspecified ? program.HidePhone : credentials.PhoneFieldStatus == FieldStatus.Enabled;
                program.HideCurrentResidence = credentials.CurrentResidenceFieldStatus == FieldStatus.Unspecified ? program.HidePhone : credentials.PhoneFieldStatus == FieldStatus.Enabled;
                program.HideIdNumber = credentials.IdNumberFieldStatus == FieldStatus.Unspecified ? program.HidePhone : credentials.PhoneFieldStatus == FieldStatus.Enabled;
                program.HideDateOfBirth = credentials.DateOfBirthFieldStatus == FieldStatus.Unspecified ? program.HidePhone : credentials.PhoneFieldStatus == FieldStatus.Enabled;
                program.HideGender = credentials.GenderFieldStatus == FieldStatus.Unspecified ? program.HidePhone : credentials.PhoneFieldStatus == FieldStatus.Enabled;

                // If custom questions were modified...
                if (credentials.CustomQuestions != null && credentials.CustomQuestions.Count != 0)
                {
                    // For each custom question...
                    for (int i = 0; i < credentials.CustomQuestions.Count; i++)
                    {
                        // Get the question
                        var question = credentials.CustomQuestions[i];

                        // Get the corresponding existing question
                        var existingQuestion = program.CustomQuestions[i];

                        // If we found a corresponding existing question...
                        if (existingQuestion != null)
                        {
                            // Update the question

                            existingQuestion.Question = question.Question ?? existingQuestion.Question;
                            existingQuestion.Type = question.Type ?? existingQuestion.Type;
                            existingQuestion.Choices = existingQuestion.Choices.Union(question.Choices).ToArray();
                            existingQuestion.MaxChoiceAllowed = question.MaxChoiceAllowed != 0 ? question.MaxChoiceAllowed : 0;
                            existingQuestion.OtherOptionEnabled = question.OtherOptionStatus == FieldStatus.Unspecified ? existingQuestion.OtherOptionEnabled : question.OtherOptionStatus == FieldStatus.Enabled;

                            program.CustomQuestions[i] = existingQuestion;
                        }
                        // Otherwise...
                        else
                        {
                            // Create new question
                            program.CustomQuestions.Add(new CustomQuestion
                            {
                                Question = question.Question,
                                Type = question.Type,
                                Choices = question.Choices,
                                MaxChoiceAllowed = question.MaxChoiceAllowed,
                                OtherOptionEnabled = question.OtherOptionStatus == FieldStatus.Enabled
                            });
                        }
                    }
                }
                
                // If personal questions was modified...
                if (credentials.PersonalQuestions != null && credentials.PersonalQuestions.Count != 0)
                {
                    // For each personal question...
                    for (int i = 0; i < credentials.PersonalQuestions.Count; i++)
                    {
                        // Get the question
                        var question = credentials.PersonalQuestions[i];

                        // Get the corresponding existing question
                        var existingQuestion = program.PersonalQuestions[i];

                        // If we found a corresponding existing question...
                        if (existingQuestion != null)
                        {
                            // Update the question

                            existingQuestion.Question = question.Question ?? existingQuestion.Question;
                            existingQuestion.Type = question.Type ?? existingQuestion.Type;
                            existingQuestion.Choices = existingQuestion.Choices.Union(question.Choices).ToArray();
                            existingQuestion.MaxChoiceAllowed = question.MaxChoiceAllowed != 0 ? question.MaxChoiceAllowed : 0;
                            existingQuestion.OtherOptionEnabled = question.OtherOptionStatus == FieldStatus.Unspecified ? existingQuestion.OtherOptionEnabled : question.OtherOptionStatus == FieldStatus.Enabled;

                            program.CustomQuestions[i] = existingQuestion;
                        }
                        // Otherwise...
                        else
                        {
                            // Create new question
                            program.PersonalQuestions.Add(new CustomQuestion
                            {
                                Question = question.Question,
                                Type = question.Type,
                                Choices = question.Choices,
                                MaxChoiceAllowed = question.MaxChoiceAllowed,
                                OtherOptionEnabled = question.OtherOptionStatus == FieldStatus.Enabled
                            });
                        }
                    }
                }

                // Update program
                context.Programs.Update(program);

                // Save changes
                await context.SaveChangesAsync();

                // Return the result
                return new OperationResult { StatusCode = StatusCodes.Status204NoContent };
            }
            catch (Exception ex)
            {
                // Log the error
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
    }
}