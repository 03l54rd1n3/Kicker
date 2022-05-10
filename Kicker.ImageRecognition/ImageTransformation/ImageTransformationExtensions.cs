using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

public static class ImageTransformationExtensions
{
    public static IImageTransformation AsImageTransformation(
        this Bitmap bitmap)
        => new BitmapTransformation(bitmap);

    public static IImageTransformation AsHomographyTranslated(
        this IImageTransformation source,
        Homography homography)
        => new HomographyTransformation(source, homography);
}