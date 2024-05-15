using Swashbuckle.AspNetCore.Filters;

namespace Starting_Project
{
    /// <summary>
    /// Update program credential examples
    /// </summary>
    public class UpdateProgramCredentialsModelExample : IExamplesProvider<UpdateProgramCredentials>
    {
        /// <summary>
        /// Gets the sample value for object properties
        /// </summary>
        /// <returns></returns>
        public UpdateProgramCredentials GetExamples()
        {
            return new UpdateProgramCredentials
            {
                Description = "The purpose of this program is to help you get through the process of migration.",
            };
        }
    }
}
