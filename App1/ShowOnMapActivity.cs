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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace App1
{
    [Activity(Label = "Puncture shop location", Icon = "@drawable/PunchApp1")]
    public class ShowOnMapActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap googleMap;
        private double _lat;
        private double _long;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShowOnMap);
            var latlongStr = Intent.Extras.GetString("item_extra");

            string[] strArr = null;
            char[] splitchar = { ',' };
            strArr = latlongStr.ToString().Split(splitchar);
            string strLat = strArr[0].ToString();
            string strLng = strArr[1].ToString();
            _lat = Double.Parse(strLat);
            _long = Double.Parse(strLng);
            SetUpMap();
            //LatLng location = new LatLng(Double.Parse(strLat), Double.Parse(strLng));            
        }

        private void SetUpMap()
        {            
            if (googleMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.my_mapfragment_container).GetMapAsync(this);                
            }
        }

        public void OnMapReady(GoogleMap map)
        {
            this.googleMap = map;
            googleMap.UiSettings.ZoomControlsEnabled = true;
            LatLng location = new LatLng(_lat,_long);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(location, 15);
            googleMap.MoveCamera(camera);
            MarkerOptions options = new MarkerOptions()
                .SetPosition(location)
                ;  //.SetTitle("Bangalore")
            googleMap.AddMarker(options);            
        }
    }    
}