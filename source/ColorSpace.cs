//参考：
// 百科：HSL和HSV色彩空间
// http://zh.wikipedia.org/w/index.php?title=HSL%E5%92%8CHSV%E8%89%B2%E5%BD%A9%E7%A9%BA%E9%97%B4&variant=zh-cn
// 百科：印刷四分色模式
// http://zh.wikipedia.org/w/index.php?title=CMYK&variant=zh-cn

//原文：
// http://hi.baidu.com/wingingbob/blog/item/7d875efa1266e39059ee90ab.html

using System;
using System.Text;

namespace Bobwei.Design
{
    static class Program
    {
        static void Main(string[] args)
        {
            int r = 0, g = 0, b = 0;
            do
            {
                bool pas = (
                    int.TryParse(Console.ReadLine(), out r) &&
                    int.TryParse(Console.ReadLine(), out g) &&
                    int.TryParse(Console.ReadLine(), out b));
                if (!pas)
                    break;
                ColorSpace.RGB Rgb = new ColorSpace.RGB(r, g, b);
                ColorSpace.HSB Hsb = ColorSpace.Convertor.RGB2HSB(Rgb);
                ColorSpace.RGB newRgb = ColorSpace.Convertor.HSB2RGB(Hsb);
                ColorSpace.HSL Hsl = ColorSpace.Convertor.RGB2HSL(Rgb);
                ColorSpace.RGB newRgb2 = ColorSpace.Convertor.HSL2RGB(Hsl);
                Console.WriteLine("HSB: {0:f} {1:f} {2:f}\tRGB: {3,3} {4,3} {5,3}\n" +
                                  "HSL: {6:f} {7:f} {8:f}\tRGB2:{9,3} {10,3} {11,3}\n--------------",
                                  Hsb.H, Hsb.S, Hsb.B, newRgb.R, newRgb.G, newRgb.B,
                                  Hsl.H, Hsl.S, Hsl.L, newRgb2.R, newRgb2.G, newRgb2.B);
            } while (true);
        }
    }
}

/// <summary>
/// 色彩空间管理
/// </summary>
public static class ColorSpace
{

    #region 结构
    public struct RGB
    {
        public byte R
        {
            get { return r; }
            set
            {
                CheckByte(value, "Red");
                r = value;
            }
        }
        public byte G
        {
            get { return g; }
            set
            {
                CheckByte(value, "Green");
                g = value;
            }
        }
        public byte B
        {
            get { return b; }
            set
            {
                CheckByte(value, "Blue");
                b = value;
            }
        }

        private byte r, g, b;

        public RGB(int r, int g, int b)
            : this()
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
        }

        private static void CheckByte(int value, string name)
        {
            if ((value < 0) || (value > 0xff))
                throw new ArgumentOutOfRangeException(string.Format("{0} valid range is {1}", name, "0~255"));
        }
    }

    public struct HSL
    {
        public float H
        {
            get { return h; }
            set
            {
                CheckHSL(value, 0, 0);
                h = value;
            }
        }
        public float S
        {
            get { return s; }
            set
            {
                CheckHSL(0, value, 0);
                s = value;
            }
        }
        public float L
        {
            get { return l; }
            set
            {
                CheckHSL(0, 0, value);
                l = value;
            }
        }

        private float h, s, l;

        public HSL(float h, float s, float l)
            : this()
        {
            H = h; S = s; L = l;
        }

        private static void CheckHSL(float h, float s, float l)
        {
            if (h < 0 || h > 360)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} valid range is {1}", "Hue", "0~360"));
            }
            if (s < 0 || s > 1)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} valid range is {1}", "Saturation", "0~1"));
            }
            if (l < 0 || l > 1)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} valid range is {1}", "Lightness", "0~1"));
            }
        }
    }

    public struct HSV
    {
        public float H
        {
            get { return h; }
            set
            {
                CheckHSV(value, 0, 0);
                h = value;
            }
        }
        public float S
        {
            get { return s; }
            set
            {
                CheckHSV(0, value, 0);
                s = value;
            }
        }
        public float V
        {
            get { return v; }
            set
            {
                CheckHSV(0, 0, value);
                v = value;
            }
        }

        private float h, s, v;

        public HSV(float h, float s, float v)
            : this()
        {
            H = h; S = s; V = v;
        }

        private static void CheckHSV(float h, float s, float v)
        {
            if (h < 0 || h > 360)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} valid range is {1}", "Hue", "0~360"));
            }
            if (s < 0 || s > 1)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} valid range is {1}", "Saturation", "0~1"));
            }
            if (v < 0 || v > 1)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} valid range is {1}", "Value", "0~1"));
            }
        }
    }

    public struct HSB
    {
        HSV hsv;

        public float H
        {
            get { return hsv.H; }
            set
            {
                try { hsv.H = value; }
                catch (ArgumentOutOfRangeException ex)
                { throw new ArgumentOutOfRangeException(ex.Message); }
            }
        }
        public float S
        {
            get { return hsv.S; }
            set
            {
                try { hsv.S = value; }
                catch (ArgumentOutOfRangeException ex)
                { throw new ArgumentOutOfRangeException(ex.Message); }
            }
        }
        public float B
        {
            get { return hsv.V; }
            set
            {
                try { hsv.V = value; }
                catch (ArgumentOutOfRangeException ex)
                { throw new ArgumentOutOfRangeException(ex.Message.Replace("Value ", "Brightness ")); }
            }
        }

        public HSB(float h, float s, float b)
            : this()
        {
            try
            {
                hsv = new HSV(h, s, b);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException(ex.Message.Replace("Value ", "Brightness "));
            }
        }
    }
    #endregion  //结构

    public static class Convertor
    {
        public static HSV RGB2HSV(RGB Rgb)
        {
            float r = Rgb.R / 255f;
            float g = Rgb.G / 255f;
            float b = Rgb.B / 255f;
            HSV Hsv = new HSV(0, 0, 0);
            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            float dif = max - min;
            if (dif == 0)
            {
                Hsv.H = 0;
            }
            else if (max == r)
            {
                if (g >= b)
                {
                    Hsv.H = (g - b) / dif * 60f;
                }
                else
                {
                    Hsv.H = (g - b) / dif * 60f + 360f;
                }
            }
            else if (max == g)
            {
                Hsv.H = (b - r) / dif * 60f + 120f;
            }
            else
            {
                Hsv.H = (r - g) / dif * 60f + 240f;
            }

            //Hsv.H /= 360f;
            Hsv.S = max == 0 ? 0 : dif / max;
            Hsv.V = max;
            return Hsv;
        }

        public static HSB RGB2HSB(RGB Rgb)
        {
            HSV Hsv = RGB2HSV(Rgb);
            return new HSB(Hsv.H, Hsv.S, Hsv.V);
        }

        public static HSL RGB2HSL(RGB Rgb)
        {
            float r = Rgb.R / 255f;
            float g = Rgb.G / 255f;
            float b = Rgb.B / 255f;
            HSL Hsl = new HSL(0, 0, 0);
            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            float dif = max - min;
            if (dif == 0)
            {
                Hsl.H = 0;
            }
            else if (max == r)
            {
                if (g >= b)
                {
                    Hsl.H = (g - b) / dif * 60f;
                }
                else
                {
                    Hsl.H = (g - b) / dif * 60f + 360f;
                }
            }
            else if (max == g)
            {
                Hsl.H = (b - r) / dif * 60f + 120f;
            }
            else
            {
                Hsl.H = (r - g) / dif * 60f + 240f;
            }

            //Hsl.H /= 360f;
            Hsl.L = (max + min) / 2f;
            if (Hsl.L == 0 || max == min)
            {
                Hsl.S = 0;
            }
            else if (Hsl.L > 0.5f)
            {
                Hsl.S = dif / (2f - (max + min));
            }
            else
            {
                Hsl.S = dif / (max + min);
            }
            return Hsl;
        }

        public static RGB HSV2RGB(HSV Hsv)
        {
            int hi = (int)(Hsv.H / 60f) % 6;
            float f, p, q, t;
            f = Hsv.H / 60f - hi;
            p = Hsv.V * (1f - Hsv.S);
            q = Hsv.V * (1f - f * Hsv.S);
            t = Hsv.V * (1f - (1f - f) * Hsv.S);

            float r = 0f, g = 0f, b = 0f;
            switch (hi)
            {
                case 0:
                    r = Hsv.V;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = Hsv.V;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = Hsv.V;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = Hsv.V;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = Hsv.V;
                    break;
                case 5:
                    r = Hsv.V;
                    g = p;
                    b = q;
                    break;
            }

            RGB Rgb = new RGB(
                (int)Math.Round(r * 255),
                (int)Math.Round(g * 255),
                (int)Math.Round(b * 255));
            return Rgb;
        }

        public static RGB HSB2RGB(HSB Hsb)
        {
            HSV Hsv = new HSV(Hsb.H, Hsb.S, Hsb.B);
            return HSV2RGB(Hsv);
        }

        public static RGB HSL2RGB(HSL Hsl)
        {
            if (Hsl.S == 0)
            {
                int l = (int)(Hsl.L * 255);
                return new RGB(l, l, l);
            }

            float q, p, hk, tr, tg, tb;
            q = Hsl.L < 0.5f ? Hsl.L * (1f + Hsl.S) : Hsl.L + Hsl.S - (Hsl.L * Hsl.S);
            p = 2f * Hsl.L - q;
            hk = Hsl.H / 360f;
            tr = hk + 1f / 3f;
            tg = hk;
            tb = hk - 1f / 3f;
            if (tr < 0) tr += 1f;
            if (tr > 1) tr -= 1f;
            if (tg < 0) tg += 1f;
            if (tg > 1) tg -= 1f;
            if (tb < 0) tb += 1f;
            if (tb > 1) tb -= 1f;
            float colorr, colorg, colorb;
            colorr = calc_ColorC(tr, p, q);
            colorg = calc_ColorC(tg, p, q);
            colorb = calc_ColorC(tb, p, q);

            RGB Rgb = new RGB(
                (int)Math.Round(colorr * 255),
                (int)Math.Round(colorg * 255),
                (int)Math.Round(colorb * 255));
            return Rgb;
        }

        private static float calc_ColorC(float tc, float p, float q)
        {
            float colorc;
            if (6f * tc < 1f)
            {
                colorc = p + ((q - p) * 6f * tc);
            }
            else if (6f * tc >= 1f && tc < 0.5f)
            {
                colorc = q;
            }
            else if (tc >= 0.5f && 3f * tc < 2f)
            {
                colorc = p + ((q - p) * 6f * (2f / 3f - tc));
            }
            else
            {
                colorc = p;
            }
            return colorc;
        }
    }
}
