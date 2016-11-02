using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageAndBase64
{
    public static class GraphicsUtility
    {
        /// <summary>
        /// 图像转换为Base64编码
        /// </summary>
        /// <param name="image">图像</param>
        /// <param name="format">图像格式</param>
        /// <returns>转换成功返回其Base64编码；失败返回空串</returns>
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            string base64String = "";
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
            }
            return base64String;
        }

        /// <summary>
        /// Base64编码转换为图像
        /// </summary>
        /// <param name="base64String">Base64字符串</param>
        /// <returns>转换成功返回图像；失败返回null</returns>
        public static Image Base64ToImage(string base64String)
        {
            MemoryStream ms = null;
            Image image = null;
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(ms, true);
            }
            catch
            {
            }
            finally
            {
                if (ms != null) ms.Close();
            }
            return image;
        }

        /// <summary>
        /// 画一个国际象棋的棋盘
        /// </summary>
        /// <param name="brush1">颜色1（如：Brushes.Black）</param>
        /// <param name="brush2">颜色2（如：Brushes.White）</param>
        /// <param name="squareLength">小方格长度</param>
        /// <param name="squareCountPerLine">每行的小方格个数（比如：8）</param>
        /// <returns></returns>
        public static Bitmap DrawChessBoard(Brush brush1, Brush brush2, int squareLength, int squareCountPerLine)
        {
            Bitmap bitmap = null;
            Graphics g = null;
            try
            {
                int boardLength = squareLength * squareCountPerLine;
                bitmap = new Bitmap(boardLength, boardLength);
                g = Graphics.FromImage(bitmap);
                for (int i = 0; i < squareCountPerLine; i++)
                {
                    for (int j = 0; j < squareCountPerLine; j++)
                    {
                        int im = i % 2, jm = j % 2;
                        Brush brush = im == 0 && jm == 0 || im != 0 && jm != 0 ? brush1 : brush2;
                        g.FillRectangle(brush, i * squareLength, j * squareLength, squareLength, squareLength);
                    }
                }
            }
            catch
            {
                //溢出 或 创建位图失败
            }
            finally
            {
                if (g != null) g.Dispose();
            }
            return bitmap;
        }
    }
}
