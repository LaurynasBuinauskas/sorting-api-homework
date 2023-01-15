using Sorting.Api.Homework.WebApi.Constants;
using Sorting.Api.Homework.WebApi.Exceptions;

namespace Sorting.Api.Homework.WebApi.Algorithms;

// Implementation class for the sorting algorithms
public class SortAlgorithm : ISortAlgorithm
{
    // Method to choose and apply the algorithm
    public void Sort(List<int> numbers, string algorithm)
    {
        switch (algorithm)
        {
            case AlgorithmNames.Bubble:
                BubbleSort(numbers);
                break;
            case AlgorithmNames.Merge:
                MergeSort(numbers);
                break;                
            case AlgorithmNames.Inertion:
                InsertionSort(numbers);
                break;
            default:
                throw new InvalidAlgorithmException("Invalid sorting algorithm was chosen");
        }
    }

    // Implementation of the BubbleSort algorithm
    private void BubbleSort(List<int> numbers)
    {
        int temp;
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            for (int j = 0; j < numbers.Count - i - 1; j++)
            {
                if (numbers[j] > numbers[j + 1])
                {
                    temp = numbers[j];
                    numbers[j] = numbers[j + 1];
                    numbers[j + 1] = temp;
                }
            }
        }
    }

    // Implementation of the MergeSort algorithm
    private void MergeSort(List<int> numbers)
    {
        if (numbers.Count <= 1)
            return;

        int middle = numbers.Count / 2;
        List<int> left = numbers.GetRange(0, middle);
        List<int> right = numbers.GetRange(middle, numbers.Count - middle);

        MergeSort(left);
        MergeSort(right);

        int i = 0;
        int j = 0;
        int k = 0;
        while (i < left.Count && j < right.Count)
        {
            if (left[i] < right[j])
            {
                numbers[k] = left[i];
                i++;
            }
            else
            {
                numbers[k] = right[j];
                j++;
            }
            k++;
        }
        while (i < left.Count)
        {
            numbers[k] = left[i];
            i++;
            k++;
        }
        while (j < right.Count)
        {
            numbers[k] = right[j];
            j++;
            k++;
        }
    }

    // Implementation of the InsertionSort algorithm
    private void InsertionSort(List<int> numbers)
    {
        int n = numbers.Count;
        for (int i = 1; i < n; ++i)
        {
            int key = numbers[i];
            int j = i - 1;
            
            while (j >= 0 && numbers[j] > key)
            {
                numbers[j + 1] = numbers[j];
                j = j - 1;
            }
            numbers[j + 1] = key;
        }
    }
}
