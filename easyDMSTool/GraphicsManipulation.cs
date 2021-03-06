﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace easyDMSTool
{
    class GraphicsManipulation
    {

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static bool ShrinkJPEG(string fileName, Int64 quality)
        {
            try
            {
                Bitmap bmp1 = new Bitmap(fileName);
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                Image img = (Image)bmp1;
                Bitmap bmp2 = new Bitmap(resizeImage(img, new System.Drawing.Size(1200, 1600)));
                bmp1.Dispose();

                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp2.Save(fileName, jgpEncoder, myEncoderParameters);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            if ((sourceWidth < 1000) || (sourceHeight < 1000))
            {
                return imgToResize;
            }

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }


        public static bool ConvertTiffToJpeg(string fileName)
        {

            try
            {
                using (Image imageFile = Image.FromFile(fileName))
                {
                    FrameDimension frameDimensions = new FrameDimension(
                        imageFile.FrameDimensionsList[0]);

                    int frameNum = imageFile.GetFrameCount(frameDimensions);
                    string[] jpegPaths = new string[frameNum];

                    for (int frame = 0; frame < frameNum; frame++)
                    {
                        // Selects one frame at a time and save as jpeg. 
                        imageFile.SelectActiveFrame(frameDimensions, frame);
                        using (Bitmap bmp = new Bitmap(imageFile))
                        {
                            jpegPaths[frame] = String.Format("{0}\\{1}{2}.jpg",
                                Path.GetDirectoryName(fileName),
                                Path.GetFileNameWithoutExtension(fileName),
                                frame);
                            bmp.Save(jpegPaths[frame], ImageFormat.Jpeg);
                        }
                    }

                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        //public static string ConvertBmpToJpeg(string fileName)
        public static bool ConvertBmpToJpeg(string fileName)
        {
            string jpegPaths = string.Empty;
            try
            {
                using (Image imageFile = Image.FromFile(fileName))
                {
                    // Selects one frame at a time and save as jpeg. 
                    using (Bitmap bmp = new Bitmap(imageFile))
                    {
                        jpegPaths = String.Format("{0}\\{1}.jpg",
                                Path.GetDirectoryName(fileName),
                                Path.GetFileNameWithoutExtension(fileName));
                        bmp.Save(jpegPaths, ImageFormat.Jpeg);
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            
            
        }

    }
}
