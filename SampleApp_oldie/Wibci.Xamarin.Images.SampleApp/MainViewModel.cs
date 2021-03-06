﻿using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using Wibci.LogicCommand;
using Xamarin.Forms;
using System;

namespace Wibci.Xamarin.Images.SampleApp
{
    public class MainViewModel : BindableBase
    {
        private string _dimensionLimit;
        private string _imageHeight;

        private string _imageWidth;

        private byte[] _logo;

        private string _message;
        private string _orientation;

        private bool _upscaleImage;

        public MainViewModel()
        {
            SelectPictureCommand = new DelegateCommand(SelectPicture);
            ConvertImageCommand = new DelegateCommand(ConvertImage);
            NewImageDimensionLimit = "130";
        }

        private async void ConvertImage()
        {
            if (Logo != null)
            {
                var imageTools = DependencyService.Get<IImageTools>();

                var convertResult = await imageTools.ConvertImageAsync(new ConvertImageContext(Logo, ImageFormat.Png));
                if (convertResult.TaskResult == TaskResult.Success)
                {
                    ConvertedLogo = convertResult.ConvertedImage;
                    ConvertedSize = ConvertedLogo.SizeInKB().ToString();
                }
            }
            else
            {
                Message = "No Image to convert";
            }
        }

        public string ImageHeight
        {
            get { return _imageHeight; }
            set { SetProperty(ref _imageHeight, value); }
        }

        public string ImageWidth
        {
            get { return _imageWidth; }
            set { SetProperty(ref _imageWidth, value); }
        }

        private string _size;

        public string Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }

        private string _convertedSize;

        public string ConvertedSize
        {
            get { return _convertedSize; }
            set { SetProperty(ref _convertedSize, value); }
        }

        public byte[] Logo
        {
            get { return _logo; }
            set { SetProperty(ref _logo, value); }
        }

        private byte[] _convertedLogo;
        public byte[] ConvertedLogo
        {
            get { return _convertedLogo; }
            set { SetProperty(ref _convertedLogo, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string NewImageDimensionLimit
        {
            get { return _dimensionLimit; }
            set { SetProperty(ref _dimensionLimit, value); }
        }

        public string Orientation
        {
            get { return _orientation; }
            set { SetProperty(ref _orientation, value); }
        }

        public ICommand SelectPictureCommand { get; }

        public ICommand ConvertImageCommand { get; }

        public bool UpscaleImage
        {
            get { return _upscaleImage; }
            set { SetProperty(ref _upscaleImage, value); }
        }

        private async void SelectPicture()
        {
            int limit;
            bool valid = int.TryParse(NewImageDimensionLimit, out limit);
            if (!valid || limit == 0)
            {
                Message = "please select valid image dimension limit value";
                return;
            }

            SelectPictureResult pictureResult = null;
            var choosePicture = DependencyService.Get<IChoosePictureCommand>();
            var request = new ChoosePictureRequest { MaxPixelDimension = limit, UpscaleImageIfSmaller = UpscaleImage };
            pictureResult = await choosePicture.ExecuteAsync(request);

            if (pictureResult.TaskResult == TaskResult.Success)
            {
                var analyseImage = DependencyService.Get<IAnalyseImageCommand>();
                var analyseResult = await analyseImage.ExecuteAsync(new AnalyseImageContext { Image = pictureResult.Image });
                if (analyseResult.IsValid())
                {
                    Logo = pictureResult.Image;
                    ImageWidth = analyseResult.Width.ToString();
                    ImageHeight = analyseResult.Height.ToString();
                    Orientation = analyseResult.Orientaion.ToString();
                    Size = analyseResult.SizeInKB.ToString();
                }
                else
                {
                    Logo = pictureResult.Image;
                }
            }
        }
    }
}