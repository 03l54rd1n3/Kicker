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
        var custom1 = Color.FromSystemDrawing(systemDrawing);
        var custom2 = new Color(randomInt);

        // Act
        var actual1 = custom1.Value;
        var actual2 = custom2.Value;

        // Assert
        Assert.Equal(systemDrawing.ToArgb(), randomInt);
        Assert.Equal(systemDrawing.ToArgb(), actual1);
        Assert.Equal(systemDrawing.ToArgb(), actual2);
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