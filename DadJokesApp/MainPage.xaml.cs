﻿using DadJokesApp.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace DadJokesApp
{
    public partial class MainPage : ContentPage
    {

        private string _latestjoke;

        public string LatestJoke
        {
            get { return _latestjoke; }
            set
            {
                _latestjoke = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewJokeCommand { get; set; }

        private HttpClient _client;

        public MainPage()
        {
            InitializeComponent();
            NewJokeCommand = new Command(GetLatestJoke);
            BindingContext = this;
            _client = new HttpClient();
            // set up headers 
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async void GetLatestJoke( object parameters) 
        {
            //this will get info from the internet
          string response = await _client.GetStringAsync(new Uri("https://icanhazdadjoke.com/"));
          
            // converts this to a dad joke 
            DadJoke latestJoke = JsonConvert.DeserializeObject<DadJoke>(response);
            if(latestJoke!= null) 
            {
                LatestJoke = latestJoke.joke;            
            }
            
        }
        public async Task ShareText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && text == LatestJoke)
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = $"Dad Joke 101: {text}",
                    Title = "Dad Joke:"
                });
            }
            else
            {
                // Display an error message or take appropriate action
                
                await DisplayAlert("Error 404", "Dad Joke Not Found, Please Have A Laugh!", "OK");
            }
        }
        private async void OnShareButtonClicked(object sender, EventArgs e)
        {
            await ShareText(LatestJoke);
        }

    }

}
