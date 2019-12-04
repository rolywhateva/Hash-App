using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Security.Cryptography;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Hash_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    //
   
    public sealed partial class MainPage : Page
    {
        ObservableCollection<string> algorithms = new ObservableCollection<string>();
        Action<Exception> generalErrorHandler = async (exception) => {
            await new MessageDialog(exception.Message).ShowAsync();
        };
        string chosenAlgorithmName;
        public MainPage()
        {
            /*   switch (name)
            {
                case "MD5": Algorithm = MD5.Create(); break;
                case "SHA256":Algorithm = SHA256.Create(); break;
                case "SHA1Managed":Algorithm = SHA1Managed.Create(); break;

            }
             * 
             * */

            this.InitializeComponent();
            algorithms.Add("MD5");
            algorithms.Add("SHA256");
            algorithms.Add("SHA1Managed");
         

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string algo = e.AddedItems[0].ToString();
            chosenAlgorithmName = algo;

        }

        private async void  HashButton_ClickAsync(object sender, RoutedEventArgs e)
        {

            //  StorageFile file = null;
            // GetFile((chosenFile) => file = chosenFile);
            StorageFile file = await GetFile();
           byte[] rez=await  GetBytesFromFile(file);
            HashController controller = new HashController(new HashModel(SHA256.Create()), new HashView());
           
            controller.SetBasedOnString(chosenAlgorithmName);
            controller.UpdateView(rez, async (result) => {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                    builder.Append(result[i].ToString("x2"));
                ResultTextBlock.Text = builder.ToString();
              //  await new MessageDialog(builder.ToString()).ShowAsync();


            });
           
           


        }
        #region Utils 
        private async   Task<StorageFile> GetFile(Action<Exception> errorHandler=null)
        {
           
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");
            Windows.Storage.StorageFile file = null; 
            try
            {
                 file = await picker.PickSingleFileAsync();
            }catch(Exception e)
            {
                if (errorHandler == null)
                    generalErrorHandler(e);
                else 
                errorHandler(e);
            }
            return file;
        }
        private async  Task<byte[]>  GetBytesFromFile(StorageFile file,Action<Exception> errorHandler=null)
        {
            IBuffer buffer = null;
            try
            {
               buffer = await FileIO.ReadBufferAsync(file);
            }
            catch(Exception exception)
            {
                if (errorHandler == null)
                    generalErrorHandler(exception);
                else
                    errorHandler(exception);
            }
            BitArray bytes = new BitArray(buffer.ToArray());
            byte[] array = new byte[bytes.Length];
            for (int i = 0; i < array.Length; i++)
                array[i] = (byte)(bytes[i] ? 1 : 0);
            return array;
        }

        #endregion 
    }
}
