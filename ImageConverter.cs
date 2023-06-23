using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageFormatConverter
{
    public partial class ImageConverter : Form
    {
        private int totalImagesConverted;
        private int totalImagesToConvert;
        private int imagesConverted;
        private DateTime startTime;
        private Dictionary<Guid, string> formatMap;

        public ImageConverter()
        {
            InitializeComponent();
            InitializeFormatMap();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.AddRange(new object[] { "GIF", "JPEG", "PNG", "TIF", "BMP" });

        }

        private void InitializeFormatMap()
        {
            formatMap = new Dictionary<Guid, string>
            {
                { ImageFormat.Bmp.Guid, ".bmp" },
                { ImageFormat.Jpeg.Guid, ".jpeg" },
                { ImageFormat.Png.Guid, ".png" },
                { ImageFormat.Gif.Guid, ".gif" },
                { ImageFormat.Tiff.Guid, ".tiff" },
           };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = @"C:\";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolderPath = folderBrowserDialog.SelectedPath;
                textBox2.Text = selectedFolderPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set initial directory and filter for file selection
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                textBox2.Text = selectedFilePath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the selected data type from the ComboBox
            string selectedDataType = comboBox2.SelectedItem?.ToString();

            // Validate if a data type is selected
            if (string.IsNullOrEmpty(selectedDataType))
            {
                MessageBox.Show("Please select a data type.");
                return;
            }

            // Get the path of the selected item (file or folder)
            string selectedPath = textBox2.Text;

            // Validate if a path is selected
            if (string.IsNullOrEmpty(selectedPath))
            {
                MessageBox.Show("Please select a file or folder.");
                return;
            }

            // Reset progress and time variables
            totalImagesToConvert = 0;
            imagesConverted = 0;
            totalImagesConverted = 1;
            startTime = DateTime.Now;

            // Disable the Convert button during the conversion process
            button1.Enabled = false;

            try
            {
                // Check if the selected path is a file
                if (File.Exists(selectedPath))
                {
                    // Convert the selected file to the selected data type
                    string convertedImagePath = ConvertImageToDataType(selectedPath, selectedDataType);
                    richTextBox1.AppendText(convertedImagePath + Environment.NewLine);
                    MessageBox.Show("Image conversion completed successfully.");
                }
                else if (Directory.Exists(selectedPath))
                {
                    // Get all image files in the selected folder
                    string[] imageFiles = Directory.GetFiles(selectedPath, "*.*", SearchOption.AllDirectories)
                        .Where(file => IsValidImageFile(file))
                        .ToArray();

                    // Set total number of images to convert
                    totalImagesToConvert = imageFiles.Length;

                    // Convert each image to the selected data type
                    foreach (string imageFile in imageFiles)
                    {
                        try
                        {
                            // Convert the image to the selected data type
                            string convertedImagePath = ConvertImageToDataType(imageFile, selectedDataType);
                            richTextBox1.AppendText(convertedImagePath + Environment.NewLine);

                            // Increment the total images converted counter
                            totalImagesConverted++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error occurred during image conversion: {ex.Message}");
                        }
                    }

                    richTextBox1.Text = "Image conversion completed successfully for all images in the selected folder.";
                }
                else
                {
                    MessageBox.Show("The selected path does not exist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred during image conversion: " + ex.Message);
            }
            finally
            {
                // Calculate the time taken
                TimeSpan timeTaken = DateTime.Now - startTime;

                // Enable the Convert button after the conversion process
                button1.Enabled = true;

                // Display the total images converted and time taken
                string resultMessage = $"Total Images Converted: {totalImagesConverted}" +
                                       $"{Environment.NewLine}Time Taken: {timeTaken.TotalSeconds} seconds";

                // Append the result message to the existing content in the richTextBox1
                richTextBox1.AppendText(Environment.NewLine + resultMessage);
            }
        }

        private string ConvertImageToDataType(string imagePath, string dataType)
        {
            // Check if the image is already in the selected format
            string imageExtension = Path.GetExtension(imagePath).ToLower();
            string selectedExtension = "." + dataType.ToLower();
            if (imageExtension == selectedExtension)
            {
                throw new ArgumentException(imagePath +" is already in "+ selectedExtension +" format.");
            }
            // Generate a new file name for the converted image based on the selected data type
            string convertedImagePath = GetConvertedImagePath(imagePath, dataType);

            // Load the original image
            using (Image image = Image.FromFile(imagePath))
            {
                // Save the image in the desired format
                ImageFormat imageFormat;
                switch (dataType.ToLower())
                {
                    case "jpg":
                    case "jpeg":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case "png":
                        imageFormat = ImageFormat.Png;
                        break;
                    case "gif":
                        imageFormat = ImageFormat.Gif;
                        break;
                    case "tif":
                        imageFormat = ImageFormat.Tiff;
                        break;
                    case "bmp":
                        imageFormat = ImageFormat.Bmp;
                        break;
                    default:
                        throw new NotSupportedException("Selected data type is not supported.");
                }

                // Save the image with the specified image format
                image.Save(convertedImagePath, imageFormat);
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

        private bool IsValidImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            string[] validExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

            return validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                textBox2.Text = selectedFilePath;
                try
                {
                    Image image = Image.FromFile(openFileDialog.FileName);
                    ImageFormat format = image.RawFormat;

                    // Check the format for image
                    if (formatMap.TryGetValue(format.Guid, out string fileExtension))
                    {
                        richTextBox1.Text = $"Image format: {fileExtension}";
                    }
                    else
                    {
                        richTextBox1.Text = "Unknown format";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

