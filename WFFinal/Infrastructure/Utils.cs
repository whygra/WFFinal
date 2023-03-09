﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFFinal.Models;

namespace WFFinal.Infrastructure
{
    public static class Utils
    {
        public static string DataPath = "App_Data/";
        public static string TempFilePath = DataPath + "film_distributor.json";

        // генерация случайных чисел
        static Random Random = new Random();
        public static int GetRand(int lo, int hi)
        {
            return Random.Next(lo, hi);
        }
        public static bool GetRand()
        {
            return Random.Next(2) == 0;
        }

        // вернуть случайный элемент коллекции
        public static T SelectRand<T>(IEnumerable<T> collection)
        {
            int numElements = collection.Count();

            return collection.ElementAt(GetRand(0, numElements));
        }

    }
}
