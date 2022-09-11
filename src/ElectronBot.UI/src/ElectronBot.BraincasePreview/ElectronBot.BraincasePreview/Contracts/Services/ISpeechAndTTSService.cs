﻿using Windows.Globalization;
using Windows.Storage.Streams;

namespace ElectronBot.BraincasePreview.Contracts.Services;
public interface ISpeechAndTTSService
{
    Task StartAsync();
    Task CancelAsync();
    Task InitializeRecognizerAsync(Language recognizerLanguage);
    Task ReleaseRecognizerAsync();
    Task<IRandomAccessStream> TextToSpeechAsync(string text);
}
