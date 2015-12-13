using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Perspex.Android.Platform.Specific;
using Perspex.Android.Platform.Specific.Helpers;
using Perspex.Input;
using Perspex.Input.Raw;
using Perspex.Platform;
using Perspex.Skia.Android;
using System;
using Perspex.Controls;

namespace Perspex.Android.Platform.SkiaPlatform
{
    public class WindowImpl : SkiaView, IAndroidView, IWindowImpl, ISurfaceHolderCallback
    {
        protected AndroidKeyboardEventsHelper<WindowImpl> _keyboardHelper;

        private AndroidTouchEventsHelper<WindowImpl> _touchHelper;

        public WindowImpl(Context context) : base((Activity)context)
        {
            _keyboardHelper = new AndroidKeyboardEventsHelper<WindowImpl>(this);
            _touchHelper = new AndroidTouchEventsHelper<WindowImpl>(this, () => InputRoot, p => GetPerspexPointFromEvent(p));

            MaxClientSize = new Size(Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
            ClientSize = MaxClientSize;
            Init();
        }

        public WindowImpl() : this(PerspexLocator.Current.GetService<IAndroidActivity>().Activity)
        {
        }

        void ISurfaceHolderCallback.SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {
            var newSize = new Size(width, height);
            if (newSize != ClientSize)
            {
                MaxClientSize = newSize;
                ClientSize = newSize;
                Resized?.Invoke(ClientSize);
            }

            base.SurfaceChanged(holder, format, width, height);
        }

        protected virtual void Init()
        {
        }

        private bool _handleEvents;

        public bool HandleEvents
        {
            get { return _handleEvents; }
            set
            {
                _handleEvents = value;
                _keyboardHelper.HandleEvents = _handleEvents;
            }
        }

        public virtual Point GetPerspexPointFromEvent(MotionEvent e) => new Point(e.GetX(), e.GetY());

        public IInputRoot InputRoot { get; private set; }

        public Size ClientSize { get; set; }

        public Action Closed { get; set; }

        public Action Deactivated { get; set; }

        public Action<RawInputEventArgs> Input { get; set; }

        public Size MaxClientSize { get; private set; }

        public Action<Rect> Paint { get; set; }

        public Action<Size> Resized { get; set; }

        public View View => this;

        Action ITopLevelImpl.Activated { get; set; }

        IPlatformHandle ITopLevelImpl.Handle => this;

        public void Activate()
        {
        }

        public void Hide()
        {
            this.Visibility = ViewStates.Invisible;
        }

        public void SetSystemDecorations(bool enabled)
        {
        }
        public void Invalidate(Rect rect)
        {
            if (Holder?.Surface?.IsValid == true) base.Invalidate();
        }

        public Point PointToScreen(Point point)
        {
            return point;
        }

        public void SetCursor(IPlatformHandle cursor)
        {
            //still not implemented
        }

        public void SetInputRoot(IInputRoot inputRoot)
        {
            InputRoot = inputRoot;
        }

        public void SetTitle(string title)
        {
        }

        public void Show()
        {
            this.Visibility = ViewStates.Visible;
        }

        public void BeginMoveDrag()
        {
            //Not supported
        }

        public void BeginResizeDrag(WindowEdge edge)
        {
            //Not supported
        }

        public Point Position { get; set; }

        public IDisposable ShowDialog()
        {
            throw new NotImplementedException();
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            bool callBase;
            bool? result = _touchHelper.DispatchTouchEvent(e, out callBase);
            bool baseResult = callBase ? base.DispatchTouchEvent(e) : false;

            return result != null ? result.Value : baseResult;
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            bool callBase;
            bool? res = _keyboardHelper.DispatchKeyEvent(e, out callBase);
            bool baseResult = callBase ? base.DispatchKeyEvent(e) : false;

            return res != null ? res.Value : baseResult;
        }
        
        protected override void Draw()
        {
            Paint?.Invoke(new Rect(new Point(0, 0), ClientSize));
        }
    }
}