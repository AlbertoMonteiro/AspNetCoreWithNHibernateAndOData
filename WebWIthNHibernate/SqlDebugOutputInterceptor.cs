using NHibernate;
using NHibernate.SqlCommand;
using System;

namespace WebWIthNHibernate
{
    public class SqlDebugOutputInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("info: NHibernate Query");
            Console.ResetColor();
            Console.WriteLine(sql);

            return base.OnPrepareStatement(sql);
        }
    }
}
