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
        /// Represents the success of the validation.
        /// </summary>
        public static ValidationResult Success => new ValidationResult();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="errors">Errors.</param>
        public ValidationResult(string errorMessage, IList<string> errors = null)
        {
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
        /// Constructor for a successful validation result.
        /// </summary>
        protected ValidationResult()
        {
            IsSuccess = true;
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
