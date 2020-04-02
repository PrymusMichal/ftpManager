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
using MahApps.Metro.Controls;
using System.Web.UI.WebControls;

namespace ftpManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        
        
        public MainWindow()
        {
            InitializeComponent();
            
        }


        private void refreshDirectoryContent()
        {
            var list = new List<string>();

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer.Text);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            using (var response=(FtpWebResponse)request.GetResponse())
            {
                using (var stream=response.GetResponseStream())
                {
                    using (var reader=new StreamReader(stream,true))
                    {
                        while (!reader.EndOfStream)
                        {
                            list.Add(reader.ReadLine());
                        }
                    }
                }
                ftpResponse.Text = $"Refresh complete, status {response.StatusDescription}";
            }
            directoryContent.ItemsSource = list;
        }
        private void send_Click(object sender, RoutedEventArgs e)
        {

           /* 
            string fileName = "test.txt";
            string conent = sendContent.Text;
            string host = ftpServer.Text+'/'+fileName;
            Byte[] content = new UTF8Encoding(true).GetBytes("test");
            using (FileStream fs=File.Create(fileName))
            {
                fs.Write(content, 0, content.Length);
            }
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.ContentLength = conent.Length;
            using (Stream requestStream=request.GetRequestStream())
            {
                requestStream.Write(content, 0, content.Length);
            }

            using (FtpWebResponse response=(FtpWebResponse)request.GetResponse())
            {
                ftpResponse.Text = $"Upload File complete, status {response.StatusDescription}";
            }*/
            refreshDirectoryContent();
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem=directoryContent.SelectedItem.ToString();
            ftpResponse.Text = selectedItem;
            string message = "Are you sure that you want to delete this file? " + selectedItem;
            var result = MessageBox.Show(message, "",
                MessageBoxButton.YesNo);
           
            if (result == MessageBoxResult.Yes)
            {
               
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer.Text+'/'+selectedItem);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpRequest(request);
            }
        }
        

        private void uploadItem(string filePath)
        {
            string destination=ftpServer.Text+'/'+System.IO.Path.GetFileName(filePath);
            using (var client = new WebClient())
            {
                client.UploadFile(destination, WebRequestMethods.Ftp.UploadFile, filePath);
            }
            refreshDirectoryContent();
        }

        private void dropBoxUpload(object sender, RoutedEventArgs e)
        {
            foreach(string item in dropBox.Items)
            {
                uploadItem(item);
            }
            refreshDirectoryContent();
        }
        private void ftpRequest(FtpWebRequest request)
        {
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            ftpResponse.Text =response.StatusDescription;
            refreshDirectoryContent();
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            refreshDirectoryContent();
        }

        private void dropBox_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            
            foreach (var file in files)
            {
                dropBox.Items.Add(new File(System.IO.Path.GetFileName(file),file));
            }

        }

        private void dropBox_Enter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop,false))
            {
                e.Effects = DragDropEffects.All;
                
            }
        }
    }
}
