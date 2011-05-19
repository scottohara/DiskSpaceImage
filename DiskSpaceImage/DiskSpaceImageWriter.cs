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
        private DriveInfo[] allDrives;
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
            // Get the list of disk drives
            this.allDrives = DriveInfo.GetDrives();

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
            // Clear the display string
            this.displayString = String.Empty;

            Double totalFreeSpace;
            Double totalSize;

            // Loop through the collection
            foreach (DriveInfo d in this.allDrives)
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
                    this.displayString += ": ";

                    totalFreeSpace = Convert.ToDouble(d.TotalFreeSpace);
                    totalSize = Convert.ToDouble(d.TotalSize);

                    // Determine the appropriate unit of measure (ie. GB, MB, KB)
                    if (totalFreeSpace >= GB)
                    {
                        totalFreeSpace = totalFreeSpace / GB;
                        this.displayString += totalFreeSpace.ToString("0.0") + "GB";
                    }
                    else if (totalFreeSpace >= MB)
                    {
                        totalFreeSpace = totalFreeSpace / MB;
                        this.displayString += totalFreeSpace.ToString("0.0") + "MB";
                    }
                    else if (totalFreeSpace >= KB)
                    {
                        totalFreeSpace = totalFreeSpace / KB;
                        this.displayString += totalFreeSpace.ToString("0.0") + "KB";
                    }
                    else
                    {
                        this.displayString += totalFreeSpace.ToString() + "bytes";
                    }

                    this.displayString += "/";

                    // Determine the appropriate unit of measure (ie. GB, MB, KB)
                    if (totalSize >= GB)
                    {
                        totalSize = totalSize / GB;
                        this.displayString += totalSize.ToString("0.0") + "GB";
                    }
                    else if (totalSize >= MB)
                    {
                        totalSize = totalSize / MB;
                        this.displayString += totalSize.ToString("0.0") + "MB";
                    }
                    else if (totalSize >= KB)
                    {
                        totalSize = totalSize / KB;
                        this.displayString += totalSize.ToString("0.0") + "KB";
                    }
                    else
                    {
                        this.displayString += totalSize.ToString() + "bytes";
                    }

                    // Add a CRLF
                    this.displayString += "\r\n";
                }
            }

            // Write out the string as an image
            this.displayImage.DrawString(displayString, displayFont, displayBrush, 0.0F, 0.0F);
        }

        public void SaveImage()
        {
            // Save the image
            this.displayBitmap.Save(this.imageName, ImageFormat.Png);
        }
    }
}
