using System.Windows.Controls;

namespace ChosonTyping.Views;

/// <summary>타속·정확도·진행률 상태띠 — 모든 련습화면 아래에 늘 보인다(설계서 6항 공통).</summary>
public partial class StatsBar : UserControl
{
    public StatsBar()
    {
        InitializeComponent();
    }

    public void Update(double cpm, double accuracy, double progress)
    {
        CpmText.Text = $"{cpm:0} 타/분";
        AccText.Text = $"{accuracy:0} %";
        ProgText.Text = $"진행률 {progress:0}%";
        Fill.Width = Track.ActualWidth * Math.Clamp(progress, 0, 100) / 100.0;
    }
}
