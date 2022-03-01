using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class ThreadedDataRequester : MonoBehaviour {

	static ThreadedDataRequester instance;
	Queue<ThreadInfo> dataQueue = new Queue<ThreadInfo>();

	void Awake() {
		instance = FindObjectOfType<ThreadedDataRequester> ();
	}

	// func<object> is delegate with a return type of object 
	public static void RequestData(Func<object> generateData, Action<object> callback) {
		ThreadStart threadStart = delegate {
			instance.DataThread (generateData, callback);
		};

		// function mapDataThread is now running on a diffierent thread
		new Thread (threadStart).Start ();
	}

	void DataThread(Func<object> generateData, Action<object> callback) {
		object data = generateData ();
		// because we are running this on a thread mapdataQueue could be accesed by another function
        // at the same time so we lock it, means no other thread can try to excute this.
		lock (dataQueue) {
			dataQueue.Enqueue (new ThreadInfo (callback, data));
		}
	}
		

	void Update() {
		if (dataQueue.Count > 0) {
			for (int i = 0; i < dataQueue.Count; i++) {
				ThreadInfo threadInfo = dataQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}
	}

	struct ThreadInfo {
		public readonly Action<object> callback;
		public readonly object parameter;

		public ThreadInfo (Action<object> callback, object parameter)
		{
			this.callback = callback;
			this.parameter = parameter;
		}

	}
}