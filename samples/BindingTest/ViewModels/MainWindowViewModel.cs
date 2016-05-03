using System;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using System.Reactive.Linq;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace BindingTest.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ImmutableArray<TestItem> _items;
        private string _booleanString = "True";
        private double _doubleValue = 5.0;
        private string _stringValue = "Simple Binding";
        private bool _booleanFlag = false;

        public MainWindowViewModel()
        {
            Items = new List<TestItem>(
                Enumerable.Range(0, 20).Select(x => new TestItem
                {
                    StringValue = "Item " + x,
                    Detail = "Item " + x + " details",
                })).ToImmutableArray();

            SelectedItems = new ObservableCollection<TestItem>();

            ShuffleItems = ReactiveCommand.Create();
            ShuffleItems.Subscribe(_ =>
            {
                //var r = new Random();
                //Items.Move(r.Next(Items.Count), 1);
            });

            StringValueCommand = ReactiveCommand.Create();
            StringValueCommand.Subscribe(param =>
            {
                BooleanFlag = !BooleanFlag;
                StringValue = param.ToString();
            });

            AddItemCommand = ReactiveCommand.Create();
            AddItemCommand.Subscribe(param =>
            {
                Items = Items.Add(new TestItem { StringValue = "Item", Detail = "Detail" });
            });
        }

        public ImmutableArray<TestItem> Items
        {
            get { return _items; }
            set { this.RaiseAndSetIfChanged(ref _items, value); }
        }

        public ObservableCollection<TestItem> SelectedItems { get; }
        public ReactiveCommand<object> ShuffleItems { get; }

        public string BooleanString
        {
            get { return _booleanString; }
            set { this.RaiseAndSetIfChanged(ref _booleanString, value); }
        }

        public double DoubleValue
        {
            get { return _doubleValue; }
            set { this.RaiseAndSetIfChanged(ref _doubleValue, value); }
        }

        public string StringValue
        {
            get { return _stringValue; }
            set { this.RaiseAndSetIfChanged(ref _stringValue, value); }
        }

        public bool BooleanFlag
        {
            get { return _booleanFlag; }
            set { this.RaiseAndSetIfChanged(ref _booleanFlag, value); }
        }

        public ReactiveCommand<object> StringValueCommand { get; }
        public ReactiveCommand<object> AddItemCommand { get; }
    }
}
