using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Common.Repository
{
    public static class SqlHelper
    {
        public const char OpenQuote = '`';
        public const char CloseQuote = '`';
        public const char Seperator = ',';

        public static string Columns(IEnumerable<string> columns)
        {
            return columns.Aggregate(
                new StringBuilder(),
                (sb, s) => sb.AppendFormat(sb.Length == 0 ? "{0}{1}{2}" : "{3}{0}{1}{2}", 
                    OpenQuote, s, CloseQuote, Seperator),
                sb => sb.ToString());
        }
        public static string Columns(string table, IEnumerable<string> columns)
        {
            return columns.Aggregate(
                new StringBuilder(),
                (sb, s) => sb.AppendFormat(sb.Length == 0 ? "{0}{1}{2}.{0}{3}{2}" : "{4}{0}{1}{2}.{0}{3}{2}", 
                    table, OpenQuote, s, CloseQuote, Seperator),
                sb => sb.ToString());
        }
        public static string ColumnsWithAlias(string table, IEnumerable<string> columns)
        {
            return columns.Aggregate(
                new StringBuilder(),
                (sb, s) => sb.AppendFormat(sb.Length == 0 ? "{0}{1}{2}.{0}{3}{2} AS {1}{3}" : "{4}{0}{1}{2}.{0}{3}{2} AS {1}{3}",
                    table, OpenQuote, s, CloseQuote, Seperator),
                sb => sb.ToString());
        }
        public static string Params(IEnumerable<string> columns)
        {
            return columns.Aggregate(
                new StringBuilder(),
                (sb, s) => sb.AppendFormat(sb.Length == 0 ? "@{0}" : "{1}@{0}",
                    s, Seperator),
                sb => sb.ToString());
        }
        public static string Sets(IEnumerable<string> columns)
        {
            return columns.Aggregate(
                new StringBuilder(),
                (sb, s) => sb.AppendFormat(sb.Length == 0 ? "{0}{1}{2}=@{1}" : "{3}{0}{1}{2}=@{1}",
                    OpenQuote, s, CloseQuote, Seperator),
                sb => sb.ToString());
        }
    }
}
