namespace RedShot.Infrastructure.Uploading.Common
{
    /// <summary>
    /// Upload result.
    /// </summary>
    public class UploadResult
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resultType">Result type.</param>
        public UploadResult(UploadResultType resultType)
        {
            ResultType = resultType;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resultType">Result type.</param>
        /// <param name="errorMessage">Error message.</param>
        public UploadResult(UploadResultType resultType, string errorMessage)
        {
            ResultType = resultType;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Get successful upload result.
        /// </summary>
        public static UploadResult Successful => new UploadResult(UploadResultType.Successful);

        /// <summary>
        /// Get canceled upload result.
        /// </summary>
        public static UploadResult Canceled => new UploadResult(UploadResultType.Canceled);

        /// <summary>
        /// Get upload result with error.
        /// </summary>
        public static UploadResult Error(string message) => new UploadResult(UploadResultType.Error, message);

        /// <summary>
        /// ResultType.
        /// </summary>
        public UploadResultType ResultType { get; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string ErrorMessage { get; }
    }
}
