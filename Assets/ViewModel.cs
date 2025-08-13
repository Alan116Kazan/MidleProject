using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private string _health = " ";

    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public string Health 
    { 
        get => _health;
        set
        {
            if (_health.Equals(value)) return;
            _health = value;                 
            OnPropertyChanged("Health");
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
