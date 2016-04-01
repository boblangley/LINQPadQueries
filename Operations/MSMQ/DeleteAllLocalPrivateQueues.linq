<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Messaging.dll</Reference>
</Query>

var queues = System.Messaging.MessageQueue.GetPrivateQueuesByMachine("LOCALHOST");

queues.ToList().ForEach(queue => System.Messaging.MessageQueue.Delete(queue.Path));