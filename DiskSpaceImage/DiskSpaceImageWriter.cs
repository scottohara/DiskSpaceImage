using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text;

namespace ScoWare
{
    public class DiskSpaceImageWriter
    {
        private string displayString;
        private Bitmap displayBitmap;
        private Font displayFont;
        private SolidBrush displayBrush;
        private Graphics displayImage;
        private string imageName;
        private Boolean showVolumeLabel;

        private const Double KB = 1024.0;
        private const Double MB = KB * 1024.0;
        private const Double GB = MB * 1024.0;

        public Bitmap Image {
            get
            {
                return this.displayBitmap;
            }
        }

        public DiskSpaceImageWriter(int imageWidth, int imageHeight, string fontName, float fontSize, string fontColour, string imageName, Boolean showVolumeLabel)
        {
            // Initialise the image to the specified size
            this.displayBitmap = new Bitmap(imageWidth, imageHeight);
            this.displayBitmap.MakeTransparent();

            // Initialise the image style
            this.displayFont = new Font(fontName, fontSize);
            this.displayBrush = new SolidBrush(Color.FromName(fontColour));

            // Initialise the graphics object
            this.displayImage = Graphics.FromImage(this.displayBitmap);
            this.displayImage.TextRenderingHint = TextRenderingHint.AntiAlias;

            // Save the image name property
            this.imageName = imageName;

            // Save the image content options
            this.showVolumeLabel = showVolumeLabel;
        }

        public void RenderImage()
        {
            // Re-initialise the image to the specified size
            this.displayBitmap = new Bitmap(this.displayBitmap.Width, this.displayBitmap.Height);
            this.displayBitmap.MakeTransparent();

            // Re-initialise the graphics object
            this.displayImage = Graphics.FromImage(this.displayBitmap);
            this.displayImage.TextRenderingHint = TextRenderingHint.AntiAlias;

            // Clear the display string
            this.displayString = String.Empty;

            // Loop through the collection
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                // Only interested in fixed drives
                if (d.IsReady == true && d.DriveType == DriveType.Fixed)
                {
                    // Construct the display string
                    this.displayString += d.Name;
                    if (this.showVolumeLabel)
                    {
                        this.displayString += " (" + d.VolumeLabel + ")";
                    }
                    this.displayString += ": " + this.capacityToString(Convert.ToDouble(d.TotalFreeSpace)) + "/" + this.capacityToString(Convert.ToDouble(d.TotalSize)) + "\r\n";
                }
            }

            // Write out the string as an image
            this.displayImage.DrawString(displayString, displayFont, displayBrush, 0.0F, 0.0F);
        }

        private string capacityToString(Double capacity)
        {
            string unitOfMeasure;

            // Determine the appropriate unit of measure (ie. GB, MB, KB)
            if (capacity >= GB)
            {
                capacity = capacity / GB;
                unitOfMeasure = "GB";
                
            }
            else if (capacity >= MB)
            {
                capacity = capacity / MB;
                unitOfMeasure = "MB";
            }
            else if (capacity >= KB)
            {
                capacity = capacity / KB;
                unitOfMeasure = "KB";
            }
            else
            {
                unitOfMeasure = "bytes";
            }

            return capacity.ToString("0.0") + unitOfMeasure;
        }

        public void SaveImage()
        {
            // Save the image
            this.displayBitmap.Save(this.imageName, ImageFormat.Png);
        }
    }
}
