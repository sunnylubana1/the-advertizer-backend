using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Framework
{
    public static class ImageUtility
    {

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> encoders = null;

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (encoders == null)
                {
                    encoders = new Dictionary<string, ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return encoders;
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            //calculate ratio to scale image (proportion)
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            //calculate new width and heigt according to ratio
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            //a holder for the result
            Bitmap result = new Bitmap(newWidth, newHeight);
            // set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            //return the resulting bitmap
            return result;
        }

        public static void ScaleResizeAndSaveImages(string uploadPath, string strFullFilePath)
        {
            Image image;

            using (FileStream fs = new FileStream(strFullFilePath, FileMode.Open))
            {
                try
                {
                    image = Image.FromStream(fs);
                }
                catch (Exception ex)
                {
                    image = Bitmap.FromFile(strFullFilePath);
                }
            }

            using (Bitmap bitmap = new Bitmap(strFullFilePath))
            {
                image = Bitmap.FromFile(strFullFilePath);
            }



            Bitmap image30x30 = ResizeImage(image, 30, 30);
            Bitmap image90x100 = ResizeImage(image, 90, 100);
            Bitmap image200x200 = ResizeImage(image, 200, 200);


            string strResizedImageExtension = ".jpg";

            image30x30.Save(uploadPath + "-30x30" + strResizedImageExtension, ImageFormat.Jpeg);
            image90x100.Save(uploadPath + "-90x100" + strResizedImageExtension, ImageFormat.Jpeg);
            image200x200.Save(uploadPath + "-200x200" + strResizedImageExtension, ImageFormat.Jpeg);
        }

        public static void CreateImagesForMapMarker(System.Drawing.Image frameImage, System.Drawing.Image image, int width, int height, int x, int y, string strFilePath)
        {
            using (var bitmap = new Bitmap(width, height))
            {
                using (frameImage)
                {
                    using (var canvas = Graphics.FromImage(bitmap))
                    {
                        Image roundImage = GetRoundedCornerImage(image, 15);
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.DrawImage(roundImage, new Rectangle(x, y, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                        canvas.DrawImage(frameImage, new Rectangle(0, 0, frameImage.Width, frameImage.Height), new Rectangle(0, 0, frameImage.Width, frameImage.Height), GraphicsUnit.Pixel);
                        canvas.Save();
                    }
                }

                bitmap.Save(strFilePath, ImageFormat.Png);
            }
        }

        private static Image GetRoundedCornerImage(Image sourceImage, int radius)
        {
            radius *= 2;
            Bitmap RoundedImage = new Bitmap(sourceImage.Width - 10, sourceImage.Height);
            Graphics g = Graphics.FromImage(RoundedImage);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Brush brush = new TextureBrush(sourceImage);
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(0, 0, radius, radius, 180, 90);
            gp.AddArc(0 + RoundedImage.Width - radius, 0, radius, radius, 270, 90);
            gp.AddArc(0 + RoundedImage.Width - radius, 0 + RoundedImage.Height - radius, radius, radius, 0, 90);
            gp.AddArc(0, 0 + RoundedImage.Height - radius, radius, radius, 90, 90);
            g.FillPath(brush, gp);
            return RoundedImage;
        }

        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path">Path to which the image would be saved.</param> 
        /// <param name="quality">An integer from 0 to 100, with 100 being the 
        /// highest quality</param> 
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static void SaveJpeg(string path, Image image, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            image.Save(path, jpegCodec, encoderParams);
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }
    }
}
