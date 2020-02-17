using Assets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KubernetesWatcher : MonoBehaviour
{
    private const float API_CHECK_MAXTIME = 5;
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    public GameObject prefab;

    private ConcurrentQueue<PodEvent> newPods = new ConcurrentQueue<PodEvent>();
    private ConcurrentQueue<PodEvent> deletePods = new ConcurrentQueue<PodEvent>();


    void Start()
    {
        CheckRandomAsync();
    }
    void Update()
    {
        var rand = new System.Random();

        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            //CheckRandomAsync();
            apiCheckCountdown = API_CHECK_MAXTIME;
        }

        foreach (var o in newPods)
        {
            var gm = Instantiate(prefab, new Vector3((float)rand.NextDouble() * 4, 17, (float)rand.NextDouble() * 4), new Quaternion());
            gm.name = o.Object.metadata.name;

            Color background = new Color(
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f)
            );


            gm.GetComponent<Renderer>().material.color = background;
            newPods.TryDequeue(out _);
        }

        foreach (var o in deletePods)
        {
            deletePods.TryDequeue(out _);
            var gm = GameObject.Find(o.Object.metadata.name);
            Destroy(gm);
        }

    }


    public async void CheckRandomAsync()
    {
        var r = await GetRandomFromApi();


        //for (int i = 0; i < r.random; i++)
        //{
        //    Instantiate(prefab);
        //    await Task.Delay(500);
        //}

    }
    private async Task<int> GetRandomFromApi()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8001/api/v1/pods?watch=1");
        request.KeepAlive = true;


        using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
        {
            using (Stream sm = resp.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(sm, Encoding.Default))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync().ConfigureAwait(false)) != null)
                    {

                        Console.WriteLine(line);
                        var info = JsonUtility.FromJson<PodEvent>(line.Replace("object", "Object").Replace("namespace", "Namespace"));

                        if (info.type == "ADDED")
                        {
                            newPods.Enqueue(info);
                        }
                        if (info.type == "DELETED")
                        {
                            deletePods.Enqueue(info);
                        }

                    }
                }
            }
        }
        return 0;
    }


}
