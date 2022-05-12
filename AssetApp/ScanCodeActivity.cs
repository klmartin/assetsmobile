using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using EDMTDev.ZXingXamarinAndroid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetApp
{
    [Activity(Label = "Scan Code")]
    public class ScanCodeActivity : Activity,IResultHandler
    {
        private const int PERMISSIONS_REQUEST_CAMERA = 100;
        private ZXingScannerView scanView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_scan_code);

            scanView = FindViewById<ZXingScannerView>(Resource.Id.zxScan);


            scanView.SetAutoFocus(true);

            scanView.SetShouldScaleToFill(true);

            List<ZXing.BarcodeFormat> formats = new List<ZXing.BarcodeFormat>();
            formats.Add(ZXing.BarcodeFormat.CODE_128);

            
            scanView.SetFormats(formats);
            

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted)
            {

                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, PERMISSIONS_REQUEST_CAMERA);
            }
            else
            {
                scanView.SetResultHandler(this);
                scanView.StartCamera();
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case PERMISSIONS_REQUEST_CAMERA:
                    {
                        if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                        {
                            scanView.SetResultHandler(this);
                            scanView.StartCamera();
                        }
                        else
                        {
                            Intent intent = new Intent();

                            intent.PutExtra("barcode", string.Empty);

                            SetResult(Result.Ok, intent);
                            Finish();
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        protected override void OnDestroy()
        {
            scanView.StopCamera();
            base.OnDestroy();
        }

        public void HandleResult(ZXing.Result rawResult)
        {
            Intent intent = new Intent(); 

            intent.PutExtra("barcode", rawResult.Text);
            
            SetResult(Result.Ok, intent);

            Finish();
        }
    }

 
}