using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Bomberman
{
    public class TeamConfig
    {
        public string Name { get; set; }
        public string DLLPath { get; set; }
    }

    /// <summary>
    /// Class that handles configuration and stores it in an XML file (config.xml)
    /// </summary>
    public class ConfigHandler
    {
        private const string configFilePath = "config.xml";
        public string MapOrigin
        {
            get
            {
                return m_document.Root.Element("mapOptions").Element("mapOrigin").Value;
            }
        }

        public string MapPath
        {
            get
            {
                return m_document.Root.Element("mapOptions").Element("mapImgPath").Value;
            }
        }

        private int ExplosiveRadius
        {
            get
            {
                string val = m_document.Root.Element("gameOptions").Element("explosiveRadius").Value;
                m_explosiveRadius = int.Parse(val);
                return m_explosiveRadius;
            }
        }

        public int getExplosiveRadius()
        {
            if (m_explosiveRadius == -1)    //if uninitialized
                return ExplosiveRadius;
            else
                return m_explosiveRadius;
        }

        private int MaxCountOfPlayerBombs
        {
            get
            {
                string val = m_document.Root.Element("gameOptions").Element("maxCountOfPlayerBombs").Value;
                m_maxCountOfPlayerBombs = int.Parse(val);
                return m_maxCountOfPlayerBombs;
            }
        }

        public int getMaxCountOfPlayerBombs()
        {
            if (m_maxCountOfPlayerBombs == -1)  //if uninitialized
                return MaxCountOfPlayerBombs;
            else
                return m_maxCountOfPlayerBombs;
        }

        //public int MapSize    //TODO: remove
        //{
        //    get
        //    {
        //        return int.Parse(m_document.Root.Element("mapOptions").Element("mapSize").Value);
        //    }
        //}

        //public int NumberOfMines
        //{
        //    get
        //    {
        //        return int.Parse(m_document.Root.Element("mapOptions").Element("numberOfMines").Value);
        //    }
        //}

        public List<TeamConfig> Teams
        {
            get
            {
                List<TeamConfig> result = new List<TeamConfig>();

                foreach (var teamElement in m_document.Root.Element("teams").Elements("team"))
                {
                    TeamConfig tc = new TeamConfig();
                    tc.Name = teamElement.Element("name").Value;
                    tc.DLLPath = teamElement.Element("dllPath").Value;

                    result.Add(tc);
                }

                return result;
            }
        }

        public int WaitTimeBetweenStepsMS
        {
            get
            {
                return int.Parse(m_document.Root.Element("waitTimeBetweenStepsMs").Value);
            }
        }


        private XDocument m_document;
        private int m_explosiveRadius = -1;
        private int m_maxCountOfPlayerBombs = -1;

        private ConfigHandler()
        {

        }

        /// <summary>
        /// Function to instantiate the config class.
        /// </summary>
        /// <returns>The config handler instance</returns>
        public static ConfigHandler GetConfig()
        {
            ConfigHandler configHandler = new ConfigHandler();

            //Try to load the config from file
            try
            {
                configHandler.m_document = XDocument.Load(configFilePath);
                return configHandler;
            }
            catch (Exception e)
            {
                string s = e.Message;
            }

            return null;
        }
    }
}
