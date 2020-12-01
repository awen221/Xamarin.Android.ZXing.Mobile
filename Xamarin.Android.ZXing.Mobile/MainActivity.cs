using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using ZXing;
using ZXing.Mobile;

namespace Xamarin.Android.ZXing.Mobile
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        //建立操作物件指標
        private TextView _barcodeFormat, _barcodeData;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


            //掃描元件初始化
            MobileBarcodeScanner.Initialize(Application);

            //簡化操作的位置(變數指標)
            _barcodeFormat = FindViewById<TextView>(Resource.Id.barcode_format);
            _barcodeData = FindViewById<TextView>(Resource.Id.barcode_data);

            //建立按鈕 Click 事件
            var button = FindViewById<Button>(Resource.Id.button_scan);
            button.Click += async (Senders, args) =>
            {
                //設定掃描元件操作
                var opts = new MobileBarcodeScanningOptions
                {
                    //限定(啟用)可掃描識別種類
                    PossibleFormats = new System.Collections.Generic.List<BarcodeFormat>
                    {
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.QR_CODE
                    }
                };
                //建立可執行化實例
                var scanner = new MobileBarcodeScanner();

                object result = null;

                //new Thread(new ThreadStart(delegate
                //{
                //    while (result == null)
                //    {
                //        scanner.AutoFocus();
                //        Thread.Sleep(0);
                //    }
                //})).Start();

                //利用 await 執行掃描，等待回應
                var _result = await scanner.Scan(opts);
                result = _result;

                //將回傳結果輸出到 TextView
                _barcodeFormat.Text = _result?.BarcodeFormat.ToString() ?? string.Empty;
                _barcodeData.Text = _result?.Text ?? string.Empty;
            };
        }
    }
}