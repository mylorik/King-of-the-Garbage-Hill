﻿using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace King_of_the_Garbage_Hill.Helpers
{
    public sealed class SecureRandom : IServiceTransient
    {
        private readonly RNGCryptoServiceProvider _csp;

        public SecureRandom()
        {
            _csp = new RNGCryptoServiceProvider();
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public int Random(int minValue, int maxExclusiveValue)
        {
            if (minValue == maxExclusiveValue) return minValue;

            if (minValue >= maxExclusiveValue)
                // ReSharper disable once NotResolvedInText its ok.
                throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");
            maxExclusiveValue += 1;
            var diff = (long) maxExclusiveValue - minValue;
            var upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                ui = GetRandomUInt();
            } while (ui >= upperBound);

            return (int) (minValue + ui % diff);
        }

        private uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        private byte[] GenerateRandomBytes(int bytesNumber)
        {
            var buffer = new byte[bytesNumber];
            _csp.GetBytes(buffer);
            return buffer;
        }
    }
}