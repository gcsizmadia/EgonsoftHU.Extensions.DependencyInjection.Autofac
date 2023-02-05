// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Microsoft.Maui.Controls;

namespace Company.Product.Net7.MauiClient.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnNavigateToDependencyInjectionTestPageClickedAsync(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(DependencyInjectionTestPage)}");
        }
    }
}
