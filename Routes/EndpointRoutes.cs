namespace Starting_Project
{
    /// <summary>
    /// The endpoint routes for the application
    /// </summary>
    public class EndpointRoutes
    {
        /// <summary>
        /// The route to the CreateProgram API endpoint
        /// </summary>
        public const string CreateProgram = "program/create";

        /// <summary>
        /// The route to the UpdateProgram API endpoint
        /// </summary>
        public const string UpdateProgram = "program/update/{programId}";

        /// <summary>
        /// The routes to the FetchProgram API endpoint
        /// </summary>
        public const string FetchProgram = "program/fetch/{programId}";

        /// <summary>
        /// The routes to the SubmitApplication API endpoint
        /// </summary>
        public const string SubmitApplication = "application/submit";
    }
}