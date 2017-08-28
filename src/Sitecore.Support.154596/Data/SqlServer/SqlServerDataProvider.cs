using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Eventing;
using Sitecore.Support.Data.Eventing;

namespace Sitecore.Support.Data.SqlServer
{
    public class SqlServerDataProvider : Sitecore.Data.SqlServer.SqlServerDataProvider
    {
        public SqlServerDataProvider(string connectionString) : base(connectionString)
        {
        }

        public override EventQueue GetEventQueue()
        {
            return new SqlServerEventQueue(base.Api, base.Database);
        }
    }
}