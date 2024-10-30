using FileUploadWithResume.AppClient.Api;

namespace FileUploadWithResume.AppClient
{
    public partial class Form1 : Form
    {
        private readonly IFileApiClient _fileApiClient;
        private Dictionary<string, string> _uploadIdDic;
        private CancellationTokenSource _cts;

        public Form1(IFileApiClient fileApiClient)
        {
            InitializeComponent();
            _fileApiClient = fileApiClient;
            _fileApiClient.SetReportProgress(UploladProgress);
            _uploadIdDic = [];
            _cts = new();
        }

        /// <summary> 選取檔案 </summary>
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { Multiselect = false };
            if (dialog.ShowDialog() == DialogResult.OK)
                txtFilePath.Text = dialog.FileName;
        }

        /// <summary> 點選上傳 </summary>
        private async void btnUpload_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("請選取檔案");
                return;
            }

            try
            {
                btnUpload.Enabled = false;
                btnCancel.Enabled = true;
                _cts.Dispose();
                _cts = new();

                string uploadId = await GetOrCreateUploadId(filePath);
                using var fs = File.OpenRead(filePath);
                await _fileApiClient.ChunkedUpload(fs, uploadId, _cts.Token);
                MessageBox.Show("上傳完成");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("上傳已取消");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"上傳發生錯誤 {ex.Message}");
            }
            finally
            {
                btnUpload.Enabled = true;
                btnCancel.Enabled = false;
            }
        }

        /// <summary> 建立一個新的UploadTask並由取得UploadId，或是使用現有的UploadId </summary>
        private async Task<string> GetOrCreateUploadId(string filePath)
        {
            // 使用Dictionary模擬client端儲存上傳狀態的Repository，此處以filePath識別檔案
            _uploadIdDic.TryGetValue(filePath, out string? uploadId);
            if (string.IsNullOrEmpty(uploadId))
            {
                uploadId = await _fileApiClient.CreateUploadTask();
                _uploadIdDic[filePath] = uploadId;
            }
            return uploadId;
        }

        /// <summary> 點選取消 </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
        }

        private void UploladProgress(string message)
        {
            txtProgress.AppendText(message);
            txtProgress.AppendText(Environment.NewLine);
        }
    }
}
