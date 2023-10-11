using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Notification.Extensions;
using Notification.Models;

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
    }
}