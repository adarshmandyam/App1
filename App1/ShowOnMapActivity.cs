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
    [Activity(Label = "ShowOnMapActivity")]
    public class ShowOnMapActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShowOnMap);
            var latlongStr = Intent.Extras.GetString("item_extra");
            //MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.my_mapfragment_container);
            //GoogleMap map = mapFrag.Map;
            //if (map != null)
            //{
            //    MarkerOptions markerOpt1 = new MarkerOptions();
            //    markerOpt1.SetPosition(new LatLng(50.379444, 2.773611));
            //    markerOpt1.SetTitle("Vimy Ridge");
            //    map.AddMarker(markerOpt1);
            //}
        }
    }
}