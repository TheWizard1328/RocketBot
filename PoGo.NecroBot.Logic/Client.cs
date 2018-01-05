#region using directives

using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Enums;
using POGOLib.Official;
using POGOLib.Official.Exceptions;
using POGOLib.Official.LoginProviders;
using POGOLib.Official.Net;
using POGOLib.Official.Net.Authentication;
using POGOLib.Official.Net.Authentication.Data;
using POGOLib.Official.Net.Captcha;
using POGOLib.Official.Util.Device;
using POGOLib.Official.Util.Hash;
using POGOLib.Official.Util.Hash.PokeHash;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Exceptions;
using static POGOProtos.Networking.Envelopes.Signature.Types;
using POGOProtos.Data;
using PoGo.NecroBot.Logic.Logging;
//using static POGOProtos.Networking.Envelopes.Signature.Types;

#endregion

namespace PoGo.NecroBot.Logic
{
    public class Client
    {
        public Version VersionStr;
        public uint AppVersion;
        public Session ClientSession;
        public bool LoggedIn = false;
        public GetPlayerMessage.Types.PlayerLocale PlayerLocale { get; set; }
        private DeviceWrapper ClientDeviceWrapper { get; set; }
        private WebProxy Proxy;

        public float CurrentSpeed { get; set; }

        public void Logout()
        {
            if (!LoggedIn)
                return;
            LoggedIn = false;
            ClientSession.AssetDigestUpdated -= OnAssetDisgestReceived;
            ClientSession.ItemTemplatesUpdated -= OnItemTemplatesReceived;
            ClientSession.UrlsUpdated -= OnDownloadUrlsReceived;
            ClientSession.LocalConfigUpdated -= OnLocalConfigVersionReceived;
            ClientSession.AccessTokenUpdated -= SessionAccessTokenUpdated;
            ClientSession.CaptchaReceived -= SessionOnCaptchaReceived;
            ClientSession.InventoryUpdate -= SessionInventoryUpdate;
            ClientSession.MapUpdate -= SessionMapUpdate;
            ClientSession.CheckAwardedBadgesReceived -= OnCheckAwardedBadgesReceived;
            ClientSession.HatchedEggsReceived -= OnHatchedEggsReceived;
            ClientSession.Shutdown();
        }

        public Client(ISettings settings)
        {
            SetSettings(settings);
        }

        public async Task<bool> DoLogin(ISettings settings)
        {
            SetSettings(settings);
            // TODO: see how do this only once better.
            if (!(Configuration.Hasher is PokeHashHasher))
            {
                // By default Configuration.Hasher is LegacyHasher type  (see Configuration.cs in the pogolib source code)
                // -> So this comparation only will run once.
                if (settings.UseCustomAPI)
                {
                    Configuration.Hasher = new PokeHashHasher(settings.AuthAPIKey);
                    Configuration.HasherUrl = new Uri(settings.UrlHashServices);
                    Configuration.HashEndpoint = settings.EndPoint;
                }
                else
                    Configuration.Hasher = new PokeHashHasher(settings.AuthAPIKey);

                // TODO: make this configurable. To avoid bans (may be with a checkbox in hash keys tab).
                //Configuration.IgnoreHashVersion = true;
                VersionStr = Configuration.Hasher.PokemonVersion;
                AppVersion = Configuration.Hasher.AppVersion;
                // TODO: Revise sleeping
                //((PokeHashHasher)Configuration.Hasher).PokehashSleeping += OnPokehashSleeping;
            }
            // *****

            ILoginProvider loginProvider;

            switch (settings.AuthType)
            {
                case AuthType.Google:
                    loginProvider = new GoogleLoginProvider(settings.Username, settings.Password);
                    break;
                case AuthType.Ptc:
                    loginProvider = new PtcLoginProvider(settings.Username, settings.Password, Proxy);
                    break;
                default:
                    throw new ArgumentException("Login provider must be either \"google\" or \"ptc\".");
            }

            ClientSession = await GetSession(loginProvider, settings.AccountLatitude, settings.AccountLongitude, true);

            // Send initial requests and start HeartbeatDispatcher.
            // This makes sure that the initial heartbeat request finishes and the "session.Map.Cells" contains stuff.
            var msgStr = "Session couldn't start up.";
            LoggedIn = false;
            try
            {
                ClientSession.AssetDigestUpdated += OnAssetDisgestReceived;
                ClientSession.ItemTemplatesUpdated += OnItemTemplatesReceived;
                ClientSession.UrlsUpdated += OnDownloadUrlsReceived;
                ClientSession.LocalConfigUpdated += OnLocalConfigVersionReceived;
                ClientSession.AccessTokenUpdated += SessionAccessTokenUpdated;
                ClientSession.CaptchaReceived += SessionOnCaptchaReceived;
                ClientSession.InventoryUpdate += SessionInventoryUpdate;
                ClientSession.MapUpdate += SessionMapUpdate;
                ClientSession.CheckAwardedBadgesReceived += OnCheckAwardedBadgesReceived;
                ClientSession.HatchedEggsReceived += OnHatchedEggsReceived;

                if (await ClientSession.StartupAsync(true))
                {
                    LoggedIn = true;
                    msgStr = "Successfully logged into server.";

                    //ClientManager.LogCaller(new LoggerEventArgs("Succefully added all events to the client.", LoggerTypes.Debug));

                    if (ClientSession.Player.Warn)
                    {
                        /*/ ClientManager.LogCaller(new LoggerEventArgs("The account is flagged.", LoggerTypes.Warning));

                         if (ClientManager.UserSettings.StopAtMinAccountState == AccountState.Flagged)
                         {
                             //Remove proxy
                             ClientManager.RemoveProxy();
                             ClientManager.Stop();

                             msgStr = "The account is flagged.";
                         }//*/
                    }

                    if (ClientSession.Player.Banned)
                    {
                        /*
                        ClientManager.AccountState = AccountState.PermAccountBan;
                        ClientManager.LogCaller(new LoggerEventArgs("The account is banned.", LoggerTypes.FatalError));

                        //Remove proxy
                        ClientManager.RemoveProxy();

                        ClientManager.Stop();

                        msgStr = "The account is banned.";*/
                    }

                    //Closes bot on captcha received need utils for solve

                    SaveAccessToken(ClientSession.AccessToken);
                }
            }
            catch (PtcOfflineException)
            {
                //ClientManager.Stop();

                //ClientManager.LogCaller(new LoggerEventArgs("Ptc server offline. Please try again later.", LoggerTypes.Warning));

                msgStr = "Ptc server offline.";
            }
            catch (AccountNotVerifiedException)
            {
                //ClientManager.Stop();
                //ClientManager.RemoveProxy();

                //ClientManager.LogCaller(new LoggerEventArgs("Account not verified. Stopping ...", LoggerTypes.Warning));

                //ClientManager.AccountState = Enums.AccountState.NotVerified;

                msgStr = "Account not verified.";
            }
            catch (WebException ex)
            {
                /*/ClientManager.Stop();

                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    //if (String.IsNullOrEmpty(ClientManager.Proxy))
                    //{
                    //    ClientManager.LogCaller(new LoggerEventArgs("Login request has timed out.", LoggerTypes.Warning));
                    //}
                   // else
                    //{
                   //     ClientManager._proxyIssue = true;
                   //     ClientManager.LogCaller(new LoggerEventArgs("Login request has timed out. Possible bad proxy.", LoggerTypes.ProxyIssue));
                  //  }

                    msgStr = "Request has timed out.";
                }

                if (!String.IsNullOrEmpty(ClientManager.Proxy))
                {
                    if (ex.Status == WebExceptionStatus.ConnectionClosed)
                    {
                        ClientManager._proxyIssue = true;
                        ClientManager.LogCaller(new LoggerEventArgs("Potential http proxy detected. Only https proxies will work.", LoggerTypes.ProxyIssue));

                        msgStr = "Http proxy detected";
                    }
                    else if (ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.ProtocolError || ex.Status == WebExceptionStatus.ReceiveFailure
                        || ex.Status == WebExceptionStatus.ServerProtocolViolation)
                    {
                        ClientManager._proxyIssue = true;
                        ClientManager.LogCaller(new LoggerEventArgs("Proxy is offline", LoggerTypes.ProxyIssue));

                        msgStr = "Proxy is offline";
                    }
                }

                ClientManager._proxyIssue |= !String.IsNullOrEmpty(ClientManager.Proxy);

                ClientManager.LogCaller(new LoggerEventArgs("Failed to login due to request error", LoggerTypes.Exception, ex.InnerException));
                */
                msgStr = "Failed to login due to request error" + ex;
            }
            catch (TaskCanceledException)
            {
                /* ClientManager.Stop();

                 if (String.IsNullOrEmpty(ClientManager.Proxy))
                 {
                     ClientManager.LogCaller(new LoggerEventArgs("Login request has timed out", LoggerTypes.Warning));
                 }
                 else
                 {
                     ClientManager._proxyIssue = true;
                     ClientManager.LogCaller(new LoggerEventArgs("Login request has timed out. Possible bad proxy", LoggerTypes.ProxyIssue));
                 }
                 */
                msgStr = "Login request has timed out";
            }
            catch (InvalidCredentialsException ex)
            {
                /*/Puts stopping log before other log.
                ClientManager.Stop();
                ClientManager.RemoveProxy();

                ClientManager.LogCaller(new LoggerEventArgs("Invalid credentials or account lockout. Stopping bot...", LoggerTypes.Warning, ex));
                */
                msgStr = "Username or password incorrect" + ex;
            }
            catch (IPBannedException)
            {
                /*
                if (ClientManager.UserSettings.StopOnIPBan)
                {
                    ClientManager.Stop();
                }

                string message = String.Empty;

                if (!String.IsNullOrEmpty(ClientManager.Proxy))
                {
                    if (ClientManager.CurrentProxy != null)
                    {
                        ClientManager.ProxyHandler.MarkProxy(ClientManager.CurrentProxy, true);
                    }

                    message = "Proxy IP is banned.";
                }
                else
                {
                    message = "IP address is banned.";
                }

                ClientManager._proxyIssue = true;

                ClientManager.LogCaller(new LoggerEventArgs(message, LoggerTypes.ProxyIssue));
                */
                //msgStr = message;
            }
            catch (GoogleLoginException ex)
            {
                /* ClientManager.Stop();
                 ClientManager.RemoveProxy();

                 ClientManager.LogCaller(new LoggerEventArgs(ex.Message, LoggerTypes.Warning));
                 */
                msgStr = "Failed to login" + ex;
            }
            catch (PokeHashException)
            {
                //ClientManager.AccountState = AccountState.HashIssues;

                msgStr = "Hash issues";
            }
            catch (Exception ex)
            {
                //ClientManager.Stop();
                //RemoveProxy();

                //ClientManager.LogCaller(new LoggerEventArgs("Failed to login", LoggerTypes.Exception, ex));

                msgStr = "Failed to login" + ex;
            }

            Logger.Write(msgStr, LogLevel.Info);
            return LoggedIn;
        }

        private void OnAssetDisgestReceived(object sender, List<POGOProtos.Data.AssetDigestEntry> data)
        {
             var filename = "data/" + ClientSession.Device.DeviceInfo.DeviceId + "_AD.json";
             if (!Directory.Exists("data"))
                 Directory.CreateDirectory("data");
             if (File.Exists(filename))
                 File.Delete(filename);
             File.WriteAllText(filename, JsonConvert.SerializeObject(data));     
        }

        private void OnItemTemplatesReceived(object sender, List<DownloadItemTemplatesResponse.Types.ItemTemplate> data)
        {
            var filename = "data/" + ClientSession.Device.DeviceInfo.DeviceId + "_IT.json";
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
            if (File.Exists(filename))
                File.Delete(filename);
            File.WriteAllText(filename, JsonConvert.SerializeObject(data));
        }

        private void OnDownloadUrlsReceived(object sender, List<POGOProtos.Data.DownloadUrlEntry> data)
        {
            var filename = "data/" + ClientSession.Device.DeviceInfo.DeviceId + "_UR.json";
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
            if (File.Exists(filename))
                File.Delete(filename);
            File.WriteAllText(filename, JsonConvert.SerializeObject(data));            
        }

        private void OnLocalConfigVersionReceived(object sender, DownloadRemoteConfigVersionResponse data)
        {           
            var filename = "data/" + ClientSession.Device.DeviceInfo.DeviceId + "_LCV.json";
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
            if (File.Exists(filename))
                File.Delete(filename);
            File.WriteAllText(filename, JsonConvert.SerializeObject(data));            
        }

        private event EventHandler<int> OnPokehashSleeping;

        private void PokehashSleeping(object sender, int sleepTime)
        {
            OnPokehashSleeping?.Invoke(sender, sleepTime);
        }

        private void SessionMapUpdate(object sender, EventArgs e)
        {
            // Update BuddyPokemon Stats
            //var msg = $"BuddyWalked Candy: {ClientSession.Player.BuddyCandy}";
            //ClientManager.LogCaller(new LoggerEventArgs(msg, LoggerTypes.Success));
        }

        public void SessionOnCaptchaReceived(object sender, CaptchaEventArgs e)
        {
            //ClientManager.AccountState = AccountState.CaptchaReceived;
            //2captcha needed to solve or chrome drive for solve url manual
            //e.CaptchaUrl;
        }

        private void SessionInventoryUpdate(object sender, EventArgs e)
        {
            //ClientManager.UpdateInventory();
        }

        private void OnHatchedEggsReceived(object sender, GetHatchedEggsResponse hatchedEggResponse)
        {
            //
        }

        private void OnCheckAwardedBadgesReceived(object sender, CheckAwardedBadgesResponse e)
        {
            //
        }

        private void SessionAccessTokenUpdated(object sender, EventArgs e)
        {
            SaveAccessToken(ClientSession.AccessToken);
        }

        public void SetSettings(ISettings settings)
        {

            int osId = OsVersions[settings.FirmwareType.Length].Length;
            var firmwareUserAgentPart = OsUserAgentParts[osId];
            var firmwareType = OsVersions[osId];

            if (settings.UseProxy)
            {
                var userproxy = new NetworkCredential();
                if (settings.UseProxyAuthentication)
                    userproxy = new NetworkCredential
                    {
                        UserName = settings.UseProxyUsername,
                        Password = settings.UseProxyPassword
                    };

                Proxy = new WebProxy
                {
                    Address = new Uri(settings.UseProxyHost + ":" + settings.UseProxyPort),
                    Credentials = userproxy
                };
            }
            else
                Proxy = new WebProxy();

            ClientDeviceWrapper = new DeviceWrapper
            {
                UserAgent = $"pokemongo/1 {firmwareUserAgentPart}",
                DeviceInfo = new DeviceInfo
                {
                    DeviceId = settings.DeviceId,
                    DeviceBrand = settings.DeviceBrand,
                    DeviceModel = settings.DeviceModel,
                    DeviceModelBoot = settings.DeviceModelBoot,
                    HardwareManufacturer = settings.HardwareManufacturer,
                    HardwareModel = settings.HardwareModel,
                    FirmwareBrand = settings.FirmwareBrand,
                    FirmwareType = settings.FirmwareType
                },
                Proxy = Proxy
            };

            PlayerLocale = new GetPlayerMessage.Types.PlayerLocale
            {
                Country = settings.Country,
                Language = settings.Language,
                Timezone = settings.TimeZone
            };

        }

        private void SaveAccessToken(AccessToken accessToken)
        {
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), "Cache", $"{accessToken.Uid}.json");

            File.WriteAllText(fileName, JsonConvert.SerializeObject(accessToken, Formatting.Indented));
        }

        /// <summary>
        /// Login to PokémonGo and return an authenticated <see cref="ClientSession" />.
        /// </summary>
        /// <param name="loginProvider">Provider must be PTC or Google.</param>
        /// <param name="initLat">The initial latitude.</param>
        /// <param name="initLong">The initial longitude.</param>
        /// <param name="mayCache">Can we cache the <see cref="AccessToken" /> to a local file?</param>
        private async Task<Session> GetSession(ILoginProvider loginProvider, double initLat, double initLong, bool mayCache = false)
        {
            var cacheDir = Path.Combine(Directory.GetCurrentDirectory(), "Cache");
            var fileName = Path.Combine(cacheDir, $"{loginProvider.UserId}-{loginProvider.ProviderId}.json");

            if (mayCache)
            {
                if (!Directory.Exists(cacheDir))
                    Directory.CreateDirectory(cacheDir);

                if (File.Exists(fileName))
                {
                    var accessToken = JsonConvert.DeserializeObject<AccessToken>(File.ReadAllText(fileName));

                    if (!accessToken.IsExpired)
                        return Login.GetSession(loginProvider, accessToken, initLat, initLong, ClientDeviceWrapper, PlayerLocale);
                }
            }

            var session = await Login.GetSession(loginProvider, initLat, initLong, ClientDeviceWrapper, PlayerLocale);

            //My files resources here       
            var filename = "data/" + session.Device.DeviceInfo.DeviceId + "_IT.json";
            if (File.Exists(filename))
                session.Templates.ItemTemplates = JsonConvert.DeserializeObject<List<DownloadItemTemplatesResponse.Types.ItemTemplate>>(File.ReadAllText(filename));
            filename = "data/" + session.Device.DeviceInfo.DeviceId + "_UR.json";
            if (File.Exists(filename))
                session.Templates.DownloadUrls = JsonConvert.DeserializeObject<List<DownloadUrlEntry>>(File.ReadAllText(filename));
            filename = "data/" + session.Device.DeviceInfo.DeviceId + "_AD.json";
            if (File.Exists(filename))
                session.Templates.AssetDigests = JsonConvert.DeserializeObject<List<AssetDigestEntry>>(File.ReadAllText(filename));
            filename = "data/" + session.Device.DeviceInfo.DeviceId + "_LCV.json";
            if (File.Exists(filename))
                session.Templates.LocalConfigVersion = JsonConvert.DeserializeObject<DownloadRemoteConfigVersionResponse>(File.ReadAllText(filename));
            //*/

            if (mayCache)
                SaveAccessToken(session.AccessToken);

            return session;
        }

        private readonly string[] OsUserAgentParts = {
            "CFNetwork/758.0.2 Darwin/15.0.0",  // 9.0
            "CFNetwork/758.0.2 Darwin/15.0.0",  // 9.0.1
            "CFNetwork/758.0.2 Darwin/15.0.0",  // 9.0.2
            "CFNetwork/758.1.6 Darwin/15.0.0",  // 9.1
            "CFNetwork/758.2.8 Darwin/15.0.0",  // 9.2
            "CFNetwork/758.2.8 Darwin/15.0.0",  // 9.2.1
            "CFNetwork/758.3.15 Darwin/15.4.0", // 9.3
            "CFNetwork/758.4.3 Darwin/15.5.0",  // 9.3.2
            "CFNetwork/807.2.14 Darwin/16.3.0", // 10.3.3
            "CFNetwork/889.3 Darwin/17.2.0",    // 11.1.0
            "CFNetwork/893.10 Darwin/17.3.0",   // 11.2.0
            "CFNetwork/893.14.2 Darwin/17.4.0"  // 11.2.5
        };

        private static readonly string[][] Devices =
        {
            new[] {"iPad5,1", "iPad", "J96AP"},
            new[] {"iPad5,2", "iPad", "J97AP"},
            new[] {"iPad5,3", "iPad", "J81AP"},
            new[] {"iPad5,4", "iPad", "J82AP"},
            new[] {"iPad6,7", "iPad", "J98aAP"},
            new[] {"iPad6,8", "iPad", "J99aAP"},
            new[] {"iPhone5,1", "iPhone", "N41AP"},
            new[] {"iPhone5,2", "iPhone", "N42AP"},
            new[] {"iPhone5,3", "iPhone", "N48AP"},
            new[] {"iPhone5,4", "iPhone", "N49AP"},
            new[] {"iPhone6,1", "iPhone", "N51AP"},
            new[] {"iPhone6,2", "iPhone", "N53AP"},
            new[] {"iPhone7,1", "iPhone", "N56AP"},
            new[] {"iPhone7,2", "iPhone", "N61AP"},
            new[] {"iPhone8,1", "iPhone", "N71AP"},
            new[] {"iPhone8,2", "iPhone", "MKTM2"}, //iphone 6s plus
            new[] {"iPhone9,3", "iPhone", "MN9T2"}  //iphone 7
        };

        private static readonly string[] OsVersions = {
            "9.0",
            "9.0.1",
            "9.0.2",
            "9.1",
            "9.2",
            "9.2.1",
            "9.3",
            "9.3.2",
            "10.3.3",
            "11.1.0",
            "11.2.0",
            "11.2.5"
        };

        private Random randomizer = new Random();
        public async Task RandomAPICall()
        {
            await Task.Delay(0); //remove warn
            var apiIndex = randomizer.Next(0, 3);

            switch (apiIndex)
            {
                case 1:
                    //await Client.Inventory.GetInventory().ConfigureAwait(false);
                    break;

                case 2:
                    //await Client.Player.CheckChallenge().ConfigureAwait(false);
                    break;

                case 3:
                    //await Client.Player.GetNewlyAwardedBadges().ConfigureAwait(false);
                    break;
                case 4:
                    //await Client.Player.GetPlayerProfile().ConfigureAwait(false);
                    break;
                default:
                    break;
            }
        }

    }
}
