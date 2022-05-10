using System.Drawing;
using System.Numerics;
using HomographySharp;

namespace Kicker.ImageRecognition;

public class Homography
{
    public short Width { get; private set; }
    public short Height { get; private set; }

    private bool _originFrameSet;
    private bool _targetFrameSet;
    
    private Vector2 _originTopLeft;
    private Vector2 _originTopRight;
    private Vector2 _originBottomRight;
    private Vector2 _originBottomLeft;

    private HomographyMatrix<float>? _homographyMatrix;
    private Point[,]? _translationMap;

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
        short width,
        short height)
    {
        if (_targetFrameSet)
            throw new InvalidOperationException("Already initialized");
        
        Width = width;
        Height = height;
        
        _targetFrameSet = true;

        return this;
    }

    public Homography CalculateHomographyMatrix()
    {
        if (!_originFrameSet || !_targetFrameSet)
            throw new InvalidOperationException("Origin or target frame not set");
        
        if (_homographyMatrix is not null)
            throw new InvalidOperationException("Already initialized");

        var originList = new List<Vector2> { _originTopLeft, _originTopRight, _originBottomRight, _originBottomLeft };
        var targetList = new List<Vector2> { new(0, 0), new(Width - 1, 0), new(Width - 1, Height - 1), new(0, Height - 1) };

         _homographyMatrix = HomographySharp.Homography.Find(targetList, originList); // Inverse

         return this;
    }

    public Homography CalculateTranslationMap()
    {
        if (_homographyMatrix is null)
            throw new InvalidOperationException($"{nameof(_homographyMatrix)} not calculated");
        
        if (_translationMap is not null)
            throw new InvalidOperationException("Already initialized");

        var translationMap = new Point[Height, Width];

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var point2 = _homographyMatrix.Translate(x, y);
                var pointX = (int)Math.Round(point2.X);
                var pointY = (int)Math.Round(point2.Y);
                translationMap[y, x] = new Point(pointX, pointY);
            }
        }

        _translationMap = translationMap;

        return this;
    }

    public Point Translate(
        int x,
        int y)
    {
        if (_translationMap is null)
            throw new InvalidOperationException("Not initialized");

        return _translationMap[y, x];
    }
    
}