using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Starting_Project.Controllers
{
    /// <summary>
    /// Manages standard Web API for retrieving and submitting applications
    /// </summary>
    public class ApplicationController : ControllerBase
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationService"/>
        /// </summary>
        private readonly ApplicationService applicationService;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<ApplicationController> logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationController(ApplicationService applicationService, ILogger<ApplicationController> logger)
        {
            this.applicationService = applicationService;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Endpoint to submit application
        /// </summary>
        /// <param name="credentials">The application credentials</param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.SubmitApplication)]
        public async Task<ActionResult> SubmitApplicationAsync([FromBody] ApplicationCredentials credentials)
        {
            try
            {
                // Trigger the operation
                var operation = await applicationService.SubmitAsync(credentials);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: ErrorTitles.SystemError,
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }
    }
}
