using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [System.Serializable]
    public class Coordinate
    {
        public string type;
        public double x;
        public double y;
    }

    [System.Serializable]
    public class Station
    {
        public string id;
        public string name;
        public object score;
        public Coordinate coordinate;
        public object distance;
    }

    [System.Serializable]
    public class Coordinate2
    {
        public string type;
        public double x;
        public double y;
    }

    [System.Serializable]
    public class Station2
    {
        public string id;
        public string name;
        public object score;
        public Coordinate2 coordinate;
        public object distance;
    }

    [System.Serializable]
    public class Prognosis
    {
        public object platform;
        public object arrival;
        public object departure;
        public object capacity1st;
        public object capacity2nd;
    }

    [System.Serializable]
    public class Coordinate3
    {
        public string type;
        public object x;
        public object y;
    }

    [System.Serializable]
    public class Location
    {
        public string id;
        public object name;
        public object score;
        public Coordinate3 coordinate;
        public object distance;
    }

    [System.Serializable]
    public class Stop
    {
        public Station2 station;
        public object arrival;
        public object arrivalTimestamp;
        public DateTime departure;
        public int departureTimestamp;
        public object delay;
        public string platform;
        public Prognosis prognosis;
        public object realtimeAvailability;
        public Location location;
    }

    [System.Serializable]
    public class Coordinate4
    {
        public string type;
        public double? x;
        public double? y;
    }

    [System.Serializable]
    public class Station3
    {
        public string id;
        public string name;
        public object score;
        public Coordinate4 coordinate;
        public object distance;
    }

    [System.Serializable]
    public class Prognosis2
    {
        public object platform;
        public object arrival;
        public object departure;
        public object capacity1st;
        public object capacity2nd;
    }

    [System.Serializable]
    public class Coordinate5
    {
        public string type;
        public double? x;
        public double? y;
    }

    [System.Serializable]
    public class Location2
    {
        public string id;
        public string name;
        public object score;
        public Coordinate5 coordinate;
        public object distance;
    }

    [System.Serializable]
    public class PassList
    {
        public Station3 station;
        public DateTime? arrival;
        public int? arrivalTimestamp;
        public DateTime? departure;
        public int? departureTimestamp;
        public object delay;
        public string platform;
        public Prognosis2 prognosis;
        public object realtimeAvailability;
        public Location2 location;
    }

    [System.Serializable]
    public class Stationboard : IComparable
    {
        public Stop stop;
        public string name;
        public string category;
        public object subcategory;
        public object categoryCode;
        public string number;
        public string @operator;
        public string to;
        public List<PassList> passList;
        public object capacity1st;
        public object capacity2nd;

        public int CompareTo(object obj)
        {
            if (obj is Stationboard)
            {
                return this.stop.departureTimestamp.CompareTo((obj as Stationboard).stop.departureTimestamp);
            }
            throw new ArgumentException("Object is not a Stationboard");
        }
    }

    [System.Serializable]
    public class RootObject
    {
        public Station station;
        public List<Stationboard> stationboard;
    }
}
