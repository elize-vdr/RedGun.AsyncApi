using RedGun.AsyncApi.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedGun.AsyncApi
{
    internal static class AsyncAPIWriterExtensions
    {
        /// <summary>
        /// Temporary extension method until we add Settings property to IAsyncApiWriter in next major version
        /// </summary>
        /// <param name="asyncApiWriter"></param>
        /// <returns></returns>
        internal static AsyncApiWriterSettings GetSettings(this IAsyncApiWriter asyncApiWriter) 
        {
            if (asyncApiWriter is AsyncApiWriterBase)
            {
                return ((AsyncApiWriterBase)asyncApiWriter).Settings;
            }
            return new AsyncApiWriterSettings();
        }
    }
}
