<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Messaging.dll</Reference>
</Query>

var allQueues = System.Messaging.MessageQueue.GetPrivateQueuesByMachine("LOCALHOST").ToList();

var queuesToCreate = new List<string>
{
	"particular.servicecontrol",
	"particular.servicecontrol.staging",
	"audit",
	"error"
};

var notFound = queuesToCreate.Except(allQueues.Select(q => q.Path.Split('\\').Last())).ToList();

notFound.Dump("Creating Queues:");
notFound.ForEach(path => System.Messaging.MessageQueue.Create(Environment.MachineName + "\\Private$\\" + path,true));