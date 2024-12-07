using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Core.Infrastructure.HttpHandle
{
    public interface IApplicationResponse
    {
        string TraceId { get; }

        int StatusCode { get; }

        string TranslationKey { get; set; }

        object Response();
    }
}
