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
using System.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App1
{
    [Activity(Label = "PunchApp")]
    public class ListItemsActivity : Activity
    {
        ListView listView;
        List<TableItem> tableItems = new List<TableItem>();

        protected override void OnCreate(Bundle savedInstanceState)
        {            
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SearchResults); // loads the HomeScreen.axml as this activity's view
            listView = FindViewById<ListView>(Resource.Id.List); // get reference to the ListView in the layout

            var addressListStr = Intent.Extras.GetString("item_extra");
            JObject rss = JObject.Parse(addressListStr.ToString());
            var postTitles = from p in rss["results"]
                             select (string)p["name"] + System.Environment.NewLine
                             + "Address: " + (string)p["vicinity"] + System.Environment.NewLine
                             + "Rating: " + (string)p["rating"];

            foreach (var val in postTitles)
            {
                //ShowAlert(item);
                TableItem item = new TableItem();
                item.Heading = val;
                item.SubHeading = "SB";
                item.ImageResourceId = 1;
                tableItems.Add(item);
            }

            listView.Adapter = new HomeScreenAdapter(this, tableItems);
            listView.ItemClick += OnListItemClick;  // to be defined
        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = tableItems[e.Position];
            Android.Widget.Toast.MakeText(this, t.Heading, Android.Widget.ToastLength.Short).Show();
        }
    }
}