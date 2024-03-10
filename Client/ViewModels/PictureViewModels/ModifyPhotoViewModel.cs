using Client.CommandPattern;
using Client.CommandPattern.ConcretePictureCommands;
using Client.Helpers;
using Common.Contract;
using Common.Model;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.ViewModels.PictureViewModels
{
    public class ModifyPhotoViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        public Picture Picture { get; set; }
        private IServices Proxy { get; set; }
        private ImageSource uploadedImage;
        public Picture UnModifiedPicture { get; set; }
        public ICommand LoadImageCommand { get; set; }
        public ICommand SubmitModifyPhotoCommand { get; set; }
        public IInvoker Invoker { get; set; }
        public ObservableCollection<Picture> Pictures { get; set; }

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

        public ModifyPhotoViewModel(Picture SelectedPicture, IServices proxy, IInvoker invoker, ObservableCollection<Picture> Pictures)
        {
            this.LoadImageCommand = new RelayCommand(LoadImageExecute, LoadImageCanExecute);
            this.SubmitModifyPhotoCommand = new RelayCommand(SubmitModifyPhotoExecute, SubmitModifyPhotoCanExecute);

            this.UnModifiedPicture = new Picture() { PictureID = SelectedPicture.PictureID, Name = SelectedPicture.Name, Date = SelectedPicture.Date, Tags = SelectedPicture.Tags, PartOfAlbum = SelectedPicture.PartOfAlbum, CreatedByUser = SelectedPicture.CreatedByUser, Rating = SelectedPicture.Rating, Image = SelectedPicture.Image };
            this.Picture = SelectedPicture;
            this.Proxy = proxy;
            this.Invoker = invoker;
            this.Pictures = Pictures;
            log.Debug("ModifyPhoto: ViewModel constructor called");
        }

        public bool SubmitModifyPhotoCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String photoName = parametars[0] as String;
            DateTime photoDate = (DateTime)parametars[1];
            String photoTags = parametars[2] as String;
            ImageSource uploadedPicSource = parametars[3] as ImageSource;
            String photoID = parametars[5] as String;

            if (parametars == null || string.IsNullOrEmpty(photoName) || photoDate == null || string.IsNullOrEmpty(photoTags) || uploadedPicSource == null || Int32.Parse(photoID) <= 0)
                return false;

            return true;
        }

        public void SubmitModifyPhotoExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];
            ImageSource uploadedPic = parametars[3] as ImageSource;
            String photoID = parametars[5] as String;

            bool IsSuccessfullyExecuted = false;

            Picture.PictureID = Int32.Parse(photoID);
            Picture.Image = imageToByteArray();

            try
            {
                IsSuccessfullyExecuted = Proxy.ModifyPhoto(Picture);
                CommandModifyPicture commandModifyPicture = new CommandModifyPicture(Picture, new ServerReceiver(), Proxy, Pictures, UnModifiedPicture);
                Invoker.RememberCommand(commandModifyPicture);
            }
            catch(Exception e)
            {
                log.Error(String.Format("ModifyPhoto Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("ModifyPhoto: Successfully executed");
            else
                log.Error("ModifyPhoto: Failed to execute");
            UserControl uc = parametars[4] as UserControl;

            Window window = Window.GetWindow(uc);
            window.Close();
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
