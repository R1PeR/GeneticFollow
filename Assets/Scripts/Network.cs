using System.Collections.Generic;
using UnityEngine;
using System;
public class Network : IComparable<Network>
{
    public float fitness;
    private int[] layers;
    private float[][] neurons;
    public float[][][] weights;
    public Network(Network copy)
    {
        this.layers = new int[copy.layers.Length];
        for (int i = 0; i < copy.layers.Length; i++)
        {
            this.layers[i] = copy.layers[i];
        }
        InitNeurons();
        InitWeights();
        CopyWeights(copy.weights);
    }
    public Network(int[]layers)
    {
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }
        InitNeurons();
        InitWeights();
    }
    private void InitNeurons()
    {
        List<float[]> neuronList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            neuronList.Add(new float[layers[i]]);
        }
        neurons = neuronList.ToArray();
    }
    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronInPreviousLayer = layers[i - 1]; 
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronInPreviousLayer];
                for (int k = 0; k < neuronInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-1.0f,1.0f);
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }
    public void CopyWeights(float[][][] copy)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copy[i][j][k];
                }
            }
        }
    }
    public float[] FeedForward(float[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            neurons[0][i] = input[i];
        }
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0.0f;
                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = (float)System.Math.Tanh(value);
            }
        }
        return neurons[neurons.Length-1];
    }
    public void Mutate(float percent)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];
                    float randomNumber = UnityEngine.Random.Range(0f, 2000f);
                    if (randomNumber <= percent / 1)
                    { //if 1
                      //flip sign of weight
                        weight *= -1f;
                    }
                    else if (randomNumber <= percent / 2)
                    { //if 3
                      //randomly increase by -100% to 100%
                        float factor = UnityEngine.Random.Range(-1f, 1f);
                        weight *= factor;
                    }
                    else if (randomNumber <= percent / 3)
                    { //if 2
                      //pick random weight between -1 and 1
                        weight = UnityEngine.Random.Range(-1f, 1f);
                    }
                    
                    weights[i][j][k] = weight;
                }
            }
        }
    }
    public void CrossOver(Network n, float percent)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];
                    float randomNumber = UnityEngine.Random.Range(0f, 100f);
                    if (randomNumber <= percent)
                    {
                        weight = UnityEngine.Random.Range(-1f, 1f);
                    }
                    weights[i][j][k] = weight;
                }
            }
        }
    }
    public int CompareTo(Network other)
    {
        if(other == null)
        {
            return 1;
        }
        if(fitness > other.fitness)
        {
            return 1;
        }else if (fitness < other.fitness)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}