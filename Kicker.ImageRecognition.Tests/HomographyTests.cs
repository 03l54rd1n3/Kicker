using Kicker.Shared;
using Snapshooter.Xunit;

namespace Kicker.ImageRecognition.Tests;

public class HomographyTests
{
    [Fact]
    public void CalculateTranslationMap_Snapshot_ReturnsCorrectPoints()
    {
        // Arrange
        const short size = 50;
        var topLeft = new PointS();
        var topRight = new PointS(size -1, size / 5 - 1);
        var bottomRight = new PointS(size - 1, size + size / 5 - 1);
        var bottomLeft = new PointS(0, size -1);

        // Act
        using var homography = new Homography()
            .SetOriginFrame(topLeft, topRight, bottomRight, bottomLeft)
            .SetTargetFrame(size, size)
            .CalculateHomographyMatrix()!
            .CalculateTranslationMap()!;

        // Assert
        var translationMap = new PointS[size, size];
        for (short y = 0; y < size; y++)
        for (short x = 0; x < size; x++)
            translationMap[y, x] = homography.Translate(x, y);

        Snapshot.Match(translationMap);
    }
}