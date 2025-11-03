# DotnetSubtitleConverter docs

## Overview

After adding the library to your project.

Add the namespace, like shown:

```
using DotnetSubtitleConverter;
```

```DotnetSubtitleConverter``` contains the following: 
- ```SubtitleType``` Enum, contains all of the supported subtitle formats as values.
- ```SubtitleConverter``` Static class. This class contains the API. More about the API in "**SubtitleConverter Functions**" section.



## SubtitleConverter Functions

```SubtitleConverter``` library offers following functions: 

### ```ConvertTo()``` </br>
Reads given file and returns the converted subtitle as a string.

Parameters: </br>
```filePath```: path to the file you want to convert. </br>
```subtitleType```: to what format the subtitle gets converted to. </br>
```msOffset``` \<optional\>: offset subtitle timings in milliseconds. Can be a negative number. </br>
```returnOnOffsetOverflow``` \<optional\>: If true and offset makes subtitle timestamp negative, library will throw exception. While false and offset makes timestamp negative, will make the stamp 0. 



Example usage: </br>
```csharp
string output = SubtitleConverter.ConvertTo("./example.srt", SubtitleType.VTT)
StreamWriter sw = new StreamWriter(output_vtt_path);
sw.WriteLine(output);
sw.Close();
```

### ```SubtitleType GetSubtitleType()``` </br>

parameters: </br>
```filePath```: path to the file.

Returns the type of the given subtitle as ```SubtitleType``` enum value.

## Exceptions 

Following exceptions can be thrown while using this.

- OffsetOverFlowException
- InvalidSubtitleException
- FileNotFoundException

Other expections related to <a href="https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=net-9.0">StreamReader</a> are also possible.

## SubtitleType

SubtitleType is enum that contains all of the supported subtitle formats as values. 
