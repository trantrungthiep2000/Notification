using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Notification.Extensions
{
    public class FirebaseAppInitializer
    {
        private static FirebaseApp _app;
        private static readonly object _lock = new object();
        private static bool _initialized = false;

        public static FirebaseApp GetFirebaseApp()
        {
            lock (_lock)
            {
                if (!_initialized)
                {
                    InitializeFirebaseApp();
                    _initialized = true;
                }
                return _app;
            }
        }

        private static void InitializeFirebaseApp()
        {
            if (_app == null)
            {
                _app = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile("testnotification-59252-firebase-adminsdk-ri6n0-a8f78fb478.json"),
                });
            }
        }
    }
}
