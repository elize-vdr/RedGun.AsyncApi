
using RedGun.AsyncApi.Services;

namespace RedGun.AsyncApi.Writers
{
    /// <summary>
    /// Indicates if and when the reader should convert references into complete object renderings
    /// </summary>
    public enum ReferenceInlineSetting
    {
        /// <summary>
        /// Create placeholder objects with an AsyncApiReference instance and UnresolvedReference set to true.
        /// </summary>
        DoNotInlineReferences,
        /// <summary>
        /// Convert local references to references of valid domain objects.
        /// </summary>
        InlineLocalReferences,
        /// <summary>
        /// Convert all references to references of valid domain objects.
        /// </summary>
        InlineAllReferences
    }

    /// <summary>
    /// Configuration settings to control how AsyncAPI documents are written
    /// </summary>
    public class AsyncApiWriterSettings
    {
        internal LoopDetector LoopDetector { get; } = new LoopDetector();
        /// <summary>
        /// Indicates how references in the source document should be handled.
        /// </summary>
        public ReferenceInlineSetting ReferenceInline { get; set; } = ReferenceInlineSetting.DoNotInlineReferences;
    }
}
