using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Starting_Project
{
    /// <summary>
    /// Manages standard Web API for Program operations
    /// </summary>
    public class ProgramController : ControllerBase
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ProgramService"/>
        /// </summary>
        private readonly ProgramService programService;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger"/>
        /// </summary>
        private readonly ILogger<ProgramController> logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="programService">The injected <see cref="ProgramService"/></param>
        /// <param name="logger">The injected logger</param>
        public ProgramController(ProgramService programService, ILogger<ProgramController> logger)
        {
            this.programService = programService;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Retrieves program with specified id
        /// </summary>
        /// <param name="programId">The specified program id</param>
        /// <returns></returns>
        [HttpGet(EndpointRoutes.FetchProgram)]
        public async Task<ActionResult> FetchProgramAsync(string programId)
        {
            try
            {
                // Trigger the operation
                var operation = await programService.FetchAsync(programId);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Ok(operation.Result);
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

        /// <summary>
        /// Manages the request to create program with specified credentials
        /// </summary>
        /// <param name="credentials">The specified credentials</param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateProgram)]
        public async Task<ActionResult> CreateProgramAsync([FromBody] ProgramCredentials credentials)
        {
            try
            {
                // Trigger the operation
                var operation = await programService.CreateAsync(credentials);

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
                // Log the error
                logger.LogError(ex.Message);

                // Return response
                return Problem(title: ErrorTitles.SystemError,
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Updates existing program with specified credentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        [HttpPut(EndpointRoutes.UpdateProgram)]
        public async Task<ActionResult> UpdateProgramAsync([FromBody] UpdateProgramCredentials credentials, string programId)
        {
            try
            {
                // Trigger the operation
                var operation = await programService.UpdateAsync(credentials, programId);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return NoContent();
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