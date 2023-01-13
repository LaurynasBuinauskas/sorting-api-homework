namespace Sorting.Api.Homework.WebApi.Algorithms
{
    public class SortAlgorithm : ISortAlgorithm
    {
        public void Sort(List<int> numbers, string algorithm)
        {
            switch (algorithm)
            {
                case "BubbleSort":
                    BubbleSort(numbers);
                    break;
                case "MergeSort":
                    MergeSort(numbers);
                    break;
                case "HeapSort":
                    HeapSort(numbers);
                    break;
                case "InsertionSort":
                    InsertionSort(numbers);
                    break;
                default:
                    throw new Exception("Invalid sorting algorithm was chosen");
            }
        }

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

        private void HeapSort(List<int> numbers)
        {
            int n = numbers.Count;
            
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(numbers, n, i);
            
            for (int i = n - 1; i >= 0; i--)
            {                
                int temp = numbers[0];
                numbers[0] = numbers[i];
                numbers[i] = temp;
                Heapify(numbers, i, 0);
            }                      
        }

        private void Heapify(List<int> numbers, int n, int i)
        {
            int largest = i; 
            int l = 2 * i + 1;
            int r = 2 * i + 2;
            
            if (l < n && numbers[l] > numbers[largest])
                largest = l;
            
            if (r < n && numbers[r] > numbers[largest])
                largest = r;
            
            if (largest != i)
            {
                int swap = numbers[i];
                numbers[i] = numbers[largest];
                numbers[largest] = swap;
                
                Heapify(numbers, n, largest);
            }
        }

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
}
