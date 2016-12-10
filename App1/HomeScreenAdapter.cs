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
using Android.Gms.Maps.Model;

namespace App1
{
    public class HomeScreenAdapter : BaseAdapter<TableItem>
    {
        List<TableItem> items;
        Activity context;
        private string _remarks = string.Empty;

        public HomeScreenAdapter(Activity context, List<TableItem> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TableItem this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);

            //calculate distance from current location
            var coord1 = new LatLng(item.Latitude, item.Longitude);
            var coord2 = new LatLng(item.CurrentLatitude, item.CurrentLongitude);
            var distanceInRadius = UtilityHelper.HaversineDistance(coord1, coord2, UtilityHelper.DistanceUnit.Kilometers);
            _remarks = string.Format("{0} kilometers away from your location.", Math.Round(distanceInRadius,2));

            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.ShopName;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = "Address: " + item.Address;
            view.FindViewById<TextView>(Resource.Id.Text3).Text = "Rating: " + item.Rating;
            //view.FindViewById<TextView>(Resource.Id.Text4).Text = "Latitude: " + item.Latitude.ToString();
            //view.FindViewById<TextView>(Resource.Id.Text5).Text = "Longitude: " + item.Longitude.ToString();
            view.FindViewById<TextView>(Resource.Id.Text6).Text = _remarks;

            return view;
        }
    }
}