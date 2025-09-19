# SubtitleConverter

 Simple class library, that can be used to convert subtitle files to other subtitle formats and offset subtitle timings

## Supported subtitle formats
- SRT
- VTT

## Usage

```SubtitleConverter``` library offers following functions: 

### ```string ConvertTo(string filePath, SubtitleType subtitleType, int msOffset = 0, bool returnOnOffsetOverflow = false)``` </br>
Reads given file and returns the converted subtitle as a string.

Parameters: </br>
```filePath```: path to the file you want to convert. </br>
```subtitleType```: to what format the subtitle gets converted to. </br>
```msOffset``` \<optional\>: offset subtitle timings. Can be a negative number. </br>
```returnOnOffsetOverflow``` \<optional\>: If true and offset makes subtitle timestamp negative, library will throw exception. While false and offset makes timestamp negative, will make the stamp 0. 



Example usage: </br>
```
string output = SubtitleConverter.ConvertTo("./example.srt", SubtitleConverter.SubtitleType.VTT)
StreamWriter sw = new StreamWriter(output_vtt_path);
sw.WriteLine(output);
sw.Close();
```

### ```SubtitleType GetSubtitleType(string filePath)``` </br>

Returns the type of the given subtitle.
