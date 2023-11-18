using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class RandomNumber
{

    public static int[] IntRandomNumber(int tedad, int min, int max, bool tekrari)
    {
        int[] RndNumber = new int[tedad];
        for (int i = 0; i < tedad; i++)
        {
            RndNumber[i] = -100000;
        }
        //RndNumber[0] = 0;
        if (!tekrari)
        {
            for (int i = 0; i < tedad; i++)
            {
                int count = i;
                int temp = Random.Range(min, max + 1);
                
                for (int j = 0; j <= i; j++)
                {
                    
                    if (RndNumber[j] == temp)
                    {
                        i = i - 1;
                        break;
                    }
                }
                if (count == i)
                {
                    
                    RndNumber[i] = temp;
                }
            }
            
            return RndNumber;
        }
        else
        {
            for (int i = 0; i < tedad; i++)
            {
                RndNumber[i] = Random.Range(min, max + 1);
            }
            return RndNumber;
        }
    }
    public static float[] FloatRandomNumber(int tedad, float min, float max, bool tekrari)
    {
        float[] RndNumber = new float[tedad];
        RndNumber[0] = 0;
        if (!tekrari)
        {
            for (int i = 0; i < tedad; i++)
            {
                int count = i;
                float temp = Random.Range(min, max + 1);
                for (int j = 0; j <= i; j++)
                {
                    if (RndNumber[j] == temp)
                    {
                        i = i - 1;
                        break;
                    }
                }
                if (count == i)
                {
                    RndNumber[i] = temp;
                }
            }
            return RndNumber;
        }
        else
        {
            for (int i = 0; i < tedad; i++)
            {
                RndNumber[i] = Random.Range(min, max + 1);
            }
            return RndNumber;
        }
    }

    public static int[] Makhloot(int tedad , int min , int max)
    {
        int[] rndNumber = new int[tedad];
        for (int i = 0; i < tedad; i++)
        {
            rndNumber[i] = min + i;
        }
        for (int i = 0; i < 1000 ; i++)
        {
            int[] rnd = new int[2];
            rnd = IntRandomNumber(2, 0, tedad - 1, false);
            var tmp = rndNumber[rnd[0]];
            rndNumber[rnd[0]] = rndNumber[rnd[1]];
            rndNumber[rnd[1]] = tmp;
        }
        return rndNumber;
    }
    public static int RandomForPuzzle()
    {
        int ForPuzzle = Random.Range(1,5)*90;
        
        return ForPuzzle;
    }

    public static void PositionRandom(GameObject[] gm)
    {
        int[] rnd = new int[2];
        for (int i = 0; i < gm.Length * 40; i++)
        {
            rnd = IntRandomNumber(2, 0, gm.Length - 1, false);
            Vector3 pos = gm[rnd[0]].transform.localPosition;
            gm[rnd[0]].transform.localPosition = gm[rnd[1]].transform.localPosition;
            gm[rnd[1]].transform.localPosition = pos;
        }
    }
    public static void PositionRandom(Image[] gm)
    {
        int[] rnd = new int[2];
        for (int i = 0; i < gm.Length * 40; i++)
        {
            rnd = IntRandomNumber(2, 0, gm.Length - 1, false);
            Vector3 pos = gm[rnd[0]].transform.localPosition;
            gm[rnd[0]].transform.localPosition = gm[rnd[1]].transform.localPosition;
            gm[rnd[1]].transform.localPosition = pos;
        }
    }
    public static void PositionRandom(Button[] gm)
    {
        int[] rnd = new int[2];
        for (int i = 0; i < gm.Length * 40; i++)
        {
            rnd = IntRandomNumber(2, 0, gm.Length - 1, false);
            Vector3 pos = gm[rnd[0]].transform.localPosition;
            gm[rnd[0]].transform.localPosition = gm[rnd[1]].transform.localPosition;
            gm[rnd[1]].transform.localPosition = pos;
        }
    }

    public static void MakhlootArray(int[] input)
    {
        for (int i = 0; i < input.Length * 150; i++)
        {
            int[] rnd = new int[2];
            rnd = IntRandomNumber(2, 0, input.Length - 1, false);
            int tmp = input[rnd[0]];
            input[rnd[0]] = input[rnd[1]]; ;
            input[rnd[1]] = tmp;
        }
    }
    public static void MakhlootArray(float[] input)
    {
        for (int i = 0; i < input.Length * 150; i++)
        {
            int[] rnd = new int[2];
            rnd = IntRandomNumber(2, 0, input.Length - 1, false);
            float tmp = input[rnd[0]];
            input[rnd[0]] = input[rnd[1]]; ;
            input[rnd[1]] = tmp;
        }
    }
    public static void MakhlootArray(GameObject[] input)
    {
        for (int i = 0; i < input.Length * 150; i++)
        {
            int[] rnd = new int[2];
            rnd = IntRandomNumber(2, 0, input.Length - 1, false);
            GameObject tmp = input[rnd[0]];
            input[rnd[0]] = input[rnd[1]]; ;
            input[rnd[1]] = tmp;
        }
    }
    public static void RandomBox(GameObject[] gm)
    {

        GameObject tmp;
        int[] rnd = new int[2];
        for (int i = 0; i < gm.Length * 100; i++)
        {
            rnd = IntRandomNumber(2, 0, gm.Length - 1, false);
            tmp = gm[rnd[0]];
            gm[rnd[0]] = gm[rnd[1]];
            gm[rnd[1]] = tmp;
        }
    }
    public static void RandomBox(Transform[] gm)
    {
        Transform tmp;
        int[] rnd = new int[2];
        for (int i = 0; i < gm.Length * 100; i++)
        {
            rnd = IntRandomNumber(2, 0, gm.Length - 1, false);
            tmp = gm[rnd[0]];
            gm[rnd[0]] = gm[rnd[1]];
            gm[rnd[1]] = tmp;
        }
    }
}
