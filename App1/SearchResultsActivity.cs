using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using System.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App1
{
    [Activity(Label = "Puncture Shops", Icon = "@drawable/PunchApp1")]
    public class SearchResultsActivity : Activity
    {
        ListView listView;
        RootObject root = null;
        List<TableItem> tableItems = null;
        TableItem item = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchResults);
            listView = FindViewById<ListView>(Resource.Id.ListView1);
            var addressListStr = Intent.Extras.GetString("item_extra");
            try
            {
                root = JsonConvert.DeserializeObject<RootObject>(addressListStr.ToString());
                tableItems = new List<TableItem>();
                foreach (var result in root.results)
                {
                    double _latitude = result.geometry.location.lat;
                    double _longitude = result.geometry.location.lng;

                    item = new TableItem();
                    item.ShopName = result.name;
                    item.Address = result.vicinity;
                    item.Rating = result.rating.ToString();
                    item.Latitude = _latitude;
                    item.Longitude = _longitude;

                    tableItems.Add(item);
                }

                // populate the listview with data
                listView.Adapter = new HomeScreenAdapter(this, tableItems);
                listView.ItemClick += OnListItemClick;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = tableItems[e.Position];
            Android.Widget.Toast.MakeText(this, t.Address, Android.Widget.ToastLength.Short).Show();

            //using google map api
            string latlong = t.Latitude + "," + t.Longitude;
            var activity2 = new Intent(this, typeof(ShowOnMapActivity)); //addresslistactivity
            activity2.PutExtra("item_extra", latlong.ToString());
            StartActivity(activity2);

            //#region Google Map application
            //string geoLocation = "geo:" + t.Latitude + "," + t.Longitude + "?z=15";
            //var geoUri = Android.Net.Uri.Parse(geoLocation);
            //var mapIntent = new Intent(Intent.ActionView, geoUri);
            //StartActivity(mapIntent);
            //#endregion
        }
    }
}