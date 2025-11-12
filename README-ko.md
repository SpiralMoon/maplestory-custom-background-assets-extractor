한국어 | [English](./README.md)
# MapleStory Custom Background Assets Extractor

[![GitHub license](https://img.shields.io/github/license/SpiralMoon/maplestory-custom-background-assets-extractor.svg)](https://github.com/SpiralMoon/maplestory-custom-background-assets-extractor/blob/master/LICENSE)

메이플스토리의 클라이언트 데이터 파일(.wz)을 분석하여 커스텀 배경 에셋 정보를 추출하여 파일로 출력하는 스크립트입니다.

.wz 파일을 분석하기 위해 [WzComparerR2](https://github.com/Kagamia/WzComparerR2)의 [WzLib](https://github.com/Kagamia/WzComparerR2/tree/master/WzComparerR2.WzLib) 라이브러리를 사용합니다.

## Get Started

### Initialize SubModule

프로젝트를 시작하기 전, submodule을 초기화 해야합니다.

```bash
git submodule update --init --recursive
```

### Configuration

추출 대상이 될 데이터 파일(Base.wz)의 경로와 추출 결과물이 저장될 디렉토리 경로를 설정할 수 있습니다.

```json
{
  "Paths": {
    "WzFilePath": "C:\\Nexon\\Maple\\Data\\Base\\Base.wz",
    "OutputDirectory": "../output"
  }
}
```

설정 파일 경로는 `CustomBackgroundExtractor/appsettings.json` 입니다.

### How to execute?

빠른 실행을 위해 즉시 실행 가능한 배치파일(run.bat)을 제공하고 있습니다.

```bash
$ run.bat
```
또는 CustomBackgroundExtractor 디렉토리에서 직접 dotnet 명령어로 실행할 수 있습니다.
```bash
$ cd CustomBackgroundExtractor
$ dotnet run
```

## Output Results
### Image File Output
- 출력 위치: `output/images/`
- 커스텀 배경 이미지 파일
  <img width="912" height="539" alt="image" src="https://github.com/user-attachments/assets/4549ed89-7ca7-404e-a7e4-c43e9e5fffd2" />

### Custom Background Information
- 출력 위치: `output/ring.json`

```json
{
  "custom_backgrounds": [
    {
      "code": "23",
      "name": "메이플 대학교",
      "background_image_frames": [
        "0"
      ],
      "card_image_frames": [
        "0"
      ],
      "nameplace_image_frames": [],
      "border_line_image_frames": [],
      "is_animation_card": false,
      "has_nameplace": false,
      "has_border_line": false,
      "left_label_font_color": "",
      "left_label_border_line": 5,
      "left_label_border_line_color": "#8826C0",
      "left_info_font_color": "#FFFFFF",
      "right_label_font_color": "#FFFFFF",
      "right_label_border_line": 5,
      "right_label_border_line_color": "#8826C0",
      "right_info_font_color": "#FFFFFF",
      "level_font_color": "#FFFFFF",
      "job_font_color": "#FFFFFF",
      "name_font_color": "#FFFFFF",
      "name_border_line": 100,
      "name_border_line_color": "#3CC1D7"
    },
    ... other custom backgrounds
  ]
}
```


## Dependencies

- [WzComparerR2](https://github.com/SpiralMoon/WzComparerR2)