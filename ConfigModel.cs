namespace MeadowClockGraphics
{
    // Decode the JSON config file 
    public class ConfigModel
    {
        public string SettingA { get; set; }
        public int SettingB { get; set; }
    }

    public class ProgInst
    {
        public int Id { get; set; }
        public int groupId { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }

    public class ProgStep
    {
        public string op { get; set; }
        public int seq { get; set; }

        public ProgInst data { get; set; }
    }
}
