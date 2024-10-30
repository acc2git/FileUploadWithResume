using FileUploadWithResume.Web.Models;
using FileUploadWithResume.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FileUploadWithResume.Web.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileApiController : Controller
    {
        private readonly IFileUploadService _fileUploadService;

        public FileApiController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        /// <summary> 檔案上傳 </summary>
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadRequestDto model, CancellationToken ct)
        {
            var rangeHeader = Request.Headers.ContentRange.ToString();
            if (!ContentRangeHeaderValue.TryParse(rangeHeader, out ContentRangeHeaderValue? requestRange))
                return BadRequest(new UploadResultDto { Success = false, Error = "缺少Range資訊" });
            if (requestRange == null || requestRange.From == null)
                return BadRequest(new UploadResultDto { Success = false, Error = "缺少Range資訊" });
            try
            {
                long start = requestRange.From.Value;
                UploadResultDto result = await _fileUploadService.SaveUploadFile(model, start, ct);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (OperationCanceledException)
            {
                return BadRequest(new UploadResultDto { Success = false, Error = "請求已取消" });
            }
            catch (Exception)
            {
                return BadRequest(new UploadResultDto { Success = false, Error = "上傳錯誤" });
            }
        }

        /// <summary> 建立UploadTask並回傳uploadId </summary>
        [HttpPost]
        public IActionResult CreateUploadTask()
        {
            string uploadId = _fileUploadService.CreateUploadTask();
            return Ok(new { Data = uploadId });
        }

        /// <summary> 由uploadId取得目前檔案大小 </summary>
        [HttpGet("{uploadId}")]
        public IActionResult GetFileLength(string uploadId)
        {
            long fileLength = _fileUploadService.GetFileLength(uploadId);
            return Ok(new { Data = fileLength });
        }
    }
}
