// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Attributes;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// The reference type.
    /// </summary>
    public enum ReferenceType
    {
        /// <summary>
        /// Schema item.
        /// </summary>
        [Display("schemas")] Schema,

        /// <summary>
        /// Message item.
        /// </summary>
        [Display("messages")] Message,

        /// <summary>
        /// SecurityScheme item.
        /// </summary>
        [Display("securitySchemes")] SecurityScheme,

        /// <summary>
        /// Parameter item.
        /// </summary>
        [Display("parameters")] Parameter,

        /// <summary>
        /// CorrelationId item.
        /// </summary>
        [Display("correlationIds")] CorrelationId,

        /// <summary>
        /// OperationTrait item.
        /// </summary>
        [Display("operationTraits")] OperationTrait,

        /// <summary>
        /// MessageTrait item.
        /// </summary>
        [Display("messageTraits")] MessageTrait,

        /// <summary>
        /// ServerBindings item.
        /// </summary>
        [Display("serverBindings")] ServerBindings,

        /// <summary>
        /// ChannelBindings item.
        /// </summary>
        [Display("channelBindings")] ChannelBindings,
        
        /// <summary>
        /// OperationBindings item.
        /// </summary>
        [Display("operationBindings")] OperationBindings,
        
        /// <summary>
        /// MessageBindings item.
        /// </summary>
        [Display("messageBindings")] MessageBindings,

        /// <summary>
        /// Tag item.
        /// </summary>
        [Display("tags")] Tag
    }
}
