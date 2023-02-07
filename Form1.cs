using System.Net.Http.Headers;

namespace WinFormsAppUploadPDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Pdf Files|*.pdf",
                Multiselect = false
            };
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                if (dialog.CheckFileExists)
                {
                    label1.Text = Path.GetFullPath(dialog.FileName);
                }
            }
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            if (!File.Exists(label1.Text))
            {
                return;
            }
            try
            {
                HttpClient client = new HttpClient();
                var multiForm = new MultipartFormDataContent();

                // var
                multiForm.Add(new StringContent("111"), "Year");
                multiForm.Add(new StringContent("1050101"), "ScanDate");

                // file
                FileStream fs = File.OpenRead(label1.Text);
                var streamContent = new StreamContent(fs);
                var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                multiForm.Add(fileContent, "files", Path.GetFileName(label1.Text));

                var url = "https://localhost:44349/api/uploadpdf";//your target api here
                using var response = await client.PostAsync(url, multiForm);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Api Suucess");
                }
                else
                {
                    MessageBox.Show("Api Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}