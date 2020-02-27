using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;

public class Timetable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTimetable", 2, 60);
    }
    void UpdateTimetable()
    {
        var info1 = GetTimetableFromApi("Europaplatz Süd");
        var info2 = GetTimetableFromApi("Europaplatz Nord");
        var info3 = GetTimetableFromApi("Europaplatz Bahnhof");

        var stationboard = info1.stationboard;
        stationboard.AddRange(info2.stationboard);
        stationboard.AddRange(info3.stationboard);
        stationboard.Sort();

        leftString = "";
        rightString = "";

        // category number - to - time
        foreach (var i in stationboard)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(i.stop.departureTimestamp);

            leftString += i.category + i.number + "\t" + i.to + "\n";
            rightString += i.stop.platform + "\t" + dateTimeOffset.LocalDateTime.ToString("HH:mm") + "\n";
        }

        leftColumnText.GetComponent<TextMeshPro>().SetText(leftString);
        rightColumnText.GetComponent<TextMeshPro>().SetText(rightString);
    }

    private RootObject GetTimetableFromApi(string station)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://transport.opendata.ch/v1/stationboard?station={0}&limit=3", station));
        request.Proxy = new WebProxy("http://intranet.proxy.eda.admin.ch:7070");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        var info = JsonUtility.FromJson<RootObject>(jsonResponse);
        return info;
    }



    private string leftString = "";
    private string rightString = "";
    public GameObject leftColumnText;
    public GameObject rightColumnText;


    // Update is called once per frame
    void Update()
    {
        
    }
}
