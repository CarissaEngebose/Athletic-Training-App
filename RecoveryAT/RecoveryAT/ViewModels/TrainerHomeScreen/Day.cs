using System.ComponentModel;

namespace CalendarManagment;
public class Day : INotifyPropertyChanged
{
    private bool _isSelected;
    public string DayName { get; set; } // mon, tue etc
    public int DayNumber { get; set; }
    public event Action<Day> IsSelectedEvent;  // Event for when a day is selected
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            if(value){
                IsSelectedEvent?.Invoke(this);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }

    public Day(String dayName, int dayNumber)
    {
        this.DayName = dayName;
        this.DayNumber = dayNumber;
        this.IsSelected = false;
    }  
    public override bool Equals(object o)
    {
        if (!(o is Day))
            return false;
        Day SecondDay = (Day)o;
        return this.DayName == SecondDay.DayName && this.DayNumber == SecondDay.DayNumber;
    }
}