using System;
using System.IO;
using System.Text;

namespace RTool
{
    class Program
    {
        static void Main( string[] args )
        {
            OpenAndFixData();
            Console.ReadLine();
        }

        public delegate void OpenWindowDelegate( Action yesBack = null );
        public static OpenWindowDelegate confirmWindowDel;
        public static OpenWindowDelegate warningWindowDel;

        /// <summary>
        /// 将文件中列数据增加制定数值后，覆盖保存
        /// </summary>
        /// <param name="fullPath">要修改文件完整路径。建议修改为固定路径，只需更改文件名即可</param>
        public static void OpenAndFixData( string fullPath = "C:/Users/前端程序机/Desktop/航天路20170407.bgs" , decimal offset = 0.2815m )
        {
            FileInfo fileInfo = new FileInfo( fullPath );
            if( !File.Exists( fullPath ) )
            {
                warningWindowDel?.Invoke();
                return;
            }

            StringBuilder sb = new StringBuilder();
            using( var fs = fileInfo.OpenText() )
            {
                string allLines = fs.ReadToEnd();
                string[] rowArr = allLines.Split( "\r\n" );
                for( int i = 0; i < rowArr.Length; i++ )
                {
                    string[] colArr = rowArr[ i ].Split( " " );
                    //默认处理一行的3组数据
                    if( colArr.Length > 2 )
                    {
                        decimal middleNum = decimal.Parse( colArr[ 1 ] );
                        middleNum += offset;
                        colArr[ 1 ] = middleNum.ToString();
                    }
                    else
                    {
                        warningWindowDel?.Invoke();
                    }
                    sb = sb.AppendJoin( " " , colArr );
                    sb.Append( "\r\n" );
                }
            }

            string finalResult = sb.ToString();
            StreamWriter sw = new StreamWriter( fullPath );
            sw.WriteLine( finalResult );
            sw.Close();
        }
    }
}
