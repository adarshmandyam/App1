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

namespace App1
{
    public class TableItem
    {
        public string ShopName { get; set; }
        public string Address { get; set; }
        public string Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}