# Performance comparison of item lookup

the application is testing lookup times of List, Dictionary and Hashset.
It first generates an array of Guids and uses it as source dataset to record the execution time of:

- Collection creation

  ![alt text](https://github.com/mizrael/collections-lookup/raw/master/creation.png "Collection creation")

  Lists here perform definitely better, likely because Dictionaries and Hashsets have to pay the cost of creating the hash used as key for every item added.

- Collection creation and lookup

  ![alt text](https://github.com/mizrael/collections-lookup/raw/master/creation_lookup.png "Collection creation and lookup")

  Here things start to get interesting: the first case shows the performance of creation and a single lookup, leading to more or less the same stats as simple creation. In the second case instead lookup is performed 1000 times, leading to a net win of Dictionary and Hashset. This is obviously due to the fact that a lookup on a List takes linear time ( O(n) ), being constant instead ( O(1) ) for the other two data structures.

- Lookup of a single item

  ![alt text](https://github.com/mizrael/collections-lookup/raw/master/lookup.png "Lookup of a single item")

  In this case Dictionaries and Hashset win in both executions, due to the fact that the collections have been populated previously.

- Lookup in a Where()

  ![alt text](https://github.com/mizrael/collections-lookup/raw/master/lookup_where.png "Lookup in a Where()")

  For the last example the system is looping over an existing dataset and performing a lookup for the current item. As expected, Dictionaries and Hashset perform definitely better than List.
  

It's easy to see that in almost all the cases makes no difference which data structure is used if the dataset is relatively small, less than 10000 items. The only case where the choice matters is when we have the need to cross two collections and do a search.
