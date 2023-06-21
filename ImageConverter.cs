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
            comboBox2.Items.AddRange(new object[] { "GIF", "JPEG", "PNG", "TIF" });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Clear textBox1 when Browse Folder button is clicked
            textBox1.Text = "";

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

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear textBox1 when Browse File button is clicked
            textBox1.Text = "";

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
                // Get the path of the selected item (file or folder)
                string selectedPath = textBox2.Text;

                // Validate if a path is selected
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    try
                    {
                        // Check if the selected path is a file
                        if (File.Exists(selectedPath))
                        {
                            // Convert the selected file to the selected data type
                            string convertedImagePath = ConvertImageToDataType(selectedPath, selectedDataType);

                            // Display the converted image path in a text box or perform any other desired action
                            textBox1.Text = convertedImagePath;

                            MessageBox.Show("Image conversion completed successfully.");
                        }
                        else if (Directory.Exists(selectedPath))
                        {
                            // Get all image files in the selected folder
                            string[] imageFiles = Directory.GetFiles(selectedPath, "*.*", SearchOption.AllDirectories)
                                .Where(file => IsValidImageFile(file))
                                .ToArray();

                            // Convert each image to the selected data type
                            foreach (string imageFile in imageFiles)
                            {
                                try
                                {
                                    // Convert the image to the selected data type
                                    string convertedImagePath = ConvertImageToDataType(imageFile, selectedDataType);

                                    // Display the converted image path in a text box or perform any other desired action
                                    textBox1.AppendText(convertedImagePath + Environment.NewLine);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error occurred during image conversion: {ex.Message}");
                                }
                            }

                            MessageBox.Show("Image conversion completed successfully for all images in the selected folder.");
                        }
                        else
                        {
                            MessageBox.Show("The selected path does not exist.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during the conversion process
                        MessageBox.Show("Error occurred during image conversion: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a file or folder.");
                }
            }
            else
            {
                MessageBox.Show("Please select a data type.");
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
                    default:
                        // Unsupported data type
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
            string[] validExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" }; // Add more valid extensions if needed

            return validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
