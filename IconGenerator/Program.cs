using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace IconGenerator;

static class IconBuilder
{
    static void Main()
    {
        var target = @"D:/Mario/RoknaCafe/src/RoknaCafe/rokn-hady.ico";
        using var icon = CreateIcon();
        using var fs = new FileStream(target, FileMode.Create, FileAccess.Write);
        icon.Save(fs);
        Console.WriteLine($"Icon saved to: {target}");
    }

    static Icon CreateIcon()
    {
        var sizes = new[] { 16, 32, 48, 64, 128, 256 };
        var entryCount = (short)sizes.Length;
        var folderOffset = 6 + 16 * entryCount;

        var iconDir = BuildIconDir(entryCount, entryCount, sizes, folderOffset);
        var imageData = BuildImageData(sizes);

        using var ms = new MemoryStream((int)(6 + 16L * entryCount + imageData.Length));
        iconDir.CopyTo(ms);
        imageData.CopyTo(ms);
        ms.Position = 0;
        return new Icon(ms);
    }

    static MemoryStream BuildIconDir(short reserved, short type, int[] sizes, int folderOffset)
    {
        var ms = new MemoryStream();
        var writer = new BinaryWriter(ms);

        writer.Write((ushort)reserved);
        writer.Write((ushort)type);
        writer.Write((ushort)sizes.Length);

        var offset = folderOffset;
        foreach (var size in sizes)
        {
            writer.Write((byte)size);
            writer.Write((byte)size);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((ushort)1);
            writer.Write((ushort)32);
            var data = Render(size);
            writer.Write((int)data.Length);
            writer.Write(offset);
            offset += (int)data.Length;
        }

        writer.Flush();
        ms.Position = 0;
        return ms;
    }

    static MemoryStream BuildImageData(int[] sizes)
    {
        var ms = new MemoryStream();
        foreach (var size in sizes)
        {
            var data = Render(size);
            data.CopyTo(ms);
        }
        ms.Position = 0;
        return ms;
    }

    static MemoryStream Render(int size)
    {
        using var bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var backColor = Color.FromArgb(32, 42, 28);
        var cupColor = Color.FromArgb(52, 152, 219);
        var cupDarkColor = Color.FromArgb(41, 128, 185);
        var steamColor = Color.FromArgb(235, 238, 242);
        var borderColor = Color.FromArgb(18, 24, 15);

        g.Clear(Color.Transparent);

        var pad = size / 12f;
        var cx = size / 2f;
        var cy = size / 2f;
        var r = size / 2f - pad;

        if (size <= 16)
        {
            using var bg = new SolidBrush(backColor);
            g.FillEllipse(bg, pad, pad, size - pad * 2, size - pad * 2);

            using var cup = new SolidBrush(cupColor);
            var cupW = r * 1.6f;
            var cupH = r * 1.1f;
            g.FillRectangle(cup, cx - cupW / 2f, cy - cupH / 4f, cupW, cupH);
        }
        else
        {
            using var bg = new SolidBrush(backColor);
            g.FillEllipse(bg, 0, 0, size, size);

            var cupW = r * 1.55f;
            var cupH = r * 1.08f;
            var cupX = cx - cupW / 2f;
            var cupY = cy + pad * 0.6f;

            using var cup = new SolidBrush(cupColor);
            using var cupDark = new SolidBrush(cupDarkColor);
            g.FillRectangle(cup, cupX, cupY, cupW, cupH);
            g.FillRectangle(cupDark, cupX, cupY + cupH * 0.66f, cupW, cupH * 0.34f);

            using var handle = new SolidBrush(Color.FromArgb(235, 238, 242));
            using var handlePen = new Pen(borderColor, Math.Max(1f, size / 48f));
            var handleRect = new RectangleF(cupX + cupW, cupY + cupH * 0.22f, cupH * 0.42f, cupH * 0.56f);
            g.FillEllipse(handle, handleRect);
            g.DrawEllipse(handlePen, handleRect);

            if (size >= 64)
            {
                var steamY = cupY - cupH * 0.52f;
                var steamBrush = new SolidBrush(steamColor);
                DrawSteam(g, cx - cupW * 0.28f, steamY, size, steamBrush);
                DrawSteam(g, cx, steamY, size, steamBrush);
                DrawSteam(g, cx + cupW * 0.28f, steamY, size, steamBrush);
                steamBrush.Dispose();
            }
        }

        var ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Png);
        ms.Position = 0;
        return ms;
    }

    static void DrawSteam(Graphics g, float x, float y, int size, Brush brush)
    {
        using var path = new GraphicsPath();
        path.AddArc(x - size / 42f, y - size / 10f, size / 20f, size / 10f, 18f, 150f);
        path.AddArc(x + size / 100f, y - size / 5f, size / 20f, size / 10f, 210f, 150f);
        using var pen = new Pen(brush, Math.Max(1f, size / 56f));
        g.DrawPath(pen, path);
    }
}
