using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonAssistant.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatsonAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        ChatBotViewModel vm;
        public ChatPage()
        {
            InitializeComponent();

            BindingContext = vm = new ChatBotViewModel();

            vm.Messages.CollectionChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var target = vm.Messages[vm.Messages.Count - 1];
                    MessagesList.ScrollTo(target, ScrollToPosition.End, true);
                });
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.iBMWatsonAssistant.CreateSession();
        }
    }
}