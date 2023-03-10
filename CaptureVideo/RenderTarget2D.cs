using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;

namespace CaptureVideo
{
    public class RenderTarget2D : IDisposable
    {
        SharpDX.Direct3D11.Device _drawDevice;
        SwapChain _swapChain;
        Texture2D _backBuffer;
        RenderTargetView _backBufferView;
        RenderTarget _renderTarget2D;
        SharpDX.Mathematics.Interop.RawViewportF _viewPort;

        int _width;
        public int Width
        {
            get { return _width; }
        }

        int _height;
        public int Height
        {
            get { return _height; }
        }

        public void Initialize(IntPtr windowHandle, int width, int height)
        {
            _width = width;
            _height = height;

            var desc = new SwapChainDescription()
            {
                BufferCount = 1,

                ModeDescription = new ModeDescription(width, height,
                                         new Rational(60, 1), Format.B8G8R8A8_UNorm),


                IsWindowed = true,
                OutputHandle = windowHandle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput,
            };

            _viewPort = new SharpDX.Mathematics.Interop.RawViewportF
            {
                X = 0,
                Y = 0,
                Width = width,
                Height = height,
            };

            SharpDX.Direct3D11.Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware,
                 SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport,
                 new[] { SharpDX.Direct3D.FeatureLevel.Level_10_0 }, desc, out _drawDevice, out _swapChain);

            _backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);
            _backBufferView = new RenderTargetView(_drawDevice, _backBuffer);

            using (SharpDX.Direct2D1.Factory factory = new SharpDX.Direct2D1.Factory())
            {
                using (var surface = _backBuffer.QueryInterface<Surface>())
                {
                    _renderTarget2D = new RenderTarget(factory, surface,
                        new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied)));
                    _renderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
                }
            }
        }

        public void Dispose()
        {
            ReleaseResource();
        }

        void ReleaseResource()
        {
            if (_renderTarget2D != null)
            {
                _renderTarget2D.Dispose();
                _renderTarget2D = null;
            }

            if (_backBufferView != null)
            {
                _backBufferView.Dispose();
                _backBufferView = null;
            }

            if (_backBuffer != null)
            {
                _backBuffer.Dispose();
                _backBuffer = null;
            }

            if (_swapChain != null)
            {
                _swapChain.Dispose();
                _swapChain = null;
            }

            if (_drawDevice != null)
            {
                _drawDevice.Dispose();
                _drawDevice = null;
            }
        }

        public void Render(Action<RenderTarget> render)
        {
            if (_drawDevice == null)
            {
                return;
            }

            _drawDevice.ImmediateContext.Rasterizer.SetViewport(_viewPort);
            _drawDevice.ImmediateContext.OutputMerger.SetTargets(_backBufferView);

            _renderTarget2D.BeginDraw();

            render(_renderTarget2D);

            _renderTarget2D.EndDraw();

            _swapChain.Present(0, PresentFlags.None);
        }

        public Bitmap CreateBitmap(DataStream stream)
        {
            var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.B8G8R8A8_UNorm,
                    SharpDX.Direct2D1.AlphaMode.Ignore));

            if (_renderTarget2D == null)
            {
                return null;
            }

            return new SharpDX.Direct2D1.Bitmap(_renderTarget2D, new Size2(_width, _height), stream,
                        _width * sizeof(int), bitmapProperties);
        }

        public Bitmap CreateBitmap(DataPointer dataPointer)
        {
            var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.B8G8R8A8_UNorm,
                    SharpDX.Direct2D1.AlphaMode.Ignore));

            if (_renderTarget2D == null)
            {
                return null;
            }

            return new SharpDX.Direct2D1.Bitmap(_renderTarget2D, new Size2(_width, _height), dataPointer, _width * sizeof(int), bitmapProperties);
        }
    }
}