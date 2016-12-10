using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Xml.Linq;
using Android.Content;
using Android.Content.Res;
using System.Net;
using System.IO;
using System.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App1
{
    [Activity(Label = "PunctApp", MainLauncher = true, Icon = "@drawable/PunchApp1")]
    public class MainActivity : Activity, ILocationListener
    {
        static readonly string TAG = "X:" + typeof(MainActivity).Name;
        TextView _addressText;
        Location _currentLocation;
        LocationManager _locationManager;       
        Button _addressListButton;
        Button _currentLocationButton;
        string _locationProvider;
        TextView _locationText;
        TextView _jsonStr;
        List<string> phoneNumbers = new List<string>();
        RootObject root = new RootObject();

        protected override void OnCreate(Bundle bundle)
        {            
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _addressText = FindViewById<TextView>(Resource.Id.address_text);
            _locationText = FindViewById<TextView>(Resource.Id.location_text);
            _jsonStr = FindViewById<TextView>(Resource.Id.textView1);

            _addressListButton = FindViewById<Button>(Resource.Id.AddressButton);
            _currentLocationButton = FindViewById<Button>(Resource.Id.get_address_button);
            FindViewById<Button>(Resource.Id.get_address_button).Click += get_address_button_OnClick;

            InitializeLocationManager();

            _currentLocationButton.Click += _currentLocationButton_Click;

            //string jsonReturnStr = ParseJsonFile();
            //ShowAlert(jsonReturnStr.ToString());

            _addressListButton.Click += async (Object sender, EventArgs e) =>
            {
                double mLatitude = _currentLocation.Latitude;  // 12.9716;  ////12.894699;  // 
                double mLongitude = _currentLocation.Longitude; //77.596270;  // //77.5946; //

                string _serviceUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?" +
                                "location=" + mLatitude + "," + mLongitude +
                                "&radius=1000" +
                                "&types=car_repair|car_wash|bicycle_store" +
                                "&key=AIzaSyBZXZnnuOQZnQ80KvsRwChI7CnXQFdqW0s";
                //   -- old API Key

                //ShowAlert(_serviceUrl);

                JsonValue jsonReturnStr = await FetchPlacesAsync(_serviceUrl);

                //ShowAlert(jsonReturnStr.ToString());

                //root = JsonConvert.DeserializeObject<RootObject>(jsonReturnStr.ToString());
                //string jsonReturnStr = ParseJsonFile();

                #region WORKING CODE
                var activity2 = new Intent(this, typeof(SearchResultsActivity)); //AddressListActivity
                activity2.PutExtra("item_extra", jsonReturnStr.ToString());
                activity2.PutExtra("current_lat", mLatitude.ToString());
                activity2.PutExtra("current_lng", mLongitude.ToString());
                StartActivity(activity2);
                #endregion
                
            };
        }

        private async void _currentLocationButton_Click(object sender, EventArgs e)
        {
            if (_currentLocation == null)
            {
                _addressText.Text = "Can't determine the current address. Try again in a few minutes.";
                return;
            }

            Address address = await ReverseGeocodeCurrentLocation();
            DisplayAddress(address);
        }

        private async Task<JsonValue> FetchPlacesAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Use this stream to build a JSON document object:
                        JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                        // Return the JSON document:
                        return jsonDoc;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        private string ParseJsonFile()
        {            
            string jsonStr;
            AssetManager assets = this.Assets;
            using (StreamReader r = new StreamReader(assets.Open("GoogleData.json")))
            {
                string json = r.ReadToEnd();
                jsonStr = json;
            }
            return jsonStr;
        }

        public void ShowAlert(string str)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetMessage(str);
            alert.SetTitle("Response");
            alert.SetPositiveButton("OK", (senderAlert, args) => {
                // write your own set of instructions
            });

            //run the alert in UI thread to display in the screen
            RunOnUiThread(() => {
                alert.Show();
            });
        }

        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            Log.Debug(TAG, "Using " + _locationProvider + ".");
        }

        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _locationText.Text = "Unable to determine your location. Try again in a short while.";
            }
            else
            {
                _locationText.Text = string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude);
                Address address = await ReverseGeocodeCurrentLocation();
                DisplayAddress(address);
            }
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        async void get_address_button_OnClick(object sender, EventArgs eventArgs)
        {
            if (_currentLocation == null)
            {
                _addressText.Text = "Can't determine the current address. Try again in a few minutes.";
                return;
            }

            Address address = await ReverseGeocodeCurrentLocation();
            DisplayAddress(address);
        }

        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                // Remove the last comma from the end of the address.
                _addressText.Text = deviceAddress.ToString();
            }
            else
            {
                _addressText.Text = "Unable to determine the address. Try again in a few minutes.";
            }
        }        
    }
}

