# PNNL OMICS

The PNNL Omics Library (PNNLOmics.dll) is a collection of data analysis, 
file I/O, and visualization routines used by several of the 
omics-related software developed at PNNL.

## Important Classes in PNNLOmics.dll

| Class            | Description |
|------------------|-------------|
| CommandLineParser |  Flexible, powerful class for keeping parameters flags and properties for command line arguments tied together, supporting properties of primitive types (and arrays of primitive types). Supports parameter flags similar to /d -dd --dir, with case sensitivity when needed, with the separator between parameter flag and parameter as ' ' or ':' |
 GenericParserOptions | Methods that demonstrates how to decorate properties in a class so that the CommandLineParser can use them to match command line arguments |

## Contacts

Written by Brian LaMarche, Bryson Gibbons, and Chris Wilkins for the Department of Energy (PNNL, Richland, WA) \
Copyright 2017, Battelle Memorial Institute.  All Rights Reserved. \
E-mail: proteomics@pnnl.gov \
Website: https://panomics.pnl.gov/ or https://omics.pnl.gov

## License

Licensed under the Apache License, Version 2.0; you may not use this file except
in compliance with the License.  You may obtain a copy of the License at
http://www.apache.org/licenses/LICENSE-2.0
