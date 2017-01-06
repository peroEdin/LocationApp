using System;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java.Lang;
using Android.Content;
using Android.Hardware;

namespace GetLocation.Droid
{
    [Activity (Label = "GetLocation.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, ILocationListener, ISensorEventListener
    {
        
        int count = 1;
        static readonly string TAG = "X:" + typeof(Activity).Name;
        TextView _addressText;
        Location _currentLocation;
        LocationManager _locationManager;

        static readonly object _syncLock = new object();
        SensorManager _sensorManager;
        TextView _sensorTextView;

        string _locationProvider;
        TextView _locationText;
        TextView _statusText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate
            {
                button.Text = string.Format("{0} clicks!", count++);
            };


            _addressText = FindViewById<TextView>(Resource.Id.address_text);
            _locationText = FindViewById<TextView>(Resource.Id.location_text);
            _statusText = FindViewById<TextView>(Resource.Id.status_text);
            FindViewById<TextView>(Resource.Id.get_address_button).Click += AddressButton_OnClick;

            //InitializeFuseLocationManager();
            InitializeLocationManager();

            _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);

        }

        private void InitializeFuseLocationManager()
        {
            throw new NotImplementedException();
            //if (mGoogleApiClient == null)
            //{
            //    mGoogleApiClient = new GoogleApiClient.Builder(this)
            //        .addConnectionCallbacks(this)
            //        .addOnConnectionFailedListener(this)
            //        .addApi(LocationServices.API)
            //        .build();        }
        }

        protected override void OnResume()
        {
            base.OnResume();

            Criteria locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;

            _locationProvider = _locationManager.GetBestProvider(locationCriteria, true);

            if (_locationProvider != null)
            {
                _locationManager.RequestLocationUpdates(_locationProvider, 2000, 1, this);
            }
            else
            {
                //Log.Info(tag, "No location providers available");
            }

            _sensorManager.RegisterListener(this,
                                _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                                SensorDelay.Ui);

        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
            _sensorManager.UnregisterListener(this);
        }


        void InitializeLocationManager()
        {
            _locationManager = GetSystemService(Context.LocationService) as LocationManager;
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Coarse
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            //Log.Debug(TAG, "Using " + _locationProvider + ".");
        }

        async void AddressButton_OnClick(object sender, EventArgs eventArgs)
        {
            if (_currentLocation == null)
            {
                _addressText.Text = "Can't determine the current address. Try again in a few minutes.";
                return;
            }

            Address address = await ReverseGeocodeCurrentLocation();
            DisplayAddress(address);
        }

        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                System.Text.StringBuilder deviceAddress = new System.Text.StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                // Remove the last comma from the end of the address.
                _addressText.Text = deviceAddress.ToString();
            }
            else
            {
                _addressText.Text = "Unable to determine the address. Try again in a few minutes.";
            }
        }


        void ILocationListener.OnProviderDisabled(string provider)
        {
            _locationText.Text = "ProviderDisabled";
        }

        void ILocationListener.OnProviderEnabled(string provider)
        {
            _locationText.Text = provider + " enabled";
        }

        void ILocationListener.OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            _statusText.Text = provider + " " + status.ToString() + " " + extras.ToString();
        }

        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _locationText.Text = "Unable to determine your location. Try again in a short while.";
            }
            else
            {
                _locationText.Text = string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude);
                Address address = await ReverseGeocodeCurrentLocation();
                DisplayAddress(address);
            }
        }

        void ISensorEventListener.OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //throw new NotImplementedException();
        }

        void ISensorEventListener.OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock)
            {
                _sensorTextView.Text = string.Format("x={0:f}, y={1:f}, y={2:f}", e.Values[0], e.Values[1], e.Values[2]);
            }
        }
    }
}


