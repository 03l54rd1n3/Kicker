using System.Drawing;
using Snapshooter.Xunit;

namespace Kicker.ImageRecognition.Tests;

public class HomographyTests
{
    [Fact]
    public void CalculateTranslationMap_Snapshot_ReturnsCorrectPoints()
    {
        // Arrange
        const int size = 50;
        var topLeft = new Point(0, 0);
        var topRight = new Point(size -1, size / 5 - 1);
        var bottomRight = new Point(size - 1, size + size / 5 - 1);
        var bottomLeft = new Point(0, size -1);
        
        // Act
        using var homography = new Homography()
            .SetOriginFrame(topLeft, topRight, bottomRight, bottomLeft)
            .SetTargetFrame(size, size)
            .CalculateHomographyMatrix()! 
            .CalculateTranslationMap()!;

        // Assert
        var translationMap = new Point[size, size];
        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
            translationMap[y, x] = homography.Translate(x, y);
        
        Snapshot.Match(translationMap);
    }
}