# collections-lookup

small performance comparison of item lookup in .NET Core.
The application is generating an array of Guids and using it to generate a List, a Dictionary and an Hashset.
Then it is measuring lookup time, taking in consideration build time during the first execution.

Example results:
![alt text](https://github.com/mizrael/collections-lookup/raw/master/capture.jpeg "Example results")
