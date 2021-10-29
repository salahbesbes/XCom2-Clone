using System;
using System.Collections.Generic;
using UnityEngine;

public class PathRequest
{
	public Action callback;

	public PathRequest(Action callback)
	{
		this.callback = callback;
	}
}

public class PathRequestManager : MonoBehaviour
{
	public static int counter = 0;
	public static PathRequestManager Instance;
	public bool isProcessing = false;
	private playerAction currentPathRequest;
	private Queue<playerAction> pathRequestQueue = new Queue<playerAction>();

	public void Enqueue(playerAction act)
	{
		Instance.pathRequestQueue.Enqueue(act);
		Instance.TryProcessNext();
	}

	public void finishedProcessingPath()
	{
		isProcessing = false;
		Debug.Log($"finish the process");
		//TryProcessNext();
	}

	public void TryProcessNext()
	{
		Debug.Log($"processing {isProcessing}  length {pathRequestQueue.Count}");
		// if we are not processing and the que queue is not empty
		if (!isProcessing && pathRequestQueue.Count > 0)
		{
			//Debug.Log($" queue count =  {pathRequestQueue.Count} ");
			isProcessing = true;
			currentPathRequest = pathRequestQueue.Dequeue();
			Debug.Log($"{currentPathRequest}");
			currentPathRequest.executeAction();
		}
	}

	public void Awake()
	{
		if (Instance == null) Instance = this;
	}
}