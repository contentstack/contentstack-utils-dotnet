using System;
using System.Collections.Generic;

namespace Contentstack.Utils.Interfaces
{
    public interface IEntryEmbedable
    {
        Dictionary<string, List<IEmbeddedContentTypeUid>> embeddedEntries
        {
            get;
            set;
        }

        Dictionary<string, List<IEmbeddedAsset>> embeddedAssets
        {
            get;
            set;
        }
    }
}
