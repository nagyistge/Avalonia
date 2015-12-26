using Perspex;
using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace XamlTestApplication.Views
{
    public class UserControl1 : UserControl
    {
        public UserControl1()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var s = base.MeasureOverride(availableSize);
            sw.Stop();
            System.Diagnostics.Debug.WriteLine("UserControl1 MeasureOverride " + sw.Elapsed.TotalMilliseconds + "ms");
            return s;
        }
    }
}
