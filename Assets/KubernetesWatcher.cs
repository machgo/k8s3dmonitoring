using Assets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

    private int NumberNodes = 0;

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
            Color background = new Color(
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f)
            );

            var node = GameObject.Find(o.Object.spec.nodeName);
            if (node == null) {
                node = Instantiate(GameObject.Find("Node"), this.transform.parent);
                node.name = o.Object.spec.nodeName;
                node.transform.position = node.transform.position + new Vector3(NumberNodes * 15, 0);
                NumberNodes++;
            }

            var a = node.transform.Find("Cube").gameObject;
            var gm = Instantiate(a,node.transform);
            gm.name = o.Object.metadata.name;
            gm.SetActive(true);
            gm.transform.position = node.transform.position + new Vector3(0, 20);
            if (o.Object.spec.containers[0].resources.limits.cpu != null)
            {
                var scale = gm.transform.localScale;
                var c = Int32.Parse(GetNumbers(o.Object.spec.containers[0].resources.limits.cpu));
                var m = Int32.Parse(GetNumbers(o.Object.spec.containers[0].resources.limits.memory));
                scale.Set(3, 3, 3); // todo: correct calcs
                gm.transform.localScale = scale;
            }
            else {
                var scale = gm.transform.localScale;
                scale.Set(2, 2, 2); // todo: correct calcs
                gm.transform.localScale = scale;
            }


            //TODO:
            //generate size of cubes based on limits
            //generate color based on namespace or container image? maybe generate a hash and convert this into rgb numbers?
            //instantiate 4 walls per node and spawn the cubes inside

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


    private static string GetNumbers(string input)
    {
        return new string(input.Where(c => char.IsDigit(c)).ToArray());
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

    public string token;
    public string url;

    private async Task<int> GetRandomFromApi()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/v1/pods?watch=1");
        if (token != "")
        {
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
        }
        request.KeepAlive = true;
        request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;


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
