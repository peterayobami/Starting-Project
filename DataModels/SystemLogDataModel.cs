namespace Starting_Project
{
    /// <summary>
    /// The system logs database table representational model
    /// </summary>
    public class SystemLogDataModel : BaseDataModel
    {
        /// <summary>
        /// The log level
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// The message of the log
        /// </summary>
        public string Message { get; set; }
    }
}