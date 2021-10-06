// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Validations
{
    /// <summary>
    /// Constrained interface used to provide context to rule implementation
    /// </summary>
    public interface IValidationContext
    {
        /// <summary>
        /// Register an error with the validation context.
        /// </summary>
        /// <param name="error">Error to register.</param>
        void AddError(AsyncApiValidatorError error);

        /// <summary>
        /// Allow Rule to indicate validation error occured at a deeper context level.  
        /// </summary>
        /// <param name="segment">Identifier for context</param>
        void Enter(string segment);

        /// <summary>
        /// Exit from path context elevel.  Enter and Exit calls should be matched.
        /// </summary>
        void Exit();

        /// <summary>
        /// Pointer to source of validation error in document
        /// </summary>
        string PathString { get; }

    }
}
