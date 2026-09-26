using System;
using System.IO;
using System.Text;
using NAudio.Wave;
using System.Threading.Tasks;

using static Model.Write;

namespace Model;

public static class Sound
{
    public static bool IsPlaying = false;

    

    class Program
    {
        // static void Main()
        // {
        //     // 1. Создаем тестовый звук средствами C# (чистая математика)
        //     int sampleRate = 44100; // 44100 замеров в секунду (стандарт)
        //     float duration = 2.0f;  // Длительность: 2 секунды
        //     int totalSamples = (int)(sampleRate * duration);
            
        //     float[] testAudio = new float[totalSamples];
        //     float frequency = 440.0f; // Нота "Ля" первой октавы

        //     for (int i = 0; i < totalSamples; i++)
        //     {
        //         // Формула синусоидальной волны
        //         float time = (float)i / sampleRate;
        //         testAudio[i] = MathF.Sin(2 * MathF.PI * frequency * time);
        //     }

        //     // 2. Превращаем наш массив float в аудиофайл на диске
        //     string outputPath = "result_sound.wav";
        //     SaveToWav(testAudio, outputPath, sampleRate);

        //     Console.WriteLine($"Звук успешно сгенерирован и сохранен в: {Path.GetFullPath(outputPath)}");
        // }

        /// <summary>
        /// Конвертирует массив float [-1.0 ... 1.0] в полноценный аудиофайл WAV (16-bit PCM, Mono)
        /// </summary>
        static void SaveToWav(float[] floatBuffer, string filePath, int sampleRate)
        {
            // Переводим float-значения в 16-битные числа (short от -32768 до 32767)
            short[] intBuffer = new short[floatBuffer.Length];
            for (int i = 0; i < floatBuffer.Length; i++)
            {
                // Ограничиваем значения, чтобы не выходить за рамки [-1, 1] и избежать хрипа
                float clamped = Math.Clamp(floatBuffer[i], -1.0f, 1.0f);
                
                // Масштабируем до диапазона short.MaxValue
                intBuffer[i] = (short)(clamped * short.MaxValue);
            }

            // Открываем поток для записи файла
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                int byteLength = intBuffer.Length * 2; // 2 байта на один сэмпл (16 бит)

                // ---- ФОРМИРУЕМ WAV ЗАГОЛОВОК (44 байта) ----
                
                bw.Write(Encoding.ASCII.GetBytes("RIFF"));         // ChunkID
                bw.Write(36 + byteLength);                         // ChunkSize (размер файла минус 8 байт)
                bw.Write(Encoding.ASCII.GetBytes("WAVE"));         // Format
                
                bw.Write(Encoding.ASCII.GetBytes("fmt "));         // Subchunk1ID
                bw.Write(16);                                      // Subchunk1Size (16 для формата PCM)
                bw.Write((short)1);                                // AudioFormat (1 означает PCM - несжатый звук)
                bw.Write((short)1);                                // NumChannels (1 = Моно, 2 = Стерео)
                bw.Write(sampleRate);                              // SampleRate (например, 44100)
                bw.Write(sampleRate * 1 * 2);                      // ByteRate (SampleRate * NumChannels * BitsPerSample/8)
                bw.Write((short)(1 * 2));                          // BlockAlign (NumChannels * BitsPerSample/8)
                bw.Write((short)16);                               // BitsPerSample (16 бит)
                
                bw.Write(Encoding.ASCII.GetBytes("data"));         // Subchunk2ID
                bw.Write(byteLength);                              // Subchunk2Size (размер чистого аудио в байтах)

                // ---- ЗАПИСЫВАЕМ САМИ ЗВУКОВЫЕ ДАННЫЕ ----
                foreach (short sample in intBuffer)
                {
                    bw.Write(sample);
                }
            }
        }
    }

    
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