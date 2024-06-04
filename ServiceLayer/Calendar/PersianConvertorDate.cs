using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace ServiceLayer
{
    public static class PersianConvertorDate
    {
        public static string ToShamsi_Date(this DateTime value)
        {
            PersianCalendar pc=new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }
        public static string ToShamsi_Time(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetHour(value).ToString("00") + ":" +
                   pc.GetMinute(value).ToString("00");
        }
    }
}