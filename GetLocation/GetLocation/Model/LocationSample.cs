using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetLocation.Model
{
    public class LocationSample
    {
        private double _longitude;

        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }
        private double _latitude;

        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        private double _ax;

        public double Ax
        {
            get { return _ax; }
            set { _ax = value; }
        }
        private double _ay;

        public double Ay
        {
            get { return _ay; }
            set { _ay = value; }
        }
        private double _az;

        public double Az
        {
            get { return _az; }
            set { _az = value; }
        }
        private double _px;

        public double Px
        {
            get { return _px; }
            set { _px = value; }
        }
        private double _py;

        public double Py
        {
            get { return _py; }
            set { _py = value; }
        }
        private double _pz;

        public double Pz
        {
            get { return _pz; }
            set { _pz = value; }
        }
        private double _vx;

        public double Vx
        {
            get { return _vx; }
            set { _vx = value; }
        }
        private double _vy;

        public double Vy
        {
            get { return _vy; }
            set { _vy = value; }
        }
        private double _vz;

        public double Vz
        {
            get { return  _vz; }
            set {  _vz = value; }
        }

    }
}
