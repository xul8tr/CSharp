using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSHelper
{
    public class Utils
    {
        /// Gets TFS workitem store.
        /// </summary>
        /// <param name="tfsUrl">TFS URL</param>
        /// <returns>Returns TFS workitem store.</returns>
        public static WorkItemStore GetWorkItemStore(string tfsUrl)
        {

            // Connect to the server and the store. 
            TfsTeamProjectCollection teamProjectCollection =
               new TfsTeamProjectCollection(new Uri(tfsUrl));

            return teamProjectCollection.GetService<WorkItemStore>();
        }




        /// <summary>
        /// <para>Find the TFS QueryDefinition for a specified Team Project</para>
        /// <para>Note that if multiple queries match the requested queryName
        /// only the first will be used</para>
        /// </summary>
        /// <param name=”tfsUrl”>URL to the TFS project, including the
        /// collection name (Eg, http://tfsserver:8080/tfs/DefaultCollection)</param>
        /// <param name=”projectName”>Name of TFS Team Project</param>
        /// <param name=”queryName”>Name of Stored Query. Note if multiple
        /// exist the first found will be used</param>
        /// <returns></returns>
        public static QueryDefinition FindQueryItem(string tfsUrl, string projectName, string queryName)
        {
            WorkItemStore workItemStore = GetWorkItemStore(tfsUrl);
            Project project = workItemStore.Projects[projectName];

            return FindQueryItem(queryName, project.QueryHierarchy);
        }

        /// <summary>
        /// Find the Query in TFS
        /// </summary>
        /// <param name="workItemStore">TFS workitem store.</param>
        /// <param name="projectName">TFS project name.</param>
        /// <param name="queryName">TFS workitem query name.</param>
        /// <returns>Returns the TFS Query.</returns>
        public static QueryDefinition FindQueryItem(WorkItemStore workItemStore, string projectName, string queryName)
        {
            Project project = workItemStore.Projects[projectName];
            return FindQueryItem(queryName, project.QueryHierarchy);
        }

        ///<summary>
        /// Recursively find the QueryDefinition based on the requested queryName.
        ///<para>Note that if multiple queries match the requested queryName
        /// only the first will be used</para>
        ///</summary>
        ///<param name=”queryName”>Name of Stored Query. Note if multiple exist
        /// the first found will be used</param>
        ///<param name=”currentNode”>Pointer to the current node in the recursive search</param>
        ///<returns>QueryDefinition</returns>
        public static QueryDefinition FindQueryItem(string queryName, QueryItem currentNode)
        {

            // Attempt to cast to a QueryDefinition
            QueryDefinition queryDefinition = currentNode as QueryDefinition;

            // Check if we’ve found a match
            if (queryDefinition != null && queryDefinition.Name == queryName)

                return queryDefinition;

            // Attempt to cast the current node to a QueryFolder
            QueryFolder queryFolder = currentNode as QueryFolder;

            // All further checks are for child nodes so if this is not an
            // instance of QueryFolder then no further processing is required.
            if (queryFolder == null)

                return null;

            // Loop through all the child query item
            foreach (QueryItem qi in queryFolder)
            {

                // Recursively call FindQueryItem
                QueryDefinition ret = FindQueryItem(queryName, qi);

                // If a match is found no further checks are required
                if (ret != null)

                    return ret;

            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workItemStore"></param>
        /// <param name="projectName"></param>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public static WorkItemCollection GetWorkItemCollectionWithStoredQuery(WorkItemStore workItemStore, string projectName, string queryName)
        {
            QueryDefinition queryDefinition = FindQueryItem(workItemStore, projectName, queryName);
            var variables = new Dictionary<string, string>
            {
                {"project", projectName}
            };

            return workItemStore.Query(queryDefinition.QueryText, variables);
        }



        public static WorkItemCollection QueryWorkItems(WorkItemStore workItemStore, string wiqlQuery)
        {
            WorkItemCollection witCollection = workItemStore.Query(wiqlQuery);
            return witCollection;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="tfsServer"></param>
        ///// <param name="workItemType"></param>
        ///// <param name="project"></param>
        ///// <param name="values"></param>
        //public static void WriteToTFS(String tfsServer, String workItemType, String project, Hashtable values)
        //{
        //    //TeamFoundationServer teamFoundationServer =
        //    //TeamFoundationServerFactory.GetServer(tfsServer);
        //    //teamFoundationServer.Authenticate();
        //    //WorkItemStore workItemStore = new
        //    //WorkItemStore(tfsServer);
        //    //Project tfsProject = workItemStore.Projects["DevX"];
        //    //WorkItemType wIType =
        //    //tfsProject.WorkItemTypes[workItemType];
        //    //WorkItem workItem = new WorkItem(wIType);
        //    //workItem.Title = values["Title"].ToString();
        //    //workItem.Description = values["Description"].ToString();
        //    //workItem.State = values["State"].ToString();
        //    //workItem.Reason = values["Reason"].ToString();
        //    //workItem.Fields["Assigned to"].Value =
        //    //values["Assigned To"].ToString();
        //    //ArrayList result = workItem.Validate();
        //    //if (result.Count > 0)
        //    //    Console.WriteLine("There was an error adding this work item to the work item repository");
        //    //else
        //    //    workItem.Save();
        //}
    }
}
