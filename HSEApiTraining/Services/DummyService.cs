using System;

namespace HSEApiTraining
{
    public interface IDummyService
    {
        int DummyInt(int n);
    }

    public class DummyService : IDummyService
    {
        private readonly Random _random = new Random();

        public int DummyInt(int n) => _random.Next(0, n);
    }
}