﻿using System.Runtime.InteropServices;
using ElectronBot.BraincasePreview.Contracts.Services;
using ElectronBot.BraincasePreview.Helpers;
using ElectronBot.BraincasePreview.Models;
using ElectronBot.BraincasePreview.Services.EbotGrpcService;
using OpenCvSharp;
using Verdure.ElectronBot.Core.Models;
using Windows.ApplicationModel;

namespace ElectronBot.BraincasePreview.Services;
public class DefaultActionExpressionProvider : IActionExpressionProvider
{
    public string Name => "Default";

    public Task PlayActionExpressionAsync(string actionName)
    {
        Mat image = new();

        var capture = new VideoCapture(Package.Current.InstalledLocation.Path + $"\\Assets\\Emoji\\{actionName}.mp4");

        while (true)
        {
            capture.Read(image);

            capture.Set(OpenCvSharp.VideoCaptureProperties.PosFrames,
                capture.Get(OpenCvSharp.VideoCaptureProperties.PosFrames) + 1);

            if (image.Empty())
            {
                break;
            }
            else
            {

                //var mat1 = image.Resize(new OpenCvSharp.Size(240, 240), 0, 0, OpenCvSharp.InterpolationFlags.Lanczos4);

                //var mat2 = mat1.CvtColor(OpenCvSharp.ColorConversionCodes.RGBA2BGR);

                var dataMeta = image.Data;

                var data = new byte[240 * 240 * 3];

                Marshal.Copy(dataMeta, data, 0, 240 * 240 * 3);

                EmojiPlayHelper.Current.Enqueue(new EmoticonActionFrame(data));
            }
        }

        return Task.CompletedTask;
    }

    public async Task PlayActionExpressionAsync(string actionName, List<ElectronBotAction> actions)
    {
        Mat image = new();

        var capture = new VideoCapture(Package.Current.InstalledLocation.Path + $"\\Assets\\Emoji\\{actionName}.mp4");

        var frameCount = capture.FrameCount;

        var currentAction = new ElectronBotAction();

        while (true)
        {
            capture.Read(image);

            capture.Set(OpenCvSharp.VideoCaptureProperties.PosFrames,
                capture.Get(OpenCvSharp.VideoCaptureProperties.PosFrames) + 4);

            if (image.Empty())
            {
                break;
            }
            else
            {
                if (actions != null && actions.Count > 0)
                {
                    var pos = capture.PosFrames;

                    var bili = ((double)pos / frameCount);

                    var actionCount = (int)(actions.Count * bili);

                    if (actionCount > actions.Count)
                    {
                        actionCount = actions.Count;
                    }


                    currentAction = actions[actionCount];
                }

                //var mat1 = image.Resize(new OpenCvSharp.Size(240, 240), 0, 0, OpenCvSharp.InterpolationFlags.Lanczos4);

                //var mat2 = mat1.CvtColor(OpenCvSharp.ColorConversionCodes.RGBA2BGR);

                var dataMeta = image.Data;

                var data = new byte[240 * 240 * 3];

                Marshal.Copy(dataMeta, data, 0, 240 * 240 * 3);

                var frameData = new EmoticonActionFrame(data, true,
                    currentAction.J1,
                    currentAction.J2,
                    currentAction.J3,
                    currentAction.J4,
                    currentAction.J5,
                    currentAction.J6);

                //EmojiPlayHelper.Current.Enqueue(frameData);

                //通过grpc通讯和树莓派传输数据 
                var grpcClient = App.GetService<EbGrpcService>();

                await grpcClient.PlayEmoticonActionFrameAsync(frameData);
            }
        }
    }

    public Task PlayActionExpressionAsync(EmoticonAction emoticonAction)
    {
        Mat image = new();

        var capture = new VideoCapture(emoticonAction.EmojisVideoPath);

        while (true)
        {
            capture.Read(image);

            capture.Set(OpenCvSharp.VideoCaptureProperties.PosFrames,
                capture.Get(OpenCvSharp.VideoCaptureProperties.PosFrames) + 1);

            if (image.Empty())
            {
                break;
            }
            else
            {

                //var mat1 = image.Resize(new OpenCvSharp.Size(240, 240), 0, 0, OpenCvSharp.InterpolationFlags.Lanczos4);

                //var mat2 = mat1.CvtColor(OpenCvSharp.ColorConversionCodes.RGBA2BGR);

                var dataMeta = image.Data;

                var data = new byte[240 * 240 * 3];

                Marshal.Copy(dataMeta, data, 0, 240 * 240 * 3);

                EmojiPlayHelper.Current.Enqueue(new EmoticonActionFrame(data));
            }
        }
        return Task.CompletedTask;
    }
}