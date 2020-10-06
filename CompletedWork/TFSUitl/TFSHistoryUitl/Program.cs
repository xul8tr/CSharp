using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Data;
using System.Collections;
using System.IO;

namespace TFSHistoryUitl
{
    class Program
    {

        //Separator in the output
        const string sep = ";";

        static void Main(string[] args)
        {
            string uri = "http://defthw99wbtsrv.ww004.siemens.net:8080/tfs/tia";
            string projectName = "TIA";
            //string changedDateEnd = DateTime.Today.ToString();

            DateTime changedDateStart;
            DateTime changedDateEnd = DateTime.Today;
            
            Dictionary<string, string> arguments = ReadArgs(args);
            if (!arguments.ContainsKey("changeddatestart"))
            {
                throw new Exception("Changed date stating was not defined.");
            }
                            
            if (!DateTime.TryParse(arguments["changeddatestart"],  out changedDateStart))
            {
                  throw new Exception("ChangedDateStart has the wrong format.");
            }
            

            if ((arguments.ContainsKey("changeddatestart")) && (!DateTime.TryParse(arguments["changeddateend"],  out changedDateEnd)))
            {
                  throw new Exception("ChangedDateEnd has the wrong format.");
            }
            changedDateEnd.AddDays(1);


            string fullpath = GetFullPath();

            if (FileIsLocked(fullpath))
            {
                Console.WriteLine("File is open, please close it. Path:" + fullpath);
                Console.ReadLine();
            }
            
            WorkItemStore workItemStore = Utils.GetWorkItemStore(uri);
            string userName = GetTFSUserName(projectName, workItemStore);
            WorkItemCollection workItemCollection = ReadTFSHistoryWorkItemCollection(projectName, changedDateStart.ToString(), changedDateEnd.ToString(), workItemStore);
            string historyData = ParseTFSHistory(changedDateStart, changedDateEnd, userName, workItemCollection);

            WriteDataToCSV(historyData);
            Console.ReadLine();
        }


        /// <summary>
        /// Gets the application arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Return the application arguments</returns>
        private static Dictionary<string, string> ReadArgs(string[] args)
        {
            /// <summary>
            /// A dictionary for storing the command-line arguments
            /// </summary>
            Dictionary<string, string> arguments = new Dictionary<string, string>();

            //copy the arguments to the arguments dictionary
            if (args.Length == 0) throw new Exception("No arguments were passed.");
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    string key = args[i].ToLower();
                    while (key.StartsWith("-"))
                    {
                        key = key.Substring(1);
                    }
                    if (++i < args.Length)
                    {
                        string value = args[i];
                        arguments.Add(key, value);
                    }
                    else throw new Exception("Error while parsing arguments.");
                }
            }

            return arguments;
        }

        /// <summary>
        /// Gets the current TFS user name.
        /// </summary>
        /// <param name="projectName">TFS project name.</param>
        /// <param name="workItemStore">TFS workitemstore.</param>
        /// <returns> Gets the current TFS user name.</returns>
        private static string GetTFSUserName(string projectName, WorkItemStore workItemStore)
        {
            string userName = string.Empty;
            //create TFS WIQL qery
            string wiqlme = "SELECT * FROM WorkItems WHERE [System.TeamProject] ='" + projectName + "' AND  [System.AssignedTo] = @me";
            WorkItemCollection workItemCollection = Utils.QueryWorkItems(workItemStore, wiqlme);
            if (workItemCollection.Count > 0)
            {
                //Gets the user name from first task 
                userName = workItemCollection[0].Fields["Assigned To"].Value.ToString();
            }

            return userName;
        }

        /// <summary>
        /// Gets the TFS history data.
        /// </summary>
        /// <param name="projectName">TFS prject name</param>
        /// <param name="changedDateStart">The first date of changing.</param>
        /// <param name="changedDateEnd">The last date of changing.</param>
        /// <param name="workItemStore">Returns the TFS history data.</param>
        /// <returns></returns>
        private static WorkItemCollection ReadTFSHistoryWorkItemCollection(string projectName, string changedDateStart, string changedDateEnd, WorkItemStore workItemStore)
        {
            //Create TFS WIQL string        
        
            string wiql = "SELECT * FROM WorkItems WHERE [System.TeamProject] ='" + projectName + "' AND  [System.WorkItemType] = 'Task'  AND" +
                      "( [System.AreaPath] UNDER 'TIA\\Development\\HMI Runtime Innovation Line\\IOWA Runtime Innovation Line Platform\\Integrationstest'  OR  [System.AreaPath] UNDER 'TIA\\Development\\HMI Runtime Innovation Line\\WinCC OA for BT' )" +
                      " AND  [System.ChangedDate] >= '" + changedDateStart + "' AND  [System.ChangedDate] <= '" + changedDateEnd +
                      "' AND  [Microsoft.VSTS.Scheduling.CompletedWork] >= 0";


            //wqil = [Siemens.Industry.Common.Project] IN ('SIMATIC HMI RT IL Platform', 'SIMATIC HMI RT IL')  
            //AND  [Siemens.Industry.Common.Product] = 'RTIL_Platform'  AND  [System.WorkItemType] = 'Request' AND  [System.Title] NOT CONTAINS 'Unittest'  AND  [System.AssignedTo] NOT CONTAINS 'MKS'  AND  [System.AssignedTo] &lt;&gt; 'Roesch, Christoph'  AND  [System.AssignedTo] &lt;&gt; 'Langguth, Matthias'  AND  [System.AssignedTo] &lt;&gt; 'Nitzsche, Stefan'  AND  [System.AssignedTo] &lt;&gt; 'Jakob,Torsten'  AND  [Siemens.Industry.Common.ProjectTags] NOT CONTAINS 'Unittest'  AND  [System.AssignedTo] &lt;&gt; 'Strobel, Holger' 

            //Read with WIQL from TFS history.
            WorkItemCollection workItemCollection = Utils.QueryWorkItems(workItemStore, wiql);
            //Return the TFS workitemcollection.
            return workItemCollection;
        }

        /// <summary>
        /// Parse TFS history data. 
        /// </summary>
        /// <param name="changedDateStart">The first date of changing.</param>
        /// <param name="changedDateEnd">The last date of changing.</param>
        /// <param name="userName">The TFS user name.</param>
        /// <param name="workItemCollection">TFS workitemcollection.</param>
        /// <returns>Returns the filtered TFS history data.</returns>
        private static string ParseTFSHistory(DateTime changedDateStart, DateTime changedDateEnd, string userName, WorkItemCollection workItemCollection)
        {
          
            StringBuilder sb = new StringBuilder();
            List<string> headerList = new List<string> { "Changed Date", "Changed By", "ID", "Title", "Iteration Path", "Stack Rank", 
                                                         "CompletedWork OriginalValue", "CompletedWork NewValue", "RemainingWork OriginalValue", "RemainingWorkField NewValue"};
            AddValuesToSB(sep, sb, headerList);         

            List<string> historyValues = new List<string>();

            foreach (WorkItem workItem in workItemCollection)
            {
                // grab the history field
                foreach (Revision workItemRevisions in workItem.Revisions)
                {
                    //Get the field Remaining work from this revision
                    Field remainingWorkField = workItemRevisions.Fields["Remaining Work"];
                    //Get the field Completed work from this revision
                    Field completedWorkField = workItemRevisions.Fields["Completed Work"];
                    string changedBy = workItemRevisions.Fields["Changed By"].Value.ToString();
                    bool writeData = (userName.Contains("Csaba") || userName.Contains("Manfred") || userName.Contains("Levente"));
                    if (!writeData)
                    {
                        writeData = (changedBy == userName);
                    }

                    if ((completedWorkField.OriginalValue != null) && (completedWorkField.Value != completedWorkField.OriginalValue) &&
                        ((DateTime)workItemRevisions.Fields["Changed Date"].Value >= changedDateStart) &&
                        ((DateTime)workItemRevisions.Fields["Changed Date"].Value <= changedDateEnd) &&
                        !changedBy.Contains("tfs") && writeData)
                    {
                        historyValues.Clear();
                        historyValues.Add(workItemRevisions.Fields["Changed Date"].Value.ToString());
                        historyValues.Add(workItemRevisions.Fields["Changed By"].Value.ToString());
                        historyValues.Add(workItemRevisions.Fields["ID"].Value.ToString());
                        Console.WriteLine(workItemRevisions.Fields["ID"].Value.ToString());
                        historyValues.Add(workItemRevisions.Fields["Title"].Value.ToString());
                        historyValues.Add(workItemRevisions.Fields["Iteration Path"].Value.ToString());
                        if (workItemRevisions.Fields["Stack Rank"].Value == null)
                        {
                            historyValues.Add(string.Empty);
                        }
                        else
                        {
                            historyValues.Add(workItemRevisions.Fields["Stack Rank"].Value.ToString());
                        }                        
                        historyValues.Add(completedWorkField.OriginalValue.ToString());
                        historyValues.Add(completedWorkField.Value.ToString());
                        historyValues.Add(remainingWorkField.OriginalValue.ToString());
                        historyValues.Add(remainingWorkField.Value.ToString());
                        AddValuesToSB(sep, sb, historyValues);                     
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Append the values to the StringBuilder
        /// </summary>
        /// <param name="sep">Separator.</param>
        /// <param name="sb">SB.</param>
        /// <param name="values">Values to write.</param>
        private static void AddValuesToSB(string sep, StringBuilder sb, IList values)
        {
            foreach (var value in values)
            {
                sb.Append(value);
                sb.Append(sep);
            }
           
            sb.Append(Environment.NewLine);
        }


        /// <summary>
        /// Write TFS history result to a csv file.
        /// </summary>
        /// <param name="historyData">The filtered TFS history result.</param>
        private static void WriteDataToCSV(string historyData)
        {
            string fullpath = GetFullPath();

            if (FileIsLocked(fullpath))
            {
                Console.WriteLine("File is open, please close it. Path:" + fullpath);
            }
            else
            {
                //Write to a file
                using (StreamWriter outfile = new StreamWriter(fullpath, false))
                {
                    outfile.Write(historyData);
                }
                Console.WriteLine("Data was written to csv file:" + fullpath);
            }
            Console.WriteLine("PRESS ENTER!");
        }

        /// <summary>
        /// Returns the full path.
        /// </summary>
        /// <returns></returns>
        private static string GetFullPath()
        {
            // to get the location the assembly is executing from
            //(not neccesarily where the it normally resides on disk)
            // in the case of the using shadow copies, for instance in NUnit tests, 
            // this will be in a temp directory.
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            //To get the location the assembly normally resides on disk or the install directory
            //string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            //once you have the path you get the directory with:
            string fullpath = System.IO.Path.GetDirectoryName(path) + "\\CompletedHoursITest.csv";
            return fullpath;
        }

        /// <summary>
        /// Gets whether the file already open or in use.
        /// </summary>
        /// <param name="strFullFileName">Fullpath</param>
        /// <returns> Returns whether the file already open or in use.</returns>
        private static bool FileIsLocked(string strFullFileName)
        {
            bool blnReturn = false;
            System.IO.FileStream fs;
            try
            {
                fs = System.IO.File.Open(strFullFileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                fs.Close();
            }
            catch (System.IO.IOException ex)
            {
                blnReturn = true;
            }
            return blnReturn;
        }
    }
}



