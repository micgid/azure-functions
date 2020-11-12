using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using QRCoder;
using Microsoft.Extensions.Logging;

namespace tyndalos.feedback
{
        public static class GetQrCode
    {
        [FunctionName("GetQrCode")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetQrCode processed a request.");
            var url = req.Query["url"];

            if(string.IsNullOrWhiteSpace(url)){
                log.LogWarning("Url is null or empty.");
                return new BadRequestResult();
            }

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);
            log.LogInformation("QR code image returned for url.", url);
            return new FileContentResult(qrCodeImage, "image/bmp");
        }
    }
}
