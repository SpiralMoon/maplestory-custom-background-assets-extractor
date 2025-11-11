English | [한국어](./README-ko.md)
# MapleStory Custom Background Assets Extractor

[![GitHub license](https://img.shields.io/github/license/SpiralMoon/maplestory-custom-background-assets-extractor.svg)](https://github.com/SpiralMoon/maplestory-custom-background-assets-extractor/blob/master/LICENSE)

A script that analyzes MapleStory client data files (.wz) to extract custom background asset information, then outputs them as files.

Uses the [WzLib](https://github.com/Kagamia/WzComparerR2/tree/master/WzComparerR2.WzLib) library from [WzComparerR2](https://github.com/Kagamia/WzComparerR2) to analyze .wz files.

## Get Started

### Initialize SubModule

Before starting the project, you need to initialize the submodule.

```bash
git submodule update --init --recursive
```

### Configuration

You can configure the path to the data file (Base.wz) to be extracted and the directory path where the extraction results will be saved.

```json
{
  "Paths": {
    "WzFilePath": "C:\\Nexon\\Maple\\Data\\Base\\Base.wz",
    "OutputDirectory": "../output"
  }
}
```

The configuration file path is `CustomBackgroundExtractor/appsettings.json`.

### How to execute?

A batch file (run.bat) is provided for quick execution.

```bash
$ run.bat
```
Alternatively, you can run it directly with dotnet commands from the CustomBackgroundExtractor directory.
```bash
$ cd CustomBackgroundExtractor
$ dotnet run
```

## Output Results
### Image File Output
- Output location: `output/images/`
- Custom background image files
  <img width="912" height="539" alt="image" src="https://github.com/user-attachments/assets/4549ed89-7ca7-404e-a7e4-c43e9e5fffd2" />

### Custom Background Information
- Output location: `output/custom_background.json`

```json
{
  "custom_backgrounds": [
    {
      "code": "23",
      "name": "메이플 대학교",
      "is_animation_card": false,
      "has_nameplace": false,
      "has_border_line": false,
      "left_label_font_color": "",
      "left_label_border_line": 5,
      "left_label_border_line_color": "8826C0",
      "left_info_font_color": "FFFFFF",
      "right_label_font_color": "FFFFFF",
      "right_label_border_line": 5,
      "right_label_border_line_color": "8826C0",
      "right_info_font_color": "FFFFFF",
      "level_font_color": "FFFFFF",
      "job_font_color": "FFFFFF",
      "name_font_color": "FFFFFF",
      "name_border_line": 100,
      "name_border_line_color": "3CC1D7"
    },
    ... other custom backgrounds
  ]
}
```

## Dependencies

- [WzComparerR2](https://github.com/SpiralMoon/WzComparerR2)