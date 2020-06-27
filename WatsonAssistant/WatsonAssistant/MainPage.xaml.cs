using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WatsonAssistant
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            lbEntrar.IsVisible = true;
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {

            lbEntrar.IsVisible = false;
            var task = slContent.TranslateTo(0, -(slContent.Height) + 105, 1000);

            _ = Task.Run(async () =>
            {
                var p = new ChatPage();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await task;
                    Application.Current.MainPage = p;

                });
            });
        }
    }
}
