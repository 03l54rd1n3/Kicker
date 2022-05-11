using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

public static class ImageTransformationExtensions
{
    public static IImageTransformation AsImageTransformation(
        this Bitmap bitmap)
        => new BitmapTransformation(bitmap);
    
    public static IImageTransformation AsImageTransformation(
        this DirectBitmap directBitmap)
        => new DirectBitmapTransformation(directBitmap);

    public static IImageTransformation AsHomographyTranslated(
        this IImageTransformation source,
        Homography? homography)
        => new HomographyTransformation(source, homography);

    public static IImageTransformation AsMemorized(
        this IImageTransformation source)
        => new MemorizedImageTransformation(source);
    
    public static IImageTransformation AsFastMemorized(
        this IImageTransformation source)
        => new FastMemorizedImageTransformation(source);
}