using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;

namespace Model;

public class Microphone
{
    private WaveInEvent? _waveIn;
    private List<float> _recordedSamples = new List<float>(); // Для сохранения всей сессии

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

    public void StartRecording()
    {
        // Очищаем старые данные перед новой записью
        _recordedSamples.Clear();

        _waveIn = new WaveInEvent
        {
            WaveFormat = new WaveFormat(16000, 16, 1) // 16кГц, 16бит, Моно
        };

        // Подписываемся на события
        _waveIn.DataAvailable += OnDataAvailable;
        _waveIn.RecordingStopped += OnRecordingStopped; // Важное событие!


        _waveIn.StartRecording();
    }

#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

    public void StopRecording()
    {
        // Проверяем, идет ли запись в данный момент
        if (_waveIn != null)
        {
            // Асинхронно останавливаем запись
            _waveIn.StopRecording(); 
        }
    }

    private void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // Конвертируем байты в float (от -1.0 до 1.0)
        float[] samples = new float[e.BytesRecorded / 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = BitConverter.ToInt16(e.Buffer, i * 2);
            samples[i] = sample / 32768f; 
        }

        // Вариант А: Копим данные в общий массив, если нейросеть обрабатывает весь кусок сразу
        #region Копим звук
        #endregion
        
        _recordedSamples.AddRange(samples);

        // Вариант Б: Здесь же можно отправлять samples в потоковую нейросеть (real-time)
        #region Потоковый Эфир
        #endregion
    }

    private void OnRecordingStopped(object sender, StoppedEventArgs e)
    {
        // Освобождаем ресурсы микрофона после полной остановки
        _waveIn?.Dispose();
        _waveIn = null;

        // Здесь всё готово. Массив `_recordedSamples` можно отправлять в нейросеть.
        float[] finalAudioData = _recordedSamples.ToArray();
        #region Конец записи
        #endregion
        
        if (e.Exception != null)
        {
            // Обработка ошибок, если запись прервалась аппаратно
            Console.WriteLine($"Ошибка при записи: {e.Exception.Message}");
        }
    }
}