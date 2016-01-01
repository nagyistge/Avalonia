// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Generic;

namespace XamlTestApplication.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Items = new List<TestItemBase>();

            for (int i = 0; i < 5; ++i)
            {
                Items.Add(new TestItem1($"Item1 {i}", $"Item1 {i} Value"));
            }

            for (int i = 0; i < 5; ++i)
            {
                Items.Add(new TestItem2($"Item2 {i}", $"Item2 {i} Value"));
            }

            Nodes = new List<TestNode>
            {
                new TestNode
                {
                    Header = "Root",
                    SubHeader = "Root Item",
                    Children = new[]
                    {
                        new TestNode
                        {
                            Header = "Child 1",
                            SubHeader = "Child 1 Value",
                        },
                        new TestNode
                        {
                            Header = "Child 2",
                            SubHeader = "Child 2 Value",
                            Children = new[]
                            {
                                new TestNode
                                {
                                    Header = "Grandchild",
                                    SubHeader = "Grandchild Value",
                                },
                                new TestNode
                                {
                                    Header = "Grandmaster Flash",
                                    SubHeader = "White Lines",
                                },
                            }
                        },
                    }
                }
            };
        }

        public List<TestItemBase> Items { get; }
        public List<TestNode> Nodes { get; }
    }
}
