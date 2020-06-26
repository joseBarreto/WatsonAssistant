using System;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using WatsonAssistant.Services;

namespace WatsonAssistant.Models
{
    public class ChatBotViewModel : BindableObject
    {
        private string _outGoingText;
        public ObservableCollection<ChatMessage> Messages { get; }

        public readonly IBMWatsonAssistant iBMWatsonAssistant;

        public ChatBotViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            OutGoingText = string.Empty;
            iBMWatsonAssistant = new IBMWatsonAssistant();

            
        }

        ~ChatBotViewModel()
        {
            iBMWatsonAssistant.DeleteSession();
        }


        public string OutGoingText
        {
            get
            {
                return _outGoingText;
            }
            set
            {
                _outGoingText = value;
                OnPropertyChanged();
            }
        }



        public ICommand SendCommand => new Command(SendMessage);

        private async void SendMessage()
        {
            if (!string.IsNullOrEmpty(OutGoingText))
            {
                Messages.Add(new ChatMessage { Text = OutGoingText, IsIncoming = false, MessageDateTime = DateTime.Now });
                string temp = OutGoingText;
                OutGoingText = string.Empty;

                await Task.Run(() =>
                {
                    var res = iBMWatsonAssistant.Message(temp);
                    OnWatsonMessagerecieved(JsonConvert.SerializeObject(res, Formatting.Indented));
                });
            }
        }


        private void OnWatsonMessagerecieved(string data)
        {
            var message = JsonConvert.DeserializeObject<WatsonMessage>(data);

            if (message.Output.Generic != null)
            {
                foreach (var item in message.Output.Generic)
                {
                    var listItem = new ChatMessage
                    {
                        IsIncoming = true,
                        MessageDateTime = DateTime.Now
                    };

                    if (item.ResponseType.Equals("image"))
                    {
                        listItem.Image = item.Source.ToString();
                    }
                    if (item.ResponseType.Equals("text"))
                    {
                        listItem.Text = item.Text;
                    }
                    if (item.ResponseType.Equals("option"))
                    {
                        listItem.Text += Environment.NewLine + item.Title;
                        item.Options.ForEach(option => listItem.Text += $"{Environment.NewLine}{option.label}");
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Messages.Add(listItem);
                    });
                }

            }

        }

    }
}
