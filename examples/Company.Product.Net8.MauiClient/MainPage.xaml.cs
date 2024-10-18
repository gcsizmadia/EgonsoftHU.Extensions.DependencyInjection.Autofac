// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace Company.Product.Net8.MauiClient
{
    public partial class MainPage : ContentPage
    {
        private int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            CounterBtn.Text =
                count == 1
                    ? $"Clicked {count} time"
                    : $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
