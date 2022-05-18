using Kicker.ImageRecognition.Imaging;

namespace Kicker.ImageRecognition.Tests.Imaging;

public class ColorTests
{
    [Fact]
    public void Value_SystemDrawingArgb_AreEqual()
    {
        // Arrange
        var random = new Random();
        var randomInt = random.Next();
        var systemDrawing = System.Drawing.Color.FromArgb(randomInt);
        var custom = Color.FromSystemDrawing(systemDrawing);

        // Act
        var actual = custom.Value;

        // Assert
        Assert.Equal(systemDrawing.ToArgb(), randomInt);
        Assert.Equal(systemDrawing.ToArgb(), actual);
    }

    [Fact]
    public void Value_FromArgbBitwise_AreEqual()
    {
        // Arrange
        var randomInt = new Random().Next();
        var expected = System.Drawing.Color.FromArgb(randomInt);

        // Act
        var actual = new Color(randomInt);

        // Assert
        Assert.True(actual == expected);
    }
}