using System.Device.Gpio;
using System.Diagnostics;
using RJCP.IO.Ports;

namespace ArduinoUploader.BootloaderProgrammers.ResetBehavior
{
    internal class ResetThroughTogglingDtrBehavior : IResetBehavior
    {
        private bool Toggle { get; }

        private GpioController Controller { get; set; }

        public ResetThroughTogglingDtrBehavior(bool toggle)
        {
            Toggle = toggle;
        }

        public SerialPortStream Reset(SerialPortStream serialPort, SerialPortConfig config)
        {
            if (config.DTRPin.HasValue)
            {
                if (this.Controller == null)
                {
                    this.Controller = new GpioController(PinNumberingScheme.Board);
                    this.Controller.OpenPin(config.DTRPin.Value, PinMode.Output);
                }

                this.Controller.Write(config.DTRPin.Value, PinValue.High);
                System.Threading.Thread.Sleep(32);
                this.Controller.Write(config.DTRPin.Value, PinValue.Low);
            }

            serialPort.DtrEnable = Toggle;
            return serialPort;
        }
    }
}