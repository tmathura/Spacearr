using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Spacearr.Common.Services.Implementations
{
    public class SendFirebasePushNotificationService : ISendFirebasePushNotificationService
    {
        private static ILogger _logger;

        public SendFirebasePushNotificationService(ILogger logger)
        {
            _logger = logger;

            var request = new
            {
                type = "service_account",
                project_id = "ProjectId",
                private_key_id = "PrivateKeyId",
                private_key = "PrivateKey",
                client_email = "ClientEmail",
                client_id = "ClientId",
                auth_uri = "https://accounts.google.com/o/oauth2/auth",
                token_uri = "https://oauth2.googleapis.com/token",
                auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs",
                client_x509_cert_url = "ClientX509CertUrl"
            };
            var jsonRequest = JsonConvert.SerializeObject(request);

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(jsonRequest)
            });
        }

        /// <summary>
        /// Send a firebase push notification one device.
        /// </summary>
        /// <returns></returns>
        public void SendNotification(string token, string notificationTitle, string notificationMessage)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = notificationTitle,
                        Body = notificationMessage
                    },
                    Token = token
                };

                _ = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
            }
            catch (Exception ex)
            {
                _logger.LogErrorAsync(ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Send a firebase push notification to multiple devices.
        /// </summary>
        /// <returns></returns>
        public void SendNotificationMultipleDevices(List<string> tokens, string notificationTitle, string notificationMessage)
        {
            try
            {
                var message = new MulticastMessage()
                {
                    Notification = new Notification
                    {
                        Title = notificationTitle,
                        Body = notificationMessage
                    },
                    Tokens = tokens
                };

                _ = FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).Result;
            }
            catch (Exception ex)
            {
                _logger.LogErrorAsync(ex.Message, ex.StackTrace);
            }
        }
    }
}