using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Validation result.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Initialize validation result.
        /// </summary>
        public ValidationResult(bool isSuccess, string errorMessage = default, IList<string> errors = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;

            if (errors == null)
            {
                Errors = new ReadOnlyCollection<string>(new List<string>());
            }
            else
            {
                Errors = new ReadOnlyCollection<string>(errors);
            }
        }

        /// <summary>
        /// List of errors.
        /// </summary>
        public IReadOnlyCollection<string> Errors { get; }

        /// <summary>
        /// Status of validation.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Error message of validation.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Get the validation result in string format.
        /// </summary>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Status: {IsSuccess}");

            if (!IsSuccess)
            {
                stringBuilder.AppendLine($"Error message: {ErrorMessage}");

                foreach (var error in Errors)
                {
                    stringBuilder.AppendLine($" #: {error}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
