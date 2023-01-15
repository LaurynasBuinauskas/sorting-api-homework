namespace Sorting.Api.Homework.WebApi.Tests.Unit.Algorithms;

internal class SortAlgorithmTests
{
    protected SortAlgorithm _sortAlgorithm;
    protected List<int> _sortedNumbers;
    protected const string InvalidAlgorithmExceptiontMessage = "Invalid sorting algorithm was chosen";

    [SetUp]
    public void Setup()
    {
        _sortAlgorithm = new SortAlgorithm();
        _sortedNumbers = new List<int> { 
            1, 3, 5, 6, 7, 8, 9, 11, 12, 14, 15, 16, 19, 20, 111 
        };
    }

    [TestCase("BubbleSort")]
    [TestCase("MergeSort")]        
    [TestCase("InsertionSort")]
    public void Should_Sort_Using_Chosen_Algorithm(string algorithm)
    {
        // Given
        var numbers = new List<int> {
            8, 20, 11, 14, 1, 12, 19, 7, 3, 15, 111, 9, 6, 5, 16
        };            

        // When
        _sortAlgorithm.Sort(numbers, algorithm);            

        // Then
        CollectionAssert.AreEqual(_sortedNumbers, numbers);
    }

    [Test]
    public void Should_Throw_Invalid_Algorithm_Exception()
    {
        // Given
        var numbers = new List<int> {
            8, 20, 11, 14, 1, 12, 19, 7, 3, 15, 111, 9, 6, 5, 16
        };

        var algorithm = "Invalid";

        // When
        var exception = Assert.Throws<InvalidAlgorithmException>(() => _sortAlgorithm.Sort(numbers, algorithm));

        // Then
        Assert.That(exception.Message, Is.EqualTo("Invalid sorting algorithm was chosen"));
    }        
}
