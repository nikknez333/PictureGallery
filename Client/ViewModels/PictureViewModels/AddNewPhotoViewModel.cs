using Client.Helpers;
using Common.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Drawing;
using Common.Model;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Collections.ObjectModel;
using log4net;
using Client.CommandPattern;
using Client.CommandPattern.ConcretePictureCommands;

namespace Client.ViewModels.PictureViewModels
{
    public class AddNewPhotoViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public ICommand LoadImageCommand { get; set; }
        public ICommand SubmitAddPhotoCommand { get; set; }
        private ImageSource uploadedImage;
        public User User { get; set; }
        public IInvoker Invoker { get; set; }
        public ObservableCollection<Picture> PicturesUpdate { get; set; }


        public ImageSource UploadedImage
        {
            get
            {
                return uploadedImage;
            }
            set
            {
                if (uploadedImage == value)
                    return;

                uploadedImage = value;
                OnPropertyChanged(nameof(UploadedImage));
            }
        }

        public AddNewPhotoViewModel()
        {

        }

        public AddNewPhotoViewModel(ObservableCollection<Picture> Pictures, IServices proxy, User u, IInvoker invoker)
        {
            LoadImageCommand = new RelayCommand(LoadImageExecute, LoadImageCanExecute);
            SubmitAddPhotoCommand = new RelayCommand(SubmitAddPhotoExecute, SubmitAddPhotoCanExecute);         
            this.Proxy = proxy;
            this.PicturesUpdate = Pictures;
            this.User = u;
            this.Invoker = invoker;
            log.Debug("AddNewPhoto: ViewModel constructor called");
        }

        public bool SubmitAddPhotoCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String photoName = parametars[0] as String;
            DateTime? photoDate = (DateTime?)parametars[1];
            String photoTags = parametars[2] as String;
            ImageSource uploadedPicSource = parametars[3] as ImageSource;
            String photoID = parametars[5] as String;

            if (parametars == null || string.IsNullOrEmpty(photoName) || string.IsNullOrEmpty(photoTags) || photoDate == null || uploadedPicSource == null || Int32.Parse(photoID) <= 0)
                return false;

            return true;
        }

        public void SubmitAddPhotoExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];
            String photoName = parametars[0] as String;
            DateTime photoDate = (DateTime)parametars[1];
            String photoTags = parametars[2] as String;
            ImageSource uploadedPic = parametars[3] as ImageSource;
            String photoID = parametars[5] as String;
            Picture Picture = new Picture();



            Picture.PictureID = Int32.Parse(photoID);
            Picture.Name = photoName;
            Picture.Date = photoDate;
            Picture.Tags = photoTags;
            Picture.Image = imageToByteArray();
            Picture.PartOfAlbum = null;
            Picture.Rating = 0;
            Picture.CreatedByUser = this.User.Username;

            Tuple<bool, int, Picture> result = new Tuple<bool, int, Picture>(false, 0, new Picture());

            try
            {
                result = Proxy.AddNewPhoto(Picture);
                if (result.Item3 != null || result.Item3.PictureID != 0)
                {
                    CommandAddPhoto commandAddPhoto = new CommandAddPhoto(Picture, new ServerReceiver(), Proxy, PicturesUpdate);
                    Invoker.RememberCommand(commandAddPhoto);
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("AddNewPhoto Server Error: {0}", e.Message));
            }

            if (result.Item1)
                log.Info("AddNewPhoto: Successfully executed");
            else
                log.Error("AddNewPhoto: Failed to execute");

            Picture.PictureID = result.Item2;
            PicturesUpdate.Add(Picture);

            UserControl uc = parametars[4] as UserControl;

            Window window = Window.GetWindow(uc);
            window.Close();
        }

        public bool LoadImageCanExecute(object parametar)
        {
            return true;
        }

        public void LoadImageExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];
           
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                if (File.Exists(op.FileName))
                {
                    UploadedImage = new BitmapImage(new Uri(op.FileName, UriKind.Absolute));
                }
            }
        }

        public byte[] imageToByteArray()
        {
            byte[] bytes = null;
            var bitmapSource = UploadedImage as BitmapSource;

            if (bitmapSource != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
