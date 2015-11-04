using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MontefioreClient
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer;
        private Board board;
        private TextBlock[,] machineTexts;


        public MainPage()
        {
            InitializeComponent();

            machineTexts = new TextBlock[4, 2];
            machineTexts[0, 0] = wash1;
            machineTexts[1, 0] = wash2;
            machineTexts[2, 0] = wash3;
            machineTexts[3, 0] = wash4;
            machineTexts[0, 1] = dry1;
            machineTexts[1, 1] = dry2;
            machineTexts[2, 1] = dry3;
            machineTexts[3, 1] = dry4;

            Debug.WriteLine("MainPage creating board");
            board = new Board();

            Debug.WriteLine("MainPage openning board");
            if (board.TryOpenBoard())
            {
                Debug.WriteLine("MainPage starting timer");
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(5);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            else
            {
                Debug.WriteLine("MainPage unable to open board");
                errorText.Text = "בעיה בפתיחת הלוח. בדוק חיבורים והפעל מחשב מחדש.";
            }
        }

        private async void Timer_Tick(object sender, object e)
        {
            Debug.WriteLine("Timer_Tick ============================");

            String sLine;
            try
            {
                Debug.WriteLine("Timer_Tick making web request");
                WebRequest wrGETURL = WebRequest.Create("http://m13test.azurewebsites.net/api/data/working");
                using (var res = await wrGETURL.GetResponseAsync())
                {
                    using (var ress = res.GetResponseStream())
                    {
                        StreamReader objReader = new StreamReader(ress);
                        sLine = objReader.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Timer_Tick network error {0}", ex.Message));
                return;
            }

            Debug.WriteLine("Timer_Tick reading response");

            Debug.WriteLine(String.Format("Timer_Tick response: {0}", sLine));
            var opens = sLine.Replace("[", "").Replace("]", "").Split(',');
            Debug.WriteLine("found {0} openings", opens.Length / 3);

            List<Record> recs = new List<Record>();
            for (int i = 0; i < opens.Length / 3; i++)
            {
                recs.Add(new Record() { Id = Int32.Parse(opens[i * 3]), Floor = Int32.Parse(opens[i * 3 + 1]), Type = Int32.Parse(opens[i * 3 + 2]) });
                Debug.WriteLine("Timer_Tick ==== id {0} floor {1} type {2} ", opens[i * 3], opens[i * 3 + 1], opens[i * 3 + 2]);
            }

            for (int floor = 1; floor < 5; floor++)
            {
                for (int type = 1; type < 3; type++)
                {
                    Debug.WriteLine("Timer_Tick checking floor {0} type {1}", floor, type);
                    var o = recs.FirstOrDefault(oo => oo.Floor == floor && oo.Type == type);

                    if (o != null)
                    {
                        Debug.WriteLine("Timer_Tick on");
                        board.TryOpen(type, floor);

                        machineTexts[floor - 1, type - 1].Text = o.Id.ToString();
                        machineTexts[floor - 1, type - 1].Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        Debug.WriteLine("Timer_Tick off");
                        board.TryClose(type, floor);

                        machineTexts[floor - 1, type - 1].Text = "-";
                        machineTexts[floor - 1, type - 1].Foreground = new SolidColorBrush(Colors.Black);
                    }
                }
            }

            Debug.WriteLine("Timer_Tick Sleeping");
        }
    }

    public class Record
    {
        public int Floor { get; set; }
        public int Type { get; set; }
        public int Id { get; set; }
    }
}
