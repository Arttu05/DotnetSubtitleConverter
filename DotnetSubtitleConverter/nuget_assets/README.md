# SubtitleConverter

Simple class library, used to convert subtitle files to other subtitle formats.

Documentation can be found in the [Github page](https://github.com/Arttu05/DotnetSubtitleConverter) under the "docs" directory.

## Supported subtitle formats

**IMPORTANT** styling/positioning will be LOST while converting.


| **Format** | **Supported**                     | **Notes**                                              |
|------------|-----------------------------------|-------------------------------------------------------|
| **SRT**    | YES | Will sanitize dialogue, for example removes \<i\>\</i\> and \{\\an8\} etc. |
| **VTT**    | YES | Will sanitize dialogue. Incorrect timestamps will be ignored. |
| **SBV**    | YES ||
| **ASS**    | YES | Asumes that the last field is the text/dialogue. Start and End can be anywhere in the format|


## Usage

Example usage:

```
string output = SubtitleConverter.ConvertTo("./example.srt", SubtitleType.VTT)
StreamWriter sw = new StreamWriter(output_vtt_path);
sw.WriteLine(output);
sw.Close();
```
