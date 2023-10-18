using FirebaseAdmin.Messaging;
using Google.Apis.FirebaseDynamicLinks.v1.Data;
using Google.Apis.FirebaseDynamicLinks.v1;
using Microsoft.AspNetCore.Mvc;
using Notification.Extensions;
using Notification.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

namespace Notification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushNotificationsController : ControllerBase
    {
        [HttpPost]
        [Route("SendPushNotification")]
        public IActionResult SendPushNotification([FromBody] PushNotificationRequest pushNotificationRequest)
        {
            try
            {
                var app = FirebaseAppInitializer.GetFirebaseApp();

                var message = new Message()
                {
                    Token = pushNotificationRequest.DeviceToken,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = pushNotificationRequest.Title,
                        Body = pushNotificationRequest.Body,
                    }
                };

                var messaging = FirebaseMessaging.DefaultInstance;
                var response = messaging.SendAsync(message).Result;

                return Ok($"Push notification sent: {response}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error sending push notification: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("CreateDynamicLink")]
        public async Task<IActionResult> CreateDynamicLink(string linkSource)
        {
            try
            {
                // Tạo đối tượng GoogleCredential từ tệp JSON chứa thông tin xác thực của bạn
                GoogleCredential googleCredential = GoogleCredential.FromFile("testnotification-59252-firebase-adminsdk-ri6n0-a8f78fb478.json")
                    .CreateScoped(FirebaseDynamicLinksService.Scope.Firebase);

                // Tạo dịch vụ Firebase Dynamic Links
                FirebaseDynamicLinksService service = new FirebaseDynamicLinksService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = googleCredential
                });

                var link = new DynamicLinkInfo 
                {
                    Link = linkSource, // Link thật của bạn
                    DomainUriPrefix = "thieptrantrung.page.link", // Đặt tên miền của bạn
                    AndroidInfo = new AndroidInfo
                    {
                        AndroidPackageName = "com.example.app", // ID gói ứng dụng Android
                    },
                    IosInfo = new IosInfo
                    {
                        IosBundleId = "com.example.app", // ID gói ứng dụng iOS
                    },
                    NavigationInfo = new NavigationInfo
                    {
                        EnableForcedRedirect = true, // Tùy chọn để buộc chuyển hướng
                    }
                };

                var link1 = new CreateShortDynamicLinkRequest
                {
                    DynamicLinkInfo = link,
                };

                var response = await service.ShortLinks.Create(link1).ExecuteAsync();
                string dynamicLinkUrl = response.ShortLink.ToString();

                return Ok(dynamicLinkUrl);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating link: {ex.Message}");
            }
        }
    }
}