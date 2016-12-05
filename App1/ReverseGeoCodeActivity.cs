using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;

namespace App1
{
    [Activity(Label = "ReverseGeoCodeActivity")]
    public class ReverseGeoCodeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GeoCoderLayout);
            var button = FindViewById<Button>(Resource.Id.revGeocodeButton);
            button.Click += async (sender, e) => {
                var geo = new Geocoder(this);
                var addresses = await geo.GetFromLocationAsync(42.37419, -71.120639, 1);

                var addressText = FindViewById<TextView>(Resource.Id.addressText);
                if (addresses.Any())
                {
                    addresses.ToList().ForEach(addr => addressText.Append(addr + System.Environment.NewLine + System.Environment.NewLine));
                }
                else
                {
                    addressText.Text = "Could not find any addresses.";
                }
            };
        }
    }
}