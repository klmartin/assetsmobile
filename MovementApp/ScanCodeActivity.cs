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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace MovementApp
{
    [Activity(Label = "Scan Code")]
    public class ScanCodeActivity : Activity,IResultHandler
    {
        private const int PERMISSIONS_REQUEST_CAMERA = 100;
        private ZXingScannerView scanView;
        private List<string> barcodes = new List<string>();
        private TextView done;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_scan_code);

            scanView = FindViewById<ZXingScannerView>(Resource.Id.zxScan);
            done = FindViewById<TextView>(Resource.Id.scan_done_btn);

            done.Click += Done_Click;
            
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

        private void Done_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();

            string codes = JsonConvert.SerializeObject(barcodes);
            intent.PutExtra("barcode", codes);

            SetResult(Result.Ok, intent);
            Finish();
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
            if (!barcodes.Contains(rawResult.Text))
            {
                barcodes.Add(rawResult.Text);
                try
                {
                    // Use default vibration length
                    Vibration.Vibrate();

                    // Or use specified time
                    var duration = TimeSpan.FromSeconds(0.4);
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                }
                Toast.MakeText(this, rawResult.Text, ToastLength.Short).Show();

            }
            else
            {
                Toast.MakeText(this, ""+"Already Scanned"+"", ToastLength.Short).Show();
            }

            scanView.ResumeCameraPreview(this);

        }
    }

}