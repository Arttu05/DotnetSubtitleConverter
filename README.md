# DotnetSubtitleConverter
Simple class library, that can be used to convert subtitle files to other subtitle formats

# Supported subtitle formats
-SRT
-VTT

# Usage
```
string output = SubtitleConverter.ConvertTo(example_SRT_file_path, SubtitleConverter.SubtitleType.VTT)
StreamWriter sw = new StreamWriter(output_vtt_path);
sw.WriteLine(output);
sw.Close();
```
# subtitleType

SubtitleConverter.SubtitleType is enum, that contains all of the subtitle types (that are supported). 
