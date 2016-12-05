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
    [Activity(Label = "@string/addressList")]
    public class AddressListActivity : ListActivity
    {
        RootObject root = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.Layout1);
            //ListView _listView1 = FindViewById<ListView>(Resource.Id.listView1);
            var addressListStr = Intent.Extras.GetString("item_extra");

            List<string> list = new List<string>();

            //working code using JSON to LINQ
            //#region Working code using JSON to LINQ
            //JObject rss = JObject.Parse(addressListStr.ToString());
            //var postTitles = from p in rss["results"]
            //                 select (string)p["name"] + System.Environment.NewLine
            //                 + "Address: " + (string)p["vicinity"] + System.Environment.NewLine
            //                 + "Rating: " + (string)p["rating"];
            //foreach (var item in postTitles)
            //{
            //    //ShowAlert(item);
            //    list.Add(item.ToString());
            //}
            //#endregion

            
            try
            {
                root = JsonConvert.DeserializeObject<RootObject>(addressListStr.ToString());

                foreach (var result in root.results)
                {
                    double _latitude = result.geometry.location.lat;
                    double _longitude = result.geometry.location.lng;
                    
                    list.Add(result.name + System.Environment.NewLine
                                     + "Address: " + result.vicinity + System.Environment.NewLine
                                     + "Rating: " + result.rating + System.Environment.NewLine
                                     + "Latitude: " + _latitude.ToString() + System.Environment.NewLine
                                     + "Longitude: " + _longitude.ToString());
                }
                //JArray 
                this.ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void GetJsonArrayList(JsonValue jsonStr)
        {
            JArray arrayList = new JArray();

            var model = JsonConvert.DeserializeObject<List<RootObject>>(jsonStr);

            foreach (var singleResult in model)
            {
                foreach (var result in singleResult.results)
                {
                    string name = result.name.ToString();
                    ShowAlert(name);
                    //arrayList.Add(name);
                }
            }
            //return arrayList;
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

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            string selection = l.GetItemAtPosition(position).ToString();
            Android.Widget.Toast.MakeText(this, selection, Android.Widget.ToastLength.Short).Show();
        }
    }
}