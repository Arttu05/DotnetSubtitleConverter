<h1 align="center"> 
    DotnetSubtitleConverter 
</h1>

<p align="center">
    Simple class library, that can be used to convert subtitle files to other subtitle formats and offset subtitle timings
</p>

<p align="center">
    <a href="https://github.com/Arttu05/DotnetSubtitleConverter/actions/workflows/RunTests.yml">
        <img src="https://github.com/Arttu05/DotnetSubtitleConverter/actions/workflows/RunTests.yml/badge.svg" alt="Tests">
    </a>
    <a href="https://github.com/Arttu05/DotnetSubtitleConverter/actions/workflows/CD.yml">
        <img src="https://github.com/Arttu05/DotnetSubtitleConverter/actions/workflows/CD.yml/badge.svg" alt="CD Pipeline">
    </a>
</p>

<p align="center">
  <img src="./assets/Logo.jpg" width=250 alt="project's logo" />
</p>

## Supported subtitle formats

**IMPORTANT** styling/positioning will be LOST while converting.

| **Format** | **Supported**                     | **Notes**                                              |
|------------|-----------------------------------|-------------------------------------------------------|
| **SRT**    | YES | Will sanitize dialogue, for example removes \<i\>\</i\> and \{\\an8\} etc. |
| **VTT**    | YES | Will sanitize dialogue. Incorrect timestamps will be ignored. |
| **SBV**    | YES ||
| **ASS**    | YES | Asumes that the last field is the text/dialogue. Start and End can be anywhere in the format|

## Documentation

[Documentation](./docs) can be found under the "docs" directory

## How to add to your project

### Nuget package

This library is now available as a [nuget package](https://www.nuget.org/packages/SubtitleConverter). 

Dotnet CLI:
```
dotnet add package SubtitleConverter --version 1.0.0
```

Visual Studio Package Manager Console: 
```
PM> NuGet\Install-Package SubtitleConverter -Version 1.0.0
```


### Manually 

1. Clone this repository.
2. build with following command ```dotnet build --configuration release```, while your working directory is in this project's root. 
3. Now you should find the .dll file from ```./DotnetSubtitleConverter/bin/Release/net8.0/DotnetSubtitleConverter.dll```. Copy the path of the dll
4. Add reference to the dll
   - With visual studio: [follow this instruction from stackoverflow](https://stackoverflow.com/a/65017892)
   - manually:
     Add the following string to your projetc's ```.csproj``` file and replace the path with your path </br>
     ```
          <ItemGroup>
            <Reference Include="DotnetSubtitleConverter">
              <HintPath>./your/path/to/the/dll</HintPath>
            </Reference>
          </ItemGroup>
     ```

   
## TODO

- [x] more subtitle types
- [x] offset feature
- [x] integration tests should verify outputs.
- [x] more unit tests 
- [x] custom exceptions
- [x] nuget package
- [x] cd pipeline

## How to contribute

### Adding new subtitle type

1. Add the subtitle type to ```SubtitleType``` enum in ```SubtitleConverter.cs```.

2. Create **public static** class in ```./DotnetSubtitleConverter/Subtitles/``` for the subtitle type and create the following public methods:
    - ```GetSubtitleData(ref StreamReader reader)``` </br>
          This function should parse the given file to ```List<SubtitleData>``` and then return the ```List<SubtitleData>``` .
          ```reader``` is a StreamReader that is at the begining of the given file. </br>
          **Returns** ```List<SubtitleData>```
      
    - ```GetConvertedString(List<SubtitleData> subtitleData)``` </br>
           Creates subtitle from ```List<SubtitleData>```. </br>
           **Returns** ```string```
    
    - ```Check(ref StreamReader reader)``` </br>
          Parses the given file with ```reader```. Used by ```GetSubtitleType()``` to figure out what subtitle format the given file is. </br>
          **Returns** ```bool```, that indicates whether the given file is valid subtitle file. 
      
    This class can and should have private helper methods, but instead of making these methods private make them internal. SubtitleConverter project has set internal functions to be visible for ```Tests``` project. This makes unit testing possible for these helper functions.

3. In ```SubtitleConverter.cs``` Within ```ConvertTo()``` function, add your subtitle format's ```GetSubtitleData()``` To the first switch statement and the ```GetConvertedString()``` to the second switch statement.

4. In ```SubtitleConverter.cs``` Within ```GetSubtitleType()``` function, add your subtitle format's check function in to the switch statement.

**Note** ```CommonUtils.cs``` may contain useful helper functions.
