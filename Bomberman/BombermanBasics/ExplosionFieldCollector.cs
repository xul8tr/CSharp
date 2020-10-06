
using System.Collections.Generic;
using System.Linq;

namespace BombermanBasics
{
    public class ExplosionFieldCollector
    {
        public ExplosionFieldCollector()
        {
            m_table = new List<OrderedFields>();
        }

        public void addExplodedField(int timeFactor, int distanceFactor, Field field)
        {
            if (m_table.Find(item => item.FieldP == field) == null) // if the field cannot be found in the list
                m_table.Add(new OrderedFields(timeFactor, distanceFactor, field));
            m_isSortedByTime = false;
        }

        public bool isEmpty()
        {
            if (m_table.Count > 0)
                return false;
            return true;
        }

        public void sortByTimeFactor()
        {
            m_table.Sort(delegate(OrderedFields a, OrderedFields b)
            {
                if (a == null && b == null)
                    return 0;
                else if (a == null)
                    return -1;
                else if (b == null)
                    return 1;
                else if (a.TimeFactor < b.TimeFactor)
                    return -1;
                else if (a.TimeFactor == b.TimeFactor)
                    return 0;
                else
                    return 1;
            });
            m_isSortedByTime = true;
        }

        /// <summary>
        /// returns results with the least time factor plus sorted by distanceFactor
        /// </summary>
        /// <returns></returns>
        public List<OrderedFields> popTheLeastTimeFactor()
        {
            if (!m_isSortedByTime)
                sortByTimeFactor();
            OrderedFields[] tableArray = m_table.ToArray();
            int leastTimeFactor = (m_table.Count > 0) ? (tableArray[0].TimeFactor) : 0;     //time factor of the first element
            int lastIndex = 0;
            while (lastIndex < tableArray.Length)
            {
                if (tableArray[lastIndex].TimeFactor == leastTimeFactor)
                    ++lastIndex;
                else
                    break;
            }
            List<OrderedFields> result = new List<OrderedFields>();
            result.AddRange(m_table.Take(lastIndex + 1));
            int itemCountToRemove = (m_table.Count < (lastIndex + 1)) ? m_table.Count : lastIndex + 1;
            m_table.RemoveRange(0, itemCountToRemove);
            result.Sort(delegate (OrderedFields a, OrderedFields b) //sort by distanceFactor
            {
                if (a == null && b == null)
                    return 0;
                else if (a == null)
                    return -1;
                else if (b == null)
                    return 1;
                else if (a.DistanceFactor < b.DistanceFactor)
                    return -1;
                else if (a.DistanceFactor == b.DistanceFactor)
                    return 0;
                else
                    return 1;
            });
            return result;
        }

        //========================================== nested class =============================================
        public class OrderedFields
        {
            public OrderedFields(int timeFactor, int distanceFactor, Field field)
            {
                TimeFactor = timeFactor;
                DistanceFactor = distanceFactor;
                FieldP = field;
            }

            public int TimeFactor
            {
                get;
                set;
            }

            public int DistanceFactor
            {
                get;
                set;
            }

            public Field FieldP
            {
                get;
                set;
            }
        }
        //=============================== end of the nested class =============================================

        private List<OrderedFields> m_table;
        private bool m_isSortedByTime;
    }
}
