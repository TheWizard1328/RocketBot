using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AeroWizard;
using Google.Protobuf;
using Google.Protobuf.Collections;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data.Player;
using POGOProtos.Enums;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Forms
{
    public partial class InitialTutorialForm : System.Windows.Forms.Form
    {
        private ISession session;
        private RepeatedField<TutorialState> tutState;
        CheckTosState state;

        public InitialTutorialForm()
        {
            InitializeComponent();
        }

        public InitialTutorialForm(CheckTosState s, RepeatedField<TutorialState> tutState, ISession session)
        {
            InitializeComponent();
            state = s;
            this.tutState = tutState;
            this.session = session;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
        }

        private void WizardPage4_Initialize(object sender, WizardPageInitEventArgs e)
        {
            Task.Run(async () =>
                {
                    if (!tutState.Contains(TutorialState.AvatarSelection))
                    {
                        int gender = rdoMale.Checked ? 0 : 1;

                        var avatarRes = new PlayerAvatar()
                        {
                            Backpack = 0,
                            Eyes = 0,
                            Avatar = gender,
                            Hair = 0,
                            Hat = 0,
                            Pants = 0,
                            Shirt = 0,
                            Shoes = 0,
                            Skin = 0
                        };

                        var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                        {
                            RequestType = RequestType.SetAvatar,
                            RequestMessage = new SetAvatarMessage
                            {
                                PlayerAvatar = avatarRes
                            }.ToByteString()
                        }, true);

                        SetAvatarResponse setAvatarResponse = null;

                        setAvatarResponse = SetAvatarResponse.Parser.ParseFrom(response);

                        if (setAvatarResponse.Status == SetAvatarResponse.Types.Status.AvatarAlreadySet ||
                            setAvatarResponse.Status == SetAvatarResponse.Types.Status.Success)
                        {
                            var tuto = new RepeatedField<TutorialState> { TutorialState.AvatarSelection };

                            var resp = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                            {
                                RequestType = RequestType.MarkTutorialComplete,
                                RequestMessage = new MarkTutorialCompleteMessage
                                {
                                   SendMarketingEmails = false,
                                   SendPushNotifications = false,
                                   TutorialsCompleted = { tuto }
                                }.ToByteString()
                            }, true);

                            MarkTutorialCompleteResponse markTutorialCompleteResponse = null;

                            markTutorialCompleteResponse = MarkTutorialCompleteResponse.Parser.ParseFrom(resp);

                            if (markTutorialCompleteResponse.Success)
                            {
                                session.EventDispatcher.Send(new NoticeEvent()
                                {
                                    Message = $"Selected your avatar, now you are {gender}!"
                                });

                                Invoke(new Action(() =>
                                {
                                    wizardControl1.NextPage();
                                }), null);

                                return true;
                            }
                        }

                        Invoke(new Action(() =>
                        {
                            lblNameError.Text = "Error selecting avatar gender!";
                            lblNameError.Visible = true;
                            wizardControl1.PreviousPage();
                        }));
                    }
                    return true;
                });
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        ///
        /// Handling the window messages
        ///
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int) message.Result == HTCLIENT)
                message.Result = (IntPtr) HTCAPTION;
        }

        private void WizardPage3_Initialize(object sender, WizardPageInitEventArgs e)
        {
            PokemonId firstPoke = rdoBulbasaur.Checked
                ? PokemonId.Bulbasaur
                : rdoCharmander.Checked
                    ? PokemonId.Charmander
                    : PokemonId.Squirtle;
                    //TODO: nedd pikachu;

            Task.Run(async () =>
                {
                    if (!tutState.Contains(TutorialState.PokemonCapture))
                    {
                        var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                        {
                            RequestType = RequestType.EncounterTutorialComplete,
                            RequestMessage = new EncounterTutorialCompleteMessage
                            {
                                PokemonId = firstPoke
                            }.ToByteString()
                        }, true);

                        EncounterTutorialCompleteResponse encounterTutorialCompleteResponse = null;

                        encounterTutorialCompleteResponse = EncounterTutorialCompleteResponse.Parser.ParseFrom(response);

                        if (encounterTutorialCompleteResponse.Result == EncounterTutorialCompleteResponse.Types.Result.Success)
                        {
                            session.EventDispatcher.Send(new NoticeEvent()
                            {
                                Message = $"Caught Tutorial pokemon! it's {firstPoke}!"
                            });

                            Invoke(new Action(() =>
                            {
                                wizardControl1.NextPage();
                            }), null);
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                lblNameError.Text = "Error catching tutorial pokemon.";
                                lblNameError.Visible = true;
                                wizardControl1.PreviousPage();
                            }));
                        }
                    }
                });
        }

        private void WizardPage6_Initialize(object sender, WizardPageInitEventArgs e)
        {
            string nickname = txtNick.Text;
            
            bool markTutorialComplete = false;
            string errorText = null;
            string warningText = null;
            string infoText = null;
            Task.Run(async () =>
                {
                    if (!tutState.Contains(TutorialState.NameSelection))
                    {
                        var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                        {
                            RequestType = RequestType.ClaimCodename,
                            RequestMessage = new ClaimCodenameMessage
                            {
                                Codename = nickname,
                                //Force = true
                            }.ToByteString()
                        }, true);

                        ClaimCodenameResponse claimCodenameResponse = null;

                        claimCodenameResponse = ClaimCodenameResponse.Parser.ParseFrom(response);

                        switch (claimCodenameResponse.Status)
                        {
                            case ClaimCodenameResponse.Types.Status.Unset:
                                errorText = "Unset, somehow";
                                break;
                            case ClaimCodenameResponse.Types.Status.Success:
                                infoText = $"Your name is now: {claimCodenameResponse.Codename}";
                                markTutorialComplete = true;
                                break;
                            case ClaimCodenameResponse.Types.Status.CodenameNotAvailable:
                                errorText = $"That nickname ({nickname}) isn't available, pick another one!";
                                break;
                            case ClaimCodenameResponse.Types.Status.CodenameNotValid:
                                errorText = $"That nickname ({nickname}) isn't valid, pick another one!";
                                break;
                            case ClaimCodenameResponse.Types.Status.CurrentOwner:
                                warningText = $"You already own that nickname!";
                                markTutorialComplete = true;
                                break;
                            case ClaimCodenameResponse.Types.Status.CodenameChangeNotAllowed:
                                warningText = "You can't change your nickname anymore!";
                                markTutorialComplete = true;
                                break;
                            default:
                                errorText = "Unknown Niantic error while changing nickname.";
                                break;
                        }
                        if (!string.IsNullOrEmpty(infoText))
                        {
                            session.EventDispatcher.Send(new NoticeEvent()
                            {
                                Message = infoText
                            });
                        }
                        else if (!string.IsNullOrEmpty(warningText))
                        {
                            session.EventDispatcher.Send(new WarnEvent()
                            {
                                Message = warningText
                            });
                        }
                        else if (!string.IsNullOrEmpty(errorText))
                        {
                            session.EventDispatcher.Send(new ErrorEvent()
                            {
                                Message = errorText
                            });
                        }

                        if (markTutorialComplete)
                        {
                            var tuto = new RepeatedField<TutorialState> { TutorialState.NameSelection };

                            var rsp = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                            {
                                RequestType = RequestType.MarkTutorialComplete,
                                RequestMessage = new MarkTutorialCompleteMessage
                                {
                                   SendMarketingEmails = false,
                                   SendPushNotifications = false,
                                   TutorialsCompleted = { tuto }
                                }.ToByteString()
                            }, true);

                            MarkTutorialCompleteResponse markTutorialCompleteResponse = null;

                            markTutorialCompleteResponse = MarkTutorialCompleteResponse.Parser.ParseFrom(rsp);

                            if (markTutorialCompleteResponse.Success)
                            {
                                if (!tutState.Contains(TutorialState.FirstTimeExperienceComplete))
                                {
                                    var tutos = new RepeatedField<TutorialState> { TutorialState.FirstTimeExperienceComplete, TutorialState.PokemonBerry };

                                    var tuts = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                                    {
                                        RequestType = RequestType.MarkTutorialComplete,
                                        RequestMessage = new MarkTutorialCompleteMessage
                                        {
                                            SendMarketingEmails = false,
                                            SendPushNotifications = false,
                                            TutorialsCompleted = { tutos }
                                        }.ToByteString()
                                    }, true);

                                    MarkTutorialCompleteResponse marktutos = null;

                                    marktutos = MarkTutorialCompleteResponse.Parser.ParseFrom(tuts);

                                    if (marktutos.Success)
                                    {
                                        session.EventDispatcher.Send(new NoticeEvent()
                                        {
                                            Message = "First time experience complete, looks like i just spinned an virtual pokestop :P"
                                        });

                                        Invoke(new Action(() =>
                                        {
                                            DialogResult = DialogResult.OK;
                                            Close();
                                        }));

                                        return;
                                    }
                                }
                            }
                        }

                        Invoke(new Action(() =>
                        {
                            lblNameError.Text = errorText;
                            lblNameError.Visible = true;
                            wizardControl1.PreviousPage();
                        }));
                    }
                });
        }

        private void WizardPage1_Initialize(object sender, WizardPageInitEventArgs e)
        {
            if (tutState.Contains(TutorialState.AvatarSelection))
            {
                wizardControl1.NextPage(Step2);
            }
        }

        private void WizardPage2_Initialize(object sender, WizardPageInitEventArgs e)
        {
            if (tutState.Contains(TutorialState.PokemonCapture))
            {
                wizardControl1.NextPage(Step3);
            }
        }

        private void WizardPage5_Initialize(object sender, WizardPageInitEventArgs e)
        {
            if (tutState.Contains(TutorialState.NameSelection))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}