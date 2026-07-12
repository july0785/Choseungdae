using System.Windows.Controls;
using ChosonTyping.Core;

namespace ChosonTyping.Views;

/// <summary>타속·정확도·진행률 상태띠 — 모든 련습화면 아래에 늘 보인다(설계서 6항 공통).</summary>
public partial class StatsBar : UserControl
{
    public StatsBar()
    {
        InitializeComponent();
        CpmLabel.Text = Loc.S("stats.cpm");
        AccLabel.Text = Loc.S("stats.acc");
    }

    public void Update(double cpm, double accuracy, double progress)
    {
        CpmText.Text = $"{cpm:0} {Loc.S("stats.unit")}";
        AccText.Text = $"{accuracy:0} %";
        ProgText.Text = Loc.F("stats.progress", $"{progress:0}");
        Fill.Width = Track.ActualWidth * Math.Clamp(progress, 0, 100) / 100.0;
    }
}
