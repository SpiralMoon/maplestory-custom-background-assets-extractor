using Microsoft.Extensions.Configuration;
using CustomBackgroundExtractor;
using WzComparerR2.WzLib;

Console.WriteLine("Start");

#region load configuration

// Load by appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .Build();

var settings = new AppSettings();
config.Bind(settings);

#endregion

#region initialize output directories

string outputDir = settings.Paths.OutputDirectory;
string imagesDir = Path.Combine(outputDir, "images");

Directory.CreateDirectory(outputDir);
Directory.CreateDirectory(imagesDir);

Console.WriteLine("Initialize output directories...");

#endregion

string wzFilePath = settings.Paths.WzFilePath; // Base.wz file path
Wz_Structure wz = new Wz_Structure(); // Root WZ tree

#region open wz (source from WzComparerR2)

Console.WriteLine("Trying open Base.wz");

if (string.Equals(Path.GetExtension(wzFilePath), ".ms", StringComparison.OrdinalIgnoreCase))
{
    wz.LoadMsFile(wzFilePath);
}
else if (wz.IsKMST1125WzFormat(wzFilePath))
{
    wz.LoadKMST1125DataWz(wzFilePath);
    if (string.Equals(Path.GetFileName(wzFilePath), "Base.wz", StringComparison.OrdinalIgnoreCase))
    {
        string packsDir = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(wzFilePath)), "Packs");
        if (Directory.Exists(packsDir))
        {
            foreach (var msFile in Directory.GetFiles(packsDir, "*.ms"))
            {
                wz.LoadMsFile(msFile);
            }
        }
    }
}
else
{
    wz.Load(wzFilePath, true);
}

Console.WriteLine("Base.wz open complete.");

#endregion

Wz_File wzFile = wz.WzNode.GetNodeWzFile(); // Root WZ file

#region load images

Wz_Node imgNodeCustomBackground = wz.WzNode.FindNodeByPath("Etc\\customBackground.img");
Wz_Image imgCustomBackground = imgNodeCustomBackground.GetNodeWzImage();

imgCustomBackground.TryExtract();

Console.WriteLine("Etc\\customBackground.img is Ready.");

#endregion

Output output = new Output(); // json output schema

#region parse custom background info and extract .png

Console.WriteLine("CustomBackground parse start.");

try
{
    foreach (var nodeCustomBackground in imgCustomBackground.Node.Nodes)
    {
        string code = nodeCustomBackground.Text;

        // icon node
        Wz_Node nodeIcon = nodeCustomBackground.Nodes["icon"];
        Wz_Node nodeIconCanvas = nodeIcon.GetLinkedSourceNode(wzFile);
        Wz_Png pngIconRaw = nodeIconCanvas.GetValue<Wz_Png>();

        // Extract and save to .png file
        pngIconRaw.SaveToPng(Path.Combine(imagesDir, $"{nodeCustomBackground.FullPath.Replace("\\", ".")}.icon.png"));

        List<string> backgroundImageFrames = [];
        List<string> cardImageFrames = [];
        List<string> nameplaceImageFrames = [];
        List<string> borderLineImageFrames = [];

        // custom background (character stat info ui)
        Wz_Node nodeImage = nodeCustomBackground.Nodes["image"];

        List<Wz_Node> nodeImageFrames = nodeImage.Nodes
            .Where(node => int.TryParse(node.Text, out _))
            .OrderBy(node => int.Parse(node.Text))
            .ToList();

        backgroundImageFrames = nodeImageFrames.Select(node => node.Text).ToList();

        foreach (Wz_Node nodeFrame in nodeImageFrames)
        {
            Wz_Node nodeFrameCanvas = nodeFrame.GetLinkedSourceNode(wzFile);
            Wz_Png pngFrame = nodeFrameCanvas.GetValue<Wz_Png>();

            pngFrame.SaveToPng(Path.Combine(imagesDir, $"{nodeFrameCanvas.FullPath.Replace("\\", ".")}.png"));
        }

        Wz_Node nodeNameplace = nodeCustomBackground.Nodes["nameplace"];
        Wz_Node nodeBorderLine = nodeCustomBackground.Nodes["borderline"];

        bool hasNameplace = false;
        bool hasBorderLine = false;

        if (nodeNameplace != null)
        {
            hasNameplace = true;

            List<Wz_Node> nodeNameplaceFrames = nodeNameplace.Nodes
                .Where(node => int.TryParse(node.Text, out _))
                .OrderBy(node => int.Parse(node.Text))
                .ToList();

            nameplaceImageFrames = nodeNameplaceFrames.Select(node => node.Text).ToList();

            foreach (Wz_Node nodeFrame in nodeNameplaceFrames)
            {
                Wz_Node nodeFrameCanvas = nodeFrame.GetLinkedSourceNode(wzFile);
                Wz_Png pngFrame = nodeFrameCanvas.GetValue<Wz_Png>();

                pngFrame.SaveToPng(Path.Combine(imagesDir, $"{nodeFrameCanvas.FullPath.Replace("\\", ".")}.png"));
            }
        }

        if (nodeBorderLine != null)
        {
            hasBorderLine = true;

            List<Wz_Node> nodeBorderLineFrames = nodeBorderLine.Nodes
                .Where(node => int.TryParse(node.Text, out _))
                .OrderBy(node => int.Parse(node.Text))
                .ToList();

            borderLineImageFrames = nodeBorderLineFrames.Select(node => node.Text).ToList();

            foreach (Wz_Node nodeFrame in nodeBorderLineFrames)
            {
                Wz_Node nodeFrameCanvas = nodeFrame.GetLinkedSourceNode(wzFile);
                Wz_Png pngFrame = nodeFrameCanvas.GetValue<Wz_Png>();

                pngFrame.SaveToPng(Path.Combine(imagesDir, $"{nodeFrameCanvas.FullPath.Replace("\\", ".")}.png"));
            }
        }

        // custom background card (character equip info ui)
        Wz_Node nodeImage2 = nodeCustomBackground.Nodes["image2"];

        List<Wz_Node> nodeImage2Frames = nodeImage2.Nodes
            .Where(node => int.TryParse(node.Text, out _))
            .OrderBy(node => int.Parse(node.Text))
            .ToList();

        cardImageFrames = nodeImage2Frames.Select(node => node.Text).ToList();

        foreach (Wz_Node nodeFrame in nodeImage2Frames)
        {
            Wz_Node nodeFrameCanvas = nodeFrame.GetLinkedSourceNode(wzFile);
            Wz_Png pngFrame = nodeFrameCanvas.GetValue<Wz_Png>();

            pngFrame.SaveToPng(Path.Combine(imagesDir, $"{nodeFrameCanvas.FullPath.Replace("\\", ".")}.png"));
        }

        bool isAnimationCard = 1 < nodeImage2Frames.Count;

        string name = nodeCustomBackground.Nodes["name"].GetValue<string>();

        string leftLabelFontColor = nodeCustomBackground.FindNodeByPath("Leftlabel\\fontColor")?.GetValue<string>() ?? "";
        int leftLabelBorderLine = nodeCustomBackground.FindNodeByPath("Leftlabel\\borderline")?.GetValue<int>() ?? 0;
        string leftLabelBorderLineColor = nodeCustomBackground.FindNodeByPath("Leftlabel\\borderlinecolor")?.GetValue<string>() ?? "";
        string leftInfoFontColor = nodeCustomBackground.FindNodeByPath("LeftInfo\\fontColor")?.GetValue<string>() ?? "";

        string rightLabelFontColor = nodeCustomBackground.FindNodeByPath("Rightlabel\\fontColor")?.GetValue<string>() ?? "";
        int rightLabelBorderLine = nodeCustomBackground.FindNodeByPath("Rightlabel\\borderline")?.GetValue<int>() ?? 0;
        string rightLabelBorderLineColor = nodeCustomBackground.FindNodeByPath("Rightlabel\\borderlinecolor")?.GetValue<string>() ?? "";
        string rightInfoFontColor = nodeCustomBackground.FindNodeByPath("Rightinfo\\fontColor")?.GetValue<string>() ?? "";

        string levelFontColor = nodeCustomBackground.FindNodeByPath("Level\\fontColor")?.GetValue<string>() ?? "";
        string jobFontColor = nodeCustomBackground.FindNodeByPath("Job\\fontColor")?.GetValue<string>() ?? "";
        string nameFontColor = nodeCustomBackground.FindNodeByPath("Name\\fontColor")?.GetValue<string>() ?? "";
        int nameBorderLine = nodeCustomBackground.FindNodeByPath("Name\\borderline")?.GetValue<int>() ?? 0;
        string nameBorderLineColor = nodeCustomBackground.FindNodeByPath("Name\\borderlinecolor")?.GetValue<string>() ?? "";

        CustomBackground customBackground = new CustomBackground();
        customBackground.Code = code;
        customBackground.Name = name;
        customBackground.IsAnimationCard = isAnimationCard;
        customBackground.BackgroundImageFrames = backgroundImageFrames;
        customBackground.CardImageFrames = cardImageFrames;
        customBackground.NameplaceImageFrames = nameplaceImageFrames;
        customBackground.BorderLineImageFrames = borderLineImageFrames;
        customBackground.HasNameplace = hasNameplace;
        customBackground.HasBorderLine = hasBorderLine;
        customBackground.LeftLabelFontColor = leftLabelFontColor;
        customBackground.LeftLabelBorderLine = leftLabelBorderLine;
        customBackground.LeftLabelBorderLineColor = leftLabelBorderLineColor;
        customBackground.LeftInfoFontColor = leftInfoFontColor;
        customBackground.RightLabelFontColor = rightInfoFontColor;
        customBackground.RightLabelBorderLine = rightLabelBorderLine;
        customBackground.RightLabelBorderLineColor = rightLabelBorderLineColor;
        customBackground.RightInfoFontColor = rightInfoFontColor;
        customBackground.LevelFontColor = levelFontColor;
        customBackground.JobFontColor = jobFontColor;
        customBackground.NameFontColor = nameFontColor;
        customBackground.NameBorderLine = nameBorderLine;
        customBackground.NameBorderLineColor = nameBorderLineColor;

        output.CustomBackgrounds.Add(customBackground);

        Console.WriteLine($"CustomBackground {code} parse completed.");
    }
}
catch (Exception e)
{
    Console.WriteLine("Exception occured.");
    Console.WriteLine(e);

    return;
}

Console.WriteLine("CustomBackground parse finished.");

#endregion

#region write custom_background.json

string jsonOutputPath = Path.Combine(outputDir, "custom_background.json");

using (var writer = new StreamWriter(jsonOutputPath))
{
    var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(output, Newtonsoft.Json.Formatting.Indented);
    writer.Write(jsonStr);

    Console.WriteLine($"CustomBackground data exported to {jsonOutputPath}");
}

#endregion

Console.WriteLine("Done");