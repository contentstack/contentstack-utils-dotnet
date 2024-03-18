using System;
using System.Collections.Generic;

namespace Contentstack.Utils.Interfaces
{
    public interface IEmbeddedObject
    {
        string Uid
        {
            get;
            set;
        }
        string ContentTypeUid
        {
            get;
            set;
        }
    }

    public interface IEmbeddedEntry: IEmbeddedObject
    {
        string Title
        {
            get;
            set;
        }

        Dictionary<string, object> Properties
        {
            get;
            set;
        }
    }

    public interface IEmbeddedAsset: IEmbeddedObject
    {
        string Title
        {
            get;
            set;
        }

        string FileName
        {
            get;
            set;
        }

        string Url
        {
            get;
            set;
        }
    }
}
