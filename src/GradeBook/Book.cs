using System;
using System.Collections.Generic;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args); 

    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }
        public string Name {get; set;}
    }

    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name {get;}
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book : NamedObject, IBook
    {
        public Book(string name) : base(name)
        {
        }

        public virtual event GradeAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade);

        public virtual Statistics GetStatistics()
        {
            throw new NotImplementedException();
        }
    }

    public class InMemoryBook : Book
    {
        public InMemoryBook(string name) : base(name)
        {
            grades = new List<double>();
            Name = name;
        }

        public override void AddGrade(double grade)
        {
            if (grade <= 100.0 && grade >= 0.00)
            {
                grades.Add(grade);
                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        public void AddGrade(char letter) 
        {
            switch(letter)
            {
                case 'A':
                AddGrade(90);
                break;
                case 'B':
                AddGrade(80);
                break;
                case 'C':
                AddGrade(70);
                break;
                case 'D':
                AddGrade(60);
                break;
                default:
                AddGrade(0);
                break;
            }
        }

        public void ShowStatistics() 
        {
            var result = 0.0;
            var highGrade = double.MinValue;
            var lowGrade = double.MaxValue;

            foreach (var number in grades)
            {
                lowGrade = Math.Min(number, lowGrade);
                highGrade = Math.Max(number, highGrade);
                result += number;
            }
            result /= grades.Count;
            Console.WriteLine($"The lowest grade is {lowGrade}");
            Console.WriteLine($"The highest grade is {highGrade}");
            Console.WriteLine($"The average grade is {result:N1}");
        }

        public override Statistics GetStatistics() 
        {
            var result = new Statistics();
            result.Average = 0.0;
            result.High = double.MinValue;
            result.Low = double.MaxValue;
            
            foreach (var grade in grades)
            {
                result.Low  = Math.Min(grade, result.Low);
                result.High = Math.Max(grade, result.High);
                result.Average += grade;
            }
            result.Average /= grades.Count;
            switch(result.Average)
            {
                case var d when d >=90.0:
                result.Letter = 'A';
                break;
                case var d when d >=80.0:
                result.Letter = 'B';
                break;
                case var d when d >=70.0:
                result.Letter = 'C';
                break;
                case var d when d >=60.0:
                result.Letter = 'D';
                break;
                default:
                result.Letter = 'F';
                break;
            }

            return result;
        }

        private List<double> grades;
        public string Name {get; set;}
    }
}