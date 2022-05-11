using System.Buffers;
using System.Drawing;
using System.Numerics;
using HomographySharp;

namespace Kicker.ImageRecognition;

public class Homography : IDisposable
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private bool _originFrameSet;
    private bool _targetFrameSet;
    
    private Vector2 _originTopLeft;
    private Vector2 _originTopRight;
    private Vector2 _originBottomRight;
    private Vector2 _originBottomLeft;

    private HomographyMatrix<float>? _homographyMatrix;
    private Point[]? _translationMap;

    public Homography SetOriginFrame(
        Point topLeft,
        Point topRight,
        Point bottomRight,
        Point bottomLeft)
    {
        if (_originFrameSet)
            throw new InvalidOperationException("Already initialized");
        
        _originTopLeft = new Vector2(topLeft.X, topLeft.Y);
        _originTopRight = new Vector2(topRight.X, topRight.Y);
        _originBottomRight = new Vector2(bottomRight.X, bottomRight.Y);
        _originBottomLeft = new Vector2(bottomLeft.X, bottomLeft.Y);

        _originFrameSet = true;

        return this;
    }

    public Homography SetTargetFrame(
        int width,
        int height)
    {
        if (_targetFrameSet)
            throw new InvalidOperationException("Already initialized");
        
        Width = width;
        Height = height;
        
        _targetFrameSet = true;

        return this;
    }

    public Homography? CalculateHomographyMatrix()
    {
        if (!_originFrameSet || !_targetFrameSet)
            throw new InvalidOperationException("Origin or target frame not set");

        if (_homographyMatrix is not null)
            throw new InvalidOperationException("Already initialized");

        var arrayPool = ArrayPool<Vector2>.Shared;
        var points = arrayPool.Rent(8);
        points[0] = _originTopLeft;
        points[1] = _originTopRight;
        points[2] = _originBottomRight;
        points[3] = _originBottomLeft;
        
        points[4] = new Vector2(0, 0);
        points[5] = new Vector2(Width - 1, 0);
        points[6] = new Vector2(Width - 1, Height - 1);
        points[7] = new Vector2(0, Height - 1);

        _homographyMatrix = HomographySharp.Homography.Find(points.AsSpan(4..8), points.AsSpan(..4)); // Inverse
        
        arrayPool.Return(points);
        return this;
    }

    public Homography? CalculateTranslationMap()
    {
        if (_homographyMatrix is null)
            throw new InvalidOperationException($"{nameof(_homographyMatrix)} not calculated");
        
        if (_translationMap is not null)
            throw new InvalidOperationException("Already initialized");

        var arrayPool = ArrayPool<Point>.Shared;
        _translationMap = arrayPool.Rent(Height * Width);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var point2 = _homographyMatrix.Translate(x, y);
                var pointX = (int)Math.Round(point2.X);
                var pointY = (int)Math.Round(point2.Y);
                _translationMap[(y * Width) + x] = new Point(pointX, pointY);
            }
        }

        return this;
    }

    public Point Translate(
        int x,
        int y)
    {
        if (_translationMap is null)
            throw new InvalidOperationException("Not initialized");

        return _translationMap[(y * Width) + x];
    }

    public void Dispose()
    {
        if (_homographyMatrix is not null)
            _homographyMatrix = null;
        
        if (_translationMap is not null)
            ArrayPool<Point>.Shared.Return(_translationMap);
    }
}