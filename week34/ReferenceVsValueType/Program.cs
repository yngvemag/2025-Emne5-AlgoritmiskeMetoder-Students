using ReferenceVsValueType;

Point p1 = new() {X = 10, Y = 20};
Point p2 = p1; 
p2.X = 30;
Console.WriteLine($"Verditype p1: {p1.X}, {p2.X}"); 

Person person1 = new(){Name = "Ola"};
Person person2 = person1;
person2.Name = "Kari";
Console.WriteLine($"Referansetype: person1: {person1.Name}, person2: {person2.Name}");

Point p3 = new() {X = 10, Y = 20};
Point.ChangePoint(p3);
Console.WriteLine($"Point p3 after change: {p3.X}, {p3.Y}");

Person person3 = new() {Name = "Petter"};
Person.ChangePerson(person3);
Console.WriteLine($"Person person3 after change: {person3.Name}");

Point p4 = new() {X = 10, Y = 20};
Point.ChangePointRef(ref p4);
Console.WriteLine($"Point p4 after change ref: {p4.X}, {p4.Y}");