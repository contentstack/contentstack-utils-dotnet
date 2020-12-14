using System;
using System.Collections.Generic;

namespace Contentstack.Utils.Interfaces
{
    public interface IEntryEmbedable
    {
        Dictionary<string, List<IEmbeddedObject>> embeddedItems
        {
            get;
            set;
        }
    }
}
