using ScoWare;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScoWare
{
    public partial class Preview : Form
    {
        public Preview()
        {
            InitializeComponent();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            DiskSpaceImageWriter writer;
            int imageWidth;
            int imageHeight;
            string fontName;
            float fontSize;
            string fontColour;
            Boolean showVolumeLabel;

            imageWidth = Convert.ToInt32(this.nudWidth.Value);
            imageHeight = Convert.ToInt32(this.nudHeight.Value);
            fontName = this.llFont.Text;
            fontSize = float.Parse(Convert.ToString(this.nudSize.Value));
            fontColour = this.llColour.Text;
            showVolumeLabel = this.cbxShowVolume.Checked;

            writer = new DiskSpaceImageWriter(imageWidth, imageHeight, fontName, fontSize, fontColour, "", showVolumeLabel);
            writer.RenderImage();

            this.pbImage.Image = writer.Image;
            this.pbImage.Width = imageWidth;
            this.pbImage.Height = imageHeight;

            this.Width = imageWidth + 33;
            this.Height = imageHeight + 116;
        }

        private void llFont_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.fontDialog1.Font = new Font(this.llFont.Text, float.Parse(Convert.ToString(this.nudSize.Value)));
            if (this.fontDialog1.ShowDialog() == DialogResult.OK)
            {
                this.llFont.Text = this.fontDialog1.Font.Name;
                this.nudSize.Value = Convert.ToDecimal(this.fontDialog1.Font.Size);
            }
        }

        private void llColour_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.colorDialog1.Color = Color.FromName(this.llColour.Text);
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.llColour.Text = this.colorDialog1.Color.Name;
            }
        }
    }
}