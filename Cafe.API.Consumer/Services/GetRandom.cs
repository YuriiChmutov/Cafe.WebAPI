using System;

namespace Cafe.API.Services
{
    delegate int randomDelegate(int i);
    static class GetRandom
    {
        static readonly Random random = new Random();

        public static randomDelegate returnRandom = (length) =>
        random.Next(DateTime.Now.Millisecond) % length;
    }
}