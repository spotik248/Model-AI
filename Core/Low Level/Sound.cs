using NAudio.Wave;
using System.Threading.Tasks;

using static Model.Write;

namespace Model;

public static class Sound
{
    public static bool IsPlaying = false;
    
    public static float[] ReadMp3(string filePath)
    {
        // Принудительно конвертируем в моно, 16кГц
        var outFormat = new WaveFormat(16000, 16, 1); 
        using var reader = new AudioFileReader(filePath);
        using var resampler = new MediaFoundationResampler(reader, outFormat);
        
        // Переводим в массив float (значения от -1.0 до 1.0)
        var sampleProvider = resampler.ToSampleProvider();
        List<float> audioSamples = new List<float>();
        float[] buffer = new float[1024];
        
        int samplesRead;
        while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0) // sampleProvider.Read принимает только float
        {
            audioSamples.AddRange(buffer.Take(samplesRead));
        }
        
        return audioSamples.ToArray();
    }

    public static void PlaySound(string filePath)
    {
        if(IsPlaying) return;
        Task.Run(() =>
        {
            try
            {
                // Сборка пути через вашу утилиту Util.Combinate
                string audioPath = Util.Combine(Util.root, filePath);

                if (!File.Exists(audioPath))
                { Debug("Файла для воспроизведения звука не существует: " + audioPath); return; }

                IsPlaying = true;
                using var audioFile = new AudioFileReader(audioPath);
                using var outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Play();

                // Ожидание окончания воспроизведения
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
                IsPlaying = false;
            }
            catch (Exception ex)
            {
                Exc($"Ошибка воспроизведения звука: ", ex);
            }
        });
    }

    public static void PlayErrorSound()
    {
        //PlaySound(@"Sounds/Err.mp3");
    }
}