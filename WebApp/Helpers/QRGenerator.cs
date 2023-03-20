using SkiaSharp;
using SkiaSharp.QrCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Helpers
{
    public class QRGenerator
    {
        public Stream GenerateQR(string content)
        {
            using var generator = new QRCodeGenerator();

            //This line forces to generate High quality Data (aprox 30% accurate), Damages can be protected
            ECCLevel level = ECCLevel.H;
            QRCodeData qr = generator.CreateQrCode(content, level);

            //Size of 512 x 512 px
            SKImageInfo info = new(512, 512);
            using SKSurface surface = SKSurface.Create(info);

            SKCanvas canvas = surface.Canvas;
            canvas.Render(qr, 512, 512);

            using SKImage image = surface.Snapshot();
            using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);

            return data.AsStream();
        }
    }
}
