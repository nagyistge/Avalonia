// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace XamlTestApplication.ViewModels
{
    public abstract class TestItemBase
    {
        public virtual object Header { get; }
        public virtual string SubHeader { get; }
    }

    public class TestItem1 : TestItemBase
    {
        public TestItem1(string header, string subheader)
        {
            Header = header;
            SubHeader = subheader;
        }

        public new string Header { get; }
        public override string SubHeader { get; }
    }

    public class TestItem2 : TestItemBase
    {
        public TestItem2(string header, string subheader)
        {
            Header = header;
            SubHeader = subheader;
        }

        public override object Header { get; }
        public override string SubHeader { get; }
    }

}
