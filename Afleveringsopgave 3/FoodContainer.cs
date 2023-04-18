using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afleveringsopgave_3
{
    /// <summary>
    /// The main purpose of this class is to trigger events when changes occur to the food data.
    /// </summary>
    public class FoodContainer
    {
        public int Count => _foodList.Count;
        public ReadOnlyCollection<Food> List => _foodList.AsReadOnly();
        public event Action<string>? LogEvent;

        private List<Food> _foodList;        
        
        public FoodContainer()
        {
            _foodList = new List<Food>();
        }

        public void Add(Food food)
        {
            _foodList.Add(food);
            InvokeLogEvent($"Food \"{food.Name}\" added.");
        }

        public void Remove(Food food)
        {
            _foodList.Remove(food);
            InvokeLogEvent($"Food \"{food.Name}\" removed.");
        }


        private void InvokeLogEvent(string message) { LogEvent?.Invoke(message); }        

    }
}
