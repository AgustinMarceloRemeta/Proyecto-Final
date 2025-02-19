using System;
using System.Collections.Generic;

[Serializable]
public class StudentData 
{
    public string name;
    public string lastName;
    public List <bool> attendanceBools;
    public float attendancePercentage;
    public float absence;
    public float attendances;
    public float totalDays;
    public List <Note> notes;
    public float noteAverage;
    public float notePercentage;
    public string course;
    private Dictionary<string, DiaInfo> _historialDias = new Dictionary<string, DiaInfo>();

}
[Serializable]
public class JsonData
{
    public List<StudentData> students;
}

[Serializable]
public class Note
{
    public string referencia;
    public float value;
}