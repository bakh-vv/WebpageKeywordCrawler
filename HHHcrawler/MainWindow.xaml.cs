using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Timers;

namespace HHHcrawler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }
        MediaPlayer mediaPlayer = new MediaPlayer();
        public static System.Timers.Timer aTimer;
        string reply;

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string lineSearched = stringTextBox.Text;
            int interval = Convert.ToInt32(intervalBox.Text) * 1000;

            //extract mp3
            File.Create(String.Format("{0}Song.mp3", Directory.GetCurrentDirectory()));
            File.WriteAllBytes(String.Format("{0}\\Song.mp3", Directory.GetCurrentDirectory()), Properties.Resources.Real_Friends);
            

            WebClient client = new WebClient();
            reply = client.DownloadString(linkTextBox.Text);
            

            if (!reply.Contains(lineSearched))
            {
                StatusBox.Text = String.Format("Nothing so far. Last checked at {0}", DateTime.Now.ToString("h:mm:ss tt"));
            }


            while (!reply.Contains(lineSearched))
            {
                await Task.Delay(interval);
                
                StatusBox.Text = String.Format("Nothing so far. Last checked at {0}", DateTime.Now.ToString("h:mm:ss tt"));
                reply = client.DownloadString(linkTextBox.Text);
            }

            if (reply.Contains(lineSearched))
            {
                StatusBox.Text = String.Format("It's there! As of {0}", DateTime.Now.ToString("h:mm:ss tt"));

                Uri uri = new Uri(String.Format("{0}\\Song.mp3", Directory.GetCurrentDirectory()));
                mediaPlayer.Open(uri);
                mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
                mediaPlayer.Play();
                

            }
            
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        private void linkButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkTextBox.Text);
        }
    }
}
