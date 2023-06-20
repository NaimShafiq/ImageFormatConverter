using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageFormatConverter
{
    public partial class ImageConverter : Form
    {
        private ComboBox comboBox;

        public ImageConverter()
        {
            InitializeComponent();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.AddRange(new object[] { "JPG", "JPEG", "PNG", "TIF" });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // Set initial folder
            folderBrowserDialog.SelectedPath = @"C:\";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // Folder path selected by the user
                string selectedFolderPath = folderBrowserDialog.SelectedPath;

                // Do something with the selected folder path
                // For example, display it in a text box
                textBox2.Text = selectedFolderPath;
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set initial directory and filter for file selection
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "Text Files (*.png)|*.png|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // File path selected by the user
                string selectedFilePath = openFileDialog.FileName;

                // Do something with the selected file path
                // For example, display it in a text box
                textBox2.Text = selectedFilePath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the selected data type from the ComboBox
            string selectedDataType = comboBox2.SelectedItem.ToString();

            // Validate if a data type is selected
            if (!string.IsNullOrEmpty(selectedDataType))
            {
                // Get the path of the selected image file
                string selectedImagePath = textBox2.Text;

                // Validate if an image file is selected
                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    try
                    {
                        // Convert the image to the selected data type
                        string convertedImagePath = ConvertImageToDataType(selectedImagePath, selectedDataType);

                        // Display the converted image path in a text box or perform any other desired action
                        textBox1.Text = convertedImagePath;
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during the conversion process
                        MessageBox.Show("Error occurred during image conversion: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an image file.");
                }
            }
            else
            {
                MessageBox.Show("Please select a data type.");
            }
        }


        private string ConvertImageToDataType(string imagePath, string dataType)
        {
            // Generate a new file name for the converted image based on the selected data type
            string convertedImagePath = GetConvertedImagePath(imagePath, dataType);

            // Load the original image
            using (Image image = Image.FromFile(imagePath))
            {
                // Save the image in the desired format
                switch (dataType)
                {
                    case "jpg":
                        image.Save(convertedImagePath, ImageFormat.Jpeg);
                        break;
                    case "png":
                        image.Save(convertedImagePath, ImageFormat.Png);
                        break;
                    case "jpeg":
                        image.Save(convertedImagePath, ImageFormat.Jpeg);
                        break;
                    case "tif":
                        image.Save(convertedImagePath, ImageFormat.Tiff);
                        break;
                    default:
                        // Unsupported data type
                        throw new NotSupportedException("Selected data type is not supported.");
                }
            }

            // Return the path of the converted image
            return convertedImagePath;
        }

        private string GetConvertedImagePath(string originalImagePath, string dataType)
        {
            // Generate a new file name for the converted image based on the selected data type
            string originalFileName = Path.GetFileNameWithoutExtension(originalImagePath);
            string originalExtension = Path.GetExtension(originalImagePath);
            string newFileName = originalFileName + "." + dataType;
            string convertedImagePath = Path.Combine(Path.GetDirectoryName(originalImagePath), newFileName);

            return convertedImagePath;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
