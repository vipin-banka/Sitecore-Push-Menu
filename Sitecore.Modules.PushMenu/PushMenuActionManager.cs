using System.Collections;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Events;
using Sitecore.Modules.PushMenu.Managers;
using System;
using Sitecore.Pipelines.GetContentEditorWarnings;
using Sitecore.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Sitecore.Modules.PushMenu
{
    public static class PushMenuActionManager
    {
        private static IList<string> _databases = new List<string>();

        private static int token = 0;

        private static object lockObject = new object();

        public static void AddAction(string database)
        {
            lock (lockObject)
            {
                if (!_databases.Contains(database))
                {
                    _databases.Add(database);
                }
            }

            DoRun(false);
        }

        private static IList<string> GetClonedList()
        {
            lock (lockObject)
            {
                var result = _databases;
                _databases = new List<string>();
                return result;
            }
        }

        private static void DoRun(bool tokenAcquired)
        {
            lock (lockObject)
            {
                if (_databases.Any())
                {
                    if (!tokenAcquired)
                    {
                        tokenAcquired = AcquireToken();
                    }
                }
                else
                {
                    tokenAcquired = false;
                }
            }

            if (tokenAcquired)
            {
                try
                {
                    Run();
                }
                finally
                {
                    ReleaseToken();
                }
            }
        }

        private static void Run()
        {
            var resultList = GetClonedList();

            if (resultList != null && resultList.Any())
            {
                foreach (var item in resultList)
                {
                    try
                    {
                        var database = Factory.GetDatabase(item);
                        if (database != null)
                        {
                            new PushMenuManager(database).Generate();
                        }
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error in Run method from PushMenuActionManager", null);
                    }
                }
            }
            
            DoRun(true);
        }

        private static bool AcquireToken()
        {
            return Interlocked.CompareExchange(ref token, Thread.CurrentThread.ManagedThreadId, 0) == 0;
        }

        private static bool ReleaseToken()
        {
            return Interlocked.CompareExchange(ref token, 0, Thread.CurrentThread.ManagedThreadId) == Thread.CurrentThread.ManagedThreadId;
        }
    }
}