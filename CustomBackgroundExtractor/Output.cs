using Newtonsoft.Json;

namespace CustomBackgroundExtractor
{
    /// <summary>
    /// Json schema
    /// </summary>
    public class Output
    {
        [JsonProperty("custom_backgrounds")]
        public List<CustomBackground> CustomBackgrounds { get; set; } = new List<CustomBackground>();
    }

    public class CustomBackground
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("is_animation_card")]
        public bool IsAnimationCard { get; set; } = false;

        [JsonProperty("has_nameplace")]
        public bool HasNameplace { get; set; } = false;

        [JsonProperty("has_border_line")]
        public bool HasBorderLine{ get; set; } = false;

        [JsonProperty("left_label_font_color")]
        public string LeftLabelFontColor { get; set; } = string.Empty;

        [JsonProperty("left_label_border_line")]
        public int LeftLabelBorderLine { get; set; } = 0;

        [JsonProperty("left_label_border_line_color")]
        public string LeftLabelBorderLineColor { get; set; } = string.Empty;

        [JsonProperty("left_info_font_color")]
        public string LeftInfoFontColor { get; set; } = string.Empty;

        [JsonProperty("right_label_font_color")]
        public string RightLabelFontColor { get; set; } = string.Empty;

        [JsonProperty("right_label_border_line")]
        public int RightLabelBorderLine { get; set; } = 0;

        [JsonProperty("right_label_border_line_color")]
        public string RightLabelBorderLineColor { get; set; } = string.Empty;

        [JsonProperty("right_info_font_color")]
        public string RightInfoFontColor { get; set; } = string.Empty;

        [JsonProperty("level_font_color")]
        public string LevelFontColor { get; set; } = string.Empty;

        [JsonProperty("job_font_color")]
        public string JobFontColor { get; set; } = string.Empty;

        [JsonProperty("name_font_color")]
        public string NameFontColor { get; set; } = string.Empty;

        [JsonProperty("name_border_line")]
        public int NameBorderLine { get; set; } = 0;

        [JsonProperty("name_border_line_color")]
        public string NameBorderLineColor { get; set; } = string.Empty;
    }
}
