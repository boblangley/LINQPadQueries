<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Messaging.dll</Reference>
</Query>

var allQueues = System.Messaging.MessageQueue.GetPrivateQueuesByMachine("LOCALHOST").ToList();

var queuesToClear = new List<string>
{
	"particular.servicecontrol",
	"particular.servicecontrol.staging",
	"audit",
	"error"
};

var keep = LINQPad.Util.ReadLine<bool>("Keep NSB Queues?", true);

var queues = keep ? allQueues.Where(q => !queuesToClear.Contains(q.Path.ToLower())).ToList() : allQueues;

queues.ForEach(q => System.Messaging.MessageQueue.Delete(q.Path));

if (!keep) return;

var purge = LINQPad.Util.ReadLine<bool>("Purge Remaining Queues?",true);
if (!purge) return;

queues = allQueues.Where(q => queuesToClear.Contains(q.Path.ToLower())).ToList();

queues.ForEach(q => q.Purge());