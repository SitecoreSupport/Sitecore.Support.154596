using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Sitecore.Data.Eventing.Remote;
using Sitecore.Eventing;
using Sitecore.Eventing.Remote;
using Sitecore.Events;
using Sitecore.Pipelines;

namespace Sitecore.Support
{
    public class OnDatabasePropertyChanged
    {
        private string[] prefixes =
        {
            Sitecore.Data.Managers.IndexingManager.LastUpdatePropertyKey,
            EventQueue.TimestampPropertyPrefix
        };

        public void Process(PipelineArgs pArgs)
        {
            string key = "database:propertychanged";
            Type ev = typeof(Event);
           // ev.GetNestedType("EventSubscribers", BindingFlags.NonPublic)
            var list = ev.GetField("DynamicSubscribers", BindingFlags.Static | BindingFlags.NonPublic);
            if (list != null)
            {

                Hashtable table = ev.GetNestedType("EventSubscribers", BindingFlags.NonPublic).GetField("subscriptions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(list.GetValue(null)) as Hashtable;
               // Hashtable table = Reflection.ReflectionUtil.GetField(list.GetValue(null), "subscriptions") as Hashtable;
                if (table!=null)
                    lock (table.SyncRoot)
                    {
                        System.Collections.ArrayList arrayList = table[key] as System.Collections.ArrayList;
                        if (arrayList == null)
                        {
                            arrayList = new System.Collections.ArrayList();
                            table[key] = arrayList;
                        }
                        if(arrayList.Count>0&&arrayList[0]!=null)
                            arrayList.RemoveAt(0);
                    }
            }
            Event.Subscribe("database:propertychanged", delegate (object sender, System.EventArgs args)
            {
                string text = Event.ExtractParameter<string>(args, 0);
                if (prefixes.Any(x=>(text.StartsWith(x,StringComparison.InvariantCultureIgnoreCase))))
                {
                    return;
                }
                EventManager.QueueEvent<PropertyChangedRemoteEvent>(new PropertyChangedRemoteEvent(text));
            });


        }
    }
}