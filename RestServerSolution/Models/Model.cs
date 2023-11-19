using System;
using System.ComponentModel.DataAnnotations;

namespace RestServerSolution.Models
{
    public abstract class Model : IValidatableObject
    {
        /// <summary>
        /// Performs Validation
        /// </summary>
        public Result Validate()
        {
            var errors = new List<ValidationResult>();
            var ctx = new ValidationContext(this);
            var isValid = Validator.TryValidateObject(this, ctx, errors, true);
            return new Result(isValid, errors);
        }

        /// <summary>
        /// Sets up rules for Validation
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new ValidationResult[] { };
        }

        public class Result
        {
            public bool IsValid { get; }
            public IEnumerable<ValidationResult> Errors { get; }

            public Result(bool isValid, IEnumerable<ValidationResult> errors)
            {
                IsValid = isValid;
                Errors = errors;
            }
        }
    }
}

