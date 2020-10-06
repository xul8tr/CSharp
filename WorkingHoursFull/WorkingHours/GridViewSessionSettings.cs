using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkingHours
{
    internal sealed class GridViewSessionSettings : ApplicationSettingsBase
    {

        private static GridViewSessionSettings _defaultInstace =
        (GridViewSessionSettings)ApplicationSettingsBase
        .Synchronized(new GridViewSessionSettings());
        //---------------------------------------------------------------------
        public static GridViewSessionSettings Default
        {
            get { return _defaultInstace; }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        public IDictionary<string, IList<ColumnSettings>> SessionSettings
        {
            get
            {
                object seset = this["SessionSettings"];
                if (seset == null)
                {
                    return new Dictionary<string, IList<ColumnSettings>>();
                }

                return this["SessionSettings"] as IDictionary<string, IList<ColumnSettings>>;
            }
            set
            {
                this["SessionSettings"] = value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        public Size WindowSize
        {
            get
            {
                object winsize = this["WindowSize"];
                if (winsize == null)
                {
                    return new Size(480, 400);
                }

                return (Size)winsize;
            }
            set
            {
                this["WindowSize"] = value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        public Point WindowLocation
        {
            get
            {
                object winloc = this["WindowLocation"];
                if (winloc == null)
                {
                    return new Point(0, 0);
                }

                return (Point)winloc;
            }
            set
            {
                this["WindowLocation"] = value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        public FormWindowState WindowState
        {
            get
            {
                object winstate = this["WindowState"];
                if (winstate == null)
                {
                    return FormWindowState.Normal;
                }

                return (FormWindowState)winstate;
            }
            set
            {
                this["WindowState"] = value;
            }
        }
    }
}
