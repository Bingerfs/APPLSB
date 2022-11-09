using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLevelUtil
{
    private static long LevelsSequence(int index)
    {
        if (index < 0)
        {
            return 0;
        }

        return (long)(5 * Math.Pow(index, 2) + 5 * index);
    }

    public static long GetNextClosestThreshold(long experience)
    {
        var index = 0;
        while (experience >= LevelsSequence(index))
        {
            index++;
        }

        return LevelsSequence(index);
    }

    public static long GetPreviousClosestThreshold(float experience)
    {
        var index = 0;
        while (experience >= LevelsSequence(index))
        {
            index++;
        }

        return LevelsSequence(index - 1);
    }

    public static float GetPercentageToNextLevel(float experience)
    {
        var index = 0;
        while (experience >= LevelsSequence(index))
        {
            index++;
        }

        var previousThreshold = LevelsSequence(index - 1);
        var nextThreshold = LevelsSequence(index);
        float currentPercentage = ((float)experience - (float)previousThreshold) / ((float)nextThreshold - (float)previousThreshold);
        return currentPercentage;
    }

    public static int GetLevel(float experience)
    {
        var index = 0;
        while (experience >= LevelsSequence(index))
        {
            index++;
        }

        return index;
    }
}
