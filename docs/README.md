# DotnetSubtitleConverter docs

# Overview

After adding the library to your project.

Add the namespace, like shown:

```
using DotnetSubtitleConverter;
```

```DotnetSubtitleConverter``` contains the following: 
- ```SubtitleType``` Enum, contains all of the supported subtitle formats as values.
- ```SubtitleConverter``` Static class. This class contains the API. More about the API in "**SubtitleConverter Functions**" section.



# SubtitleConverter Functions

```SubtitleConverter``` library offers following functions: 

## ```ConvertTo()``` </br>
Reads given file and returns the converted subtitle as a string.

Parameters: </br>
```string filePath```: path to the file you want to convert. </br>
```SubtitleType subtitleType```: to what format the subtitle gets converted to. </br>
```int msOffset``` \<optional\>: offset subtitle timings in milliseconds. Can be a negative number. </br>
```bool returnOnOffsetOverflow``` \<optional\>: If true and offset makes subtitle timestamp negative, library will throw exception. While false and offset makes timestamp negative, will make the stamp 0. 

Returns: converted subtitle as a string.

Example usage: </br>
```csharp
string output = SubtitleConverter.ConvertTo("./example.srt", SubtitleType.VTT)
StreamWriter sw = new StreamWriter(output_vtt_path);
sw.WriteLine(output);
sw.Close();
```



## ```ConvertWithStringTo()``` </br>
Same as ```ConvertTo```, except takes string of the file's content, instead of the filepath.

Parameters: </br>
```string fileString```: content/text of the file as a string </br>
```SubtitleType subtitleType```: to what format the subtitle gets converted to. </br>
```int msOffset``` \<optional\>: offset subtitle timings in milliseconds. Can be a negative number. </br>
```bool returnOnOffsetOverflow``` \<optional\>: If true and offset makes subtitle timestamp negative, library will throw exception. While false and offset makes timestamp negative, will make the stamp 0. 

Returns: converted subtitle as a string.

Example usage: </br>
```csharp

StreamReader reader = new StreamReader("./subtitle.srt");
string subtitleFileString = reader.ReadToEnd();
reader.Close();

string output = SubtitleConverter.ConvertWithStringTo(subtitleFileString, SubtitleType.VTT)
StreamWriter sw = new StreamWriter(output_vtt_path);
sw.WriteLine(output);
sw.Close();
```



## ```GetSubtitleType()``` </br>

Reads a file and then returns the subtitle format of the given file. If no subtitle format is detected, throws ```InvalidSubtitleException```


parameters: </br>
```string filePath```: path to the file.

Returns the format of the given subtitle file as ```SubtitleType``` enum value. 

Example usage: </br>
```csharp

SubtitleType format = SubtitleConverter.GetSubtitleType("./subtitle.srt"); // SubtitleType.SRT

```



## ```GetSubtitleTypeWithString()```

Same as ```GetSubtitleType()```, expect instead of taking the file path, takes the file's content as a string.

parameters: </br>
```string fileString```: content/text of the file as a string.

Returns the type of the given subtitle as ```SubtitleType``` enum value.

Example usage: </br>
```csharp

StreamReader reader = new StreamReader("./subtitle.srt");
string subtitleFileString = reader.ReadToEnd();
reader.Close();

SubtitleType format = SubtitleConverter.GetSubtitleTypeWithString(subtitleFileString); // SubtitleType.SRT

```

# Exceptions 

Following exceptions can be thrown while using this.

- OffsetOverFlowException
- InvalidSubtitleException
- FileNotFoundException

Other expections related to <a href="https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=net-9.0">StreamReader</a> are also possible.

## SubtitleType

SubtitleType is enum that contains all of the supported subtitle formats as values. 
