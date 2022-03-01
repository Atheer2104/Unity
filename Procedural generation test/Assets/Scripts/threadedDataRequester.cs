using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class threadedDataRequester : MonoBehaviour {

    static threadedDataRequester instance;
    Queue<threadInfo> dataQueue = new Queue<threadInfo>();

    void Awake() {
        instance = FindObjectOfType<threadedDataRequester>();
    }

    // func<object> is delegate with a return type of object 
    public static void requestData(Func<object> generateData, Action<object> callback)
    {
        ThreadStart threadStart = delegate
        {
            instance.dataThread(generateData, callback);
        };
        // function mapDataThread is now running on a diffierent thread
        new Thread(threadStart).Start();

    }

    void dataThread(Func<object> generateData, Action<object> callback)
    {
        object data = generateData();
        // because we are running this on a thread mapdataQueue could be accesed by another function
        // at the same time so we lock it, means no other thread can try to excute this.
        lock (dataQueue)
        {
            dataQueue.Enqueue(new threadInfo (callback, data));
        }
    }


    void Update()
    {
        if (dataQueue.Count > 0)
        {
            for (int i = 0; i < dataQueue.Count; i++)
            {
                threadInfo threadInfo = dataQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    struct threadInfo {
        public readonly Action<object> callback;
        public readonly object parameter;

        public threadInfo(Action<object> callback, object parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }

}
