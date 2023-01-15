# sorting-api-homework

Endpoints:
  /api/Sorting/OrderAndSave:    
    Example request: "7 2 5 8 3"
    Example response (ok): Numbers sorted and saved to MergeSort_result_20230115223630.txt
    
  /api/Sorting/ChooseAlgorithmAndSave:   
    Example request: {
                      "ArrayString": "7 2 5 8 3 8",
                      "SortAlgorithm": "InsertionSort"
                    }
    Example response (ok): Numbers sorted and saved to InsertionSort_result_20230115223927.txt
  
  /api/Sorting/LoadLatestFile:
    No request parameters
    Example response (ok):{
                          "fileName": "InsertionSort_result_20230115223927.txt",
                          "sortedList": [
                            2,
                            3,
                            5,
                            7,
                            8,
                            8
                          ]
                        }
  
  
  
  
