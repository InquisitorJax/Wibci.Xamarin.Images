﻿using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images
{
    public enum ImageOrientation
    {
        Landscape,
        Portrait,
        Square
    }

    public interface IAnalyseImageCommand : IAsyncLogicCommand<AnalyseImageContext, AnalyseImageResult>
    {
    }

    public class AnalyseImageContext
    {
        public byte[] Image { get; set; }
    }

    public class AnalyseImageResult : CommandResult
    {
        public uint Height { get; set; }
        public ImageOrientation Orientaion { get; set; }
        public uint Width { get; set; }

        public double SizeInKB { get; set; }
    }
}