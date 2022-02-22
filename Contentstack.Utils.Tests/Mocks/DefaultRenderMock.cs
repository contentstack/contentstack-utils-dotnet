using System;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;

namespace Contentstack.Utils.Tests.Mocks
{
    public class DefaultRenderMock : Options
    {
        public DefaultRenderMock() : base()
        {
        }

        public DefaultRenderMock(IEntryEmbedable entry) : base(entry)
        {
        }
        public override string RenderOption(IEmbeddedObject entry, Metadata metadata)
        {
            return base.RenderOption(entry, metadata);
        }
    }
}
