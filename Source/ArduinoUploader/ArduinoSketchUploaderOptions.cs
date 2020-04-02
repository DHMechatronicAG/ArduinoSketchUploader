using System;
using ArduinoUploader.Hardware;

namespace ArduinoUploader
{
    public class ArduinoSketchUploaderOptions
    {
        public string FileName { get; set; }

        public string PortName { get; set; }

        public ArduinoModel ArduinoModel { get; set; }

        public Int32? DTRPin { get; set; }
    }
}