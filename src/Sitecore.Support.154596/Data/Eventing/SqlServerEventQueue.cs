using Sitecore.Data;
using Sitecore.Data.DataProviders.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Eventing;
using Sitecore.Data.Events;

namespace Sitecore.Support.Data.Eventing
{
    public class SqlServerEventQueue : Sitecore.Data.Eventing.SqlServerEventQueue
    {
        public SqlServerEventQueue(SqlDataApi api) : base(api)
        {
        }

        public SqlServerEventQueue(SqlDataApi api, Database database) : base(api, database)
        {
        }

        protected override void SetTimestampForLastProcessing(TimeStamp currentTimestamp)
        {
            using (new EventDisabler())
            {
                base.SetTimestampForLastProcessing(currentTimestamp);
            }
        }
    }
}