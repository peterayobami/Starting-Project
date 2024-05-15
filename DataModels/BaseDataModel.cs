namespace Starting_Project
{
    /// <summary>
    /// The base data model for all entries
    /// </summary>
    public class BaseDataModel
    {
        /// <summary>
        /// The unique id for data entry
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The program id for program/application entry
        /// </summary>
        public string ProgramId { get; set; }

        /// <summary>
        /// The point in time data was created
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }

        /// <summary>
        /// The point in time data was modified
        /// </summary>
        public DateTimeOffset DateModified { get; set; }
    }
}
