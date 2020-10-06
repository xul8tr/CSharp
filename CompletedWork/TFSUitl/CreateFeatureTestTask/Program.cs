using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace CreateFeatureTestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "https://iiaastfs.ww004.siemens.net/tfs/tia";
            string projectName = "TIA";
          
            WorkItemStore workItemStore = GetWorkItemStore(uri);
            Project tfsProject = workItemStore.Projects[projectName];
            WorkItemType wIType = tfsProject.WorkItemTypes["TASK"];
            //Create a hierarchy type (Parent-Child) relationship 
            WorkItemLinkType hierarchyLinkType = workItemStore.WorkItemLinkTypes[CoreLinkTypeReferenceNames.Hierarchy];

            CreateFeatureTestTasks(uri, projectName, wIType, hierarchyLinkType);
           //CreateIncrementtestTasks(uri, projectName, wIType, hierarchyLinkType);

        }

        private static void CreateIncrementtestTasks(string uri, string projectName, WorkItemType wIType, WorkItemLinkType hierarchyLinkType)
        {
            int parentID = 966669;            
            string titlePrefix = @"Increment 32 test";
            Console.WriteLine(titlePrefix + " tasks start.");
            string areaPath = @"TIA\Development\HMI Runtime Innovation Line\IOWA Runtime Innovation Line Platform\Integrationstest\Integration";
            string iterationPath = @"TIA\IOWA\IOWA 2015\Inc 32 (31.03.2014)";
            CreateTask(parentID, titlePrefix + ": AlarmingInEvent", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Csepe Zsombor");
            CreateTask(parentID, titlePrefix + ": AlarmLogging", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "Bleier, Gerald");
            CreateTask(parentID, titlePrefix + ": AlarmText, AlarmText Formatter", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Palotas Gergely");
            CreateTask(parentID, titlePrefix + ": APIMAN", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Lenart Gabor");
            CreateTask(parentID, titlePrefix + ": BasicSystemFunctions: SCS-13-2, UC53", wIType, hierarchyLinkType, areaPath, iterationPath, 3, 30, "", "Kiss Zoltan2-Miklos");
            CreateTask(parentID, titlePrefix + ": BasicSystemFunctions: SCS-13-3, SCS-13-4, ASR46", wIType, hierarchyLinkType, areaPath, iterationPath, 3, 30, "", "Homan-Bakos Krisztina");
            CreateTask(parentID, titlePrefix + ": BasicSystemFunctions manual", wIType, hierarchyLinkType, areaPath, iterationPath, 8, 30, "", "Homan-Bakos Krisztina");
            CreateTask(parentID, titlePrefix + ": Configuration Access Layer", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "Palotas Gergely");
            CreateTask(parentID, titlePrefix + ": Caching of Identification", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "Lenart Gabor");
            CreateTask(parentID, titlePrefix + ": DistributedSystem", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "Lenart Gabor");
            CreateTask(parentID, titlePrefix + ": Driver", wIType, hierarchyLinkType, areaPath, iterationPath, 2, 30, "", "Bleier, Gerald");
            CreateTask(parentID, titlePrefix + ": DriverFramework_Tracing", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Simon Attila (ext)");
            CreateTask(parentID, titlePrefix + ": General", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Palotas Gergely");
            CreateTask(parentID, titlePrefix + ": Localization", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "Kiss Zoltan2-Miklos");
            CreateTask(parentID, titlePrefix + ": NameService", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Kiss Zoltan2-Miklos");
            CreateTask(parentID, titlePrefix + @": PLCAlarming(S7-300)", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "Csepe Zsombor");
            CreateTask(parentID, titlePrefix + @": PLCAlarming(S7-400)", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "Csepe Zsombor");
            CreateTask(parentID, titlePrefix + @": RAM/CPU", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Nagy David (ext)");
            CreateTask(parentID, titlePrefix + ": SystemInterfaces_Base", wIType, hierarchyLinkType, areaPath, iterationPath, 2, 30, "", "Palotas Gergely");
            CreateTask(parentID, titlePrefix + ": SystemInterfaces_ITimer", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Palotas Gergely");
            CreateTask(parentID, titlePrefix + ": SystemInterfaces_LanguageFilter", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Kiss Zoltan2-Miklos");
            CreateTask(parentID, titlePrefix + ": TagLogging", wIType, hierarchyLinkType, areaPath, iterationPath, 5, 30, "", "Harmati Daniel");
            CreateTask(parentID, titlePrefix + ": TIA Integration RDF Config", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Homan-Bakos Krisztina");
            CreateTask(parentID, titlePrefix + ": Timedfunc.", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Csepe Zsombor");
            CreateTask(parentID, titlePrefix + ": Tracing", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 30, "", "Simon Attila (ext)");
           // CreateTask(parentID, titlePrefix + ": New1", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "");
           // CreateTask(parentID, titlePrefix + ": New2", wIType, hierarchyLinkType, areaPath, iterationPath, 4, 30, "", "");                        
            Console.WriteLine(titlePrefix + " tasks are ready. Press Enter to close this window.");
            Console.ReadLine();
        }


        private static void CreateFeatureTestTasks(string uri, string projectName, WorkItemType wIType, WorkItemLinkType hierarchyLinkType)
        {

            int parentID = 904830;
            string titlePrefix = @"LIBS-15-7 driver-instance-global connect";
            Console.WriteLine(titlePrefix + " tasks start.");
            string areaPath = @"TIA\Development\HMI Runtime Innovation Line\IOWA Runtime Innovation Line Platform\Integrationstest\FeatureTest";
             string iterationPath = @"TIA\IOWA\Backlog";
            //            string iterationPath = @"TIA\IOWA\IOWA 2015\Inc 25 (31.08.2013)";
             string tags = "DevRelease5";
             //string tags = string.Empty;
             //string assignedTo = "Palotas Gergely";
           //  string assignedTo = "Csepe Zsombor";            
             //string assignedTo = "Kiss Zoltan2-Miklos";            
             string assignedTo = string.Empty;

          //  CreateTask(parentID, titlePrefix + ": KickOff meeting", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 50, tags, assignedTo );
            CreateTask(parentID, titlePrefix + ": Create TestSpec.", wIType, hierarchyLinkType, areaPath, iterationPath, 24, 50, tags, assignedTo);
            CreateTask(parentID, titlePrefix + ": Review meeting TestSpec.", wIType, hierarchyLinkType, areaPath, iterationPath, 1, 50, tags, assignedTo);
            CreateTask(parentID, titlePrefix + ": Rework TestSpec. after review", wIType, hierarchyLinkType, areaPath, iterationPath, 16, 50, tags, assignedTo);
            CreateTask(parentID, titlePrefix + ": Testcase implementation Rank1 + Review", wIType, hierarchyLinkType, areaPath, iterationPath, 32, 50, tags, assignedTo);
            CreateTask(parentID, titlePrefix + ": Testcase implementation", wIType, hierarchyLinkType, areaPath, iterationPath, 32, 50, tags, assignedTo);
            CreateTask(parentID, titlePrefix + @": BranchTest/IntegrationTest", wIType, hierarchyLinkType, areaPath, iterationPath, 24, 50, tags, assignedTo);

            Console.WriteLine(titlePrefix + " tasks are ready. Press Enter to close this window.");
            Console.ReadLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="title"></param>
        /// <param name="wIType"></param>
        /// <param name="hierarchyLinkType"></param>
        private static void CreateTask(int parentID, string title, WorkItemType wIType, WorkItemLinkType hierarchyLinkType,
                                       string areaPath, string iterationPath, int workingHours, int stackRank, string tags, string assignedTo)
        {
            WorkItem workItem = new WorkItem(wIType);
            workItem.Title = title;
            workItem.AreaPath = areaPath;
            workItem.IterationPath = iterationPath;
            //Set user story as parent of new task
            workItem.Links.Add(new WorkItemLink(hierarchyLinkType.ReverseEnd, parentID));
            //if (String.IsNullOrEmpty(assignedTo))
            //    workItem.Fields["Assigned to"].Value = workItem.ChangedBy;
            //else
                workItem.Fields["Assigned to"].Value = assignedTo;
            workItem.Fields["Original estimate"].Value = workingHours;
            workItem.Fields["Remaining work"].Value = workingHours;
            workItem.Fields["Stack rank"].Value = stackRank;
            workItem.Fields["Tags"].Value = tags;

            System.Collections.ArrayList result = workItem.Validate();
            if (result.Count > 0)
            {
                Console.WriteLine("There was an error adding this work item to the work item repository.");
            }
            else
            {
               workItem.Save();
            }
        }


        /// <summary>
        /// Gets TFS workitem store.
        /// </summary>
        /// <param name="tfsUrl">TFS URL</param>
        /// <returns>Returns TFS workitem store.</returns>
        static WorkItemStore GetWorkItemStore(string tfsUrl)
        {

            // Connect to the server and the store. 
            TfsTeamProjectCollection teamProjectCollection =
               new TfsTeamProjectCollection(new Uri(tfsUrl));

            return teamProjectCollection.GetService<WorkItemStore>();
        }
    }
}
