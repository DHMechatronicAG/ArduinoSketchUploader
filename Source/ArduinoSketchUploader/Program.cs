using System;
using System.Reflection.Metadata.Ecma335;
using ArduinoUploader;
using CommandLine;
using NLog;

namespace ArduinoSketchUploader
{
    /// <summary>
    /// The ArduinoSketchUploader can upload a compiled (Intel) HEX file directly to an attached Arduino.
    /// </summary>
    internal class Program
    {
        private class ArduinoSketchUploaderLogger : IArduinoUploaderLogger
        {
            private static readonly Logger Logger = LogManager.GetLogger("ArduinoSketchUploader");

            public void Error(string message, Exception exception)
            {
                Logger.Error(exception, message);
            }

            public void Warn(string message)
            {
                Logger.Warn(message);
            }

            public void Info(string message)
            {
                Logger.Info(message);
            }

            public void Debug(string message)
            {
                Logger.Debug(message);
            }

            public void Trace(string message)
            {
                Logger.Trace(message);
            }
        }

        private static void Main(string[] args)
        {
            var logger = new ArduinoSketchUploaderLogger();
            ArduinoSketchUploaderOptions options = null;
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed<CommandLineOptions>(o =>
            {
                options = new ArduinoSketchUploaderOptions
                {
                    PortName = o.PortName,
                    FileName = o.FileName,
                    ArduinoModel = o.ArduinoModel
                };
            });

            if (options == null) return;

            var progress = new Progress<double>(
                p => logger.Info($"Upload progress: {p * 100:F1}% ..."));

            var uploader = new ArduinoUploader.ArduinoSketchUploader(options, logger, progress);
            try
            {
                uploader.UploadSketch();
                Environment.Exit((int) StatusCodes.Success);
            }
            catch (ArduinoUploaderException)
            {
                Environment.Exit((int) StatusCodes.ArduinoUploaderException);
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception: {ex.Message}!", ex);
                Environment.Exit((int) StatusCodes.GeneralRuntimeException);
            }
        }

        private enum StatusCodes
        {
            Success,
            ArduinoUploaderException,
            GeneralRuntimeException
        }
    }
}