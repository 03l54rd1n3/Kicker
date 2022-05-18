/*
using System.Drawing;

namespace Kicker.ImageRecognition.Tests.ImageTransformation;

public class BitmapTests
{
    [Fact]
    public void ToBitmap_ClassicImplementation_EqualsOriginal()
    {
        const int size = 100;
        // Arrange
        var expected = GetRandomBitmap(size);

        // Act
        using var actual = expected
            .AsImageTransformation()
            .AsMemorized()
            .ToBitmap();

        // Assert
        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
            Assert.Equal(expected.GetPixel(x, y), actual.GetPixel(x, y));
    }

    [Fact]
    public void ToBitmap_FastImplementation_EqualsOriginal()
    {
        const int size = 100;
        // Arrange
        var expected = GetRandomBitmap(size);

        // Act
        using var actual = expected
            .AsImageTransformation()
            .AsFastMemorized()
            .ToBitmap();

        // Assert
        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
            Assert.Equal(expected.GetPixel(x, y), actual.GetPixel(x, y));
    }

    private Bitmap GetRandomBitmap(
        int size)
    {
        var random = new Random();
        var buffer = new byte[3];
        var bitmap = new Bitmap(size, size);

        for (var y = 0; y < size; y++)
        {
            for (var x = 0; x < size; x++)
            {
                random.NextBytes(buffer);
                var red = byte.Min(buffer[0], 254);
                var green = byte.Min(buffer[1], 254);
                var blue = byte.Min(buffer[2], 254);
                bitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
            }
        }

        return bitmap;
    }
}
*/