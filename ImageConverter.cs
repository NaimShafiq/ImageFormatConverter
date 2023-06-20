using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}
