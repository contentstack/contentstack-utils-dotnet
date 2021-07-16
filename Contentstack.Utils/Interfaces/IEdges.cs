using System;
using Newtonsoft.Json;

namespace Contentstack.Utils.Interfaces
{
    public class IEdges<T> where T: IEmbeddedObject
    {
        [JsonProperty("node")]
        public T Node
        {
            get;
            set;
        }
    }
}


